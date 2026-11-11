using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Models.Auth;
using ClinicaTurnos.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ClinicaTurnos.Tests;

public class AuthenticationControllerTests
{
    [Fact]
    public async Task Login_DevuelveOkConLaRespuestaDelServicio()
    {
        var service = new Mock<IAuthenticationService>();
        var request = new LoginRequest { Email = "paciente@example.com", Dni = "12345678" };
        var response = new LoginResponse { Token = "token", PacienteId = 1, Paciente = "Paciente" };
        service.Setup(x => x.LoginAsync(request)).ReturnsAsync(response);
        var controller = new AuthenticationController(service.Object);

        var result = await controller.Login(request);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Same(response, ok.Value);
        service.Verify(x => x.LoginAsync(request), Times.Once);
    }
}
