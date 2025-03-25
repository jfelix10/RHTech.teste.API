using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using System.Data;

namespace RHTech.Teste.API.Application.Users.Commands;

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
            var userExists = await _unitOfWork.Users.UserExistsAsync(request.UserId);
            if (!userExists)
            {
                response.Message = "Usuário não encontrado ou já está desativado.";
                return false;
            }

            var rows = await _unitOfWork.Users.DeactivateAsync(request.UserId);
            if (rows)
            {
                response.succcess = true;
                response.Message = "Usuário desativado com sucesso!";
                return true;
            }

            response.Message = "Erro ao desativar o usuário.";
            return false;
        }
        catch (Exception ex)
        {
            response.Message = $"Erro ao desativar o usuário: {ex.Message}";
            return false;
        }
    }
}

