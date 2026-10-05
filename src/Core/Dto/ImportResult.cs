namespace Core.Dto;

public sealed record ImportResult(
    IReadOnlyList<OrderDto> Orders,
    IReadOnlyList<CustomerDto> Customers,
    IReadOnlyList<string> Errors
);