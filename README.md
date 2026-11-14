# 🏥 ClinicaTurnos

API REST para la **reserva de turnos de una clínica médica**. Permite registrar pacientes, administrar médicos y consultorios, consultar horarios disponibles, reservar y confirmar turnos, y registrar el pago de cada uno.

Trabajo Práctico Integrador de **Programación IV (UTN)**.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4)
![EF Core](https://img.shields.io/badge/EF%20Core-Code%20First-512BD4)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Azure](https://img.shields.io/badge/Deploy-Azure%20App%20Service-0078D4?logo=microsoftazure&logoColor=white)

---

## URL Pública

📖 **Documentación interactiva y pruebas de la API:** https://TU-URL.azurewebsites.net/scalar


## 📋 Tabla de contenidos

- [Funcionalidades](#-funcionalidades)
- [Stack tecnológico](#-stack-tecnológico)
- [Arquitectura](#-arquitectura-clean-architecture)
- [Modelo de datos](#-modelo-de-datos)
- [Reglas de negocio](#-reglas-de-negocio)
- [Endpoints](#-endpoints)
- [Autenticación](#-autenticación)
- [Cómo ejecutarlo localmente](#-cómo-ejecutarlo-localmente)
- [Probar la API](#-probar-la-api)
- [Tests](#-tests)
- [CI/CD y deploy](#-cicd-y-deploy)

---

## ✨ Funcionalidades

- CRUD de **pacientes**, **médicos**, **consultorios** y **pagos**.
- **Reserva de turnos** con validaciones (fecha, horario de atención, superposiciones, feriados).
- **Consulta de disponibilidad horaria** de un médico según el día elegido.
- Flujo de estados del turno: `Pendiente → Confirmado / Cancelado → Atendido`.
- **Login con JWT** para los pacientes.
- Integración con una **API externa de feriados** (ArgentinaDatos) mediante `HttpClientFactory`.
- **Manejo global de errores** con respuestas `ProblemDetails`.
- Documentación interactiva con **OpenAPI + Scalar**.
- Migraciones aplicadas automáticamente al iniciar la aplicación.

## 🛠️ Stack tecnológico

| Capa | Tecnología |
|---|---|
| Framework | .NET 10 · ASP.NET Core Web API |
| Persistencia | Entity Framework Core (Code First) · SQL Server |
| Autenticación | JWT Bearer |
| Documentación | OpenAPI · Scalar |
| Testing | xUnit · Moq |
| CI/CD | GitHub Actions · Azure App Service |

## 🧱 Arquitectura (Clean Architecture)

```
ClinicaTurnos/
├── ClinicaTurnos/
│   ├── Domain/          # Entidades, enums e interfaces de repositorios
│   ├── Application/     # Servicios, DTOs/Requests y excepciones de negocio
│   ├── Infrastructure/  # DbContext, repositorios, migraciones y servicios externos
│   └── Web/             # Controllers, Program.cs, seguridad y middleware
├── ClinicaTurnos.Tests/ # Tests unitarios de los controllers
└── ClinicaTurnos.slnx
```

| Proyecto | Responsabilidad | Depende de |
|---|---|---|
| `Domain` | Entidades, enums e interfaces de repositorios. | — |
| `Application` | Interfaces e implementación de servicios, modelos y excepciones. | Domain |
| `Infrastructure` | `DbContext` (EF Core), repositorios, migraciones y cliente de la API de feriados. | Domain |
| `Web` | Controladores, `Program.cs`, JWT, manejo global de errores e inyección de dependencias. | Application, Infrastructure |

## 🗂️ Modelo de datos

| Entidad | Campos principales |
|---|---|
| **Paciente** | Nombre, Apellido, Dni, Email, Dirección, ObraSocial |
| **Medico** | Nombre, Apellido, Dni, Matrícula, Especialidad |
| **Consultorio** | NroConsultorio, Disponibilidad |
| **Turno** | Fecha, Hora, Estado (`Pendiente`, `Confirmado`, `Cancelado`, `Atendido`) |
| **Pago** | MedioPago, Comprobante |

**Relaciones**

- Paciente 1 — N Turno
- Médico 1 — N Turno
- Consultorio 1 — N Turno
- Turno 1 — 1 Pago (el pago guarda la FK del turno)

## 📏 Reglas de negocio

**Horarios de atención:** turnos de 30 minutos, de **8:00 a 12:00** y de **16:00 a 20:00**.

Al crear un turno se valida que:

1. La fecha no sea pasada.
2. La hora sea un horario de atención válido.
3. Existan el paciente, el médico y el consultorio.
4. El consultorio esté disponible.
5. El médico o el consultorio no tengan ya un turno en esa fecha y hora.
6. La fecha no sea feriado (consulta a la API externa).

Otras reglas:

- Solo se pueden **confirmar** turnos en estado `Pendiente`.
- No se puede **cancelar** un turno `Cancelado` o `Atendido`.
- No se puede **pagar** un turno cancelado, y cada turno admite **un solo pago**.
- La consulta de disponibilidad devuelve una lista vacía si la fecha es pasada o feriado.
- Si la API de feriados no responde, se registra una advertencia y la reserva **no se bloquea**.
- No se puede borrar un médico, paciente o consultorio con turnos asociados (`409 Conflict`).

## 🔌 Endpoints

Todos los controllers requieren token, salvo los marcados como públicos.

### Authentication
| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| `POST` | `/api/Authentication/login` | Público | Login con email y DNI, devuelve el token |

### Paciente
| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| `GET` | `/api/Paciente` | 🔒 | Listar pacientes |
| `GET` | `/api/Paciente/{id}` | 🔒 | Obtener un paciente |
| `GET` | `/api/Paciente/{id}/turnos` | 🔒 | Turnos de un paciente |
| `POST` | `/api/Paciente` | Público | Registrar paciente |
| `PUT` | `/api/Paciente/{id}` | 🔒 | Actualizar paciente |
| `DELETE` | `/api/Paciente/{id}` | 🔒 | Eliminar paciente |

### Medico
| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| `GET` | `/api/Medico?especialidad=` | Público | Listar médicos (filtro opcional por especialidad) |
| `GET` | `/api/Medico/{id}` | Público | Obtener un médico |
| `POST` | `/api/Medico` | 🔒 | Crear médico |
| `PUT` | `/api/Medico/{id}` | 🔒 | Actualizar médico |
| `DELETE` | `/api/Medico/{id}` | 🔒 | Eliminar médico |

### Consultorio
| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| `GET` | `/api/Consultorio` | 🔒 | Listar consultorios |
| `GET` | `/api/Consultorio/{id}` | 🔒 | Obtener un consultorio |
| `POST` | `/api/Consultorio` | 🔒 | Crear consultorio |
| `PUT` | `/api/Consultorio/{id}` | 🔒 | Actualizar consultorio |
| `DELETE` | `/api/Consultorio/{id}` | 🔒 | Eliminar consultorio |

### Turno
| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| `GET` | `/api/Turno` | 🔒 | Listar turnos |
| `GET` | `/api/Turno/{id}` | 🔒 | Obtener un turno |
| `GET` | `/api/Turno/disponibilidad?medicoId=&fecha=` | Público | Horarios libres de un médico en una fecha |
| `POST` | `/api/Turno` | 🔒 | Reservar turno |
| `PATCH` | `/api/Turno/{id}/confirmar` | 🔒 | Confirmar turno |
| `PATCH` | `/api/Turno/{id}/cancelar` | 🔒 | Cancelar turno |
| `DELETE` | `/api/Turno/{id}` | 🔒 | Eliminar turno |

### Pago
| Método | Ruta | Acceso | Descripción |
|---|---|---|---|
| `GET` | `/api/Pago` | 🔒 | Listar pagos |
| `GET` | `/api/Pago/{id}` | 🔒 | Obtener un pago |
| `POST` | `/api/Pago` | 🔒 | Registrar pago de un turno |
| `DELETE` | `/api/Pago/{id}` | 🔒 | Eliminar pago |

### Códigos de error

| Código | Cuándo ocurre |
|---|---|
| `400` | Regla de negocio no cumplida |
| `401` | Credenciales incorrectas o token ausente/inválido |
| `404` | El recurso no existe |
| `409` | El registro tiene datos asociados |
| `500` | Error interno inesperado |

## 🔐 Autenticación

El sistema no tiene una entidad Usuario: el **login es del Paciente**, con **email + DNI**.

```http
POST /api/Authentication/login
Content-Type: application/json

{
  "email": "juan.perez@mail.com",
  "dni": "40123456"
}
```

La respuesta incluye el `token`, el `pacienteId` y el nombre del paciente. El token expira a la **1 hora** y se envía en las requests protegidas:

```
Authorization: Bearer <token>
```

## 🚀 Cómo ejecutarlo localmente

### Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (por defecto usa **LocalDB**: `(localdb)\MSSQLLocalDB`)

### Pasos

```bash
# 1. Clonar el repositorio
git clone https://github.com/joacomoron/TPI-Programacion-IV.git
cd TPI-Programacion-IV

# 2. Restaurar dependencias
dotnet restore

# 3. Ejecutar la API
dotnet run --project ClinicaTurnos/Web
```

Al arrancar, la aplicación **aplica las migraciones** y crea la base `ClinicaTurnosDb` automáticamente.

La API queda disponible en:

- `https://localhost:7180`
- `http://localhost:5180`

En modo `Development` se abre la documentación interactiva en `/scalar`.

### Configuración

Los valores se definen en `ClinicaTurnos/Web/appsettings.json`:

| Clave | Descripción | Valor por defecto |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | Cadena de conexión a SQL Server | LocalDB `ClinicaTurnosDb` |
| `Jwt:Issuer` / `Jwt:Audience` | Emisor y audiencia del token | `ClinicaTurnos.Api` / `ClinicaTurnos.Clientes` |
| `Jwt:Secret` | Clave de firma del JWT | Solo en `appsettings.Development.json` |
| `ApiFeriados:BaseUrl` | API externa de feriados | `https://api.argentinadatos.com/v1/` |

> ⚠️ En producción `Jwt:Secret` **no debe versionarse**: se carga como variable de entorno (`Jwt__Secret`). La clave de `appsettings.Development.json` es solo para desarrollo local.

## 🧪 Probar la API

- **Scalar:** `/scalar` (la especificación OpenAPI está en `/openapi/v1.json`).
- **Archivo `.http`:** abrí `ClinicaTurnos/Web/ClinicaTurnos.Web.http` en Visual Studio y ejecutá los requests **en orden** la primera vez (asumen base vacía). Cubre el flujo completo: registro, login, consultorio y médico, disponibilidad, reserva, superposición, feriado, pago, confirmación, cancelación y errores `401`, `404` y `409`.
- Para probar la API deployada, cambiá la variable `@host` por la URL de Azure.

## ✅ Tests

El proyecto `ClinicaTurnos.Tests` contiene tests unitarios de los controllers, con **xUnit** y **Moq**.

```bash
dotnet test
```

## 🔄 CI/CD y deploy

Con **GitHub Actions**:

| Workflow | Cuándo corre | Qué hace |
|---|---|---|
| `ci-pipeline.yml` | Push y PR a `main` y `develop` | Restaura, compila y ejecuta los tests |
| `deploy-azure.yml` | Push a `main` (o manual) | Compila, publica y despliega en **Azure App Service** (`clinicaturnos-api`) |

El deploy usa el secret `AZURE_WEBAPP_PUBLISH_PROFILE`, y las migraciones se aplican solas en cada arranque.

## 👥 Autores

Stéfano Monti, Joaquín Morón, Julián Civriati.

Proyecto desarrollado para la cátedra de **Programación IV — UTN**.
