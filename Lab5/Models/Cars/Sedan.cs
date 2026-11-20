namespace TaxiFleet.Models.Cars;

public class Sedan : Car
{
    public Sedan(CarSpecification specification) : base(specification)
    {
    }

    public override string BodyType => "Седан";

    public override string ServiceClass => "Комфорт";
}
