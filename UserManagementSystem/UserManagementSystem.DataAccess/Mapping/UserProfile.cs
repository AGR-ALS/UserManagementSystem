using AutoMapper;
using UserManagementSystem.DataAccess.Entities;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.DataAccess.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, User>();
        CreateMap<User, UserEntity>();
    }
}