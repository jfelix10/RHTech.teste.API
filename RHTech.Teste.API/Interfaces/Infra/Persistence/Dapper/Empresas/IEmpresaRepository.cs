using MediatR;
using RHTech.Teste.API.Domain.Entities.Empresas;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas
{
    public interface IEmpresaRepository : IRequest<Guid>
    {
        Task<bool> InsertEmpresaCompletaAsync(EmpresaCompleta empresa);
        Task<bool> InsertAsync(Empresa empresa);
        Task<Empresa?> GetByIdAsync(Guid id);
        Task<EmpresaCompleta?> GetEmpresaCompletaByIdAsync(Guid id);
        Task<IEnumerable<Empresa>> GetAllAsync();
        Task<IEnumerable<Empresa>> GetFilteredAsync(string filter);
        Task<bool> UpdateAsync(Empresa empresa);
    }
}
