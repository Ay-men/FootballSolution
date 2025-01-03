namespace Application.Common.Behaviors;

using System.Text;
using System.Text.Json;
using Domain.Interfaces;
using Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

public class CachingBehavior<TRequest, TResponse>(
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IDistributedCache cache
)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        TResponse response;
        if (request.BypassCache) return await next();

        async Task<TResponse> GetResponseAndAddToCache()
        {
            response = await next();
            if (response != null)
            {
                var slidingExpiration =
                    request.SlidingExpirationInMinutes == 0 ? 30 : request.SlidingExpirationInMinutes;
                var absoluteExpiration =
                    request.AbsoluteExpirationInMinutes == 0 ? 60 : request.AbsoluteExpirationInMinutes;
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));

                var serializedData = Encoding.Default.GetBytes(JsonSerializer.Serialize(response, JsonOptions));
                await cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
            }

            return response;
        }

        var cachedResponse = await cache.GetAsync(request.CacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            var t = Encoding.Default.GetString(cachedResponse);
            var tt = typeof(TResponse);
            // var tresponse = JsonSerializer.Deserialize<Result<TResponse>>(t)!;
            response = JsonSerializer.Deserialize<TResponse>(t, JsonOptions);
            logger.LogInformation("fetched from cache with key : {CacheKey}", request.CacheKey);
            cache.Refresh(request.CacheKey);
        }
        else
        {
            response = await GetResponseAndAddToCache();
            logger.LogInformation("added to cache with key : {CacheKey}", request.CacheKey);
        }

        return response;
    }
}