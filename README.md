# Gimnasio API – Semana 13

Web API REST **en capas** con ASP.NET Core (.NET 10), Entity Framework Core 10 (base de datos primero), DTOs y AutoMapper.
Tema: **gimnasio**, con la relación 1:N **Planes (1) → Socios (N)**.

## Arquitectura

```
Gimnasio.Api  ──►  Gimnasio.Negocio  ──►  Gimnasio.Datos  ──►  Gimnasio.Entidades
(controladores,     (servicios, reglas,    (DbContext,          (entidades y DTOs)
 Program.cs)         MappingProfile,        repositorios)
                     excepciones)
```

| Proyecto | Contenido |
|---|---|
| `Gimnasio.Entidades` | `Plan`, `Socio` (generadas con Scaffold) y los DTOs en `Dtos/` |
| `Gimnasio.Datos` | `GimnasioContext`, `IPlanRepositorio`/`PlanRepositorio`, `ISocioRepositorio`/`SocioRepositorio` |
| `Gimnasio.Negocio` | `IPlanServicio`/`PlanServicio`, `ISocioServicio`/`SocioServicio`, `MappingProfile`, `ReglaNegocioException`, `DatoInvalidoException` |
| `Gimnasio.Api` | `PlanesController`, `SociosController`, `Program.cs` (inyección de dependencias, Swagger, CORS) |

Los controladores solo usan servicios (por su interfaz) y DTOs: nunca el `DbContext` ni las entidades. Por eso las entidades no necesitan `[JsonIgnore]` y en Swagger solo aparecen DTOs.

## Base de datos

`Database/GimnasioDB.sql` crea `GimnasioDB` con datos de ejemplo (5 planes, 6 socios). El modelo se generó con:

```
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=GimnasioDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --project Gimnasio.Datos --startup-project Gimnasio.Api --output-dir Models --context GimnasioContext --no-onconfiguring --force
```

Después se movieron las entidades a `Gimnasio.Entidades` (namespace `Gimnasio.Entidades`), se renombró `Plane` → `Plan` (singularización en inglés) y se quitó `HasDefaultValue(true)` de `Activo`, porque con un `bool` EF ignoraría el valor `false` al insertar.

## Endpoints (11)

| Método | Ruta | Respuestas |
|---|---|---|
| GET | `/api/planes?activo=&precioMax=` | 200 |
| GET | `/api/planes/{id}` | 200, 404 |
| GET | `/api/planes/{id}/socios` (ruta anidada) | 200, 404 |
| POST | `/api/planes` | 201, 400, 409 |
| PUT | `/api/planes/{id}` | 204, 400, 404, 409 |
| DELETE | `/api/planes/{id}` | 204, 404, 409 |
| GET | `/api/socios?nombre=&planId=&edadMin=&edadMax=` | 200 |
| GET | `/api/socios/{id}` | 200, 404 |
| POST | `/api/socios` | 201, 400, 409 |
| PUT | `/api/socios/{id}` | 204, 400, 404, 409 |
| DELETE | `/api/socios/{id}` | 204, 404 |

## Reglas de negocio (capa de Negocio → `ReglaNegocioException` → 409)

1. El nombre del plan es único.
2. No se desactiva un plan que tiene socios inscritos.
3. No se elimina un plan que tiene socios inscritos.
4. El DNI del socio es único.
5. No se inscribe a un socio en un plan inactivo.

Si el `PlanId` no existe, el servicio lanza `DatoInvalidoException` y el controlador responde **400** con `ValidationProblem`.

## Sustentación de los mapeos (`MappingProfile`)

| Mapeo | Origen → Destino | ¿Por qué en ese sentido? | Configuración especial |
|---|---|---|---|
| `Plan → PlanDto` | Entidad → DTO de lectura | Es lo que la API devuelve al cliente: la entidad no debe salir de la API | `ForMember`: `CantidadSocios` toma `Socios.Count` (el repositorio hace `Include(p => p.Socios)`) |
| `PlanCrearDto → Plan` | DTO de entrada → Entidad | Llega del cliente y se convierte en entidad para guardarla con `Map<Plan>(dto)`; no se devuelve | Ninguna: los nombres coinciden |
| `PlanActualizarDto → Plan` | DTO de entrada → Entidad existente | Se aplica sobre el plan ya cargado con `Map(dto, plan)` para actualizar solo los campos que envía el cliente | Ninguna: `PlanId` y `Socios` no están en el DTO, así que se conservan |
| `Socio → SocioDto` | Entidad → DTO de lectura | Es lo que la API devuelve: en vez del objeto `Plan` (que provocaría ciclos) se envía un dato simple | `ForMember`: `Plan` toma `Plan.Nombre` (nombre de la entidad relacionada) |
| `SocioCrearDto → Socio` | DTO de entrada → Entidad | Llega del cliente al inscribir un socio y se guarda con `Map<Socio>(dto)` | `ForMember(FechaInscripcion).Ignore()`: la fecha la asigna el servicio con la fecha de hoy, no el cliente |
| `SocioActualizarDto → Socio` | DTO de entrada → Entidad existente | Se aplica sobre el socio cargado con `Map(dto, socio)` | Ninguna: `SocioId`, `FechaInscripcion` y `Plan` no están en el DTO y se conservan |

**No se usa `ReverseMap()`**: ningún DTO de lectura se convierte en entidad, ni ninguna entidad en DTO de entrada, así que cada mapeo se define solo en el sentido que se usa.

## Ejecutar

1. Ejecutar `Database/GimnasioDB.sql` en `.\SQLEXPRESS`.
2. `dotnet run --project Gimnasio.Api` y abrir `http://localhost:5102/swagger`.
3. Probar con `Gimnasio.Api/Gimnasio.Api.http` o Swagger.

AutoMapper 16 muestra en el log una advertencia de licencia; es normal en desarrollo. La clave se puede indicar en `AutoMapper:LicenseKey`.
