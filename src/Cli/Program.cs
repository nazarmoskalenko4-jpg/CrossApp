using Core.Dto;
using Core.Import;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

// Вибір імпортера за розширенням
string extension = Path.GetExtension(path).ToLowerInvariant();
ImportResult result = extension switch
{
    ".csv" => OrderCsvImporter.Load(path),
    ".json" => OrderJsonImporter.Load(path),
    _ => throw new NotSupportedException($"Формат файлу '{extension}' не підтримується.")
};

// Завдання 3: Статистика імпорту
int accepted = result.Orders.Count + result.Customers.Count;
int total = accepted + result.Errors.Count;
double errorRate = total == 0 ? 0 : (double)result.Errors.Count / total * 100;
Console.WriteLine($"Статистика: усього {total} | прийнято {accepted} | пропущено {result.Errors.Count} ({errorRate:F1}% помилок)\n");

// Вимоги DoD: виведення перших записів
Console.WriteLine("--- ПЕРШІ ЗАМОВЛЕННЯ ---");
foreach (OrderDto order in result.Orders.Take(5))
{
    Console.WriteLine($" {order.Id,-6} {order.Name,-20} {order.Price,8:F2}  {order.Note}");
}

Console.WriteLine("\n--- ПЕРШІ КЛІЄНТИ ---");
foreach (CustomerDto customer in result.Customers.Take(5))
{
    Console.WriteLine($" {customer.Id,-6} {customer.Name,-20} {customer.Phone}");
}

// Перелік пропущених рядків з номерами
if (result.Errors.Count > 0)
{
    Console.WriteLine($"\nПропущено рядків: {result.Errors.Count}");
    foreach (string error in result.Errors)
    {
        Console.WriteLine($" ! {error}");
    }
}

return 0;