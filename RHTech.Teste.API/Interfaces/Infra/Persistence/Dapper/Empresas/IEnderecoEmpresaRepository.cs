using RHTech.Teste.API.Domain.Entities.Empresas;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas
{
    public interface IEnderecoEmpresaRepository
    {
        Task<Guid> InsertAsync(EnderecoEmpresa enderecoEmpresa);
        Task<EnderecoEmpresa?> GetByIdAsync(Guid id);
        Task<IEnumerable<EnderecoEmpresa>> GetAllAsync();
        Task<IEnumerable<EnderecoEmpresa>> GetFilteredAsync(string filter);
        Task<bool> UpdateAsync(EnderecoEmpresa enderecoEmpresa);
    }
}
