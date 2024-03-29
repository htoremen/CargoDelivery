﻿using Order.Application.Deliveries.ShiftCompletions;
using Order.Application.Deliveries.CreateDeliveries;
using Order.Application.Deliveries.CreateRefunds;
using Order.Application.Deliveries.NotDelivereds;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Order.Application.NoSqls.RedisDataAdds;
using System.Text.Json;
using Order.Application.Deliveries.VerificationCodes;
using Core.Domain;

namespace Order.API.Controllers;

public class DeliveryController : ApiControllerBase
{
    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("verification-code")]
    public async Task<IActionResult> VerificationCode(Guid correlationId, Guid cargoId, int code)
    {
        var command = new VerificationCodeCommand
        {
            CargoId = cargoId,
            CorrelationId = correlationId,
            Code = code
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.VerificationCode,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("create-delivery")]
    public async Task<IActionResult> CreateDelivery(Guid correlationId, Guid cargoId, PaymentType paymentType)
    {
        var command = new CreateDeliveryCommand
        {
            CargoId = cargoId,
            CorrelationId = correlationId,
            PaymentType = paymentType
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.CreateDelivery,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("not-delivered")]
    public async Task<IActionResult> NotDelivered(Guid correlationId, Guid cargoId)
    {
        var command = new NotDeliveredCommand
        {
            CargoId = cargoId,
            CorrelationId = correlationId
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.NotDelivered,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }


    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("create-refund")]
    public async Task<IActionResult> CreateRefund(Guid correlationId, Guid cargoId)
    {
        var command = new CreateRefundCommand
        {
            CargoId = cargoId,
            CorrelationId = correlationId
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.CreateRefund,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);

        return Ok(response);
    }


    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("shift-completion")]
    public async Task<IActionResult> ShiftCompletion([FromBody] Guid correlationId)
    {
        var command = new ShiftCompletionCommand
        {
            CorrelationId = correlationId
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.ShiftCompletion,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }
}