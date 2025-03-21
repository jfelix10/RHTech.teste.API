using Dapper;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using System.Data;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Empresas
{
    public class DadosAdmEmpresaRepository : IDadosAdmEmpresaRepository
    {
        private readonly IDbConnection _dbConnection;

        public DadosAdmEmpresaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> InsertAsync(DadosAdmEmpresa dadosAdmEmpresa)
        {
            var query = @"
                INSERT INTO DadosAdmEmpresa (
                    IdEmpresa, Celular, NomeAdministrador, CPF, Email, DataInclusao, DataAlteracao
                )
                VALUES (
                    @IdEmpresa, @Celular, @NomeAdministrador, @CPF, @Email, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
                );";

            return await _dbConnection.ExecuteScalarAsync<Guid>(query, dadosAdmEmpresa);
        }

        public async Task<DadosAdmEmpresa?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM DadosAdmEmpresa WHERE Id = @Id;";
            return await _dbConnection.QueryFirstOrDefaultAsync<DadosAdmEmpresa?>(query, new { Id = id });
        }

        public async Task<IEnumerable<DadosAdmEmpresa>> GetAllAsync()
        {
            var query = "SELECT * FROM DadosAdmEmpresa;";
            return await _dbConnection.QueryAsync<DadosAdmEmpresa>(query);
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

        public async Task<bool> UpdateAsync(DadosAdmEmpresa dadosAdmEmpresa)
        {
            var query = @"
                UPDATE DadosAdmEmpresa
                SET Celular = @Celular,
                    NomeAdministrador = @NomeAdministrador,
                    CPF = @CPF,
                    Email = @Email,
                    DataAlteracao = CURRENT_TIMESTAMP
                WHERE Id = @Id;";

            var rowsAffected = await _dbConnection.ExecuteAsync(query, dadosAdmEmpresa);
            return rowsAffected > 0;
        }
    }
}
