using Xunit;
using Moq;
using FluentAssertions;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Empresas;
using Bogus;
using RHTech.Teste.API.Tests.Mocks;

namespace RHTech.Teste.API.Tests.Repositories;

public class EmpresaRepositoryTests
{
    private readonly IConfiguration _configuration;

    public EmpresaRepositoryTests()
    {
        // Configura o appsettings.Test.json para obter a string de conexão
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();
    }

    private IDbConnection CriarConexao()
    {
        // Cria uma conexão real com o banco PostgreSQL de testes
        return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    [Fact]
    public async Task InsertEmpresaCompletaAsync_DeveRetornarTrue_QuandoEmailExistir_QuandoInserirComSucesso()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real para teste
        var repository = new EmpresaRepository(connection);

        // recupera email de usuário que não tem id de empresa atrelado
        var email = await connection.QueryFirstOrDefaultAsync<string>(@"
            SELECT email FROM users WHERE idempresa is null limit 1;");

        var faker = new Faker<EmpresaCompleta>()
            .RuleFor(u => u.CNPJ, GerarCnpjFicticio.CnpjFicticio())
            .RuleFor(u => u.CPF, GerarCnpjFicticio.CpfFicticio())
            .RuleFor(u => u.NomeEmpresa, f => f.Name.FullName())
            .RuleFor(u => u.NomeAdministradorEmpresa, f => f.Name.FullName());

        var empresa = new EmpresaCompleta
        {
            NomeEmpresa = faker.Generate().NomeEmpresa,
            CNPJ = faker.Generate().CNPJ,
            CPF = faker.Generate().CPF,
            NomeAdministradorEmpresa = faker.Generate().NomeAdministradorEmpresa
        };

        var empresaCompleta = new EmpresaCompleta
        {
            NomeEmpresa = empresa.NomeEmpresa,
            CNPJ = empresa.CNPJ,
            CodTipoEmpresa = 1,
            CEP = "12345-678",
            Logradouro = "Rua",
            Endereco = "Rua Teste",
            Bairro = "Centro",
            Estado = "SP",
            Cidade = "São Paulo",
            Complemento = "Complemento Teste",
            Celular = "11912345678",
            NomeAdministradorEmpresa = empresa.NomeAdministradorEmpresa,
            CPF = empresa.CPF,
            Email = email!
        };

        // Act
        var resultado = await repository.InsertEmpresaCompletaAsync(empresaCompleta);

        // Assert
        resultado.Should().BeTrue(); // Verifica que a inserção foi bem-sucedida
    }

    [Fact]
    public async Task InsertEmpresaCompletaAsync_DeveLancarExcecao_QuandoParametrosInvalidos()
    {
        using var connection = CriarConexao(); // Conexão real para teste
        var repository = new EmpresaRepository(connection);

        var empresaCompleta = new EmpresaCompleta
        {
            NomeEmpresa = "Empresayy Teste",
            CNPJ = "99995678000190",
            CodTipoEmpresa = 1,
            CEP = "12345-678",
            Logradouro = "Rua",
            Endereco = "Rua Teste",
            Bairro = "Centro",
            Estado = "SP",
            Cidade = "São Paulo",
            Complemento = "Complemento Teste",
            Celular = "11912345678",
            NomeAdministradorEmpresa = "Administrador Teste",
            CPF = "99995678901",
            Email = "moreno@moreno.com"
        };

        // Act
        var resultado = await repository.InsertEmpresaCompletaAsync(empresaCompleta);

        // Assert
        resultado.Should().BeFalse(); // Verifica que a inserção foi bem-sucedida
    }

    [Fact]
    public async Task GetEmpresaCompletaByIdAsync_DeveRetornarEmpresa_QuandoIdExistir()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real para teste
        var repository = new EmpresaRepository(connection);

        var empresaId = Guid.Parse("ecd4261c-5193-4903-9ec1-284db55fcec1");

        // Act
        var resultado = await repository.GetEmpresaCompletaByIdAsync(empresaId);

        // Assert
        resultado.Should().NotBeNull(); // Verifica que a empresa foi encontrada
        resultado.NomeEmpresa.Should().Be("Empresayy Teste");
    }

    [Fact]
    public async Task GetEmpresaCompletaByIdAsync_DeveRetornarNull_QuandoIdNaoExistir()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real para teste
        var repository = new EmpresaRepository(connection);

        var empresaId = Guid.NewGuid(); // Gera um ID que não existe no banco

        // Act
        var resultado = await repository.GetEmpresaCompletaByIdAsync(empresaId);

        // Assert
        resultado.Should().BeNull(); // Verifica que nenhum dado foi encontrado
    }
}
