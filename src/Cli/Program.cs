using Core;
using Cli;
using Core.Abstractions;
using Core.Events;
using Core.Services;
using Core.Storage;
using Core.Domain; // Якщо є SampleData

Console.OutputEncoding = System.Text.Encoding.UTF8;

// =========================================================================
// Налаштування логів та Sinks
// =========================================================================
var logPath = Path.Combine(AppContext.BaseDirectory, "logs", "app.log");
Directory.CreateDirectory(Path.GetDirectoryName(logPath)!); // Створення каталогу для логів[cite: 4, 8, 9]

// Ручний DI: передаємо Sinks у сервіс (Додаткове завдання №1)
var sinks = new INotificationSink[] { new ConsoleSink(), new FileSink(logPath) };

string dataPath = Path.Combine(AppContext.BaseDirectory, "data", "orders.json");
IOrderStore store = new InMemoryOrderStore(SampleData.Orders()); // Для тесту (використовуй свою фабрику)

var service = new OrderService(store, sinks); // Cli передає Sinks сервісу через конструктор

// =========================================================================
// Крок 3-4. Підписка на події
// =========================================================================

// Підписник 1: Локальний метод (записує в консоль і файл)[cite: 4]
void OnOrderChanged(object? sender, OrderChangedEventArgs e)
{
    var line = $"{e.At:HH:mm:ss} {e.Kind,-14} {e.Id} | Рядків: {e.LinesCount,2} | Сума: {e.Total,9:F2} грн";
    Console.WriteLine($"[ПОДІЯ-Event] {line}");
    File.AppendAllText(logPath, line + Environment.NewLine);
}

service.Changed += OnOrderChanged;

// Підписник 2: Лямбда із замиканням для підрахунку подій[cite: 4, 6]
int changes = 0;
service.Changed += (_, _) => changes++;

// =========================================================================
// Сценарій тестування
// =========================================================================
Console.WriteLine("--- 1. ОСНОВНИЙ СЦЕНАРІЙ (Підняття подій та Sinks) ---");
var created = service.CreateOrder("Замовлення з консолі");
service.AddLine(created.Id, "Кабель UTP", 25m, 50);
service.Confirm(created.Id);

Console.WriteLine($"\nПодій отримано (лямбда-лічильник): {changes}"); // Очікується 3

// Крок 5: Відписка локального методу і перевірка тихих операцій[cite: 4, 8]
Console.WriteLine("\n--- 2. ВІДПИСКА (service.Changed -= OnOrderChanged) ---");
service.Changed -= OnOrderChanged;

var newOrder = service.CreateOrder("Тихе замовлення"); // OnOrderChanged не спрацює, але Sinks та лічильник - так
Console.WriteLine($"Подій отримано після відписки: {changes}"); // Очікується 4

// Крок 6: Сценарій відмови у try/catch[cite: 4]
Console.WriteLine("\n--- 3. СЦЕНАРІЇ ВІДМОВИ (Подій не має бути) ---");
try
{
    service.AddLine("NON-EXISTENT-ID", "Тестовий товар", 100m, 1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"[Помилка] {ex.Message}");
}
// Лічильник не змінився, бо інваріант кинув виняток до OnChanged[cite: 3, 9]
Console.WriteLine($"Подій отримано після помилки: {changes}"); // Залишається 4

Console.WriteLine("\n--- 4. ПОШУК ЧЕРЕЗ Func (service.Search) ---");
var bigOrders = service.Search(o => o.Total > 0);
Console.WriteLine($"Знайдено непорожніх замовлень: {bigOrders.Count}");