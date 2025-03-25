using AutoMapper;
using RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand;
using RHTech.Teste.API.Domain.Entities.Empresas;
using System.Diagnostics.CodeAnalysis;

namespace RHTech.Teste.API.Application.Empresas;

[ExcludeFromCodeCoverage]
public class EmpresaMapper : Profile
{
    public EmpresaMapper()
    {
        CreateMap<EmpresaCompleta, CreateEmpresaCompletaCommand>().ReverseMap();
    }
}
