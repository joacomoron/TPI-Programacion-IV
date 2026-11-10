using ClinicaTurnos.Application.Exceptions;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Medicos;
using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        // Si viene especialidad filtra (el paciente elige especialidad y después médico)
        public async Task<List<MedicoDto>> GetAllAsync(string? especialidad)
        {
            var medicos = string.IsNullOrWhiteSpace(especialidad)
                ? await _medicoRepository.GetAllAsync()
                : await _medicoRepository.GetByEspecialidadAsync(especialidad);

            return medicos.Select(MedicoDto.FromEntity).ToList();
        }

        public async Task<MedicoDto> GetByIdAsync(int id)
        {
            var medico = await ObtenerOFallar(id);
            return MedicoDto.FromEntity(medico);
        }

        public async Task<MedicoDto> CreateAsync(MedicoRequest request)
        {
            if (await _medicoRepository.ExistsByMatriculaAsync(request.Matricula))
                throw new BusinessRuleException($"Ya existe un médico con matrícula {request.Matricula}");

            var medico = new Medico
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Dni = request.Dni,
                Matricula = request.Matricula,
                Especialidad = request.Especialidad
            };

            await _medicoRepository.AddAsync(medico);
            return MedicoDto.FromEntity(medico);
        }

        public async Task<MedicoDto> UpdateAsync(int id, MedicoRequest request)
        {
            var medico = await ObtenerOFallar(id);

            if (medico.Matricula != request.Matricula && await _medicoRepository.ExistsByMatriculaAsync(request.Matricula))
                throw new BusinessRuleException($"Ya existe un médico con matrícula {request.Matricula}");

            medico.Nombre = request.Nombre;
            medico.Apellido = request.Apellido;
            medico.Dni = request.Dni;
            medico.Matricula = request.Matricula;
            medico.Especialidad = request.Especialidad;

            await _medicoRepository.UpdateAsync(medico);
            return MedicoDto.FromEntity(medico);
        }

        public async Task DeleteAsync(int id)
        {
            var medico = await ObtenerOFallar(id);
            await _medicoRepository.DeleteAsync(medico);
        }

        private async Task<Medico> ObtenerOFallar(int id)
        {
            return await _medicoRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"No existe el médico con Id {id}");
        }
    }
}
