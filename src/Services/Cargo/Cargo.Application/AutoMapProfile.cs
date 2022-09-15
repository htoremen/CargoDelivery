using AutoMapper;
using Cargo.Application.Cargos.CreateCargos;
using Cargo.Application.Cargos.SendSelfie;
using Core.Domain.Instances;

namespace Cargo.Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<ISendSelfie, SendSelfieCommand>();
        CreateMap<ICreateCargo, CreateCargoCommand>();

        CreateMap<Domain.Entities.Cargo, CargoRouteInstance > ();
    }
}