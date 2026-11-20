namespace TaxiFleet.Models.Cars;

public record CarSpecification(
    string Brand,
    string Model,
    int ManufactureYear,
    decimal Price,
    int MaxSpeed,
    double FuelConsumption,
    int PassengerSeats,
    string LicensePlate);
