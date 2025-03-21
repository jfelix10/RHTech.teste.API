namespace RHTech.Teste.API.Domain.Entities.Empresas
{
    public class EnderecoEmpresa
    {
        public Guid Id { get; set; }
        public Guid IdEmpresa { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Complemento { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
