using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Consultorios;
using ClinicaTurnos.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaTurnos.Tests;

public class ConsultorioControllerTests
{
    [Fact]
    public async Task GetAll_DevuelveLaListaDelServicio()
    {
        var service = new Mock<IConsultorioService>();
        var expected = new List<ConsultorioDto> { new() { Id = 1, NroConsultorio = 10 } };
        service.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);
        var controller = new ConsultorioController(service.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
    }

    [Fact]
    public async Task Create_DevuelveCreatedConLaRutaDelRecurso()
    {
        var service = new Mock<IConsultorioService>();
        var request = new ConsultorioRequest { NroConsultorio = 10, Disponibilidad = true };
        var expected = new ConsultorioDto { Id = 4, NroConsultorio = 10 };
        service.Setup(x => x.CreateAsync(request)).ReturnsAsync(expected);
        var controller = new ConsultorioController(service.Object);

        var result = await controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(ConsultorioController.GetById), created.ActionName);
        Assert.Equal(4, created.RouteValues!["id"]);
        Assert.Same(expected, created.Value);
    }

    [Fact]
    public async Task Delete_DevuelveNoContentYLlamaAlServicio()
    {
        var service = new Mock<IConsultorioService>();
        var controller = new ConsultorioController(service.Object);

        var result = await controller.Delete(7);

        Assert.IsType<NoContentResult>(result);
        service.Verify(x => x.DeleteAsync(7), Times.Once);
    }
}
