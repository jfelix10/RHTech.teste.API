using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using MediatR;
using RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand;
using RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById;
using RHTech.Teste.API.Controllers.V1;

namespace RHTech.Teste.API.Tests.Controllers;

public class EmpresaControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly EmpresaController _controller;

    public EmpresaControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new EmpresaController(_mediatorMock.Object);
    }

    [Fact]
    public async Task Post_DeveRetornarOk_QuandoCriarEmpresaComSucesso()
    {
        // Arrange
        var command = new CreateEmpresaCompletaCommand
        {
            NomeEmpresa = "Empresa Teste",
            CNPJ = "12.345.678/0001-90",
            CodTipoEmpresa = 1,
            CEP = "12345-678",
            Logradouro = "Rua Teste",
            Endereco = "123",
            Bairro = "Centro",
            Estado = "SP",
            Cidade = "São Paulo",
            Complemento = "Complemento Teste",
            Celular = "(11) 91234-5678",
            NomeAdministradorEmpresa = "Administrador Teste",
            CPF = "12345678901",
            Email = "admin@empresa.com"
        };

        _mediatorMock
            .Setup(m => m.Send(command, default))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Post(command);

        // Assert
        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult?.Value.Should().BeEquivalentTo(new { response = true, message = "sucesso" });
    }

    [Fact]
    public async Task Get_DeveRetornarNotFound_QuandoEmpresaNaoEncontrada()
    {
        // Arrange
        var empresaId = Guid.NewGuid();
        var query = new GetEmpresaCompletaByIdQuery { Id = empresaId };

        _mediatorMock
            .Setup(m => m.Send(query, default))
            .ThrowsAsync(new KeyNotFoundException("Empresa não encontrada."));

        // Act
        var result = await _controller.Get(empresaId);

        // Assert
        var notFoundResult = result as NotFoundObjectResult;
        notFoundResult.Should().BeNull();
        notFoundResult?.Value.Should().BeEquivalentTo(new { message = "Empresa não encontrada." });
    }
}
