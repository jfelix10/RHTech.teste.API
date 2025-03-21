using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace RHTech.Teste.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        [MapToApiVersion("2.0")]
        [HttpGet("get-teste")]
        public IActionResult Get()
        {
            return Ok(new { Message = "versão 2", NewField = "propriedade extra" });
        }
    }
}
