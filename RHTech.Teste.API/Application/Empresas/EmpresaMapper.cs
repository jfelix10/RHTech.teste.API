using AutoMapper;
using RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand;
using RHTech.Teste.API.Domain.Entities.Empresas;

namespace RHTech.Teste.API.Application.Empresas
{
    public class EmpresaMapper : Profile
    {
        public EmpresaMapper()
        {
            CreateMap<Empresa, CreateEmpresaCommand>().ReverseMap();
            CreateMap<EmpresaCompleta, CreateEmpresaCompletaCommand>().ReverseMap();
        }
    }
}
