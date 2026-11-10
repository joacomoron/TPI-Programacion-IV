using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Services;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using ClinicaTurnos.Infrastructure.Repositories;
using ClinicaTurnos.Web.Middleware;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ---------- Manejo global de errores ----------
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ---------- Contexto (EF Core + SQL Server) ----------
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

var app = builder.Build();

app.UseExceptionHandler();

// Documentación de la API: /openapi/v1.json y la interfaz para probarla en /scalar
// (habilitada también en producción para poder mostrarla deployada en el oral)
app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
