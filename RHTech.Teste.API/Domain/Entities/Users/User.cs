namespace RHTech.Teste.API.Domain.Entities.Users;

public class User : UserRequest
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "Pending";
    public Guid IdEmpresa { get; set; }
    public bool FirstAccess { get; set; }
    public bool Ativo { get; set; }
}
