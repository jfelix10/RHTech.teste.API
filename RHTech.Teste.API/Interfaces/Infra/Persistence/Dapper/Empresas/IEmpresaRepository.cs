using MediatR;
using RHTech.Teste.API.Domain.Entities.Empresas;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas
{
    public interface IEmpresaRepository : IRequest<Guid>
    {
        Task<bool> InsertEmpresaCompletaAsync(EmpresaCompleta empresa);
        Task<EmpresaCompleta?> GetEmpresaCompletaByIdAsync(Guid id);
    }
}
