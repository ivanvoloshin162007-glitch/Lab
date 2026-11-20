using TaxiFleet.Models;
using TaxiFleet.Models.Cars;
using TaxiFleet.Services;

namespace TaxiFleet.Testing;

public class FleetTester
{
    private readonly TextWriter _output;
    private int _passed;
    private int _failed;

    public FleetTester(TextWriter output) =>
        _output = output ?? throw new ArgumentNullException(nameof(output));

    public int RunAll()
    {
        _output.WriteLine("=== Тест TaxiFleet ===");

        var sedan = new Sedan(Spec(" Kia ", " K5 ", plate: " A1 "));
        var van = new Minivan(Spec(price: 3_000_000m, speed: 190, fuel: 7.5, seats: 7), 700);
        var park = new TaxiPark("Тест");
        park.AddCar(new Sedan(Spec(price: 1_000_000m, speed: 170, fuel: 6.0, plate: "B2")));
        park.AddCar(van);

        Assert("FullName и госномер", sedan.FullName == "Kia K5" && sedan.LicensePlate == "A1");
        Assert("Стоимость парка", park.CalculateTotalCost() == 4_000_000m);
        Assert("Сортировка по расходу", park.SortByFuelConsumption().First().LicensePlate == "B2");
        Assert("Фабрика авто", CarFactory.Create("Minivan", Spec(), 500) is Minivan);
        AssertThrows<ArgumentOutOfRangeException>("Ошибка цены <= 0", () => new Sedan(Spec(price: 0)));

        _output.WriteLine($"\nИтого: {_passed} OK, {_failed} FAIL.");
        return _failed == 0 ? 0 : 1;
    }

    private static CarSpecification Spec(
        string brand = "Brand", string model = "Model", int year = 2020,
        decimal price = 1_000_000m, int speed = 180, double fuel = 7.0,
        int seats = 4, string plate = "X000XX") =>
        new(brand, model, year, price, speed, fuel, seats, plate);

    private void Assert(string name, bool condition)
    {
        if (condition) _passed++; else _failed++;
        _output.WriteLine($"  [{(condition ? "OK" : "FAIL")}] {name}");
    }

    private void AssertThrows<T>(string name, Action action) where T : Exception
    {
        try { action(); Assert(name, false); }
        catch (Exception ex) { Assert(name, ex is T); }
    }
}