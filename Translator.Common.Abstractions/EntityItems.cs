using System.Text.Json.Serialization;

namespace Translator.Common.Abstractions;

public class EntityItems<TEntity>
{
    protected EntityItems()
    {
    }

    protected EntityItems(TEntity item)
    {
        Items = [item];
    }

    protected EntityItems(IEnumerable<TEntity> items)
    {
        Items = new(items);
    }
    
    [JsonPropertyName("items")]
    public List<TEntity> Items { get; set; } = [];
}