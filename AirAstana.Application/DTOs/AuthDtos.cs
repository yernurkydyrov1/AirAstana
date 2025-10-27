namespace AirAstana.Application.DTOs;

public record RegisterRequest(
    string Username,
    string Password,
    int? RoleId = null
);

public record LoginRequest(string Username, string Password);