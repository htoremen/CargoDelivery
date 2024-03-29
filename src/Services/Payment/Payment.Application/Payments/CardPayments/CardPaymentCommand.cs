﻿
using MassTransit;

namespace Payment.Application.Payments.CardPayments;

public class CardPaymentCommand : IRequest<GenericResponse<CardPaymentResponse>>
{
    public string CurrentState { get; set; }
    public Guid CorrelationId { get; set; }
    public Guid CargoId { get; set; }
}

public class CardPaymentCommandHandler : IRequestHandler<CardPaymentCommand, GenericResponse<CardPaymentResponse>>
{

    public async Task<GenericResponse<CardPaymentResponse>> Handle(CardPaymentCommand request, CancellationToken cancellationToken)
    {
        return GenericResponse<CardPaymentResponse>.Success(new CardPaymentResponse { }, 200);
    }
}
