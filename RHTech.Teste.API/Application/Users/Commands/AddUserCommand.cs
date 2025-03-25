using MediatR;
using System.ComponentModel.DataAnnotations;

namespace RHTech.Teste.API.Application.Users.Commands;

public class AddUserCommand : IRequest<bool>
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "O nome deve ter pelo menos 2 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "O ID da empresa é obrigatório.")]
    public Guid IdEmpresa { get; set; }

    public AddUserCommand(string name, string email, string password, Guid idEmpresa)
    {
        Name = name;
        Email = email;
        Password = password;
        IdEmpresa = idEmpresa;
    }
}
