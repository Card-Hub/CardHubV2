namespace WebApi.Common;

public static class DictionaryExtensions
{
    public static bool TryGetValueAs<TKey, TValue, TValueAs>(this IDictionary<TKey, TValue> dictionary, TKey key,
        out TValueAs valueAs) where TValueAs : TValue
    {
        if (dictionary.TryGetValue(key, out var value) && value is TValueAs valueAsCast)
        {
            valueAs = valueAsCast;
            return true;
        }

        valueAs = default!;
        return false;
    }
}