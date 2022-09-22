using AutoMapper;
using Cargo.Application.Cargos.Queries.GetCargoLists;
using Cargo.Application.Cargos.Queries.GetCargos;
using Grpc.Core;
using MediatR;

namespace Cargo.GRPC.Server.Services;

public class CargoService : CargoGrpc.CargoGrpcBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CargoService(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public override async Task<GetCargosResponse> GetCargoAll(GetCargosRequest request, ServerCallContext context)
    {
        var data = await _mediator.Send(new GetCargoQuery { CorrelationId = request.CorrelationId });
        var response = new GetCargosResponse();
        foreach (var item in data.Data)
        {
            var cargoItems = new List<GetCargoItems>();
            foreach (var cargoItem in item.CargoItems)
            {
                cargoItems.Add(new GetCargoItems
                {
                    Barcode = cargoItem.Barcode,
                    CargoItemId = cargoItem.CargoItemId,
                    Description = cargoItem.Description,
                    Desi = cargoItem.Desi,
                    Kg = cargoItem.Kg,
                    WaybillNumber = cargoItem.WaybillNumber,
                });
            }

            response.Cargos.Add(new GetCargos
            {
                Address = item.Address,
                CargoId = item.CargoId,
                DebitId = item.DebitId,
                CorrelationId = request.CorrelationId,
                CargoItems = { cargoItems }
            });
        }

        return response;
    }

    public override async Task<GetCargoListResponse> GetCargoList(GetCargoListRequest request, ServerCallContext context)
    {
        var cargos = await _mediator.Send(new GetCargoListQuery { CorrelationId = request.CorrelationId }); 

        var response = new GetCargoListResponse();
        foreach (var item in cargos.Data)
        {
            response.Cargos.Add(new GetCargos
            {
                Address = item.Address,
                CargoId = item.CargoId,
                DebitId = item.DebitId,
            });
        }
        return response;
    }
}