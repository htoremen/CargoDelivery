using AutoMapper;
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
        var cargos = data.Data.Select(x => new GetCargosResponse
        {

        });

        //var response = new GetCargosResponse
        //{
        //    Cargos = cargos
        //};
       // var cargos = _mapper.Map<GetCargosResponse>(data);
        return new GetCargosResponse();
    }
}
