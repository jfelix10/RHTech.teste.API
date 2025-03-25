using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RHTech.Teste.API.Application.Users.Commands;
using RHTech.Teste.API.Application.Users.Queries;
using System.Data;
using System.Xml.Linq;

namespace RHTech.Teste.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add-user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        try
        {
            var response = await _mediator.Send(command);
            if (response)
            {
                return Created(nameof(AddUser), new { success = true, message = "Usuário criado com sucesso!" });
            }

            return UnprocessableEntity(new { success = false, errors = new[] { "Erro ao criar o usuário." } });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = "Erro interno inesperado.",
                details = ex.Message
            });
        }
    }


    /// <summary>
    /// Obtém todos os usuários do sistema.
    /// </summary>
    /// <returns>Lista de usuários.</returns>
    /// <response code="200">Lista de usuários retornada com sucesso.</response>
    /// <response code="500">Erro interno inesperado.</response>
    [HttpGet("get-users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var response = await _mediator.Send(new GetAllUsersQuery());

            if (response.succcess)
            {
                return Ok(new { success = true, data = response.Data, message = response.Message });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = response.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = "Erro interno inesperado.",
                details = ex.Message
            });
        }
    }

    [HttpDelete("delete-user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteById(Guid id)
    {
        try
        {
            var response = await _mediator.Send(new DeleteUserCommand(id));
            if (response)
            {
                return Ok(new { success = true, message = "Usuário removido com sucesso!" });
            }

            return NotFound(new { success = false, message = "Usuário não encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = "Erro interno inesperado.",
                details = ex.Message
            });
        }
    }

    [HttpPatch("alter-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateById([FromBody] UpdateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
            });
        }

        try
        {
            var response = await _mediator.Send(command);
            if (response)
            {
                return Ok(new { success = true, message = "Usuário atualizado com sucesso!" });
            }

            return NotFound(new { success = false, message = "Usuário não encontrado." });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = "Erro interno inesperado.",
                details = ex.Message
            });
        }
    }

}
