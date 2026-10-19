using Core.Dto;

namespace Core.Domain;

public sealed class Order
{
    public string Id { get; }
    public string? Note { get; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderLine> _lines = [];
    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();

    public decimal Total => _lines.Sum(l => l.Price * l.Quantity);

    private Order(string id, string? note, OrderStatus status)
    {
        Id = id;
        Note = note;
        Status = status;
    }

    public static Order Create(string id, string? note = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор замовлення обов'язковий", nameof(id));

        return new Order(id.Trim(), note?.Trim(), OrderStatus.Draft);
    }

    public void AddLine(string name, decimal price, int quantity)
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException($"Замовлення {Id} не в статусі чернетки. Додавання рядків заборонено.");

        _lines.Add(OrderLine.Create(name, price, quantity));
    }

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

    // Мапінг всього агрегату (разом з усіма рядками та статусом) у DTO
    public OrderDto ToDto()
    {
        var firstLine = _lines.FirstOrDefault();
        var lineDtos = _lines.Select(l => new OrderLineDto(l.Name, l.Price, l.Quantity)).ToList();

        return new OrderDto(
            Id: Id,
            Name: firstLine?.Name ?? "Порожнє замовлення",
            Price: firstLine?.Price ?? 0m,
            Note: Note,
            Status: Status,
            Lines: lineDtos
        );
    }

    // Відновлення агрегату з DTO (підтримує і новий формат з Lines, і старий плоский з Лаб 3)
    public static Order FromDto(OrderDto dto)
    {
        var order = Create(dto.Id, dto.Note);

        if (dto.Lines is { Count: > 0 })
        {
            foreach (var line in dto.Lines)
            {
                order.AddLine(line.Name, line.Price, line.Quantity);
            }
        }
        else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != "Порожнє замовлення")
        {
            order.AddLine(dto.Name, dto.Price, 1);
        }

        // Встановлюємо збережений статус після наповнення рядками
        order.Status = dto.Status;
        return order;
    }

    public override string ToString() =>
        $"Замовлення {Id,-8} | Статус: {Status,-9} | Рядків: {_lines.Count,2} | Сума: {Total,9:F2} грн | Примітка: {Note ?? "немає"}";
}