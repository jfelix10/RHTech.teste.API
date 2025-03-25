using FluentAssertions;
using Moq;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Tests.Handlers;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateUserCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_DeveAlterarUsuario_QuandoCommandoValido()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "Updated User", "Admin");
        _unitOfWorkMock.Setup(x => x.Users.UserExistsAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(true); // Usuário existe
        _unitOfWorkMock.Setup(x => x.Users.UpdateAsync(It.IsAny<UpdateUserCommand>()))
                       .ReturnsAsync(true); // Atualização com sucesso

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(x => x.Users.UpdateAsync(It.IsAny<UpdateUserCommand>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NaoDeveAlterarUsuario_QuandoEleNaoExiste()
    {
        // Arrange
        var command = new UpdateUserCommand(Guid.NewGuid(), "Updated User", "Admin");
        _unitOfWorkMock.Setup(x => x.Users.UserExistsAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(false); // Usuário não existe

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(x => x.Users.UpdateAsync(It.IsAny<UpdateUserCommand>()), Times.Never);
    }
}

