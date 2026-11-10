using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Turnos;
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

        [HttpGet]
        public async Task<ActionResult<List<TurnoDto>>> GetAll()
        {
            return Ok(await _turnoService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TurnoDto>> GetById(int id)
        {
            return Ok(await _turnoService.GetByIdAsync(id));
        }

        // GET api/Turno/disponibilidad?medicoId=1&fecha=2026-12-01
        [HttpGet("disponibilidad")]
        public async Task<ActionResult<List<TimeOnly>>> GetDisponibilidad([FromQuery] int medicoId, [FromQuery] DateOnly fecha)
        {
            return Ok(await _turnoService.GetHorariosDisponiblesAsync(medicoId, fecha));
        }

        [HttpPost]
        public async Task<ActionResult<TurnoDto>> Create([FromBody] TurnoRequest request)
        {
            var turno = await _turnoService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = turno.Id }, turno);
        }

        // PATCH porque solo cambia una parte del turno (el estado), no el recurso completo
        [HttpPatch("{id:int}/confirmar")]
        public async Task<ActionResult<TurnoDto>> Confirmar(int id)
        {
            return Ok(await _turnoService.ConfirmarAsync(id));
        }

        [HttpPatch("{id:int}/cancelar")]
        public async Task<ActionResult<TurnoDto>> Cancelar(int id)
        {
            return Ok(await _turnoService.CancelarAsync(id));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _turnoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
