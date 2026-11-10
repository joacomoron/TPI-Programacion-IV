using ClinicaTurnos.Application.Exceptions;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Pacientes;
using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<List<PacienteDto>> GetAllAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(PacienteDto.FromEntity).ToList();
        }

        public async Task<PacienteDto> GetByIdAsync(int id)
        {
            var paciente = await ObtenerOFallar(id);
            return PacienteDto.FromEntity(paciente);
        }

        public async Task<PacienteDto> CreateAsync(PacienteRequest request)
        {
            if (await _pacienteRepository.ExistsByDniAsync(request.Dni))
                throw new BusinessRuleException($"Ya existe un paciente con DNI {request.Dni}");

            if (await _pacienteRepository.GetByEmailAsync(request.Email) is not null)
                throw new BusinessRuleException($"Ya existe un paciente con email {request.Email}");

            var paciente = new Paciente
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Dni = request.Dni,
                Email = request.Email,
                Direccion = request.Direccion,
                ObraSocial = request.ObraSocial
            };

            await _pacienteRepository.AddAsync(paciente);
            return PacienteDto.FromEntity(paciente);
        }

        public async Task<PacienteDto> UpdateAsync(int id, PacienteRequest request)
        {
            var paciente = await ObtenerOFallar(id);

            if (paciente.Dni != request.Dni && await _pacienteRepository.ExistsByDniAsync(request.Dni))
                throw new BusinessRuleException($"Ya existe un paciente con DNI {request.Dni}");

            paciente.Nombre = request.Nombre;
            paciente.Apellido = request.Apellido;
            paciente.Dni = request.Dni;
            paciente.Email = request.Email;
            paciente.Direccion = request.Direccion;
            paciente.ObraSocial = request.ObraSocial;

            await _pacienteRepository.UpdateAsync(paciente);
            return PacienteDto.FromEntity(paciente);
        }

        public async Task DeleteAsync(int id)
        {
            var paciente = await ObtenerOFallar(id);
            await _pacienteRepository.DeleteAsync(paciente);
        }

        private async Task<Paciente> ObtenerOFallar(int id)
        {
            return await _pacienteRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"No existe el paciente con Id {id}");
        }
    }
}
