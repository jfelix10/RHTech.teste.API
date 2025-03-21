using MediatR;

namespace RHTech.Teste.API.Application.Users.Commands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

        public UpdateUserCommand(Guid userId, string name, string role)
        {
            UserId = userId;
            Name = name;
            Role = role;
        }
    }
}
