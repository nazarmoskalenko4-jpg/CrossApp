using Cli;
using Core;
using Core.Abstractions;
using Core.Domain;
using Core.Services;
using Core.Storage;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// =========================================================================
// Крок 5. Composition Root у Cli — єдине місце з конкретними класами
// =========================================================================
bool useFile = args.Contains("--file");
bool useFactory = args.Contains("--factory");
bool useCache = args.Contains("--cache");

string dataPath = Path.Combine(AppContext.BaseDirectory, "data", "orders.json");

// Базовий варіант за методичкою (тернарний оператор прямо в Program.cs)
// Або виклик StoreFactory.Create(args, dataPath) для Додаткового завдання №3
IOrderStore store = useFactory
    ? StoreFactory.Create(args, dataPath)
    : useFile
        ? new FileOrderStore(dataPath)
        : new InMemoryOrderStore(SampleData.Orders());

// Додаткове завдання №1: обгортання у декоратор CachingOrderStore (якщо не через фабрику)
if (useCache && !useFactory)
{
    store = new CachingOrderStore(store);
}

var service = new OrderService(store);

Console.WriteLine($"Сховище: {store.GetType().Name}");
if (useFile)
{
    Console.WriteLine($"Шлях до файлу: {dataPath}");
}
Console.WriteLine();

// =========================================================================
// Основний сценарій (DoD п. 5: додати, змінити, знайти за id, показати список)
// =========================================================================
Console.WriteLine("--- 1. ОСНОВНИЙ СЦЕНАРІЙ (CreateOrder -> AddLine -> Confirm -> Find) ---");

var created = service.CreateOrder("Замовлення з консолі (Лаб 5)");
Console.WriteLine($"[Створено]   {created}");

service.AddLine(created.Id, "Кабель UTP cat6", 25m, 50);
service.AddLine(created.Id, "Конектор RJ-45", 5m, 20);
service.Confirm(created.Id);

var found = service.Find(created.Id);
Console.WriteLine($"[Знайдено]   {found}");
if (found is not null)
{
    foreach (var line in found.Lines)
    {
        Console.WriteLine($"   -> {line.Name,-22} | {line.Quantity,3} шт. x {line.Price,8:F2} грн");
    }
}
Console.WriteLine();

// =========================================================================
// Вивід усіх записів у сховищі
// =========================================================================
var allOrders = service.All();
Console.WriteLine($"--- 2. СПИСОК УСІХ ЗАМОВЛЕНЬ (Усього в сховищі: {allOrders.Count}) ---");
foreach (var o in allOrders)
{
    Console.WriteLine($" {o.Id,-10} | {o.Status,-9} | Позицій: {o.Lines.Count,2} | Сума: {o.Total,9:F2} грн | {o.Note}");
}
Console.WriteLine();

// =========================================================================
// Крок 6. Сценарій відмови (DoD п. 6: дубль id та відсутній id у try/catch)
// =========================================================================
Console.WriteLine("--- 3. СЦЕНАРІЇ ВІДМОВИ (Перехоплення винятків у Cli) ---");

// 6.1. Спроба змінити замовлення з неіснуючим ID
try
{
    service.AddLine("NON-EXISTENT-ID", "Тестовий товар", 100m, 1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[Помилка: відсутній ID] {ex.Message}");
}

// 6.2. Спроба створити замовлення з дублікатом ID
try
{
    service.CreateOrder("Спроба дубля", customId: created.Id);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[Помилка: дубль ID]     {ex.Message}");
}
Console.WriteLine();

// =========================================================================
// Додаткове завдання №2: Пошук з предикатом Func<Order, bool>
// =========================================================================
Console.WriteLine("--- 4. ДОДАТКОВЕ ЗАВДАННЯ №2: Пошук через Func<Order, bool> ---");
var confirmedBigOrders = service.Search(o => o.Status == OrderStatus.Confirmed && o.Total >= 10000m);
Console.WriteLine($"Підтверджені замовлення на суму від 10 000 грн (знайдено {confirmedBigOrders.Count}):");
foreach (var o in confirmedBigOrders)
{
    Console.WriteLine($" * {o.Id,-10} | Сума: {o.Total,9:F2} грн | {o.Note}");
}