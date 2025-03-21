using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Users;

namespace RHTech.Teste.API.Infra.Persistence.Repositories.Dapper
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmpresaRepository Empresas { get; }

        public IDadosAdmEmpresaRepository DadosAdmEmpresas { get; }

        public IEnderecoEmpresaRepository EnderecoEmpresas { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(IEmpresaRepository empresas, 
            IDadosAdmEmpresaRepository dadosAdmEmpresas,
            IEnderecoEmpresaRepository enderecoEmpresas,
            IUserRepository users)
        {
            Empresas = empresas;
            DadosAdmEmpresas = dadosAdmEmpresas;
            EnderecoEmpresas = enderecoEmpresas;
            Users = users;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
