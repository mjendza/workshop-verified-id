using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Portal.VerifiableCredentials.API.Configuration;

namespace Portal.VerifiableCredentials.API.Services;

public class CacheServiceWrapper
{
    private readonly IMemoryCache _cache;
    private readonly int _defaultExp;

    public CacheServiceWrapper(IMemoryCache cache, IOptions<AppSettingsModel> appSettings)
    {
        _cache = cache;
        _defaultExp = 600;
    }

    public List<string> GetKeys()
    {
        var field = typeof(MemoryCache).GetProperty("_entries", BindingFlags.NonPublic | BindingFlags.Instance);
        var collection = field.GetValue(_cache) as ICollection<KeyValuePair<object, object>>;
        var items = new List<string>();
        if (collection != null)
            foreach (var item in collection)
            {
                var methodInfo = item.GetType().GetProperty("Key");
                var val = methodInfo.GetValue(item);
                items.Add(val.ToString());
            }

        return items;
    }
    public bool GetCachedObject<T>(string key, out T @object) {
        @object = default;
        bool rc;
        if ( (rc = _cache.TryGetValue(key, out var val) ) ) {
            @object = (T)Convert.ChangeType(val, typeof(T));
        }
        return rc;
    }
    public bool GetCachedValue(string key, out string value) {
        return _cache.TryGetValue(key, out value);
    }
    public void CacheObjectWithNoExpiery(string key, object Object) {
        _cache.Set(key, Object);
    }
    public void CacheObjectWithExpiery(string key, object Object, int seconds) {
        _cache.Set(key, Object, DateTimeOffset.Now.AddSeconds(seconds));
    }
    public void RemoveCacheValue( string key ) {
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

    public static T DeserializeFromCamelCase<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
}