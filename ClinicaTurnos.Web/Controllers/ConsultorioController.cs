using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Consultorios;
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

        [HttpGet]
        public async Task<ActionResult<List<ConsultorioDto>>> GetAll()
        {
            return Ok(await _consultorioService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ConsultorioDto>> GetById(int id)
        {
            return Ok(await _consultorioService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ConsultorioDto>> Create([FromBody] ConsultorioRequest request)
        {
            var consultorio = await _consultorioService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = consultorio.Id }, consultorio);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ConsultorioDto>> Update(int id, [FromBody] ConsultorioRequest request)
        {
            return Ok(await _consultorioService.UpdateAsync(id, request));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _consultorioService.DeleteAsync(id);
            return NoContent();
        }
    }
}
