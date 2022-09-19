using AutoMapper;

namespace Route.Application;

public class AutoMapProfile : Profile
{
    public AutoMapProfile()
    {
        CreateMap<Routes.Queries.GetRoutes.GetRouteResponse, GRPC.Server.GetRouteResponse>();
    }
}