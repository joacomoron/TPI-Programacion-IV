using ClinicaTurnos.Application.Exceptions;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Pagos;
using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Enums;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;
        private readonly ITurnoRepository _turnoRepository;

        public PagoService(IPagoRepository pagoRepository, ITurnoRepository turnoRepository)
        {
            _pagoRepository = pagoRepository;
            _turnoRepository = turnoRepository;
        }

        public async Task<List<PagoDto>> GetAllAsync()
        {
            var pagos = await _pagoRepository.GetAllAsync();
            return pagos.Select(PagoDto.FromEntity).ToList();
        }

        public async Task<PagoDto> GetByIdAsync(int id)
        {
            var pago = await ObtenerOFallar(id);
            return PagoDto.FromEntity(pago);
        }

        public async Task<PagoDto> CreateAsync(PagoRequest request)
        {
            var turno = await _turnoRepository.GetByIdAsync(request.TurnoId)
                ?? throw new NotFoundException($"No existe el turno con Id {request.TurnoId}");

            if (turno.Estado == EstadoTurno.Cancelado)
                throw new BusinessRuleException("No se puede pagar un turno cancelado");

            // Relación 1 a 1: un turno tiene como mucho un pago
            if (await _pagoRepository.GetByTurnoIdAsync(request.TurnoId) is not null)
                throw new BusinessRuleException($"El turno {request.TurnoId} ya está pago");

            var pago = new Pago
            {
                TurnoId = request.TurnoId,
                MedioPago = request.MedioPago,
                Comprobante = request.Comprobante
            };

            await _pagoRepository.AddAsync(pago);
            return PagoDto.FromEntity(pago);
        }

        public async Task DeleteAsync(int id)
        {
            var pago = await ObtenerOFallar(id);
            await _pagoRepository.DeleteAsync(pago);
        }

        private async Task<Pago> ObtenerOFallar(int id)
        {
            return await _pagoRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"No existe el pago con Id {id}");
        }
    }
}
