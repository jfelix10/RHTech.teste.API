using Xunit;
using Moq;
using Bogus;
using System.Threading;
using FluentAssertions;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using AutoMapper;
using RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Tests.Mocks;

namespace RHTech.Teste.API.Tests.Handlers;

public class CreateEmpresaCompletaHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateEmpresaCompletaHandler _handler;

    public CreateEmpresaCompletaHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateEmpresaCompletaHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarTrue_QuandoEmpresaCriadaComSucesso()
    {
        // Arrange
        var faker = new Faker<CreateEmpresaCompletaCommand>()
            .RuleFor(e => e.NomeEmpresa, f => f.Company.CompanyName())
            .RuleFor(e => e.CNPJ, f => GerarCnpjFicticio.CnpjFicticio())
            .RuleFor(e => e.CodTipoEmpresa, f => f.Random.Int(1, 10));

        var command = faker.Generate();

        var empresaCompleta = new EmpresaCompleta
        {
            NomeEmpresa = command.NomeEmpresa,
            CNPJ = command.CNPJ,
            CodTipoEmpresa = command.CodTipoEmpresa
        };

        // Mockando o mapeamento de CreateEmpresaCompletaCommand para EmpresaCompleta
        _mapperMock.Setup(m => m.Map<EmpresaCompleta>(It.IsAny<CreateEmpresaCompletaCommand>()))
                   .Returns(empresaCompleta);

        // Mockando a inserção da empresa completa no UnitOfWork
        _unitOfWorkMock.Setup(u => u.Empresas.InsertEmpresaCompletaAsync(It.IsAny<EmpresaCompleta>()))
                       .ReturnsAsync(true);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeTrue(); // Verifica que o resultado é "true"
        _unitOfWorkMock.Verify(u => u.Empresas.InsertEmpresaCompletaAsync(It.IsAny<EmpresaCompleta>()), Times.Once);
        _mapperMock.Verify(m => m.Map<EmpresaCompleta>(It.IsAny<CreateEmpresaCompletaCommand>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalse_QuandoEmpresaNaoCriada()
    {
        // Arrange
        var faker = new Faker<CreateEmpresaCompletaCommand>()
            .RuleFor(e => e.NomeEmpresa, f => f.Company.CompanyName())
            .RuleFor(e => e.CNPJ, f => GerarCnpjFicticio.CnpjFicticio())
            .RuleFor(e => e.CodTipoEmpresa, f => f.Random.Int(1, 10));

        var command = faker.Generate();

        var empresaCompleta = new EmpresaCompleta
        {
            NomeEmpresa = command.NomeEmpresa,
            CNPJ = command.CNPJ,
            CodTipoEmpresa = command.CodTipoEmpresa
        };

        // Mockando o mapeamento de CreateEmpresaCompletaCommand para EmpresaCompleta
        _mapperMock.Setup(m => m.Map<EmpresaCompleta>(It.IsAny<CreateEmpresaCompletaCommand>()))
                   .Returns(empresaCompleta);

        // Mockando a inserção da empresa completa no UnitOfWork para falha
        _unitOfWorkMock.Setup(u => u.Empresas.InsertEmpresaCompletaAsync(It.IsAny<EmpresaCompleta>()))
                       .ReturnsAsync(false);

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse(); // Verifica que o resultado é "false"
        _unitOfWorkMock.Verify(u => u.Empresas.InsertEmpresaCompletaAsync(It.IsAny<EmpresaCompleta>()), Times.Once);
        _mapperMock.Verify(m => m.Map<EmpresaCompleta>(It.IsAny<CreateEmpresaCompletaCommand>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarFalse_QuandoLancarExcecao()
    {
        // Arrange
        var faker = new Faker<CreateEmpresaCompletaCommand>()
            .RuleFor(e => e.NomeEmpresa, f => f.Company.CompanyName())
            .RuleFor(e => e.CNPJ, f => GerarCnpjFicticio.CnpjFicticio())
            .RuleFor(e => e.CodTipoEmpresa, f => f.Random.Int(1, 10));

        var command = faker.Generate();

        // Mockando o mapeamento para lançar exceção
        _mapperMock.Setup(m => m.Map<EmpresaCompleta>(It.IsAny<CreateEmpresaCompletaCommand>()))
                   .Throws(new Exception("Erro de mapeamento"));

        // Act
        var resultado = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultado.Should().BeFalse(); // Verifica que o resultado é "false" devido à exceção
        _unitOfWorkMock.Verify(u => u.Empresas.InsertEmpresaCompletaAsync(It.IsAny<EmpresaCompleta>()), Times.Never);
    }
}
