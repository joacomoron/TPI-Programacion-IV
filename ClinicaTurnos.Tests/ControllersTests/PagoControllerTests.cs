using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Pagos;
using ClinicaTurnos.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaTurnos.Tests;

public class PagoControllerTests
{
    [Fact]
    public async Task GetAll_DevuelveLaListaDePagos()
    {
        var service = new Mock<IPagoService>();
        var expected = new List<PagoDto> { new() { Id = 1, TurnoId = 4, MedioPago = "Efectivo" } };
        service.Setup(x => x.GetAllAsync()).ReturnsAsync(expected);
        var controller = new PagoController(service.Object);

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(expected, ok.Value);
    }

    [Fact]
    public async Task Create_DevuelveCreatedConElPagoCreado()
    {
        var service = new Mock<IPagoService>();
        var request = new PagoRequest { TurnoId = 4, MedioPago = "Efectivo", Comprobante = "COMP-1" };
        var expected = new PagoDto { Id = 6, TurnoId = 4 };
        service.Setup(x => x.CreateAsync(request)).ReturnsAsync(expected);
        var controller = new PagoController(service.Object);

        var result = await controller.Create(request);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(PagoController.GetById), created.ActionName);
        Assert.Equal(6, created.RouteValues!["id"]);
        Assert.Same(expected, created.Value);
    }

    [Fact]
    public async Task Delete_DevuelveNoContentYLlamaAlServicio()
    {
        var service = new Mock<IPagoService>();
        var controller = new PagoController(service.Object);

        var result = await controller.Delete(6);

        Assert.IsType<NoContentResult>(result);
        service.Verify(x => x.DeleteAsync(6), Times.Once);
    }
}
