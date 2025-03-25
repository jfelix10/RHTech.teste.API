using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RHTech.Teste.API.Controllers.V1;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Application.Authservices;

namespace RHTech.Teste.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task Register_DeveRetornarOk_QuandoUsuarioRegistradoComSucesso()
    {
        // Arrange
        var registerRequest = new UserRequest
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "validpassword"
        };

        _authServiceMock
            .Setup(s => s.RegisterUser(registerRequest))
            .ReturnsAsync("token-gerado");

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.Value.Should().BeEquivalentTo(new { Token = "token-gerado" });
    }

    [Fact]
    public async Task Register_DeveRetornarBadRequest_QuandoSenhaInvalida()
    {
        // Arrange
        var registerRequest = new UserRequest
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "123" // Senha inválida
        };

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult?.Value.Should().BeEquivalentTo(new { Message = "A senha deve ter pelo menos 5 caracteres." });
    }

    [Fact]
    public async Task Register_DeveRetornarBadRequest_QuandoErroOcorrer()
    {
        // Arrange
        var registerRequest = new UserRequest
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = "validpassword"
        };

        _authServiceMock
            .Setup(s => s.RegisterUser(registerRequest))
            .ThrowsAsync(new Exception("Erro inesperado"));

        // Act
        var result = await _controller.Register(registerRequest);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult?.Value.Should().BeEquivalentTo(new { Message = "Erro inesperado" });
    }

    [Fact]
    public void Login_DeveRetornarOk_QuandoCredenciaisValidas()
    {
        // Arrange
        var loginRequest = new UserLoginRequest
        {
            Email = "test@example.com",
            Password = "validpassword"
        };

        _authServiceMock
            .Setup(s => s.Authenticate(loginRequest.Email, loginRequest.Password))
            .Returns("token-gerado");

        // Act
        var result = _controller.Login(loginRequest);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.Value.Should().BeEquivalentTo(new { Message = "token gerado com sucesso", Token = "token-gerado" });
    }

    [Fact]
    public void Login_DeveRetornarUnauthorized_QuandoCredenciaisInvalidas()
    {
        // Arrange
        var loginRequest = new UserLoginRequest
        {
            Email = "test@example.com",
            Password = "invalidpassword"
        };

        _authServiceMock
            .Setup(s => s.Authenticate(loginRequest.Email, loginRequest.Password))
            .Returns(string.Empty);

        // Act
        var result = _controller.Login(loginRequest);

        // Assert
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult?.Value.Should().BeEquivalentTo(new { Message = "Credenciais inválidas!", Token = "" });
    }

    [Fact]
    public void UpdateRoleUser_DeveRetornarOk_QuandoAtualizacaoBemSucedida()
    {
        // Arrange
        var userUpdateRequest = new UserUpdateRequest
        {
            Email = "test@example.com",
            Role = "Admin"
        };

        _authServiceMock
            .Setup(s => s.UpdateRole(userUpdateRequest))
            .Returns("token-atualizado");

        // Act
        var result = _controller.UpdateRoleUser(userUpdateRequest);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.Value.Should().BeEquivalentTo(new { Token = "token-atualizado" });
    }

    [Fact]
    public void UpdateRoleUser_DeveRetornarUnauthorized_QuandoAtualizacaoFalhar()
    {
        // Arrange
        var userUpdateRequest = new UserUpdateRequest
        {
            Email = "test@example.com",
            Role = "Admin"
        };

        _authServiceMock
            .Setup(s => s.UpdateRole(userUpdateRequest))
            .Returns(string.Empty);

        // Act
        var result = _controller.UpdateRoleUser(userUpdateRequest);

        // Assert
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult.Should().NotBeNull();
        unauthorizedResult?.Value.Should().BeEquivalentTo(new { Message = "Credenciais inválidas!" });
    }
}
