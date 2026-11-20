namespace TaxiFleet.Models.Cars;

public abstract class CarBase
{
    public string Brand { get; protected init; } = string.Empty;

    public string Model { get; protected init; } = string.Empty;

    public int ManufactureYear { get; protected init; }

    public decimal Price { get; protected init; }

    public int MaxSpeed { get; protected init; }

    public double FuelConsumption { get; protected init; }

    public int PassengerSeats { get; protected init; }

    public string LicensePlate { get; protected init; } = string.Empty;

    public abstract string BodyType { get; }

    public abstract string ServiceClass { get; }

    public string FullName => $"{Brand} {Model}";

    public virtual string GetExtraInfo() => string.Empty;

    public override string ToString() =>
        $"{FullName} ({ManufactureYear}), {LicensePlate}";
}
