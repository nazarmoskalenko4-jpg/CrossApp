using Core;
using Core.Reports;
using Core.Services;
using Core.Storage;
using Core.Domain;
using System.Diagnostics;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var store = new InMemoryOrderStore(SampleData.Orders());
var service = new OrderService(store);

// Демо відкладеного виконання (Крок 3)
var source = service.All().ToList();
var query = source.Where(o => o.Total > 15000); // Запит побудовано, але не виконано

Console.WriteLine($"[ДЕМО] Дорогих замовлень до змін: {query.Count()}");
var tmpOrder = Order.Create("TMP-001", "Тимчасове");
tmpOrder.AddLine("Тест", 20000, 1);
source.Add(tmpOrder);
Console.WriteLine($"[ДЕМО] Дорогих замовлень після змін: {query.Count()}"); // Число інше

var snapshot = query.ToList(); // Результат зафіксовано
source.Remove(tmpOrder);
Console.WriteLine($"[ДЕМО] У знімку лишилось: {snapshot.Count}\n"); // Не змінюється

// Маршрутизація команд (Крок 4)
var items = service.All();
var command = args.FirstOrDefault(a => !a.StartsWith("--")) ?? "report-top";

switch (command)
{
    case "report-top":
        Console.WriteLine("--- ТОП-5 ЗАМОВЛЕНЬ ЗА СУМОЮ ---");
        Print(ReportService.TopByTotal(items, 5));
        break;
    case "report-high":
        Console.WriteLine("--- ЗАМОВЛЕННЯ ВІД 15000 ГРН ---");
        Print(ReportService.ExpensiveOrders(items, 15000));
        break;
    case "report-groups":
        Console.WriteLine("--- ГРУПУВАННЯ ЗА СТАТУСОМ ---");
        foreach (var g in ReportService.ByStatus(items))
            Console.WriteLine($" {g.Key,-10} позицій: {g.Count,3}    сумарно: {g.TotalAmount,9:F2} грн");
        break;
    case "report-sum":
        Console.WriteLine("--- ПІДСУМКИ ---");
        var t = ReportService.Totals(items);
        Console.WriteLine($" позицій {t.Items}, всього {t.Total:F2} грн, середнє {t.Average:F2} грн, макс {t.Max:F2} грн");
        break;
    case "report-join":
        Console.WriteLine("--- ЗВЕДЕНИЙ ЗВІТ (JOIN) ---");
        foreach (var j in ReportService.JoinedOrderLines(items).Take(15)) // Виводимо 15 для компактності
            Console.WriteLine($" {j.OrderId,-10} {j.ProductName,-25} {j.Price,8:F2} грн x {j.Quantity}");
        break;
    case "report-perf":
        RunPerformanceTest();
        break;
    default:
        Console.WriteLine("Доступні команди: report-top | report-high | report-groups | report-sum | report-join | report-perf");
        break;
}

static void Print(IReadOnlyList<OrderReportLine> lines)
{
    foreach (var l in lines)
        Console.WriteLine($" {l.Id,-10} {l.Status,-10} {l.Total,9:F2} грн  | {l.Note}");
}

// Додаткове завдання 4: Вимірювання продуктивності
static void RunPerformanceTest()
{
    Console.WriteLine("--- ТЕСТ ПРОДУКТИВНОСТІ (100 000 записів) ---");
    var dummyOrders = new List<Order>(100000);
    for (int i = 0; i < 100000; i++)
    {
        var o = Order.Create($"O-{i}");
        o.AddLine("Item", 100, 1);
        dummyOrders.Add(o);
    }

    var sw = Stopwatch.StartNew();
    var res1 = dummyOrders.Where(x => x.Total > 50).OrderByDescending(x => x.Total).Take(10).ToList();
    sw.Stop();
    Console.WriteLine($"Без ToList в середині ланцюжка: {sw.ElapsedMilliseconds} мс");

    sw.Restart();
    var res2 = dummyOrders.Where(x => x.Total > 50).ToList().OrderByDescending(x => x.Total).Take(10).ToList();
    sw.Stop();
    Console.WriteLine($"З ToList в середині ланцюжка:   {sw.ElapsedMilliseconds} мс");
}