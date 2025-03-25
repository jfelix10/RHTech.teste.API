using MediatR;
using System.ComponentModel.DataAnnotations;

namespace RHTech.Teste.API.Application.Users.Commands;

public class UpdateUserCommand : IRequest<bool>
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MinLength(2, ErrorMessage = "O nome deve ter pelo menos 2 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O papel do usuário é obrigatório.")]
    [RegularExpression("^(Admin|User|Pending)$", ErrorMessage = "O papel do usuário deve ser 'Admin', 'User' ou 'Pending'.")]
    public string Role { get; set; }

    public UpdateUserCommand(Guid userId, string name, string role)
    {
        UserId = userId;
        Name = name;
        Role = role;
    }
}

