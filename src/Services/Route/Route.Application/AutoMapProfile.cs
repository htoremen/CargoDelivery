using AutoMapper;
using Route.Application.Routes.StartRoutes;

namespace Route.Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<IStartRoute, StartRouteCommand>();
    }
}