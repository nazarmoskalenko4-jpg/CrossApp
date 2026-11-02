using Core.Abstractions;
using Core.Domain;
using Core.Events;

namespace Core.Services;

public sealed class OrderService(IOrderStore store, IEnumerable<INotificationSink>? sinks = null)
{
    private readonly IOrderStore _store = store ?? throw new ArgumentNullException(nameof(store));
    private readonly IEnumerable<INotificationSink> _sinks = sinks ?? [];

    // Оголошення події
    public event EventHandler<OrderChangedEventArgs>? Changed;

    // Метод підняття події та розсилки через Sinks (Додаткове завдання №1)
    private void OnChanged(Order order, OrderChangeKind kind)
    {
        Changed?.Invoke(this, new OrderChangedEventArgs(order.Id, kind, order.Lines.Count, order.Total));

        foreach (var sink in _sinks)
        {
            sink.Notify($"Замовлення {order.Id} | Дія: {kind} | Позицій: {order.Lines.Count} | Сума: {order.Total:F2}");
        }
    }

    public Order CreateOrder(string? note = null, string? customId = null)
    {
        string id = customId ?? Guid.NewGuid().ToString("N")[..8];
        var order = Order.Create(id, note);
        _store.Add(order);
        
        OnChanged(order, OrderChangeKind.OrderCreated); // Тільки після успішної зміни
        return order;
    }

    public void AddLine(string orderId, string name, decimal price, int quantity)
    {
        var order = _store.GetById(orderId)
            ?? throw new InvalidOperationException($"Немає запису з id={orderId}.");

        order.AddLine(name, price, quantity);
        _store.Update(order);
        
        OnChanged(order, OrderChangeKind.LineAdded);
    }

    public void Confirm(string orderId)
    {
        var order = _store.GetById(orderId)
            ?? throw new InvalidOperationException($"Немає запису з id={orderId}.");

        order.Confirm();
        _store.Update(order);
        
        OnChanged(order, OrderChangeKind.OrderConfirmed);
    }

    public bool Remove(string orderId)
    {
        var order = _store.GetById(orderId);
        if (order != null && _store.Remove(orderId))
        {
            OnChanged(order, OrderChangeKind.OrderRemoved);
            return true;
        }
        return false;
    }

    public IReadOnlyList<Order> All() => _store.List();

    public Order? Find(string id) => _store.GetById(id);

    // Додаткове завдання 2: Пошук з параметром Func<Order, bool> (Вже реалізовано коректно)
    public IReadOnlyList<Order> Search(Func<Order, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _store.List().Where(predicate).ToList(); // Where будує підмножину[cite: 7]
    }
}