using System.Text;
using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Services;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using ClinicaTurnos.Infrastructure.ExternalServices;
using ClinicaTurnos.Infrastructure.Repositories;
using ClinicaTurnos.Web.Middleware;
using ClinicaTurnos.Web.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------- Manejo global de errores ----------
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ---------- Contexto (EF Core + SQL Server) ----------
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sql => sql.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

// ---------- Autenticación JWT ----------
// En Azure el secret se carga como variable de entorno "Jwt__Secret" (el __ equivale a ":")
var jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("Falta configurar Jwt:Secret");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization();

// ---------- Servicio de terceros con HttpClientFactory (typed client) ----------
builder.Services.AddHttpClient<IFeriadoService, FeriadoService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiFeriados:BaseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(5);
});

// ---------- Repositorios ----------
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<IConsultorioRepository, ConsultorioRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();

// ---------- Servicios ----------
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IMedicoService, MedicoService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<IConsultorioService, ConsultorioService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

var app = builder.Build();

// Aplica las migraciones pendientes al arrancar: así la base de Azure se crea/actualiza sola en cada deploy
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    context.Database.Migrate();
}

app.UseExceptionHandler();

// Documentación de la API: /openapi/v1.json y la interfaz para probarla en /scalar
// (habilitada también en producción para poder mostrarla deployada en el oral)
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthentication(); // ¿quién sos? (lee y valida el token)
app.UseAuthorization();  // ¿podés entrar acá? (revisa [Authorize])
app.MapControllers();

app.Run();
