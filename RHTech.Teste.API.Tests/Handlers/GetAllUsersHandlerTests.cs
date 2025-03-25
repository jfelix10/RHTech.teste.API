using Moq;
using FluentAssertions;
using AutoMapper;
using RHTech.Teste.API.Application.Users.Queries;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using RHTech.Teste.API.Domain.Entities.Users;

namespace RHTech.Teste.API.Tests.Handlers;

public class GetAllUsersHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllUsersHandler _handler;

    public GetAllUsersHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllUsersHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarUsuarios_QuandoExistiremUsuarios()
    {
        // Arrange
        var usersFromDb = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = "User 1", Email = "user1@example.com", Role = "Admin" },
            new User { Id = Guid.NewGuid(), Name = "User 2", Email = "user2@example.com", Role = "User" }
        };

        var mappedUsers = new List<User>
        {
            new User { Id = usersFromDb[0].Id, Name = usersFromDb[0].Name, Email = usersFromDb[0].Email, Role = usersFromDb[0].Role },
            new User { Id = usersFromDb[1].Id, Name = usersFromDb[1].Name, Email = usersFromDb[1].Email, Role = usersFromDb[1].Role }
        };

        // Mockando o retorno do UnitOfWork
        _unitOfWorkMock
            .Setup(u => u.Users.GetAllAsync())
            .ReturnsAsync(usersFromDb);

        // Mockando o mapeamento dos usuários
        _mapperMock
            .Setup(m => m.Map<IEnumerable<User>>(usersFromDb))
            .Returns(mappedUsers);

        var query = new GetAllUsersQuery();

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().BeEquivalentTo(mappedUsers);
        resultado.succcess.Should().BeTrue();
        resultado.Message.Should().Be("Usuários encontrados com sucesso!");
        _unitOfWorkMock.Verify(u => u.Users.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<User>>(usersFromDb), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarMensagemNenhumUsuarioEncontrado_QuandoNaoExistiremUsuarios()
    {
        // Arrange
        _unitOfWorkMock
            .Setup(u => u.Users.GetAllAsync())
            .ReturnsAsync(new List<User>()); // Simula um retorno vazio

        var query = new GetAllUsersQuery();

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().BeNullOrEmpty(); // Sem usuários no retorno
        resultado.succcess.Should().BeFalse();
        resultado.Message.Should().Be("Nenhum usuário encontrado.");
        _unitOfWorkMock.Verify(u => u.Users.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<User>>(It.IsAny<IEnumerable<User>>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveRetornarErro_QuandoExcecaoForLancada()
    {
        // Arrange
        _unitOfWorkMock
            .Setup(u => u.Users.GetAllAsync())
            .ThrowsAsync(new Exception("Erro inesperado"));

        var query = new GetAllUsersQuery();

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().BeNull();
        resultado.succcess.Should().BeFalse();
        resultado.Message.Should().Be("Erro ao buscar usuários: Erro inesperado");
        _unitOfWorkMock.Verify(u => u.Users.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<User>>(It.IsAny<IEnumerable<User>>()), Times.Never);
    }
}
