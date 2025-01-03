namespace Domain.Interfaces;

public interface ICacheable
{
    public bool BypassCache { get; }
    public string CacheKey { get; }
    public int SlidingExpirationInMinutes { get; }
    public int AbsoluteExpirationInMinutes { get; }
}