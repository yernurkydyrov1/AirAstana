using AirAstana.Domain.Entities;

namespace AirAstana.Infrastructure.Services;

public static class SeedData
{
    public static void EnsureSeedData(Data.AppDbContext db)
    {
        if (db.Roles.Any()) return;

        var userRole = new Role { Code = "User" };
        var modRole = new Role { Code = "Moderator" };
        db.Roles.AddRange(userRole, modRole);
        db.SaveChanges();

        var u1 = new User { Username = "user", PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"), RoleId = userRole.Id };
        var u2 = new User { Username = "moderator", PasswordHash = BCrypt.Net.BCrypt.HashPassword("mod123"), RoleId = modRole.Id };
        db.Users.AddRange(u1, u2);

        db.Flights.AddRange(
            new Flight { Origin = "ALA", Destination = "AST", Departure = DateTimeOffset.UtcNow.AddHours(2), Arrival = DateTimeOffset.UtcNow.AddHours(4), Status = FlightStatus.InTime },
            new Flight { Origin = "ALA", Destination = "KUL", Departure = DateTimeOffset.UtcNow.AddHours(6), Arrival = DateTimeOffset.UtcNow.AddHours(10), Status = FlightStatus.Delayed },
            new Flight { Origin = "AST", Destination = "ALA", Departure = DateTimeOffset.UtcNow.AddHours(1), Arrival = DateTimeOffset.UtcNow.AddHours(3), Status = FlightStatus.InTime }
        );
        db.SaveChanges();
    }
}