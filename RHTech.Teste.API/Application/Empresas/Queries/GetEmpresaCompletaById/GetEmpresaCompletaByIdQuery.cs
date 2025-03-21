using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Domain.Entities.Empresas;

namespace RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById
{
    public class GetEmpresaCompletaByIdQuery : IRequest<BaseResponse<EmpresaCompleta>>
    {
        public Guid Id { get; set; }
    }
}
