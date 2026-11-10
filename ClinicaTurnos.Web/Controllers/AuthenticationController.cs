using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaTurnos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // No funcional en esta etapa: se implementa con JWT para promoción
            return StatusCode(StatusCodes.Status501NotImplemented, "Autenticación pendiente de implementación");
        }
    }
}
