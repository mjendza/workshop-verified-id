using System;
using Entra.Verified.ID.WebApp.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Entra.Verified.ID.WebApp.Services;

public class CacheServiceWrapper
{
    private readonly IMemoryCache _cache;
    private readonly int _defaultExp;

    public CacheServiceWrapper(IMemoryCache cache, IOptions<AppSettingsModel> appSettings)
    {
        _cache = cache;
        _defaultExp = 600;
    }

    public bool GetCachedObject<T>(string key, out T @object)
    {
        @object = default;
        bool rc;
        if (rc = _cache.TryGetValue(key, out var val)) @object = (T) Convert.ChangeType(val, typeof(T));
        return rc;
    }

    public bool GetCachedValue(string key, out string value)
    {
        return _cache.TryGetValue(key, out value);
    }

    public void CacheObjectWithExpiery(string key, object Object, int seconds)
    {
        _cache.Set(key, Object, DateTimeOffset.Now.AddSeconds(seconds));
    }

    public void RemoveCacheValue(string key)
    {
        _cache.Remove(key);
    }
}

public static class AspNetHelper
{
    public static string SerializeToCamelCase(object request)
    {
        return JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}