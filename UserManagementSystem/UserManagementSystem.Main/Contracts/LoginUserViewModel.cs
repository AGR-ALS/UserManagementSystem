namespace UserManagementSystem.Main.Contracts;

public class LoginUserViewModel
{
    public string? Username { get; set; } = null;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsRegistration;
}