using RHTech.Teste.API.Domain.Entities.Empresas;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas
{
    public interface IDadosAdmEmpresaRepository
    {
        Task<Guid> InsertAsync(DadosAdmEmpresa dadosAdmEmpresa);
        Task<DadosAdmEmpresa?> GetByIdAsync(Guid id);
        Task<IEnumerable<DadosAdmEmpresa>> GetAllAsync();
        Task<IEnumerable<DadosAdmEmpresa>> GetFilteredAsync(string filter);
        Task<bool> UpdateAsync(DadosAdmEmpresa dadosAdmEmpresa);
    }
}
