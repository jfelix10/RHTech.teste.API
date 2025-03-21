using MediatR;

namespace RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand
{
    public class CreateEmpresaCompletaCommand : IRequest<bool>
    {
        public string NomeEmpresa { get; set; }
        public string CNPJ { get; set; }
        public int CodTipoEmpresa { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public string Celular { get; set; }
        public string NomeAdministradorEmpresa { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
    }
}
