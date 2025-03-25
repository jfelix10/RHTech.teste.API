using Dapper;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using System.Data;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Empresas;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly IDbConnection _dbConnection;

    public EmpresaRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<bool> InsertEmpresaCompletaAsync(EmpresaCompleta empresaCompleta)
    {
        // Define os parâmetros para a procedure
        var parameters = new DynamicParameters();
        parameters.Add("p_nome_empresa", empresaCompleta.NomeEmpresa);
        parameters.Add("p_cnpj", empresaCompleta.CNPJ);
        parameters.Add("p_cod_tipo_empresa", empresaCompleta.CodTipoEmpresa);
        parameters.Add("p_cep", empresaCompleta.CEP);
        parameters.Add("p_logradouro", empresaCompleta.Logradouro);
        parameters.Add("p_endereco", empresaCompleta.Endereco);
        parameters.Add("p_bairro", empresaCompleta.Bairro);
        parameters.Add("p_estado", empresaCompleta.Estado);
        parameters.Add("p_cidade", empresaCompleta.Cidade);
        parameters.Add("p_complemento", empresaCompleta.Complemento);
        parameters.Add("p_celular", empresaCompleta.Celular);
        parameters.Add("p_nome_administrador", empresaCompleta.NomeAdministradorEmpresa);
        parameters.Add("p_cpf", empresaCompleta.CPF);
        parameters.Add("p_email", empresaCompleta.Email);



        try
        {
            // Executa a procedure e captura o ID da empresa criada
            var result = await _dbConnection.ExecuteScalarAsync<bool>(
                "SELECT criar_empresa_completa(" +
                ":p_nome_empresa, " +
                ":p_cnpj, " +
                ":p_cod_tipo_empresa, " +
                ":p_cep, " +
                ":p_logradouro, " +
                ":p_endereco, " +
                ":p_bairro, " +
                ":p_estado, " +
                ":p_cidade, " +
                ":p_complemento, " +
                ":p_celular, " +
                ":p_nome_administrador, " +
                ":p_cpf, " +
                ":p_email);",
                parameters
            );

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmpresaCompleta?> GetEmpresaCompletaByIdAsync(Guid id)
    {
        var query = @"
            SELECT 
                e.Id AS EmpresaId,
                e.NomeEmpresa,
                e.CNPJ,
                e.CodTipoEmpresa,
                ee.CEP,
                ee.Logradouro,
                ee.Endereco,
                ee.Bairro,
                ee.Estado,
                ee.Cidade,
                ee.Complemento,
                da.Celular,
                da.NomeAdministrador AS NomeAdministradorEmpresa,
                da.CPF,
                da.Email
            FROM 
                public.empresa e
            LEFT JOIN 
                public.enderecoempresa ee ON e.Id = ee.IdEmpresa
            LEFT JOIN 
                public.dadosadmempresa da ON e.Id = da.IdEmpresa
            WHERE 
                e.Id = @Id;";

        return await _dbConnection.QueryFirstOrDefaultAsync<EmpresaCompleta?>(query, new { Id = id });
    }
}
