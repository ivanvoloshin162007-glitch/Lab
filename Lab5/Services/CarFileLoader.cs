using System.Globalization;
using TaxiFleet.Models.Cars;

namespace TaxiFleet.Services;

public static class CarFileLoader
{
    private const char Separator = ';';
    private const int FieldsCount = 10;

    public static List<Car> Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл с автомобилями не найден: {filePath}");

        var cars = new List<Car>();
        string[] lines = File.ReadAllLines(filePath, System.Text.Encoding.UTF8);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Length == 0 || line.StartsWith('#'))
                continue;

            try
            {
                cars.Add(ParseLine(line));
            }
            catch (Exception ex) when (ex is FormatException or ArgumentException)
            {
                throw new FormatException($"Строка {i + 1} файла {Path.GetFileName(filePath)}: {ex.Message}", ex);
            }
        }

        return cars;
    }

    private static Car ParseLine(string line)
    {
        string[] fields = line.Split(Separator);
        if (fields.Length < FieldsCount)
            throw new FormatException($"ожидается {FieldsCount} полей, найдено {fields.Length}.");

        var specification = new CarSpecification(
            Brand: fields[1],
            Model: fields[2],
            ManufactureYear: int.Parse(fields[3], CultureInfo.InvariantCulture),
            Price: decimal.Parse(fields[4], CultureInfo.InvariantCulture),
            MaxSpeed: int.Parse(fields[5], CultureInfo.InvariantCulture),
            FuelConsumption: double.Parse(fields[6], CultureInfo.InvariantCulture),
            PassengerSeats: int.Parse(fields[7], CultureInfo.InvariantCulture),
            LicensePlate: fields[8]);

        int? extra = string.IsNullOrWhiteSpace(fields[9])
            ? null
            : int.Parse(fields[9], CultureInfo.InvariantCulture);

        return CarFactory.Create(fields[0].Trim(), specification, extra);
    }
}
