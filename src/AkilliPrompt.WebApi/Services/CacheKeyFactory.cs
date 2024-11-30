using System.Reflection;
using System.Text;
using AkilliPrompt.Domain.Common;
using AkilliPrompt.WebApi.Attributes;
using StackExchange.Redis;

namespace AkilliPrompt.WebApi.Services;

public class CacheKeyFactory
{
    private readonly IDatabase _redisDb;

    public CacheKeyFactory(IConnectionMultiplexer connectionMultiplexer)
    {
        _redisDb = connectionMultiplexer.GetDatabase();
    }

    /// <summary>
    /// Cache key'i oluşturur.
    /// </summary>
    /// <typeparam name="TRequest">Cache edilebilir istek tipi.</typeparam>
    /// <param name="request">Cache edilecek istek.</param>
    /// <returns>Oluşturulan cache key'i.</returns>
    public string CreateCacheKey<TRequest>(TRequest request) where TRequest : ICacheable
    {
        // İsteğin tip adını alır.
        var typeName = typeof(TRequest).Name;

        // İsteğin CacheGroup özelliğini alır.
        var cacheGroup = request.CacheGroup;

        // CacheKeyPartAttribute özniteliğine sahip tüm özellikleri alır ve adlarına göre sıralar.
        var properties = typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<CacheKeyPartAttribute>() != null)
            .OrderBy(p => p.Name);

        // Cache key'ini oluşturacak StringBuilder nesnesi.
        var keyBuilder = new StringBuilder();

        // Key'e istek tip adını ekler.
        keyBuilder.Append(typeName);

        // Her özellik için:
        foreach (var prop in properties)
        {
            // Özelliğin CacheKeyPartAttribute özniteliğini alır.
            var attr = prop.GetCustomAttribute<CacheKeyPartAttribute>();
            if (attr == null)
            {
                continue; // Skip properties without the attribute
            }

            // Özelliğin değerini alır.
            var value = prop.GetValue(request);

            // Değeri normalize eder.
            string normalizedValue = NormalizeValue(value, attr);

            // Normalize edilmiş değeri key'e ekler.
            keyBuilder.Append($"_{attr.Prefix}{normalizedValue}");
        }

        // Oluşturulan cache key'ini döndürür. Örnek: GetAllGameRegionsQuery_123_Turkiye_tr
        var cacheKey = keyBuilder.ToString();

        // Add the cache key to the group set in Redis
        if (!string.IsNullOrEmpty(cacheGroup))
        {
            var groupSetKey = $"Group:{cacheGroup}";

            _redisDb.SetAdd(groupSetKey, cacheKey);
        }

        return cacheKey;
    }

    /// <summary>
    /// Verilen değeri normalize eder. Null değerler için "null" döndürür, aksi takdirde değeri stringe çevirir ve istenirse Uri.EscapeDataString ile encode eder.
    /// </summary>
    /// <param name="value">Normalize edilecek değer.</param>
    /// <param name="attr">CacheKeyPartAttribute özniteliği.</param>
    /// <returns>Normalize edilmiş değer.</returns>
    private string NormalizeValue(object? value, CacheKeyPartAttribute attr)
    {
        // Değer null ise "null" döndürür.
        if (value is null)
            return "null";

        // Değeri stringe çevirir, null ise "null" döndürür.
        string stringValue = value.ToString() ?? "null";

        // Encode özelliği aktif ise Uri.EscapeDataString ile encode eder.
        if (attr.Encode)
        {
            stringValue = Uri.EscapeDataString(stringValue);
        }

        // Normalize edilmiş değeri döndürür.
        return stringValue;
    }
}
