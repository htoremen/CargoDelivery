using AutoMapper;
using Route.Application.Routes.AutoRoutes;
using Route.Application.Routes.ManuelRoutes;
using Route.Application.Routes.StartRoutes;
using Route.Application.Routes.StateUpdates;

namespace Route.Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<IStartRoute, StartRouteCommand>();
        CreateMap<IStartRoute, StateUpdateCommand>();

        CreateMap<IAutoRoute, AutoRouteCommand>();
        CreateMap<IAutoRoute, StateUpdateCommand>();

        CreateMap<IManuelRoute, ManuelRouteCommand>();
        CreateMap<IManuelRoute, StateUpdateCommand>();

        CreateMap<Domain.Entities.CargoRoute, ManuelAutoRouteInstance>();
    }
}