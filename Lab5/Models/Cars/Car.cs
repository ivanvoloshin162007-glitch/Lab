namespace TaxiFleet.Models.Cars;

public abstract class Car : CarBase
{
    private const int MinManufactureYear = 1980;
    private const int MaxPassengerSeats = 8;

    protected Car(CarSpecification specification)
    {
        ArgumentNullException.ThrowIfNull(specification);
        Validate(specification);

        Brand = specification.Brand.Trim();
        Model = specification.Model.Trim();
        ManufactureYear = specification.ManufactureYear;
        Price = specification.Price;
        MaxSpeed = specification.MaxSpeed;
        FuelConsumption = specification.FuelConsumption;
        PassengerSeats = specification.PassengerSeats;
        LicensePlate = specification.LicensePlate.Trim();
    }

    private static void Validate(CarSpecification spec)
    {
        if (string.IsNullOrWhiteSpace(spec.Brand))
            throw new ArgumentException("Марка не может быть пустой.", nameof(spec));
        if (string.IsNullOrWhiteSpace(spec.Model))
            throw new ArgumentException("Модель не может быть пустой.", nameof(spec));
        if (string.IsNullOrWhiteSpace(spec.LicensePlate))
            throw new ArgumentException("Госномер не может быть пустым.", nameof(spec));
        if (spec.ManufactureYear < MinManufactureYear || spec.ManufactureYear > DateTime.Now.Year + 1)
            throw new ArgumentOutOfRangeException(nameof(spec), "Некорректный год выпуска.");
        if (spec.Price <= 0)
            throw new ArgumentOutOfRangeException(nameof(spec), "Стоимость должна быть больше 0.");
        if (spec.MaxSpeed <= 0)
            throw new ArgumentOutOfRangeException(nameof(spec), "Скорость должна быть больше 0.");
        if (spec.FuelConsumption <= 0)
            throw new ArgumentOutOfRangeException(nameof(spec), "Расход топлива должен быть больше 0.");
        if (spec.PassengerSeats < 1 || spec.PassengerSeats > MaxPassengerSeats)
            throw new ArgumentOutOfRangeException(nameof(spec),
                $"Число пассажирских мест должно быть от 1 до {MaxPassengerSeats}.");
    }
}
