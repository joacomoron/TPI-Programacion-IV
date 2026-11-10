using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Auth;
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

        // POST api/Authentication/login → devuelve el JWT para usar en el header Authorization: Bearer {token}
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            return Ok(await _authenticationService.LoginAsync(request));
        }
    }
}
