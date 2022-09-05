using Core.Domain.Enums;
using Core.Domain.MessageBrokers;

namespace Cargo.Application.Cargos.CargoApprovals;

public class CargoApprovalCommand : IRequest<GenericResponse<CargoApprovalResponse>>
{
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CargoApprovalCommandHandler : IRequestHandler<CargoApprovalCommand, GenericResponse<CargoApprovalResponse>>
{
    private readonly IMessageSender<IStartRoute> _messageSender;

    public CargoApprovalCommandHandler(IMessageSender<IStartRoute> messageSender)
    {
       _messageSender = messageSender;
    }

    public async Task<GenericResponse<CargoApprovalResponse>> Handle(CargoApprovalCommand request, CancellationToken cancellationToken)
    {

        await _messageSender.SendAsync(new StartRoute
        {
            CargoId = request.CargoId,
            CorrelationId = request.CorrelationId
        }, null, cancellationToken);

        if (false)
        {

            //await _messageSender.SendAsync(new StartRoute
            //{
            //    CargoId = request.CargoId,
            //    CorrelationId = request.CorrelationId
            //}, null, cancellationToken);
        }

        //var rnd = new Random();
        //if (rnd.Next(1, 1000) % 2 == 0)
        //{
        //    await _sendEndpoint.Send<IStartRoute>(new
        //    {
        //        CargoId = request.CargoId,
        //        CorrelationId = request.CorrelationId

        //    }, cancellationToken);
        //}
        //else
        //{
        //    await _sendEndpoint.Send<IStartRoute>(new
        //    {
        //        CargoId = request.CargoId,
        //        CorrelationId = request.CorrelationId

        //    }, cancellationToken);

        //    //await _sendEndpoint.Send<ICargoRejected>(new
        //    //{
        //    //    CargoId = request.CargoId,
        //    //    CorrelationId = request.CorrelationId
        //    //}, cancellationToken);
        //}

        return GenericResponse<CargoApprovalResponse>.Success(new CargoApprovalResponse { }, 200);
    }
}
