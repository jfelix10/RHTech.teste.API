using AutoMapper;
using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById;

public class GetEmpresaCompletaByIdHandler : IRequestHandler<GetEmpresaCompletaByIdQuery, BaseResponse<EmpresaCompleta>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetEmpresaCompletaByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<BaseResponse<EmpresaCompleta>> Handle(GetEmpresaCompletaByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<EmpresaCompleta>();
        try
        {
            var customer = await _unitOfWork.Empresas.GetEmpresaCompletaByIdAsync(request.Id); 
            if (customer is not null)
            {
                response.Data = _mapper.Map<EmpresaCompleta>(customer);
                response.succcess = true;
                response.Message = "Sucesso!";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }
        return response;
    }
}
