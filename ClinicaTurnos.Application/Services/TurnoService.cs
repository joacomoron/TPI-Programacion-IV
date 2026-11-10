using ClinicaTurnos.Application.Exceptions;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Turnos;
using ClinicaTurnos.Domain.Entities;
using ClinicaTurnos.Domain.Enums;
using ClinicaTurnos.Domain.Interfaces;

namespace ClinicaTurnos.Application.Services
{
    public class TurnoService : ITurnoService
    {
        // Horario de atención de la clínica: turnos de 30 minutos de 8:00 a 12:00 y de 16:00 a 20:00
        private static readonly List<TimeOnly> HorariosDeAtencion = GenerarHorarios();

        private readonly ITurnoRepository _turnoRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IMedicoRepository _medicoRepository;
        private readonly IConsultorioRepository _consultorioRepository;
        private readonly IFeriadoService _feriadoService;

        public TurnoService(
            ITurnoRepository turnoRepository,
            IPacienteRepository pacienteRepository,
            IMedicoRepository medicoRepository,
            IConsultorioRepository consultorioRepository,
            IFeriadoService feriadoService)
        {
            _turnoRepository = turnoRepository;
            _pacienteRepository = pacienteRepository;
            _medicoRepository = medicoRepository;
            _consultorioRepository = consultorioRepository;
            _feriadoService = feriadoService;
        }

        public async Task<List<TurnoDto>> GetAllAsync()
        {
            var turnos = await _turnoRepository.GetAllWithDetailsAsync();
            return turnos.Select(TurnoDto.FromEntity).ToList();
        }

        public async Task<TurnoDto> GetByIdAsync(int id)
        {
            var turno = await ObtenerOFallar(id);
            return TurnoDto.FromEntity(turno);
        }

        public async Task<List<TurnoDto>> GetByPacienteAsync(int pacienteId)
        {
            if (await _pacienteRepository.GetByIdAsync(pacienteId) is null)
                throw new NotFoundException($"No existe el paciente con Id {pacienteId}");

            var turnos = await _turnoRepository.GetByPacienteAsync(pacienteId);
            return turnos.Select(TurnoDto.FromEntity).ToList();
        }

        // "Ver la disponibilidad horaria según el día que seleccionen" (minuta)
        public async Task<List<TimeOnly>> GetHorariosDisponiblesAsync(int medicoId, DateOnly fecha)
        {
            if (await _medicoRepository.GetByIdAsync(medicoId) is null)
                throw new NotFoundException($"No existe el médico con Id {medicoId}");

            if (fecha < Hoy())
                return new List<TimeOnly>();

            // Servicio externo: si es feriado la clínica no atiende
            if (await _feriadoService.EsFeriadoAsync(fecha))
                return new List<TimeOnly>();

            var ocupados = (await _turnoRepository.GetActivosByMedicoAndFechaAsync(medicoId, fecha))
                .Select(t => t.Hora)
                .ToHashSet();

            return HorariosDeAtencion.Where(h => !ocupados.Contains(h)).ToList();
        }

        public async Task<TurnoDto> CreateAsync(TurnoRequest request)
        {
            if (request.Fecha < Hoy())
                throw new BusinessRuleException("No se puede reservar un turno en una fecha pasada");

            if (!HorariosDeAtencion.Contains(request.Hora))
                throw new BusinessRuleException("La hora no corresponde a un horario de atención (turnos cada 30 min, 8 a 12 y 16 a 20)");

            if (await _pacienteRepository.GetByIdAsync(request.PacienteId) is null)
                throw new NotFoundException($"No existe el paciente con Id {request.PacienteId}");

            if (await _medicoRepository.GetByIdAsync(request.MedicoId) is null)
                throw new NotFoundException($"No existe el médico con Id {request.MedicoId}");

            var consultorio = await _consultorioRepository.GetByIdAsync(request.ConsultorioId)
                ?? throw new NotFoundException($"No existe el consultorio con Id {request.ConsultorioId}");

            if (!consultorio.Disponibilidad)
                throw new BusinessRuleException($"El consultorio {consultorio.NroConsultorio} no está disponible");

            if (await _turnoRepository.ExisteSuperposicionAsync(request.MedicoId, request.ConsultorioId, request.Fecha, request.Hora))
                throw new BusinessRuleException("El médico o el consultorio ya tienen un turno en esa fecha y hora");

            // Servicio externo (API de feriados vía HttpClientFactory)
            if (await _feriadoService.EsFeriadoAsync(request.Fecha))
                throw new BusinessRuleException($"El {request.Fecha:dd/MM/yyyy} es feriado, la clínica no atiende");

            var turno = new Turno
            {
                Fecha = request.Fecha,
                Hora = request.Hora,
                Estado = EstadoTurno.Pendiente,
                PacienteId = request.PacienteId,
                MedicoId = request.MedicoId,
                ConsultorioId = request.ConsultorioId
            };

            await _turnoRepository.AddAsync(turno);

            // Lo vuelvo a buscar con los Include para devolver nombres de paciente, médico, etc.
            return TurnoDto.FromEntity(await ObtenerOFallar(turno.Id));
        }

        public async Task<TurnoDto> ConfirmarAsync(int id)
        {
            var turno = await ObtenerOFallar(id);

            if (turno.Estado != EstadoTurno.Pendiente)
                throw new BusinessRuleException($"Solo se pueden confirmar turnos pendientes (estado actual: {turno.Estado})");

            turno.Estado = EstadoTurno.Confirmado;
            await _turnoRepository.UpdateAsync(turno);
            return TurnoDto.FromEntity(turno);
        }

        public async Task<TurnoDto> CancelarAsync(int id)
        {
            var turno = await ObtenerOFallar(id);

            if (turno.Estado is EstadoTurno.Cancelado or EstadoTurno.Atendido)
                throw new BusinessRuleException($"No se puede cancelar un turno en estado {turno.Estado}");

            turno.Estado = EstadoTurno.Cancelado;
            await _turnoRepository.UpdateAsync(turno);
            return TurnoDto.FromEntity(turno);
        }

        public async Task DeleteAsync(int id)
        {
            var turno = await ObtenerOFallar(id);
            await _turnoRepository.DeleteAsync(turno);
        }

        private async Task<Turno> ObtenerOFallar(int id)
        {
            return await _turnoRepository.GetByIdWithDetailsAsync(id)
                ?? throw new NotFoundException($"No existe el turno con Id {id}");
        }

        private static DateOnly Hoy() => DateOnly.FromDateTime(DateTime.Today);

        private static List<TimeOnly> GenerarHorarios()
        {
            var horarios = new List<TimeOnly>();
            for (var h = new TimeOnly(8, 0); h < new TimeOnly(12, 0); h = h.AddMinutes(30)) horarios.Add(h);
            for (var h = new TimeOnly(16, 0); h < new TimeOnly(20, 0); h = h.AddMinutes(30)) horarios.Add(h);
            return horarios;
        }
    }
}
