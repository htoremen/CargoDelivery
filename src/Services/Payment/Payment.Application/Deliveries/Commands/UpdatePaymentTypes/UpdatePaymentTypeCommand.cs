using Payment.GRPC.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Deliveries.Commands.UpdatePaymentTypes;

public class UpdatePaymentTypeCommand : IRequest<GenericResponse<UpdatePaymentTypeResponse>>
{
    public string CorrelationId { get; set; }
    public string CargoId { get; set; }
    public int PaymentType { get; set; }
}

public class UpdatePaymentTypeCommandHandler : IRequestHandler<UpdatePaymentTypeCommand, GenericResponse<UpdatePaymentTypeResponse>>
{
    private readonly IDeliveryService _deliveryService;

    public UpdatePaymentTypeCommandHandler(IDeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }

    public async Task<GenericResponse<UpdatePaymentTypeResponse>> Handle(UpdatePaymentTypeCommand request, CancellationToken cancellationToken)
    {
        var result = await _deliveryService.UpdatePaymentType(request.CorrelationId, request.CargoId, request.PaymentType);

        var response = new UpdatePaymentTypeResponse();
        return GenericResponse<UpdatePaymentTypeResponse>.Success(response, 200);
    }
}

