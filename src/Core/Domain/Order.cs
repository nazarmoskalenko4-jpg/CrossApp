using Core.Dto;

namespace Core.Domain;

public sealed class Order
{
    public string Id { get; }
    public string? Note { get; }
    
    // Властивість з private set
    public OrderStatus Status { get; private set; }

    // Захищена колекція
    private readonly List<OrderLine> _lines = [];
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();

    public decimal Total => _lines.Sum(l => l.Price * l.Quantity);

    private Order(string id, string? note, OrderStatus status)
    {
        Id = id;
        Note = note;
        Status = status;
    }

    // Фабричний метод
    public static Order Create(string id, string? note = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор замовлення обов'язковий", nameof(id));

        return new Order(id.Trim(), note?.Trim(), OrderStatus.Draft);
    }

    // Метод зміни стану
    public void AddLine(string name, decimal price, int quantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Замовлення {Id} не в статусі чернетки. Додавання рядків заборонено.");

        _lines.Add(OrderLine.Create(name, price, quantity));
    }

    // Додаткове завдання 3: перехід станів
    public void Confirm()
    {
        if (_lines.Count == 0)
            throw new InvalidOperationException($"Замовлення {Id} порожнє, його не можна підтвердити.");

        Status = Status switch
        {
            OrderStatus.Draft => OrderStatus.Confirmed,
            OrderStatus.Confirmed => throw new InvalidOperationException($"Замовлення {Id} вже підтверджене."),
            OrderStatus.Cancelled => throw new InvalidOperationException($"Замовлення {Id} скасоване, його не можна підтвердити."),
            _ => throw new NotImplementedException()
        };
    }

    // Зв'язок з тижнем 3: Мапінг у твій існуючий DTO
    public OrderDto ToDto() 
    {
        var firstLine = _lines.FirstOrDefault();
        return new OrderDto(Id, firstLine?.Name ?? "Невідомо", firstLine?.Price ?? 0, Note);
    }

    public static Order FromDto(OrderDto dto)
    {
        // Відновлюємо сутність, проходячи ті самі перевірки фабричного методу
        var order = Create(dto.Id, dto.Note);
        order.AddLine(dto.Name, dto.Price, 1);
        return order;
    }

    public override string ToString() => $"Замовлення {Id} - Статус: {Status}, Рядків: {_lines.Count}, Сума: {Total:F2} (Примітка: {Note ?? "немає"})";
}