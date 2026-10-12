namespace Core.Domain;

public sealed class OrderLine
{
    public string Name { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    // Приватний конструктор
    private OrderLine(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }

    // Фабричний метод з перевіркою інваріантів
    public static OrderLine Create(string name, decimal price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Назва товару не може бути порожньою", nameof(name));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "Ціна не може бути від'ємною");

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Кількість має бути більшою за нуль");

        return new OrderLine(name.Trim(), price, quantity);
    }
}