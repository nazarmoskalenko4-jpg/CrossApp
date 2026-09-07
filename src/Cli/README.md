# CrossApp
Наскрізний проєкт з крос-платформного програмування.

Предметна область: Замовлення.
Сутності: Customer, Product, Order, OrderLine.
Призначення: оформлення замовлень і підрахунок сум.

## Запуск
dotnet build
dotnet run --project src/Cli

## Середовище
.NET SDK 8.0.424
macOS
RID: osx-arm64

## Додаткове завдання

### Self-contained publish
win-x64: 77 MB  
osx-arm64: 83 MB

### JSON
Запуск програми у форматі JSON:
dotnet run --project src/Cli -- --json

### Docker
Програму було запущено в Linux-контейнері Docker.
Локальний OSDescription: Darwin 23.2.0 Darwin Kernel Version...
Docker OSDescription: Debian GNU/Linux 12 (bookworm)