using Dapper;
using MediatR;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users;
using System.Data;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Users;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> InsertAsync(User user)
    {
        var rowsAffected = await _dbConnection.ExecuteAsync(
            @"
                INSERT INTO public.users (
                    id, email, role, datainclusao, name, idempresa
                )
                VALUES (
                    gen_random_uuid(), @Email, @Role, CURRENT_TIMESTAMP, @Name, @IdEmpresa
                );", user
        );

        return rowsAffected > 0;
    }

    public async Task<bool> AddAsync(User user)
    {
        var rowsAffected = await _dbConnection.ExecuteAsync(
            @"
                INSERT INTO public.users (
                    id, email, passwordhash, role, datainclusao, name, idempresa
                )
                VALUES (
                    gen_random_uuid(), @Email, @PasswordHash, @Role, CURRENT_TIMESTAMP, @Name, @IdEmpresa
                );", user
        );

        return rowsAffected > 0;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbConnection.QueryAsync<User>("SELECT * FROM public.users WHERE ativo = true;");
    }

    public async Task<bool> UpdateAsync(UpdateUserCommand request)
    {
        var rowsAffected = await _dbConnection.ExecuteAsync(
            @"
                UPDATE public.users
                SET name = @Name, role = @Role
                WHERE id = @Id;",
            new
            {
                Name = request.Name,
                Role = request.Role,
                Id = request.UserId
            }
        );

        return rowsAffected > 0;
    }

    public async Task<bool> DeactivateAsync(Guid userId)
    {
        var rowsAffected = await _dbConnection.ExecuteAsync(
            "UPDATE public.users SET ativo = false WHERE id = @UserId;",
            new { UserId = userId }
        );

        return rowsAffected > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var exists = await _dbConnection.QueryFirstOrDefaultAsync<bool>(
            "SELECT COUNT(1) FROM public.users WHERE email = @Email;",
            new { Email = email }
        );

        return exists;
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        var exists = await _dbConnection.QueryFirstOrDefaultAsync<bool>(
            "SELECT COUNT(1) FROM public.users WHERE id = @UserId;",
            new { UserId = userId }
        );

        return exists;
    }        
}
