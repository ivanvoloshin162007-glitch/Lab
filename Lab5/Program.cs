using TaxiFleet.Models;
using TaxiFleet.Presentation;
using TaxiFleet.Services;
using TaxiFleet.Testing;

namespace TaxiFleet;

public static class Program
{
    private const string ParkName = "Городское такси";
    private const int DefaultMinSpeed = 190;
    private const int DefaultMaxSpeed = 210;

    public static int Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int testResult = new FleetTester(Console.Out).RunAll();
        if (testResult != 0)
            return testResult;

        Console.WriteLine();

        if (!TryReadSpeedRange(args, out int minSpeed, out int maxSpeed))
        {
            Console.WriteLine("Использование: TaxiFleet [минимальная_скорость максимальная_скорость]");
            return 1;
        }

        try
        {
            string dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "cars.csv");

            var park = new TaxiPark(ParkName);
            park.AddCars(CarFileLoader.Load(dataPath));

            var report = new ConsoleReport(Console.Out);
            report.PrintFleet(park);
            report.PrintTotalCost(park);
            report.PrintSortedByFuel(park);
            report.PrintFoundBySpeed(park, minSpeed, maxSpeed);
            return 0;
        }
        catch (Exception ex) when (ex is IOException or FormatException
                                       or ArgumentException or InvalidOperationException)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return 2;
        }
    }

    private static bool TryReadSpeedRange(string[] args, out int minSpeed, out int maxSpeed)
    {
        minSpeed = DefaultMinSpeed;
        maxSpeed = DefaultMaxSpeed;

        if (args.Length == 0)
            return true;

        return args.Length == 2
               && int.TryParse(args[0], out minSpeed)
               && int.TryParse(args[1], out maxSpeed)
               && minSpeed <= maxSpeed;
    }
}