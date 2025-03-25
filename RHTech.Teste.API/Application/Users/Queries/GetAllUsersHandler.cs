using AutoMapper;
using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Domain.Entities.Users;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Application.Users.Queries;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, BaseResponse<IEnumerable<User>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<BaseResponse<IEnumerable<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<User>>();
        try
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            if (users is not null && users.Any())
            {
                response.Data = _mapper.Map<IEnumerable<User>>(users);
                response.succcess = true;
                response.Message = "Usuários encontrados com sucesso!";
            }
            else
            {
                response.succcess = false;
                response.Message = "Nenhum usuário encontrado.";
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Erro ao buscar usuários: {ex.Message}";
        }
        return response;
    }
}

