using Dapper;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using System.Data;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Empresas
{
    public class EnderecoEmpresaRepository : IEnderecoEmpresaRepository
    {
        private readonly IDbConnection _dbConnection;

        public EnderecoEmpresaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> InsertAsync(EnderecoEmpresa enderecoEmpresa)
        {
            var query = @"
                INSERT INTO EnderecoEmpresa (
                    IdEmpresa, CEP, Logradouro, Endereco, Bairro, Estado, Cidade, Complemento, DataInclusao, DataAlteracao
                )
                VALUES (
                    @IdEmpresa, @CEP, @Logradouro, @Endereco, @Bairro, @Estado, @Cidade, @Complemento, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP
                );";

            return await _dbConnection.ExecuteScalarAsync<Guid>(query, enderecoEmpresa);
        }

        public async Task<EnderecoEmpresa?> GetByIdAsync(Guid id)
        {
            var query = "SELECT * FROM EnderecoEmpresa WHERE Id = @Id;";
            return await _dbConnection.QueryFirstOrDefaultAsync<EnderecoEmpresa?>(query, new { Id = id });
        }

        public async Task<IEnumerable<EnderecoEmpresa>> GetAllAsync()
        {
            var query = "SELECT * FROM EnderecoEmpresa;";
            return await _dbConnection.QueryAsync<EnderecoEmpresa>(query);
        }

        public async Task<IEnumerable<EnderecoEmpresa>> GetFilteredAsync(string filter)
        {
            var query = @"
                SELECT * FROM EnderecoEmpresa
                WHERE LOWER(Endereco) LIKE LOWER(@Filter)
                   OR LOWER(Bairro) LIKE LOWER(@Filter)
                   OR LOWER(Cidade) LIKE LOWER(@Filter);";

            return await _dbConnection.QueryAsync<EnderecoEmpresa>(query, new { Filter = $"%{filter}%" });
        }

        public async Task<bool> UpdateAsync(EnderecoEmpresa enderecoEmpresa)
        {
            var query = @"
                UPDATE EnderecoEmpresa
                SET CEP = @CEP,
                    Logradouro = @Logradouro,
                    Endereco = @Endereco,
                    Bairro = @Bairro,
                    Estado = @Estado,
                    Cidade = @Cidade,
                    Complemento = @Complemento,
                    DataAlteracao = CURRENT_TIMESTAMP
                WHERE Id = @Id;";

            var rowsAffected = await _dbConnection.ExecuteAsync(query, enderecoEmpresa);
            return rowsAffected > 0;
        }
    }
}
