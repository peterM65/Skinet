namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cachKey, object response, TimeSpan timeToLive);
        Task<string> GetCacheResponseAsync(string cachKey);
    }
}
