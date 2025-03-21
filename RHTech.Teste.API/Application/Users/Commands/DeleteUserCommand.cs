using MediatR;

namespace RHTech.Teste.API.Application.Users.Commands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}
