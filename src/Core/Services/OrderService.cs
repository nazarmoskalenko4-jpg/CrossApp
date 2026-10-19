using Core.Abstractions;
using Core.Domain;

namespace Core.Services;

public sealed class OrderService(IOrderStore store)
{
    private readonly IOrderStore _store = store ?? throw new ArgumentNullException(nameof(store));

    public Order CreateOrder(string? note = null, string? customId = null)
    {
        string id = customId ?? Guid.NewGuid().ToString("N")[..8];
        var order = Order.Create(id, note);
        _store.Add(order);
        return order;
    }

    public void AddLine(string orderId, string name, decimal price, int quantity)
    {
        var order = _store.GetById(orderId)
            ?? throw new InvalidOperationException($"Немає запису з id={orderId}.");

        order.AddLine(name, price, quantity);
        _store.Update(order);
    }

    public void Confirm(string orderId)
    {
        var order = _store.GetById(orderId)
            ?? throw new InvalidOperationException($"Немає запису з id={orderId}.");

        order.Confirm();
        _store.Update(order);
    }

    public bool Remove(string orderId) => _store.Remove(orderId);

    public IReadOnlyList<Order> All() => _store.List();

    public Order? Find(string id) => _store.GetById(id);

    // Додаткове завдання 2: Пошук з параметром Func<Order, bool>
    public IReadOnlyList<Order> Search(Func<Order, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _store.List().Where(predicate).ToList();
    }
}