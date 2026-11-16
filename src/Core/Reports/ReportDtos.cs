namespace Core.Reports;

// Record-DTO для рядка звіту (відповідає StockLine з методички)
public sealed record OrderReportLine(string Id, string Status, decimal Total, string? Note);

// Record-DTO для груп (відповідає GroupSummary)
public sealed record GroupSummary(string Key, int Count, decimal TotalAmount);

// Record-DTO для зведеного звіту (Join - додаткове завдання 1)
public sealed record OrderLineJoinDto(string OrderId, string ProductName, decimal Price, int Quantity);