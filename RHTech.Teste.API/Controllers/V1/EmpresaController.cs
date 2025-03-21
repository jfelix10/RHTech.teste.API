using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using MediatR;
using RHTech.Teste.API.Domain.Entities;
using RHTech.Teste.API.Application.Empresas.Commands.CreateEmpresaCommand;
using RHTech.Teste.API.Interfaces.Infra.Persistence.Dapper.Empresas;
using RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById;

namespace RHTech.Teste.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmpresaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("criar-empresa")]
        public async Task<IActionResult> Post(CreateEmpresaCompletaCommand empresa)
        {
            var response = await _mediator.Send(empresa);
            return Ok(new { response, message = "sucesso" });
        }

        [MapToApiVersion("1.0")]
        [HttpGet("get-empresa/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var query = new GetEmpresaCompletaByIdQuery() { Id = id };
                var response = await _mediator.Send(query);
                return Ok(new { response });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}