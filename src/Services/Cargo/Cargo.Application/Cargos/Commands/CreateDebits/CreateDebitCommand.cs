﻿using Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Diagnostics;

namespace Cargo.Application.Cargos.CreateDebits;

public class CreateDebitCommand : IRequest<GenericResponse<CreateDebitResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid DebitId { get; set; }
    public Guid CourierId { get; set; }
    public string CurrentState { get; set; }
    public List<CreateDebitCargo> Cargos { get; set; }
}

public class CreateDebitCommandHandler : IRequestHandler<CreateDebitCommand, GenericResponse<CreateDebitResponse>>
{
    private IApplicationDbContext _context;

    public CreateDebitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<CreateDebitResponse>> Handle(CreateDebitCommand request, CancellationToken cancellationToken)
    {
        var debit = await _context.Debits.FirstOrDefaultAsync(x => x.CorrelationId == request.CorrelationId.ToString() && x.CourierId == request.CourierId.ToString());
        if (debit == null)
        {
            var cargos = new List<Domain.Entities.Cargo>();
            foreach (var cargo in request.Cargos)
            {
                var cargoItems = new List<CargoItem>();
                foreach (var cargoItem in cargo.CargoItems)
                {
                    cargoItems.Add(new CargoItem
                    {
                        CargoItemId = cargoItem.CargoItemId.ToString(),
                        Address = cargoItem.Address,
                        Barcode = cargoItem.Barcode,
                        Description = cargoItem.Description,
                        Desi = cargoItem.Desi,
                        Kg = cargoItem.Kg,
                        WaybillNumber = cargoItem.WaybillNumber,
                    });
                }
                cargos.Add(new Domain.Entities.Cargo
                {
                    CargoId = cargo.CargoId.ToString(),
                    Address = cargo.Address,
                    CargoItems = cargoItems
                });
            }

            debit = _context.Debits.Add(new Debit
            {
                DebitId = request.DebitId.ToString(),
                CorrelationId = request.CorrelationId.ToString(),
                CourierId = request.CourierId.ToString(),
                DistributionDate = DateTime.Now.Date,
                StartingDate = DateTime.Now,
                IsApproval = false,
                IsCompleted = false,
                CurrentState = request.CurrentState,
                Cargos = cargos
            }).Entity;
            await _context.SaveChangesAsync(cancellationToken);
        }
        return GenericResponse<CreateDebitResponse>.Success(new CreateDebitResponse { CorrelationId = request.CorrelationId }, 200);

    }

    //public async Task<GenericResponse<CreateDebitResponse>> _Handle(CreateDebitCommand request, CancellationToken cancellationToken)
    //{
    //    var debit = await _context.Debits.FirstOrDefaultAsync(x => x.DistributionDate != DateTime.Now && x.CourierId == request.CourierId.ToString());
    //    if (debit == null)
    //    {
    //        debit = _context.Debits.Add(new Debit
    //        {
    //            DebitId = request.DebitId.ToString(),
    //            CorrelationId = request.CorrelationId.ToString(),
    //            CourierId = request.CourierId.ToString(),
    //            DistributionDate = DateTime.Now.Date,
    //            StartingDate = DateTime.Now,
    //            IsApproval = false,
    //            IsCompleted = false,
    //            CurrentState = request.CurrentState
    //        }).Entity;
    //        await _context.SaveChangesAsync(cancellationToken);
    //        return GenericResponse<CreateDebitResponse>.Success(new CreateDebitResponse { CorrelationId = request.CorrelationId, DebitId = debit.DebitId }, 200);
    //    }
    //    return GenericResponse<CreateDebitResponse>.NotFoundException("", 404);
    //}


}
