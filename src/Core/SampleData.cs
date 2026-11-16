using Core.Domain;

namespace Core;

public static class SampleData
{
    public static IReadOnlyList<Order> Orders()
    {
        var list = new List<Order>();

        void AddSample(string id, string note, bool confirm, params (string Name, decimal Price, int Qty)[] lines)
        {
            var o = Order.Create(id, note);
            foreach (var l in lines)
                o.AddLine(l.Name, l.Price, l.Qty);
            if (confirm) o.Confirm();
            list.Add(o);
        }

        AddSample("ORD-1001", "Доставка кур'єром (Київ)", true, ("Ноутбук Lenovo Legion", 48000m, 1), ("Миша Razer", 1800m, 1));
        AddSample("ORD-1002", "Самовивіз з магазину", true, ("Монітор Dell 27\"", 12500m, 2));
        AddSample("ORD-1003", "Очікує уточнення адреси", false, ("Механічна клавіатура", 4200m, 1), ("Килимок XXL", 650m, 1));
        AddSample("ORD-1004", "Корпоративне замовлення", true, ("Кабель UTP cat6", 2100m, 3), ("Конектори RJ-45", 180m, 10));
        AddSample("ORD-1005", "Подарункове пакування", false, ("Навушники Sony", 13999m, 1));
        AddSample("ORD-1006", "Термінова відправка НП", true, ("SSD Kingston 1TB", 3400m, 2), ("Термопаста", 250m, 1));
        AddSample("ORD-1007", "Оплата частинами", true, ("Відеокарта RTX 4070", 28500m, 1));
        AddSample("ORD-1008", "Чернетка клієнта", false, ("Блок живлення 750W", 4600m, 1));
        AddSample("ORD-1009", "Передзамовлення", false, ("Процесор Ryzen 7", 11800m, 1), ("Кулер BeQuiet", 1950m, 1));
        AddSample("ORD-1010", "Постійний клієнт (-5%)", true, ("ОЗП 32GB DDR5", 5200m, 2));
        AddSample("ORD-1011", "Комплектація офісу", true, ("Веб-камера Logitech", 3600m, 4), ("USB-хаб Type-C", 850m, 4));
        AddSample("ORD-1012", "Перевірка наявності", false, ("Джерело безперебійного живлення", 6300m, 1));
        AddSample("ORD-1013", "Відправка у Львів", true, ("Роутер Mikrotik", 4100m, 1), ("Патч-корд 3м", 120m, 5));
        AddSample("ORD-1014", "Без дзвінка", true, ("Флешка Samsung 128GB", 690m, 3));
        AddSample("ORD-1015", "Тестове замовлення", false, ("Крісло ергономічне", 9500m, 1));
        AddSample("ORD-1016", "Юридична особа (Б/Н)", true, ("Принтер HP", 8400m, 2), ("Папір А4", 1100m, 4));
        AddSample("ORD-1017", "Скасовано через підтримку", false, ("Мікрофон Rode", 5500m, 1));
        AddSample("ORD-1018", "Замовлення з регіону", true, ("Wi-Fi Адаптер", 450m, 2));
        AddSample("ORD-1019", "Повернення коштів", false, ("Планшет Apple iPad", 16000m, 1));
        AddSample("ORD-1020", "Безкоштовна доставка", true, ("Смарт-годинник", 8200m, 1));

        return list;
    }
}