using Core.Application.Common.Interfaces;
using Core.Domain;
using Core.Domain.SerializerModels;
using Microsoft.AspNetCore.Mvc;
using Order.Application.NoSqls.RedisDataAdds;
using Order.Application.Routes.AutoRoutes;
using Order.Application.Routes.ManuelRoutes;
using System;
using System.Text.Json;

namespace Order.API.Controllers;

public class RouteController : ApiControllerBase
{
    private readonly ICacheService _cache;

    public RouteController(ICacheService cache)
    {
        _cache = cache;
    }

    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("auto-route")]

    public async Task<IActionResult> AutoRoute([FromBody] Guid correlationId)
    {
        var command = new AutoRouteCommand
        {
            CorrelationId = correlationId
        };

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.AutoRoute,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost()]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Route("manuel-route")]
    public async Task<IActionResult> ManuelRoute(ManuelRouteCommand command)
    {
        command.Routes = await GetRoutes(command.CorrelationId.ToString());

        await Mediator.Send(new RedisDataAddCommand
        {
            CacheKey = StaticKeyValues.ManuelRoute,
            CacheValue = command.CorrelationId.ToString(),
            Value = JsonSerializer.Serialize(command)
        });

        var response = await Mediator.Send(command);
        return Ok(response);
    }

    private async Task<List<ManuelRouteModel>> GetRoutes(string correlationId)
    {
        var cacheKey = StaticKeyValues.CreateDebit + correlationId.ToString();
        var data = await _cache.GetValueAsync(cacheKey);
        var response = JsonSerializer.Deserialize<CreateDebitModel>(data);

        var list = new List<ManuelRouteModel>();
        int i = 1;
        foreach (var item in response.Cargos)
        {
            list.Add(new ManuelRouteModel
            {
                CargoId = item.CargoId.ToString(),
                RouteAddress = RandomString(200),
                OrderNo = i
            });
            i += 1;
        }

        return list;
    }

    private string RandomString(int length)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
