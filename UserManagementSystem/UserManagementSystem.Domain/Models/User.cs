namespace UserManagementSystem.Domain.Models;

public class User
{
    public Guid Id {get; set;}
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool ActivityStatus {get; set;}
    public bool VerificationStatus {get; set;}
    public DateTime LastActivityTime {get; set;}
    public DateTime RegistrationTime {get; set;}
    public string PasswordHash {get; set;} = null!;

    public User(string name, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        ActivityStatus = true;
        VerificationStatus = false;
        RegistrationTime = DateTime.UtcNow;
        LastActivityTime = RegistrationTime;
    }
}