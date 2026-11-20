namespace TaxiFleet.Models.Cars;

public class Hatchback : Car
{
    public Hatchback(CarSpecification specification) : base(specification)
    {
    }

    public override string BodyType => "Хэтчбек";

    public override string ServiceClass => "Эконом";
}
