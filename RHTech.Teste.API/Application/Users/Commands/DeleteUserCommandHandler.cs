using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using System.Data;

namespace RHTech.Teste.API.Application.Users.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var rows = await _unitOfWork.Users.DeleteAsync(request.UserId);
                if (rows)
                {
                    response.succcess = true;
                    response.Message = "Sucesso!";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response.succcess;
        }
    }
}
