using AutoMapper;
using Cargo.Application.Cargos.Commands.CreateCargos;
using Cargo.Application.Cargos.CreateDebits;

namespace Cargo.Application.Consumer;
public class CreateDebitConsumer : IConsumer<ICreateDebit>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateDebitConsumer(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICreateDebit> context)
    {
        var command = context.Message;
        var model = _mapper.Map<CreateDebitCommand>(command);
        await _mediator.Send(model);

        //foreach (var cargo in command.Cargos)
        //{
        //    var cargoItems= new List<CreateCargoCargoItem>();
        //    foreach (var cargoItem in cargo.CargoItems)
        //    {
        //        cargoItems.Add(new CreateCargoCargoItem
        //        {
        //            CargoItemId = cargoItem.CargoItemId,
        //            Barcode = cargoItem.Barcode,
        //            Description = cargoItem.Description,
        //            Desi = cargoItem.Desi,
        //            Kg = cargoItem.Kg,
        //            WaybillNumber = cargoItem.WaybillNumber,
        //        });
        //    }
        //   await context.Publish<ICreateCargo>(new CreateCargo
        //   {
        //       CargoId = cargo.CargoId,
        //       Address = cargo.Address,
        //       CorrelationId=command.CorrelationId,
        //       DebitId=command.DebitId,
        //       CargoItems = cargoItems
        //   });
        //}
    }
}
