using ClinicaTurnos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaTurnos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly ITurnoService _turnoService;

        public TurnoController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }
    }
}
