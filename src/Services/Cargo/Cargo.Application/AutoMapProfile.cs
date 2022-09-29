using AutoMapper;
using Cargo.Application.Cargos.CreateDebits;
using Cargo.Application.Cargos.Queries.GetCargos;
using Cargo.Application.Cargos.SendSelfie;
using Core.Domain.Instances;

namespace Cargo.Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<ISendSelfie, SendSelfieCommand>();
        CreateMap<ICreateDebit, CreateDebitCommand>();

        CreateMap<Domain.Entities.Cargo, CargoRouteInstance>();
        CreateMap<Domain.Entities.Cargo, GetCargosResponse>()
            .ForMember(p => p.CargoItems, q => q.MapFrom(r => r.CargoItems));
    }
}