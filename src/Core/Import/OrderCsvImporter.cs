namespace Core.Import;

using System.Globalization;
using Core.Dto;

public static class OrderCsvImporter
{
    private const char Separator = ';';

    public static ImportResult Load(string path)
    {
        var orders = new List<OrderDto>();
        var customers = new List<CustomerDto>();
        var errors = new List<string>();
        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            int number = i + 1;
            string line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#')) continue;
            if (number == 1 && line.StartsWith("type", StringComparison.OrdinalIgnoreCase)) continue;

            switch (ParseLine(line))
            {
                case ParseOrderOk o: orders.Add(o.Value); break;
                case ParseCustomerOk c: customers.Add(c.Value); break;
                case ParseFailed f: errors.Add($"рядок {number}: {f.Reason}"); break;
            }
        }
        return new ImportResult(orders, customers, errors);
    }

    private static ParseOutcome ParseLine(string line)
    {
        string[] parts = line.Split(Separator, StringSplitOptions.TrimEntries);

        return parts switch
        {
            { Length: < 4 } => new ParseFailed($"очікую мінімум 4 колонки, отримав {parts.Length}"),
            
            // Патерни для Замовлень (Order)
            ["O", _, "", ..] => new ParseFailed("Назва замовлення порожня"),
            ["O", var id, var name, var priceStr, ..] when !decimal.TryParse(priceStr, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price) || price < 0 
                => new ParseFailed($"ціна '{priceStr}' не є коректним додатним числом"),
            ["O", var id, var name, var priceStr] 
                => new ParseOrderOk(new OrderDto(id, name, decimal.Parse(priceStr, CultureInfo.InvariantCulture))),
            ["O", var id, var name, var priceStr, var note] 
                => new ParseOrderOk(new OrderDto(id, name, decimal.Parse(priceStr, CultureInfo.InvariantCulture), note)),

            // Патерни для Клієнтів (Customer)
            ["C", "", ..] => new ParseFailed("ID клієнта порожній"),
            ["C", var id, var name, var phone] 
                => new ParseCustomerOk(new CustomerDto(id, name, phone)),

            // Якщо префікс не розпізнано
            [var type, ..] => new ParseFailed($"невідомий тип рядка: '{type}'")
        };
    }

    private abstract record ParseOutcome;
    private sealed record ParseOrderOk(OrderDto Value) : ParseOutcome;
    private sealed record ParseCustomerOk(CustomerDto Value) : ParseOutcome;
    private sealed record ParseFailed(string Reason) : ParseOutcome;
}