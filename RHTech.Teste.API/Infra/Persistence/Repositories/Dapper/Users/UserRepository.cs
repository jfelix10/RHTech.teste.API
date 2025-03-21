using Dapper;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> InsertAsync(User user)
        {
            var query = @"
                INSERT INTO public.users (
                    id, email, role, datainclusao, name, idempresa
                )
                VALUES (
                    gen_random_uuid(), @Email, @Role, CURRENT_TIMESTAMP, @Name, @IdEmpresa
                );";

            // Retorna o ID do usuário criado
            return await _dbConnection.ExecuteScalarAsync<bool>(query, user);
        }

        public async Task<DadosAdmEmpresa?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM DadosAdmEmpresa WHERE Id = @Id;";
            return await _dbConnection.QueryFirstOrDefaultAsync<DadosAdmEmpresa?>(query, new { Id = id });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var query = "SELECT * FROM users WHERE ativo = true;";
            var ret = await _dbConnection.QueryAsync<User>(query);
            return ret;
        }

        public async Task<IEnumerable<DadosAdmEmpresa>> GetFilteredAsync(string filter)
        {
            var query = @"
                SELECT * FROM DadosAdmEmpresa
                WHERE LOWER(NomeAdministrador) LIKE LOWER(@Filter)
                   OR CPF LIKE @Filter
                   OR LOWER(Email) LIKE LOWER(@Filter);";

            return await _dbConnection.QueryAsync<DadosAdmEmpresa>(query, new { Filter = $"%{filter}%" });
        }

        public async Task<bool> UpdateAsync(UpdateUserCommand request)
        {
            try
            {
                var sqlStr = $"UPDATE users SET name = @name, Role = @role WHERE id = '${request.UserId}'";
                var rowsAffected = await _dbConnection.ExecuteAsync(sqlStr);
                //var id = request.UserId;
                //var rowsAffected = await _dbConnection.ExecuteAsync(
                //@"UPDATE users SET name = @name, Role = @role WHERE id = @id;", new { id }
                //);
                return rowsAffected > 0;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var rowsAffected = await _dbConnection.ExecuteAsync(
            "UPDATE users SET ativo = false WHERE Id = @Id;", new { id }
            );
            return rowsAffected > 0;
        }
    }
}
