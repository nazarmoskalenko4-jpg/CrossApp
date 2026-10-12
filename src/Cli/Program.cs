using Core.Domain;
using Core.Dto;
using Core.Import;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// ==========================================
// ЛАБОРАТОРНА 3: Демонстрація імпорту
// ==========================================
string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
    return 1;
}

string extension = Path.GetExtension(path).ToLowerInvariant();
ImportResult result = extension switch
{
    ".csv" => OrderCsvImporter.Load(path),
    ".json" => OrderJsonImporter.Load(path),
    _ => throw new NotSupportedException($"Формат файлу '{extension}' не підтримується.")
};

int accepted = result.Orders.Count + result.Customers.Count;
int total = accepted + result.Errors.Count;
Console.WriteLine($"--- ІМПОРТ (Лаб 3) ---");
Console.WriteLine($"Статистика: усього {total} | прийнято {accepted} | помилок {result.Errors.Count}");
Console.WriteLine();

// --- Додаткове завдання 1 ---
Console.WriteLine("--- ДОДАТКОВЕ ЗАВДАННЯ 1: Фільтрація доменними інваріантами ---");
var (validOrders, domainErrors) = DomainImporter.ProcessImport(result);
Console.WriteLine($"Успішно створено доменних об'єктів: {validOrders.Count}");
if (domainErrors.Count > 0)
{
    Console.WriteLine("Помилки інваріантів під час мапінгу:");
    foreach (var err in domainErrors)
    {
        Console.WriteLine($"  {err}");
    }
}
Console.WriteLine();

// ==========================================
// ЛАБОРАТОРНА 4: Демонстрація доменної моделі
// ==========================================
Console.WriteLine("--- ДОМЕННА МОДЕЛЬ (Лаб 4) ---");
Console.WriteLine("\n Сценарій 1: Успіх ");
Order order = Order.Create("ORD-001", "Клієнт з магазину");
Console.WriteLine(order);

order.AddLine("Ноутбук", 25000m, 1);
order.AddLine("Миша", 500m, 2);
Console.WriteLine(order);

order.Confirm();
Console.WriteLine(order);
Console.WriteLine();


Console.WriteLine("\n Сценарій 2: Порушення інваріантів ");
TryDo("Створення рядка з від'ємною ціною", () => OrderLine.Create("Клавіатура", -100m, 1));
TryDo("Створення рядка з порожньою назвою", () => OrderLine.Create("", 100m, 1));
TryDo("Створення замовлення без ID", () => Order.Create(""));
TryDo("Підтвердження порожнього замовлення", () => 
{
    var emptyOrder = Order.Create("ORD-002");
    emptyOrder.Confirm();
});
TryDo("Додавання рядка до підтвердженого замовлення", () => 
{
    order.AddLine("Килимок", 300m, 1); // order - це підтверджене замовлення зі сценарію 1
});

Console.WriteLine("\nСтан об'єкта після спроб його зламати:");
Console.WriteLine(order); // Має залишитись "Сума: 26000.00"

// Локальна функція для обробки помилок
static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"[!] {title}: виняток НЕ спрацював — інваріант відсутній!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[x] {title}: {ex.GetType().Name} - {ex.Message}");
    }
}

return 0;