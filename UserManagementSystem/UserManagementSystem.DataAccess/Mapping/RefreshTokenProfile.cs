using AutoMapper;
using UserManagementSystem.DataAccess.Entities;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.DataAccess.Mapping;

public class RefreshTokenProfile : Profile
{
    public RefreshTokenProfile()
    {
        CreateMap<RefreshToken, RefreshTokenEntity>();
        CreateMap<RefreshTokenEntity, RefreshToken>();
    }
}