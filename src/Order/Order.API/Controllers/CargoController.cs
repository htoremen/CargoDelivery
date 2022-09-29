using Cargo.Application.Cargos.CargoApprovals;
using Cargo.Application.Cargos.CreateCargos;
using Cargo.Application.Cargos.SendSelfies;
using Cargos;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    public class CargoController : ApiControllerBase
    {
        [HttpPost()]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Route("create-cargo")]
        public async Task<IActionResult> CreateCargo()
        {
            for (int i = 0; i < 1000; i++)
            {
                var response = await Mediator.Send(new CreateCargoCommand
                {
                    CourierId = Guid.NewGuid(),
                    DebitId = Guid.NewGuid(),
                    Cargos = GetCargos()
                });
                break;
            }
            // return Ok(response);
            return Ok();


            //var response = await Mediator.Send(new CreateCargoCommand
            //{
            //    CourierId = Guid.NewGuid(),
            //    DebitId = Guid.NewGuid(),
            //    Cargos = GetCargos()
            //});

            //return Ok(response);
        }

        private List<CargoDetay> GetCargos()
        {
            Random rnd = new Random();
            var cargoLength= rnd.Next(10, 20);

            var cargos = new List<CargoDetay>();
            for (int i = 1; i <= cargoLength; i++)
            {
                var itemLength = rnd.Next(1, 5);
                cargos.Add(new CargoDetay
                {
                    Address = "Address " + i,
                    CargoItems = GetCargoItems(itemLength)
                });
            }
            return cargos;
        }

        private List<CreateCargoCargoItem> GetCargoItems(int itemLength)
        {
            var items = new List<CreateCargoCargoItem>();
            for (int i = 1; i <= itemLength; i++)
            {
                items.Add(new CreateCargoCargoItem
                {
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

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Route("send-selfie")]
        public async Task<IActionResult> SendSelfie([FromBody] Guid correlationId)
        {
            var response = await Mediator.Send(new SendSelfieCommand
            {
                CargoId = Guid.NewGuid(),
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
            var response = await Mediator.Send(new CargoApprovalCommand
            {
                CargoId = Guid.NewGuid(),
                CorrelationId = correlationId
            });
            return Ok(response);
        }
    }
}
