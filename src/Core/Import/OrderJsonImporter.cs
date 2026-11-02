namespace Core.Import;

using System.Text.Json;
using Core.Dto;

public static class OrderJsonImporter
{
    // Додано параметр Action<string>? onRowSkipped
    public static ImportResult Load(string path, Action<string>? onRowSkipped = null)
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
            string errorMessage = $"Помилка парсингу JSON: {ex.Message}";
            errors.Add(errorMessage);
            onRowSkipped?.Invoke(errorMessage); // Сповіщення про помилку імпорту
            
            return new ImportResult(new List<OrderDto>(), new List<CustomerDto>(), errors);
        }
    }
}