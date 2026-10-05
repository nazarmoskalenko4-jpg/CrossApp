namespace Core.Import;

using System.Text.Json;
using Core.Dto;

public static class OrderJsonImporter
{
    public static ImportResult Load(string path)
    {
        var errors = new List<string>();
        try
        {
            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var orders = JsonSerializer.Deserialize<List<OrderDto>>(json, options) ?? [];
            
            // Передаємо замовлення, порожній список клієнтів і помилки
            return new ImportResult(orders, new List<CustomerDto>(), errors);
        }
        catch (Exception ex)
        {
            errors.Add($"Помилка парсингу JSON: {ex.Message}");
            return new ImportResult(new List<OrderDto>(), new List<CustomerDto>(), errors);
        }
    }
}