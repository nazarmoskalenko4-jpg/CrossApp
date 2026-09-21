# CrossApp
Наскрізний проєкт з крос-платформного програмування.

Предметна область: Замовлення.
Сутності: Customer, Product, Order, OrderLine.
Призначення: оформлення замовлень і підрахунок сум.

## Структура Solution
CrossApp/
├── src/
│   ├── Core/      # Спільна логіка (Class Library)
│   └── Cli/       # Точка входу та вивід (Console App)

### Архітектура каталогу Core
Домовленість про структуру на весь семестр:
* Core/Dto/ — record-типи формату даних (DTO).
* Core/Domain/ — сутності з поведінкою та інваріантами.
* Core/Storage/ — реалізації сховищ.

## Запуск та збірка
```
dotnet build
dotnet run --project src/Cli 
```

### Публікація (Lab 02)
```
# Публікація (self-contained, з вбудованим середовищем .NET)
dotnet publish src/Cli -c Release -r osx-arm64 --self-contained true

# Публікація (framework-dependent, потребує встановленого .NET)
dotnet publish src/Cli -c Release -r osx-arm64 --self-contained false
```
### Порівняння режимів публікації
| Режим | Розмір publish | Потрібен runtime |
| :--- | :--- | :--- |
| osx-arm64 self-contained | ~83 МБ | ні |
| osx-arm64 framework-dependent | ~172 КБ | так (.NET 10) |

### Пояснення режимів публікації
***Self-contained (Автономний):** публікація містить не лише код застосунку, а й повну копію середовища виконання .NET Runtime для обраної платформи. Такий застосунок може працювати на комп'ютері користувача навіть без встановленого .NET, але займає значно більше місця.
***Framework-dependent (Залежний від фреймворку):** публікація містить лише код застосунку та його залежності, без середовища виконання. Каталог має мінімальний розмір, але вимагає, щоб на цільовій машині був попередньо встановлений .NET Runtime відповідної версії.

### Середовище
.NET SDK 10.0
macOS
RID: osx-arm64

### Додаткове завдання (Lab 01)
Self-contained publish (Lab 01)
win-x64: 77 MB
osx-arm64: 83 MB

### JSON
Запуск програми у форматі JSON:
```
dotnet run --project src/Cli -- --json
```

### Docker
Програму було запущено в Linux-контейнері Docker.
Локальний OSDescription: Darwin 23.2.0 Darwin Kernel Version...
Docker OSDescription: Debian GNU/Linux 12 (bookworm)
```
docker run --rm -v ${PWD}:/src -w /src mcr.microsoft.com/dotnet/sdk:10.0 dotnet run --project src/Cli
```
