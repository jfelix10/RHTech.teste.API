using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;
using System.Data;

namespace RHTech.Teste.API.Application.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            // Verifica se o usuário existe
            var userExists = await _unitOfWork.Users.UserExistsAsync(request.UserId);
            if (!userExists)
            {
                response.Message = "Usuário não encontrado.";
                return false;
            }

            // Atualiza o usuário
            var rows = await _unitOfWork.Users.UpdateAsync(request);
            if (rows)
            {
                response.succcess = true;
                response.Message = "Usuário atualizado com sucesso!";
                return true;
            }

            response.Message = "Falha ao atualizar o usuário.";
            return false;
        }
        catch (Exception ex)
        {
            response.Message = $"Erro ao atualizar o usuário: {ex.Message}";
            return false;
        }
    }
}

