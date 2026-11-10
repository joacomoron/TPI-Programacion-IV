using ClinicaTurnos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaTurnos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }
    }
}
