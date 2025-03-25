using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users;
using System.Diagnostics.CodeAnalysis;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper;

[ExcludeFromCodeCoverage]
public class UnitOfWork : IUnitOfWork
{
    public IEmpresaRepository Empresas { get; }


    public IUserRepository Users { get; }

    public UnitOfWork(IEmpresaRepository empresas, 
        IUserRepository users)
    {
        Empresas = empresas;
        Users = users;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
