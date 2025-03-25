using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users;

namespace RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper
{
    public interface IUnitOfWork : IDisposable
    {
        IEmpresaRepository Empresas { get; }
        IUserRepository Users { get; }
    }
}
