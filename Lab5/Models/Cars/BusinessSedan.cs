namespace TaxiFleet.Models.Cars;

public class BusinessSedan : Sedan
{
    public BusinessSedan(CarSpecification specification, int climateZones)
        : base(specification)
    {
        if (climateZones < 1)
            throw new ArgumentOutOfRangeException(nameof(climateZones),
                "Число климатических зон должно быть не меньше 1.");

        ClimateZones = climateZones;
    }

    public int ClimateZones { get; }

    public override string ServiceClass => "Бизнес";

    public override string GetExtraInfo() => $"климат-зон: {ClimateZones}";
}
