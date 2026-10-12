using Core.Dto;

namespace Core.Domain;

public static class DomainImporter
{
    // Метод бере ImportResult з 3-го тижня і намагається створити доменні сутності
    public static (List<Order> ValidOrders, List<string> Errors) ProcessImport(ImportResult result)
    {
        var validOrders = new List<Order>();
        var errors = new List<string>();

        foreach (var dto in result.Orders)
        {
            try
            {
                // Тут спрацюють наші інваріанти з Order.Create та OrderLine.Create
                var order = Order.FromDto(dto);
                validOrders.Add(order);
            }
            catch (Exception ex)
            {
                // Якщо інваріант порушено, ловимо помилку і записуємо в список
                errors.Add($"[Відхилено {dto.Id}]: {ex.Message}");
            }
        }

        return (validOrders, errors);
    }
}