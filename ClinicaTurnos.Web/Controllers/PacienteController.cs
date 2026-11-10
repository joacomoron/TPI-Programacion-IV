using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Pacientes;
using ClinicaTurnos.Application.Models.Turnos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaTurnos.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        private readonly ITurnoService _turnoService;

        public PacienteController(IPacienteService pacienteService, ITurnoService turnoService)
        {
            _pacienteService = pacienteService;
            _turnoService = turnoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PacienteDto>>> GetAll()
        {
            return Ok(await _pacienteService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PacienteDto>> GetById(int id)
        {
            return Ok(await _pacienteService.GetByIdAsync(id));
        }

        // GET api/Paciente/3/turnos → los turnos de ese paciente
        [HttpGet("{id:int}/turnos")]
        public async Task<ActionResult<List<TurnoDto>>> GetTurnos(int id)
        {
            return Ok(await _turnoService.GetByPacienteAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<PacienteDto>> Create([FromBody] PacienteRequest request)
        {
            var paciente = await _pacienteService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = paciente.Id }, paciente);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PacienteDto>> Update(int id, [FromBody] PacienteRequest request)
        {
            return Ok(await _pacienteService.UpdateAsync(id, request));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pacienteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
