using AutoMapper;
using MediatR;
using RHTech.Teste.API.Application.Commons;
using RHTech.Teste.API.Domain.Entities.Empresas;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper;

namespace RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand
{
    public class CreateEmpresaHandler : IRequestHandler<CreateEmpresaCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateEmpresaHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateEmpresaCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var empresa = _mapper.Map<Empresa>(command);
                response.Data = await _unitOfWork.Empresas.InsertAsync(empresa);
                if (response.Data)
                {
                    response.succcess = true;
                    response.Message = "Criado com sucesso!";
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
