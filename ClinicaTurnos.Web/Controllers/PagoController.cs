using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Pagos;
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

        [HttpGet]
        public async Task<ActionResult<List<PagoDto>>> GetAll()
        {
            return Ok(await _pagoService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PagoDto>> GetById(int id)
        {
            return Ok(await _pagoService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<PagoDto>> Create([FromBody] PagoRequest request)
        {
            var pago = await _pagoService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = pago.Id }, pago);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pagoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
