
using Xunit;
using Moq;
using System.Data;
using FluentAssertions;
using RHTech.Teste.API.Infra.Persistence.Repositories.Dapper.Users;
using RHTech.Teste.API.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Bogus;
using Dapper;

namespace RHTech.Teste.API.Tests.Repositories;
public class UserRepositoryTests
{
    private readonly Mock<IDbConnection> _dbConnectionMock;
    private readonly Mock<IDbCommand> _dbCommandMock;
    private readonly UserRepository _repository;
    private readonly IConfiguration _configuration;

    public UserRepositoryTests()
    {
        _dbConnectionMock = new Mock<IDbConnection>();
        _dbCommandMock = new Mock<IDbCommand>();


        // Carrega o appsettings.json para obter a string de conexão
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        // Configuramos o comando para retornar o mock de IDbCommand
        _dbConnectionMock.Setup(conn => conn.CreateCommand()).Returns(_dbCommandMock.Object);

        _repository = new UserRepository(_dbConnectionMock.Object);
    }

    private IDbConnection CriarConexao()
    {
        // Usa a string de conexão do appsettings.json
        return new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    [Fact]
    public async Task AddAsync_DeveRetornarTrue_QuandoInsercaoBemSucedida()
    {
        // Arrange
        using var connection = CriarConexao();
        var repository = new UserRepository(connection);

        // recupera email de usuário que não tem id de empresa atrelado
        var id = await connection.QueryFirstOrDefaultAsync<Guid>(@"
            SELECT ep.id FROM empresa as ep
            inner join users us
            on ep.id != us.idempresa
            limit 1;");

        var faker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email());

        var user = new User
        {
            Email = faker.Generate().Email,
            PasswordHash = "hashedpassword",
            Role = "Admin",
            Name = faker.Generate().Name,
            IdEmpresa = id
        };

        // Act
        var resultado = await repository.AddAsync(user);

        // Assert
        resultado.Should().BeTrue(); // Espera que o resultado seja "true"
    }

    [Fact]
    public async Task AddAsync_DeveRetornarFalse_QuandoInsercaoFalhar_Por_IdEmpresa_Inexistir()
    {
        // Arrange
        using var connection = CriarConexao();
        var repository = new UserRepository(connection);


        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            Role = "User",
            Name = "Test User",
            IdEmpresa = Guid.NewGuid()
        };


        // Act
        Func<Task> action = async () => await repository.AddAsync(user);

        // Assert
        await action.Should().ThrowAsync<Exception>(); // Espera uma exceção devido à restrição de chave estrangeira
    }

    [Fact]
    public async Task AddAsync_DeveRetornarFalse_QuandoInsercaoFalhar_Por_Email_Existir()
    {
        // Arrange
        using var connection = CriarConexao();
        var repository = new UserRepository(connection);


        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = "hashedpassword",
            Role = "User",
            Name = "Test User",
            IdEmpresa = Guid.Parse("a570140c-a78a-468f-8794-03874f631f12")
        };


        // Act
        Func<Task> action = async () => await repository.AddAsync(user);

        // Assert
        await action.Should().ThrowAsync<Exception>(); // Espera uma exceção devido à restrição de chave estrangeira
    }
}

