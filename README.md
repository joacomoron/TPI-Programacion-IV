# ClinicaTurnos

API de reserva de turnos para una clínica médica. TPI de Programación IV (UTN).

## Arquitectura (Clean Architecture)

| Proyecto | Responsabilidad |
|---|---|
| `ClinicaTurnos.Domain` | Entidades, enums e interfaces de repositorios. No depende de nadie. |
| `ClinicaTurnos.Application` | Interfaces e implementación de servicios. Depende de Domain. |
| `ClinicaTurnos.Infrastructure` | DbContext (EF Core), repositorios y migraciones. Depende de Domain. |
| `ClinicaTurnos.Web` | Controladores, `Program.cs` e inyección de dependencias. |

## Entidades
Paciente, Medico, Turno, Consultorio, Pago (según el diagrama de clases).

## Stack
.NET 10 · ASP.NET Core Web API · EF Core (code first) · SQL Server
