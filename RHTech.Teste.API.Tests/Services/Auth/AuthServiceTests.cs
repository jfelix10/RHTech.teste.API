using Xunit;
using Moq;
using Bogus;
using System.Data;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using RHTech.Teste.API.Application.AuthServices;
using RHTech.Teste.API.Domain.Entities.Users;
using Dapper;

namespace RHTech.Teste.API.Tests.Services.Auth;

public class AuthServiceTests
{
    private readonly Mock<IDbConnection> _dbConnectionMock;
    private readonly Mock<IDbCommand> _dbCommandMock;
    private readonly IConfiguration _configuration;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _dbConnectionMock = new Mock<IDbConnection>();
        _dbCommandMock = new Mock<IDbCommand>();

        // Carrega o appsettings.json para obter a string de conexão
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        // Configura o comando mockado
        _dbConnectionMock.Setup(conn => conn.CreateCommand()).Returns(_dbCommandMock.Object);

        // Instancia o serviço com o mock de configuração e de conexão
        _authService = new AuthService(_configuration, _dbConnectionMock.Object);
    }

    private IDbConnection CriarConexao()
    {
        // Cria conexão real com o banco de testes
        return new Npgsql.NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
    }

    [Fact]
    public async Task RegisterUser_DeveRetornarToken_QuandoUsuarioRegistradoComSucesso()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real com o banco de teste
        var service = new AuthService(_configuration, connection);

        var faker = new Faker<UserRequest>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password());

        var userRequest = faker.Generate();

        // Act
        var resultado = await service.RegisterUser(userRequest);

        // Assert
        resultado.Should().NotBeNullOrEmpty(); // Verifica que o token foi gerado
    }

    [Fact]
    public async Task RegisterUser_DeveLancarExcecao_QuandoEmailJaCadastrado()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real com o banco de teste
        var service = new AuthService(_configuration, connection);

        var faker = new Faker<UserRequest>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password());

        var userRequest = faker.Generate();

        // Insere um usuário no banco para simular o cenário de e-mail já cadastrado
        await connection.ExecuteAsync(
            "INSERT INTO Users (Name, Email, PasswordHash, Role, FirstAccess) VALUES (@Name, @Email, @PasswordHash, 'User', false);",
            new
            {
                userRequest.Name,
                userRequest.Email,
                PasswordHash = service.HashPassword(userRequest.Password)
            }
        );

        // Act
        Func<Task> action = async () => await service.RegisterUser(userRequest);

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("Email já cadastrado!");
    }

    [Fact]
    public void Authenticate_DeveRetornarToken_QuandoCredenciaisCorretas()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real com o banco de teste
        var service = new AuthService(_configuration, connection);

        var senha = "qualquersenha";

        var faker = new Faker<User>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash, f => service.HashPassword(senha));

        var user = faker.Generate();

        // Insere um usuário no banco para autenticação
        connection.Execute(
            "INSERT INTO Users (Name, Email, PasswordHash, Role, FirstAccess) VALUES (@Name, @Email, @PasswordHash, 'Admin', true);",
            user
        );

        // Act
        var token = service.Authenticate(user.Email, senha);

        // Assert
        token.Should().NotBeNullOrEmpty(); // Verifica que o token foi gerado
    }

    [Fact]
    public void Authenticate_DeveRetornarVazio_QuandoUsuarioNaoEncontrado()
    {
        // Arrange
        using var connection = CriarConexao(); // Conexão real com o banco de teste
        var service = new AuthService(_configuration, connection);

        var emailInexistente = "usuario_inexistente@example.com";
        var senha = "senha123";

        // Act
        var token = service.Authenticate(emailInexistente, senha);

        // Assert
        token.Should().BeEmpty(); // Verifica que o resultado é vazio
    }
}
