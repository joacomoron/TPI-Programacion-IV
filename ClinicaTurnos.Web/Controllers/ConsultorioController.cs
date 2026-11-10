using ClinicaTurnos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaTurnos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultorioController : ControllerBase
    {
        private readonly IConsultorioService _consultorioService;

        public ConsultorioController(IConsultorioService consultorioService)
        {
            _consultorioService = consultorioService;
        }
    }
}
