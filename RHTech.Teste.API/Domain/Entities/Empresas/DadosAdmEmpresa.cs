namespace RHTech.Teste.API.Domain.Entities.Empresas
{
    public class DadosAdmEmpresa
    {
        public Guid Id { get; set; }
        public Guid IdEmpresa { get; set; }
        public string Celular { get; set; }
        public string NomeAdministrador { get; set; }
        public string CPF { get; set; }
        public string Email { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
