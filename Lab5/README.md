# TaxiFleet (ЛР №6, вариант 5 «Таксопарк»)

Запуск: `dotnet run` или `dotnet run -- 180 220` (диапазон скорости для поиска, км/ч).

Структура:
- `Models/Cars` — иерархия автомобилей: `Car` (абстрактный) → `Hatchback`, `Sedan` → `BusinessSedan`, `Minivan`
- `Models/TaxiPark.cs` — таксопарк: стоимость, сортировка по расходу, поиск по скорости
- `Services` — чтение `Data/cars.csv` и фабрика автомобилей
- `Presentation` — вывод в консоль
- `Data/cars.csv` — параметры автомобилей

Формат `Data/cars.csv` (разделитель `;`): `Type;Brand;Model;Year;Price;MaxSpeed;FuelConsumption;Seats;LicensePlate;Extra`
- Type: Hatchback, Sedan, BusinessSedan, Minivan
- Price — руб., MaxSpeed — км/ч, FuelConsumption — л/100 км, Seats — пассажирские места
- Extra: для BusinessSedan — число климатических зон, для Minivan — объём багажника (л), иначе пусто
