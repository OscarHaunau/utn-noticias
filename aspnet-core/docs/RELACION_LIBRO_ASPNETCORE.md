# Relacion del TP con el libro "El pequeño libro de ASP.NET Core"

> Nota: usamos paginas del PDF en español de Nate Barbettini que tiene 137 paginas. Si en clase usan otra edicion/PDF, puede cambiar algun numero de pagina, pero el concepto es el mismo.

## Paginas concretas usadas como referencia

- **p.35**: separar logica en servicios para no poner todo en controladores.
- **p.40**: inyeccion de dependencias por constructor.
- **p.42**: `Task`, `async` y `await` para esperar base de datos o APIs externas.
- **p.52**: Entity Framework Core como ORM para guardar clases C# en una base de datos.
- **p.55**: agregar entidades al contexto/configuracion de base de datos.
- **p.63**: consultas con LINQ sobre datos.
- **p.105**: importancia de pruebas automaticas.
- **p.106**: pruebas unitarias usando dependencias falsas/simuladas.
- **p.114**: concepto de pruebas de integracion.

## Relacion directa con el codigo

| Punto pedido por la catedra | Archivo/metodo | Pagina del libro para explicar |
|---|---|---|
| Buscar en NewsAPI | `UtnNoticiasService.Search` / `NewsApiService.GetNewsAsync` | Servicios p.35, DI p.40, async/API p.42 |
| Crear y actualizar lista | `ReadingListAppService.CreateAsync` / `UpdateAsync` | Servicios p.35, EF Core p.52 |
| Eliminar lista | `ReadingListAppService.DeleteAsync` | Servicios p.35, EF Core p.52 |
| Agregar noticia a lista | `ReadingListAppService.AddItemAsync` | Reglas fuera del controlador p.35 |
| Crear alerta | `AlertAppService.CreateAsync` | Servicios p.35, EF Core p.52 |
| Obtener notificaciones | `AlertAppService.GetMyNotificationsAsync` | Consultas/LINQ p.63 |
| Ejecutar alertas | `AlertAppService.RunAlertsAsync` | Async/API p.42 |
| Estadisticas de monitoreo | `ApiMonitoringAppService.GetStatsAsync` | EF Core p.52, LINQ p.63 |
| Pruebas | `test/UtnNoticias.Application.Tests` | Pruebas p.105, fakes p.106, integracion p.114 |

## Explicacion simple para defensa

> "Relacionamos el TP con el libro porque usamos los conceptos principales de ASP.NET Core: servicios, inyeccion de dependencias, metodos asincronicos, EF Core para persistencia y pruebas automaticas. Aunque el libro usa una version anterior, esos conceptos siguen aplicando."

## Donde quedaron comentarios en codigo

Se dejaron comentarios `Libro: p.X` en las partes principales:

- `src/UtnNoticias.Application/News/UtnNoticiasService.cs`
- `src/UtnNoticias.Domain/News/NewsApiService.cs`
- `src/UtnNoticias.Application/Alerts/AlertAppService.cs`
- `src/UtnNoticias.Application/Monitoring/ApiMonitoringAppService.cs`
- `src/UtnNoticias.Domain/Alerts/NewsAlert.cs`
- `src/UtnNoticias.Domain/Alerts/NewsNotification.cs`
- `src/UtnNoticias.Domain/Monitoring/ApiAccessLog.cs`
- `src/UtnNoticias.EntityFrameworkCore/EntityFrameworkCore/UtnNoticiasDbContext.cs`
- `test/UtnNoticias.Application.Tests/News/FakeNewsService.cs`
- `test/UtnNoticias.Application.Tests/Alerts/AlertAppServiceTests.cs`
- `test/UtnNoticias.Application.Tests/Monitoring/ApiMonitoringAppServiceTests.cs`
