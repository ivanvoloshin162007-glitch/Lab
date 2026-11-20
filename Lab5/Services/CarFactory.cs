using TaxiFleet.Models.Cars;

namespace TaxiFleet.Services;

public static class CarFactory
{
    public static Car Create(string type, CarSpecification specification, int? extra)
    {
        return type switch
        {
            "Hatchback" => new Hatchback(specification),
            "Sedan" => new Sedan(specification),
            "BusinessSedan" => new BusinessSedan(specification,
                extra ?? throw new FormatException("Для BusinessSedan нужно число климатических зон.")),
            "Minivan" => new Minivan(specification,
                extra ?? throw new FormatException("Для Minivan нужен объём багажника.")),
            _ => throw new FormatException($"Неизвестный тип автомобиля: '{type}'.")
        };
    }
}
