using Core.Domain;

namespace Core.Reports;

public static class ReportService
{
    // 1) Фільтр + сортування + проєкція (Замовлення від певної суми)
    public static IReadOnlyList<OrderReportLine> ExpensiveOrders(IEnumerable<Order> source, decimal threshold) =>
        source.Where(o => o.Total >= threshold)
              .OrderBy(o => o.Total).ThenBy(o => o.Id, StringComparer.Ordinal)
              .Select(o => new OrderReportLine(o.Id, o.Status.ToString(), o.Total, o.Note))
              .ToList();

    // 2) Топ-N зі стабільним порядком (основне завдання)
    public static IReadOnlyList<OrderReportLine> TopByTotal(IEnumerable<Order> source, int n) =>
        source.OrderByDescending(o => o.Total).ThenBy(o => o.Id, StringComparer.Ordinal)
              .Take(n)
              .Select(o => new OrderReportLine(o.Id, o.Status.ToString(), o.Total, o.Note))
              .ToList();

    // 2.1) Топ-N з використанням власного методу розширення (Додаткове завдання 2)
    public static IReadOnlyList<OrderReportLine> TopByTotalCustom(IEnumerable<Order> source, int n) =>
        source.TopBy(o => o.Total, n) 
              .Select(o => new OrderReportLine(o.Id, o.Status.ToString(), o.Total, o.Note))
              .ToList();

    // 3) Групування з підрахунком і сумою (Method Syntax)
    public static IReadOnlyList<GroupSummary> ByStatus(IEnumerable<Order> source) =>
        source.GroupBy(o => o.Status.ToString(), StringComparer.OrdinalIgnoreCase)
              .Select(g => new GroupSummary(g.Key, g.Count(), g.Sum(o => o.Total)))
              .OrderByDescending(s => s.TotalAmount)
              .ToList();

    // 3.1) Групування (Query Syntax - Додаткове завдання 3)
    public static IReadOnlyList<GroupSummary> ByStatusQuerySyntax(IEnumerable<Order> source)
    {
        var query = from o in source
                    group o by o.Status.ToString() into g
                    orderby g.Sum(x => x.Total) descending
                    select new GroupSummary(g.Key, g.Count(), g.Sum(x => x.Total));
        
        return query.ToList();
    }

    // 4) Агрегати з обробкою порожньої послідовності (Кортеж)
    public static (int Items, decimal Total, decimal Average, decimal Max) Totals(IEnumerable<Order> source)
    {
        var list = source.ToList();
        return list.Count == 0
            ? (0, 0m, 0m, 0m)
            : (list.Count, list.Sum(o => o.Total), list.Average(o => o.Total), list.Max(o => o.Total));
    }

    // 5) З'єднання колекцій Join (Додаткове завдання 1)
    public static IReadOnlyList<OrderLineJoinDto> JoinedOrderLines(IEnumerable<Order> source)
    {
        // Імітуємо зовнішню плоску таблицю рядків для демонстрації Join
        var allLines = source.SelectMany(o => o.Lines.Select(l => new { OrderId = o.Id, Line = l })).ToList();

        return source.Join(
            allLines,
            order => order.Id,
            flatLine => flatLine.OrderId,
            (order, flatLine) => new OrderLineJoinDto(order.Id, flatLine.Line.Name, flatLine.Line.Price, flatLine.Line.Quantity)
        ).ToList();
    }
}