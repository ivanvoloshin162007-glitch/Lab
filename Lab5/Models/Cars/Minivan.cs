namespace TaxiFleet.Models.Cars;

public class Minivan : Car
{
    public Minivan(CarSpecification specification, int luggageVolumeLiters)
        : base(specification)
    {
        if (luggageVolumeLiters <= 0)
            throw new ArgumentOutOfRangeException(nameof(luggageVolumeLiters),
                "Объём багажника должен быть больше 0.");

        LuggageVolumeLiters = luggageVolumeLiters;
    }

    public int LuggageVolumeLiters { get; }

    public override string BodyType => "Минивэн";

    public override string ServiceClass => "Семейный";

    public override string GetExtraInfo() => $"багажник: {LuggageVolumeLiters} л";
}
