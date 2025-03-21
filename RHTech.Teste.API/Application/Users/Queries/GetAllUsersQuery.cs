using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Domain.Entities.Users;

namespace RHTech.Teste.API.Application.Users.Queries
{
    public class GetAllUsersQuery : IRequest<BaseResponse<IEnumerable<User>>>
    {
    }
}
