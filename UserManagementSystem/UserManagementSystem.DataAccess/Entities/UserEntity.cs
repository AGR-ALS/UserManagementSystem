namespace UserManagementSystem.DataAccess.Entities;

public class UserEntity
{
    public Guid Id {get; set;}
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool ActivityStatus {get; set;}
    public bool VerificationStatus {get; set;}
    public DateTime LastActivityTime {get; set;}
    public DateTime RegistrationTime {get; set;}
    public string PasswordHash {get; set;} = null!;
}