## Лабораторна робота 6: Доменні події, делегати Action/Func, лямбди

* **Домен:** Замовлення (`Order`, `OrderLine`, `OrderStatus`).
* **Події (Events):**
  * Тип події та аргументи: `Core.Events.OrderChangeKind`, `Core.Events.OrderChangedEventArgs`.
  * Оголошення події: `event EventHandler<OrderChangedEventArgs>? Changed` у класі `Core.Services.OrderService`.
  * Підписники (у `Cli`): локальний метод (вивід у консоль та запис у файл `logs/app.log`) і лямбда-вираз із замиканням (лічильник).
* **Делегати Action/Func:**
  * `Func<Order, bool>`: використання предикату для пошуку/фільтрації в `OrderService.Search`.
  * `Action<string>` *(додаткове завдання)*: передача сповіщень про помилки парсингу рядків в `OrderCsvImporter` та `OrderJsonImporter`.
* **INotificationSink *(додаткове завдання)*:** Інтерфейс обробника сповіщень `Core.Abstractions.INotificationSink` та дві його реалізації у Cli (`ConsoleSink`, `FileSink`), які передаються в сервіс через конструктор (ручний DI).
* **Сервіс:** `Core.Services.OrderService` (генерація подій відбувається строго після успішних змін стану сутності методами `CreateOrder`, `AddLine`, `Confirm`).

### Запуск

# Базовий режим (демонстрація роботи подій, лямбда-виразів, відписки та сценаріїв відмови)
```
dotnet run --project src/Cli
```

# Файловий режим (збереження даних у data/orders.json + події)
```
dotnet run --project src/Cli -- --file
```

# Із декоратором кешування та фабрикою
```
dotnet run --project src/Cli -- --file --cache --factory
```