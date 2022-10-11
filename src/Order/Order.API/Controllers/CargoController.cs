﻿using Cargo.Application.Cargos.CargoApprovals;
using Cargo.Application.Cargos.CreateDebits;
using Cargo.Application.Cargos.SendSelfies;
using Cargos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Order.Application.NoSqls.RedisDataAdds;
using Order.Domain;
using System.Diagnostics;

namespace Order.API.Controllers
{
    public class CargoController : ApiControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Route("create-debit")]
        public async Task<IActionResult> CreateDebit()
        {
            ActivitySource source = new ActivitySource("Order.DistributedTracing", "1.0.0");
            var correlationId = "";
            using (Activity activity = source.StartActivity("CreateDebitCommand"))
            {
                var command = new CreateDebitCommand
                {
                    CourierId = Guid.NewGuid(),
                    DebitId = Guid.NewGuid(),
                    Cargos = GetCargos()
                };

                correlationId = command.DebitId.ToString();

                await Mediator.Send(new RedisDataAddCommand
                {
                    CacheKey = StaticKeyValues.CreateDebit,
                    CacheValue = command.DebitId.ToString(),
                    Value = JsonSerializer.Serialize(command)
                });

                var response = await Mediator.Send(command);
            }
            return Ok(correlationId);
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("send-selfie")]
        public async Task<IActionResult> SendSelfie([FromBody] Guid correlationId)
        {
            var command = new SendSelfieCommand
            {
                CorrelationId = correlationId
            };

            await Mediator.Send(new RedisDataAddCommand
            {
                CacheKey = StaticKeyValues.SendSelfie,
                CacheValue = command.CorrelationId.ToString(),
                Value = JsonSerializer.Serialize(command)
            });
            var response = await Mediator.Send(new SendSelfieCommand
            {
                CorrelationId = correlationId
            });
            return Ok(response);
        }

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("cargo-approval")]
        public async Task<IActionResult> CargoApproval([FromBody] Guid correlationId)
        {
            var command = new CargoApprovalCommand
            {
                CorrelationId = correlationId
            };

            await Mediator.Send(new RedisDataAddCommand
            {
                CacheKey = StaticKeyValues.SendSelfie,
                CacheValue = command.CorrelationId.ToString(),
                Value = JsonSerializer.Serialize(command)
            });

            var response = await Mediator.Send(command);
            return Ok(response);
        }

        #region Debit Cargos Data

        private List<CreateDebitCargo> GetCargos()
        {
            Random rnd = new Random();
            var cargoLength = rnd.Next(10, 20);

            var cargos = new List<CreateDebitCargo>();
            for (int i = 1; i <= cargoLength; i++)
            {
                var itemLength = rnd.Next(1, 5);
                cargos.Add(new CreateDebitCargo
                {
                    CargoId = Guid.NewGuid(),
                    Address = "Address " + i,
                    CargoItems = GetCargoItems(itemLength)
                });
            }
            return cargos;
        }

        private List<CreateDebitCargoItem> GetCargoItems(int itemLength)
        {
            var items = new List<CreateDebitCargoItem>();
            for (int i = 1; i <= itemLength; i++)
            {
                items.Add(new CreateDebitCargoItem
                {
                    CargoItemId = Guid.NewGuid(),
                    Address = "Address" + i,
                    Barcode = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Desi = "",
                    Kg = "",
                    WaybillNumber = Guid.NewGuid().ToString(),
                });
            }
            return items;
        }

        #endregion
    }
}
