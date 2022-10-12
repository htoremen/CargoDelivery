using Cargos;
using Microsoft.AspNetCore.Mvc.Filters;
using Order.API.Controllers;
using System.Diagnostics;

namespace Order.API.Filters;

public class OrderActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ActivitySource Activity = new(nameof(context.Controller));
        using (var activity = Activity.StartActivity($"{nameof(CreateDebit)} ({nameof(context.Controller)})", ActivityKind.Server)) ;
        var result = await next();
    }
}
