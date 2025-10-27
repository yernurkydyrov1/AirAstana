namespace AirAstana.Domain.Entities;

public class Role { public int Id { get; set; } public string Code { get; set; } = default!; }
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;
}