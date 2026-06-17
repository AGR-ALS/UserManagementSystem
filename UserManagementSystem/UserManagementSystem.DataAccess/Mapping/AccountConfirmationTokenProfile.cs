using AutoMapper;
using UserManagementSystem.DataAccess.Entities;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.DataAccess.Mapping;

public class AccountConfirmationTokenProfile : Profile
{
    public AccountConfirmationTokenProfile()
    {
        CreateMap<AccountConfirmationToken, AccountConfirmationTokenEntity>();
        CreateMap<AccountConfirmationTokenEntity, AccountConfirmationToken>();
    }
}