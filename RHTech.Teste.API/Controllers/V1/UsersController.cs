using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RHTech.Teste.API.Application.Empresas.Queries.GetEmpresaCompletaById;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Application.Users.Queries;
using System.Data;
using System.Xml.Linq;

namespace RHTech.Teste.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("get-users")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = new GetAllUsersQuery();
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

        [MapToApiVersion("1.0")]
        [HttpPut("delete-user/{id}")]
        public async Task<IActionResult> DeleteById(Guid id)
        {
            try
            {
                var query = new DeleteUserCommand(id);
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

        [MapToApiVersion("1.0")]
        [HttpPatch("alter-user")]
        public async Task<IActionResult> UpdateById(UpdateUserCommand comand)
        {
            try
            {
                var query = new UpdateUserCommand(comand.UserId, comand.Name, comand.Role);
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
