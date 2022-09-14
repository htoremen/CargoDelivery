using AutoMapper;
using Cargo.Application.Cargos.SendSelfie;

namespace Cargo.Application.Profiles;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<ISendSelfie, SendSelfieCommand>();
    }
}