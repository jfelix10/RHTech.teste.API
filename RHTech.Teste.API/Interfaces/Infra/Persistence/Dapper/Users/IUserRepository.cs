using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Domain.Entities.Users;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users
{
    public interface IUserRepository
    {
        Task<bool> InsertAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> DeleteAsync(Guid id);
        Task<bool> UpdateAsync(UpdateUserCommand request);
    }
}
