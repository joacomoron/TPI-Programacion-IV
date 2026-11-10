using ClinicaTurnos.Application.Exceptions;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Consultorios;
using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class ConsultorioService : IConsultorioService
    {
        private readonly IConsultorioRepository _consultorioRepository;

        public ConsultorioService(IConsultorioRepository consultorioRepository)
        {
            _consultorioRepository = consultorioRepository;
        }

        public async Task<List<ConsultorioDto>> GetAllAsync()
        {
            var consultorios = await _consultorioRepository.GetAllAsync();
            return consultorios.Select(ConsultorioDto.FromEntity).ToList();
        }

        public async Task<ConsultorioDto> GetByIdAsync(int id)
        {
            var consultorio = await ObtenerOFallar(id);
            return ConsultorioDto.FromEntity(consultorio);
        }

        public async Task<ConsultorioDto> CreateAsync(ConsultorioRequest request)
        {
            if (await _consultorioRepository.ExistsByNumeroAsync(request.NroConsultorio))
                throw new BusinessRuleException($"Ya existe el consultorio número {request.NroConsultorio}");

            var consultorio = new Consultorio
            {
                NroConsultorio = request.NroConsultorio,
                Disponibilidad = request.Disponibilidad
            };

            await _consultorioRepository.AddAsync(consultorio);
            return ConsultorioDto.FromEntity(consultorio);
        }

        public async Task<ConsultorioDto> UpdateAsync(int id, ConsultorioRequest request)
        {
            var consultorio = await ObtenerOFallar(id);

            if (consultorio.NroConsultorio != request.NroConsultorio
                && await _consultorioRepository.ExistsByNumeroAsync(request.NroConsultorio))
                throw new BusinessRuleException($"Ya existe el consultorio número {request.NroConsultorio}");

            consultorio.NroConsultorio = request.NroConsultorio;
            consultorio.Disponibilidad = request.Disponibilidad;

            await _consultorioRepository.UpdateAsync(consultorio);
            return ConsultorioDto.FromEntity(consultorio);
        }

        public async Task DeleteAsync(int id)
        {
            var consultorio = await ObtenerOFallar(id);
            await _consultorioRepository.DeleteAsync(consultorio);
        }

        private async Task<Consultorio> ObtenerOFallar(int id)
        {
            return await _consultorioRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"No existe el consultorio con Id {id}");
        }
    }
}
