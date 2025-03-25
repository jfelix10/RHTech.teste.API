using FluentAssertions;
using Moq;
using RHTech.Teste.API.Application.AuthServices;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Application.Authservices;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Tests.Handlers;

public class AddUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AddUserCommandHandler _handler;

    public AddUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _authServiceMock = new Mock<IAuthService>();
        _handler = new AddUserCommandHandler(_unitOfWorkMock.Object, _authServiceMock.Object);
    }

    [Fact]
    public async Task Handle_DeveAdicionarUsuario_QuandoComandoValido()
    {
        // Arrange
        var command = new AddUserCommand("Test User", "test@example.com", "Password123", Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Users.EmailExistsAsync(It.IsAny<string>()))
                       .ReturnsAsync(false); // Email não registrado
        _unitOfWorkMock.Setup(x => x.Users.AddAsync(It.IsAny<User>()))
                       .ReturnsAsync(true); // Sucesso na adição

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(x => x.Users.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NaoAdicionaUsuario_QuandoEmailexiste()
    {
        // Arrange
        var command = new AddUserCommand("Test User", "test@example.com", "Password123", Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Users.EmailExistsAsync(It.IsAny<string>()))
                       .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(x => x.Users.AddAsync(It.IsAny<User>()), Times.Never);
    }
}

