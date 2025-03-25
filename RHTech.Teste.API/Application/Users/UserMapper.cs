using AutoMapper;
using RHTech.Teste.API.Application.Users.Queries;
using RHTech.Teste.API.Domain.Entities.Users;
using System.Diagnostics.CodeAnalysis;

namespace RHTech.Teste.API.Application.Users;

[ExcludeFromCodeCoverage]
public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, GetAllUsersQuery>().ReverseMap();
        CreateMap<IEnumerable<User>, IEnumerable<GetAllUsersQuery>>().ReverseMap();
    }
}
