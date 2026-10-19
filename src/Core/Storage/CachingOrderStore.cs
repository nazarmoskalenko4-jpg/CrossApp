using Core.Abstractions;
using Core.Domain;

namespace Core.Storage;

public sealed class CachingOrderStore(IOrderStore inner) : IOrderStore
{
    private readonly IOrderStore _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    private Dictionary<string, Order>? _cache;

    private Dictionary<string, Order> EnsureCache()
    {
        return _cache ??= _inner.List().ToDictionary(o => o.Id, StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyList<Order> List() => EnsureCache().Values.ToList();

    public Order? GetById(string id) => EnsureCache().GetValueOrDefault(id);

    public void Add(Order item)
    {
        _inner.Add(item);
        EnsureCache()[item.Id] = item;
    }

    public void Update(Order item)
    {
        _inner.Update(item);
        EnsureCache()[item.Id] = item;
    }

    public bool Remove(string id)
    {
        if (!_inner.Remove(id)) return false;
        EnsureCache().Remove(id);
        return true;
    }
}