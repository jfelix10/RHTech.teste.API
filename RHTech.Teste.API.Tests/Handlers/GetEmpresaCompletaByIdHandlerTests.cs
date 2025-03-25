using Xunit;
using Moq;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Tests.Handlers;

public class GetEmpresaCompletaByIdHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetEmpresaCompletaByIdHandler _handler;

    public GetEmpresaCompletaByIdHandlerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetEmpresaCompletaByIdHandler(_unitOfWorkMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarBaseResponseComDados_QuandoEmpresaExiste()
    {
        // Arrange
        var empresaId = Guid.NewGuid();

        var empresaDb = new EmpresaCompleta
        {
            NomeEmpresa = "Empresa Teste",
            CNPJ = "12.345.678/0001-90",
            CodTipoEmpresa = 5
        };

        var empresaCompleta = new EmpresaCompleta
        {
            NomeEmpresa = empresaDb.NomeEmpresa,
            CNPJ = empresaDb.CNPJ,
            CodTipoEmpresa = empresaDb.CodTipoEmpresa
        };

        // Mockando o retorno do UnitOfWork
        _unitOfWorkMock
            .Setup(u => u.Empresas.GetEmpresaCompletaByIdAsync(empresaId))
            .ReturnsAsync(empresaDb);

        // Mockando o mapeamento
        _mapperMock
            .Setup(m => m.Map<EmpresaCompleta>(empresaDb))
            .Returns(empresaCompleta);

        var query = new GetEmpresaCompletaByIdQuery { Id = empresaId };

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().BeEquivalentTo(empresaCompleta);
        resultado.succcess.Should().BeTrue();
        resultado.Message.Should().Be("Sucesso!");
        _unitOfWorkMock.Verify(u => u.Empresas.GetEmpresaCompletaByIdAsync(empresaId), Times.Once);
        _mapperMock.Verify(m => m.Map<EmpresaCompleta>(empresaDb), Times.Once);
    }

    [Fact]
    public async Task Handle_DeveRetornarBaseResponseSemDados_QuandoEmpresaNaoExiste()
    {
        // Arrange
        var empresaId = Guid.NewGuid();

        // Mockando retorno nulo do UnitOfWork
        _unitOfWorkMock
            .Setup(u => u.Empresas.GetEmpresaCompletaByIdAsync(empresaId))
            .ReturnsAsync((EmpresaCompleta)null);

        var query = new GetEmpresaCompletaByIdQuery { Id = empresaId };

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().BeNull();
        resultado.succcess.Should().BeFalse();
        resultado.Message.Should().BeNull();
        _unitOfWorkMock.Verify(u => u.Empresas.GetEmpresaCompletaByIdAsync(empresaId), Times.Once);
        _mapperMock.Verify(m => m.Map<EmpresaCompleta>(It.IsAny<EmpresaCompleta>()), Times.Never);
    }

    [Fact]
    public async Task Handle_DeveRetornarBaseResponseComErro_QuandoExcecaoForLancada()
    {
        // Arrange
        var empresaId = Guid.NewGuid();

        // Mockando o UnitOfWork para lançar uma exceção
        _unitOfWorkMock
            .Setup(u => u.Empresas.GetEmpresaCompletaByIdAsync(empresaId))
            .ThrowsAsync(new Exception("Erro inesperado"));

        var query = new GetEmpresaCompletaByIdQuery { Id = empresaId };

        // Act
        var resultado = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Data.Should().BeNull();
        resultado.succcess.Should().BeFalse();
        resultado.Message.Should().Be("Erro inesperado");
        _unitOfWorkMock.Verify(u => u.Empresas.GetEmpresaCompletaByIdAsync(empresaId), Times.Once);
        _mapperMock.Verify(m => m.Map<EmpresaCompleta>(It.IsAny<EmpresaCompleta>()), Times.Never);
    }
}
