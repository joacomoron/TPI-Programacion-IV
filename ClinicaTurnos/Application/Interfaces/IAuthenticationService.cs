using ClinicaTurnos.Application.Models.Auth;

namespace ClinicaTurnos.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
