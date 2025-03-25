using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Domain.Entities.Users;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users
{
    public interface IUserRepository
    {
        Task<bool> InsertAsync(User user);
        Task<bool> AddAsync(User user);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> UpdateAsync(UpdateUserCommand request);
        Task<bool> DeactivateAsync(Guid userId);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserExistsAsync(Guid userId);
    }
}
