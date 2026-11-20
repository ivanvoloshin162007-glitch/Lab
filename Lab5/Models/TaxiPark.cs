using TaxiFleet.Models.Cars;

namespace TaxiFleet.Models;

public class TaxiPark
{
    private readonly List<Car> _cars = new();

    public TaxiPark(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название таксопарка не может быть пустым.", nameof(name));

        Name = name.Trim();
    }

    public string Name { get; }

    public IReadOnlyList<Car> Cars => _cars;

    public void AddCar(Car car)
    {
        ArgumentNullException.ThrowIfNull(car);

        bool plateExists = _cars.Any(c =>
            string.Equals(c.LicensePlate, car.LicensePlate, StringComparison.OrdinalIgnoreCase));
        if (plateExists)
            throw new InvalidOperationException(
                $"Автомобиль с госномером {car.LicensePlate} уже есть в парке.");

        _cars.Add(car);
    }

    public void AddCars(IEnumerable<Car> cars)
    {
        ArgumentNullException.ThrowIfNull(cars);

        foreach (var car in cars)
            AddCar(car);
    }

    public decimal CalculateTotalCost() => _cars.Sum(c => c.Price);

    public IEnumerable<Car> SortByFuelConsumption() =>
        _cars.OrderBy(c => c.FuelConsumption);

    public IEnumerable<Car> FindBySpeedRange(int minSpeed, int maxSpeed)
    {
        if (minSpeed > maxSpeed)
            throw new ArgumentException("Нижняя граница скорости больше верхней.");

        return _cars.Where(c => c.MaxSpeed >= minSpeed && c.MaxSpeed <= maxSpeed);
    }
}
