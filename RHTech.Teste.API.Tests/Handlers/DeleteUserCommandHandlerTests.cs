using FluentAssertions;
using Moq;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHTech.Teste.API.Tests.Handlers;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new DeleteUserCommandHandler(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_DeveDesativarUsuario_QuandoeleExiste()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Users.UserExistsAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(true); // Usuário existe
        _unitOfWorkMock.Setup(x => x.Users.DeactivateAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(true); // Desativação com sucesso

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _unitOfWorkMock.Verify(x => x.Users.DeactivateAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NaoDeveDesativarUsuario_QuandoeleNaoExiste()
    {
        // Arrange
        var command = new DeleteUserCommand(Guid.NewGuid());
        _unitOfWorkMock.Setup(x => x.Users.UserExistsAsync(It.IsAny<Guid>()))
                       .ReturnsAsync(false); // Usuário não existe

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        _unitOfWorkMock.Verify(x => x.Users.DeactivateAsync(It.IsAny<Guid>()), Times.Never);
    }
}

