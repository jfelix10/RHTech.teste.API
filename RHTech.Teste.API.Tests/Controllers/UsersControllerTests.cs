using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Application.Users.Queries;
using RHTech.Teste.API.Controllers.V1;
using RHTech.Teste.API.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RHTech.Teste.API.Tests.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsersController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetAll_DeveretornarUsuario_QuandoeleExiste()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Name = "Test User 1", Email = "test1@example.com", Role = "Admin", Ativo = true },
            new User { Name = "Test User 2", Email = "test2@example.com", Role = "User", Ativo = true }
        };
        var response = new BaseResponse<IEnumerable<User>> { Data = users, succcess = true };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Verifica que o retorno é 200 OK
        var resultValue = okResult.Value; // Pega o valor do retorno sem converter o tipo
        Assert.NotNull(resultValue);

        // Verifica as propriedades dinamicamente
        Assert.True((bool)(resultValue.GetType().GetProperty("success").GetValue(resultValue)));
    }
}

