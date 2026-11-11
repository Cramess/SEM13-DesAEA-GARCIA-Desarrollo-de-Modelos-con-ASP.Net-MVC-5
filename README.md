# Gimnasio API – Semana 13

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

