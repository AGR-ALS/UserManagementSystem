using UserManagementSystem.DataAccess.Entities;

namespace UserManagementSystem.DataAccess.Configurations;

public class AccountConfirmationTokenConfiguration(string tableName)
    : SecureTokenConfiguration<AccountConfirmationTokenEntity>(tableName);