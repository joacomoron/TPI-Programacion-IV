using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Medicos;
using ClinicaTurnos.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaTurnos.Tests;

public class MedicoControllerTests
{
    [Fact]
    public async Task GetAll_PasaLaEspecialidadAlServicio()
    {
        var service = new Mock<IMedicoService>();
        var expected = new List<MedicoDto> { new() { Id = 1, Especialidad = "Cardiología" } };
        service.Setup(x => x.GetAllAsync("Cardiología")).ReturnsAsync(expected);
        var controller = new MedicoController(service.Object);

        var result = await controller.GetAll("Cardiología");

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
        service.Verify(x => x.GetAllAsync("Cardiología"), Times.Once);
    }

    [Fact]
    public async Task Create_DevuelveCreatedConElMedicoCreado()
    {
        var service = new Mock<IMedicoService>();
        var request = new MedicoRequest { Nombre = "Ana", Apellido = "Pérez", Dni = "12345678", Matricula = "M-1", Especialidad = "Clínica" };
        var expected = new MedicoDto { Id = 3, Nombre = "Ana" };
        service.Setup(x => x.CreateAsync(request)).ReturnsAsync(expected);
        var controller = new MedicoController(service.Object);

        var result = await controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(3, created.RouteValues!["id"]);
        Assert.Same(expected, created.Value);
    }

    [Fact]
    public async Task Delete_DevuelveNoContentYLlamaAlServicio()
    {
        var service = new Mock<IMedicoService>();
        var controller = new MedicoController(service.Object);

        var result = await controller.Delete(8);

        Assert.IsType<NoContentResult>(result);
        service.Verify(x => x.DeleteAsync(8), Times.Once);
    }
}
