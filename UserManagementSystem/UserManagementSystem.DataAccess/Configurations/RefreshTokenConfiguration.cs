using UserManagementSystem.DataAccess.Entities;

namespace UserManagementSystem.DataAccess.Configurations;

public class RefreshTokenConfiguration(string tableName) : SecureTokenConfiguration<RefreshTokenEntity>(tableName);