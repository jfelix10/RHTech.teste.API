using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Application.Authservices;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Application.Users.Commands;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public AddUserCommandHandler(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            // Verifica se o e-mail já está registrado
            var emailExists = await _unitOfWork.Users.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                response.Message = "O e-mail já está registrado.";
                return false; // Retorna direto
            }

            // Inserindo o novo usuário no banco de dados
            var rows = await _unitOfWork.Users.AddAsync(new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = "Admin",
                FirstAccess = true,
                IdEmpresa = request.IdEmpresa
            });

            if (rows)
            {
                response.succcess = true;
                response.Message = "Usuário adicionado com sucesso!";
                return response.succcess; // Retorna apenas no contexto do try
            }

            response.Message = "Erro ao adicionar o usuário.";
            return false;
        }
        catch (Exception ex)
        {
            response.Message = $"Erro ao adicionar o usuário: {ex.Message}";
            return false; // Retorna false explicitamente em caso de exceção
        }
    }

    private string HashPassword(string password)
    {
        return _authService.HashPassword(password);
    }
}
