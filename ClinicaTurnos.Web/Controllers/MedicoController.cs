using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Medicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicaTurnos.Web.Controllers
{
    [ApiController]
    [Authorize] // todo el controller pide token, salvo lo marcado con [AllowAnonymous]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        public MedicoController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        // GET api/Medico  o  GET api/Medico?especialidad=Cardiología
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<MedicoDto>>> GetAll([FromQuery] string? especialidad)
        {
            return Ok(await _medicoService.GetAllAsync(especialidad));
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MedicoDto>> GetById(int id)
        {
            return Ok(await _medicoService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<MedicoDto>> Create([FromBody] MedicoRequest request)
        {
            var medico = await _medicoService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = medico.Id }, medico);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MedicoDto>> Update(int id, [FromBody] MedicoRequest request)
        {
            return Ok(await _medicoService.UpdateAsync(id, request));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _medicoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
