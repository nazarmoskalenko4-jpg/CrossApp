namespace Core.Dto;

public record OrderDto(
    string Id,
    string Name,
    decimal Price,
    string? Note = null
);