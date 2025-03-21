using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RHTech.Teste.API.Application.AuthServices;
using RHTech.Teste.API.Domain.Entities.Users;
using YamlDotNet.Core.Tokens;

namespace RHTech.Teste.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest registerRequest)
        {
            if (registerRequest.Password.Length < 5)
            {
                return BadRequest(new { Message = "A senha deve ter pelo menos 5 caracteres." });
            }

            try
            {
                var success = await _authService.RegisterUser(registerRequest);
                if (!string.IsNullOrEmpty(success))
                {
                    return Ok(new { Token = success });
                }

                return BadRequest(new { Message = "Erro ao registrar o usuário." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest loginRequest)
        {
            var token = _authService.Authenticate(loginRequest.Email, loginRequest.Password);
            
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Credenciais inválidas!", Token = "" });
            }

            if (token == "primeiro acesso")
                return Ok(new { Message = "primeiro acesso", Token = "" });

            return Ok(new { Message = "token gerado com sucesso", Token = token });
        }

        [MapToApiVersion("1.0")]
        [HttpPut("update-role")]
        public IActionResult UpdateRoleUser([FromBody] UserUpdateRequest loginRequest)
        {
            var token = _authService.UpdateRole(loginRequest);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "Credenciais inválidas!" });
            }

            return Ok(new { Token = token });
        }
    }
}
