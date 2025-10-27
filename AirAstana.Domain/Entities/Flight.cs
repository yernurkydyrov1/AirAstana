
namespace AirAstana.Domain.Entities;

public enum FlightStatus { InTime, Delayed, Cancelled }

public class Flight
{
    public int Id { get; set; }
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public FlightStatus Status { get; set; }
}