using AutoMapper;
using RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand;
using RHTech.Teste.API.Application.Users.Queries;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Domain.Entities.Users;

namespace RHTech.Teste.API.Application.Users
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, GetAllUsersQuery>().ReverseMap();
            CreateMap<IEnumerable<User>, IEnumerable<GetAllUsersQuery>>().ReverseMap();
        }
    }
}
