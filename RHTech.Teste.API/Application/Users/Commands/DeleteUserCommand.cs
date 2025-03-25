using MediatR;
using System.ComponentModel.DataAnnotations;

namespace RHTech.Teste.API.Application.Users.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
    public Guid UserId { get; set; }

    public DeleteUserCommand(Guid userId)
    {
        UserId = userId;
    }
}
