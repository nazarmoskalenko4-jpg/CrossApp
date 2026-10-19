## Лабораторна робота 5: Сервісний шар, інтерфейси, два сховища, ручний DI

* **Домен:** Замовлення (`Order`, `OrderLine`, `OrderStatus`)
* **Інтерфейс сховища:** `Core.Abstractions.IOrderStore`
* **Реалізації сховища:**
  * `Core.Storage.InMemoryOrderStore` — зберігання у пам'яті (`Dictionary<string, Order>`) з початковими даними з `SampleData.Orders()`.
  * `Core.Storage.FileOrderStore` — збереження у JSON-файл (`data/orders.json`) через мапінг у `OrderDto` / `OrderLineDto`.
  * `Core.Storage.CachingOrderStore` *(додаткове завдання)* — декоратор кешування поверх будь-якої реалізації `IOrderStore`.
* **Сервіс:** `Core.Services.OrderService` (бізнес-операції `CreateOrder`, `AddLine`, `Confirm`, `All`, `Find`, `Search`).
* **Фабрика:** `Cli.StoreFactory` *(додаткове завдання)* — створення та декорування сховища на основі аргументів командного рядка.

### Запуск

# Режим у пам'яті (InMemoryOrderStore + SampleData, дані зникають після виходу)
```
dotnet run --project src/Cli
```

# Файловий режим (FileOrderStore, дані накопичуються у data/orders.json)
```
dotnet run --project src/Cli -- --file
```

# Із декоратором кешування та фабрикою (додаткові завдання)
```
dotnet run --project src/Cli -- --file --cache --factory
```