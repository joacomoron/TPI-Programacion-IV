using ClinicaTurnos.Application.Interfaces;
using ClinicaTurnos.Application.Services;
using ClinicaTurnos.Domain.Interfaces;
using ClinicaTurnos.Infrastructure.Data;
using ClinicaTurnos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
