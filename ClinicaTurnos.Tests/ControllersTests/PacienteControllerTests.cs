using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Pacientes;
using ClinicaTurnos.Application.Models.Turnos;
using ClinicaTurnos.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaTurnos.Tests;

public class PacienteControllerTests
{
    [Fact]
    public async Task GetById_DevuelveElPacienteSolicitado()
    {
        var pacienteService = new Mock<IPacienteService>();
        var turnoService = new Mock<ITurnoService>();
        var expected = new PacienteDto { Id = 2, Nombre = "Luis" };
        pacienteService.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(expected);
        var controller = new PacienteController(pacienteService.Object, turnoService.Object);

        var result = await controller.GetById(2);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        pacienteService.Verify(x => x.GetByIdAsync(2), Times.Once);
    }

    [Fact]
    public async Task Create_DevuelveCreatedConElPacienteCreado()
    {
        var pacienteService = new Mock<IPacienteService>();
        var turnoService = new Mock<ITurnoService>();
        var request = new PacienteRequest { Nombre = "Luis", Apellido = "Gómez", Dni = "12345678", Email = "luis@example.com" };
        var expected = new PacienteDto { Id = 5, Nombre = "Luis" };
        pacienteService.Setup(x => x.CreateAsync(request)).ReturnsAsync(expected);
        var controller = new PacienteController(pacienteService.Object, turnoService.Object);

        var result = await controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(5, created.RouteValues!["id"]);
        Assert.Same(expected, created.Value);
    }

    [Fact]
    public async Task GetTurnos_DevuelveLosTurnosDelPaciente()
    {
        var pacienteService = new Mock<IPacienteService>();
        var turnoService = new Mock<ITurnoService>();
        var expected = new List<TurnoDto> { new() { Id = 9, PacienteId = 2 } };
        turnoService.Setup(x => x.GetByPacienteAsync(2)).ReturnsAsync(expected);
        var controller = new PacienteController(pacienteService.Object, turnoService.Object);

        var result = await controller.GetTurnos(2);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
    }
}
