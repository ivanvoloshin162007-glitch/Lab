using System.Globalization;
using TaxiFleet.Models;
using TaxiFleet.Models.Cars;

namespace TaxiFleet.Presentation;

public class ConsoleReport
{
    private static readonly CultureInfo RuCulture = CultureInfo.GetCultureInfo("ru-RU");

    private readonly TextWriter _output;

    public ConsoleReport(TextWriter output)
    {
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    public void PrintFleet(TaxiPark park)
    {
        PrintTitle($"Таксопарк «{park.Name}»: {park.Cars.Count} авто");
        PrintCars(park.Cars);
    }

    public void PrintTotalCost(TaxiPark park)
    {
        PrintTitle("Стоимость автопарка");
        _output.WriteLine($"  Итого: {park.CalculateTotalCost().ToString("N0", RuCulture)} ₽");
    }

    public void PrintSortedByFuel(TaxiPark park)
    {
        PrintTitle("Сортировка по расходу топлива (по возрастанию)");
        PrintCars(park.SortByFuelConsumption());
    }

    public void PrintFoundBySpeed(TaxiPark park, int minSpeed, int maxSpeed)
    {
        PrintTitle($"Автомобили со скоростью {minSpeed}–{maxSpeed} км/ч");

        var found = park.FindBySpeedRange(minSpeed, maxSpeed).ToList();
        if (found.Count == 0)
        {
            _output.WriteLine("  Подходящих автомобилей нет.");
            return;
        }

        PrintCars(found);
    }

    private void PrintCars(IEnumerable<Car> cars)
    {
        _output.WriteLine(
            $"  {"Класс",-9}{"Кузов",-9}{"Автомобиль",-22}{"Год",-6}" +
            $"{"Цена, ₽",14}{"Км/ч",6}{"л/100км",9}{"Мест",6}  Особенности");

        foreach (var car in cars)
        {
            _output.WriteLine(
                $"  {car.ServiceClass,-9}{car.BodyType,-9}{car.FullName,-22}{car.ManufactureYear,-6}" +
                $"{car.Price.ToString("N0", RuCulture),14}{car.MaxSpeed,6}" +
                $"{car.FuelConsumption.ToString("F1", RuCulture),9}{car.PassengerSeats,6}  {car.GetExtraInfo()}");
        }
    }

    private void PrintTitle(string title)
    {
        _output.WriteLine();
        _output.WriteLine($"=== {title} ===");
    }
}
