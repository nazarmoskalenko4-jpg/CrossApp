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
            if (confirm)
                o.Confirm();
            list.Add(o);
        }

        AddSample("ORD-1001", "Доставка кур'єром (Київ)", true, ("Ноутбук Lenovo Legion", 48000m, 1), ("Миша Razer", 1800m, 1));
        AddSample("ORD-1002", "Самовивіз з магазину", true, ("Монітор Dell 27\"", 12500m, 2));
        AddSample("ORD-1003", "Очікує уточнення адреси", false, ("Механічна клавіатура Keychron", 4200m, 1), ("Килимок XXL", 650m, 1));
        AddSample("ORD-1004", "Корпоративне замовлення", true, ("Кабель UTP cat6 (бухта)", 2100m, 3), ("Конектори RJ-45 (уп.)", 180m, 10));
        AddSample("ORD-1005", "Подарункове пакування", false, ("Навушники Sony WH-1000XM5", 13999m, 1));
        AddSample("ORD-1006", "Термінова відправка НП", true, ("SSD Kingston 1TB NVMe", 3400m, 2), ("Термопаста Arctic MX-4", 250m, 1));
        AddSample("ORD-1007", "Оплата частинами", true, ("Відеокарта RTX 4070 Super", 28500m, 1));
        AddSample("ORD-1008", "Чернетка клієнта", false, ("Блок живлення 750W Gold", 4600m, 1));
        AddSample("ORD-1009", "Передзамовлення", false, ("Процесор Ryzen 7 7700", 11800m, 1), ("Кулер BeQuiet Pure Rock", 1950m, 1));
        AddSample("ORD-1010", "Постійний клієнт (-5%)", true, ("Оперативна пам'ять 32GB DDR5", 5200m, 2));
        AddSample("ORD-1011", "Комплектація офісу", true, ("Веб-камера Logitech C920", 3600m, 4), ("USB-хаб Type-C", 850m, 4));
        AddSample("ORD-1012", "Перевірити наявність на складі", false, ("Джерело безперебійного живлення", 6300m, 1));
        AddSample("ORD-1013", "Відправка у Львів", true, ("Роутер Mikrotik hAP ax2", 4100m, 1), ("Патч-корд 3м", 120m, 5));
        AddSample("ORD-1014", "Без дзвінка оператора", true, ("Флешка Samsung 128GB", 690m, 3));
        AddSample("ORD-1015", "Тестове замовлення", false, ("Крісло ергономічне", 9500m, 1));
        AddSample("ORD-1016", "Юридична особа (Б/Н)", true, ("Принтер лазерний HP", 8400m, 2), ("Папір офісний А4 (ящик)", 1100m, 4));

        return list;
    }
}