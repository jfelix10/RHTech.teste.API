namespace RHTech.Teste.API.Domain.Entities.Empresas;

public class Empresa
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NomeEmpresa { get; set; }
    public string CNPJ { get; set; }
    public int CodTipoEmpresa { get; set; }
    public DateTime DataInclusao { get; set; }
    public DateTime DataAlteracao { get; set; }
}
