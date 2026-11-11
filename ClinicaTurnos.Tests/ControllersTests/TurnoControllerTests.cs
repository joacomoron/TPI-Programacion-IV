using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Turnos;
using ClinicaTurnos.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaTurnos.Tests;

public class TurnoControllerTests
{
    [Fact]
    public async Task GetDisponibilidad_PasaMedicoYFechaAlServicio()
    {
        var service = new Mock<ITurnoService>();
        var fecha = new DateOnly(2026, 12, 1);
        var expected = new List<TimeOnly> { new(9, 0), new(10, 0) };
        service.Setup(x => x.GetHorariosDisponiblesAsync(3, fecha)).ReturnsAsync(expected);
        var controller = new TurnoController(service.Object);

        var result = await controller.GetDisponibilidad(3, fecha);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        service.Verify(x => x.GetHorariosDisponiblesAsync(3, fecha), Times.Once);
    }

    [Fact]
    public async Task Create_DevuelveCreatedConElTurnoCreado()
    {
        var service = new Mock<ITurnoService>();
        var request = new TurnoRequest { Fecha = new DateOnly(2026, 12, 1), Hora = new TimeOnly(9, 0), PacienteId = 1, MedicoId = 2, ConsultorioId = 3 };
        var expected = new TurnoDto { Id = 7, PacienteId = 1, MedicoId = 2 };
        service.Setup(x => x.CreateAsync(request)).ReturnsAsync(expected);
        var controller = new TurnoController(service.Object);

        var result = await controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(7, created.RouteValues!["id"]);
        Assert.Same(expected, created.Value);
    }

    [Fact]
    public async Task Confirmar_DevuelveElTurnoConfirmado()
    {
        var service = new Mock<ITurnoService>();
        var expected = new TurnoDto { Id = 7, Estado = "Confirmado" };
        service.Setup(x => x.ConfirmarAsync(7)).ReturnsAsync(expected);
        var controller = new TurnoController(service.Object);

        var result = await controller.Confirmar(7);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        service.Verify(x => x.ConfirmarAsync(7), Times.Once);
    }

    [Fact]
    public async Task Cancelar_DevuelveElTurnoCancelado()
    {
        var service = new Mock<ITurnoService>();
        var expected = new TurnoDto { Id = 7, Estado = "Cancelado" };
        service.Setup(x => x.CancelarAsync(7)).ReturnsAsync(expected);
        var controller = new TurnoController(service.Object);

        var result = await controller.Cancelar(7);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        service.Verify(x => x.CancelarAsync(7), Times.Once);
    }

    [Fact]
    public async Task Delete_DevuelveNoContentYLlamaAlServicio()
    {
        var service = new Mock<ITurnoService>();
        var controller = new TurnoController(service.Object);

        var result = await controller.Delete(7);

        Assert.IsType<NoContentResult>(result);
        service.Verify(x => x.DeleteAsync(7), Times.Once);
    }
}
