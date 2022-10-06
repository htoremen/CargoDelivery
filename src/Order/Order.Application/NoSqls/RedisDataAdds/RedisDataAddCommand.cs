using Core.Application.Common.Interfaces;
namespace Order.Application.NoSqls.RedisDataAdds;

public class RedisDataAddCommand : IRequest
{
    public string CacheKey { get; set; }
    public string CacheValue { get; set; }
    public string Value { get; set; }
}

public class RedisDataAddCommandHandler : IRequestHandler<RedisDataAddCommand>
{
    private readonly ICacheService _cacheService;

    public RedisDataAddCommandHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Unit> Handle(RedisDataAddCommand request, CancellationToken cancellationToken)
    {
        var _cacheKey = request.CacheKey + request.CacheValue;
        await _cacheService.SetAsync(_cacheKey, request.Value);
        return Unit.Value;
    }
}
