public interface ICachableRequest
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
}
