using Core.Domain;

namespace Core.Dto;

public record OrderLineDto(
    string Name,
    decimal Price,
    int Quantity
);

public record OrderDto(
    string Id,
    string Name,
    decimal Price,
    string? Note = null,
    OrderStatus Status = OrderStatus.Draft,
    IReadOnlyList<OrderLineDto>? Lines = null
);