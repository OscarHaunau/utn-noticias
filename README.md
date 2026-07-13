# UtnNoticias - TP Final Backend

Este repositorio contiene la entrega local del TP final de **UtnNoticias**.
El foco de la entrega esta en el **backend con ASP.NET Core / ABP**, como pidio la catedra.

## Estado de la entrega

| Requisito de la catedra | Estado | Donde verlo |
|---|---:|---|
| Buscar en NewsAPI y devolver resultados | Hecho | `UtnNoticiasService.Search` |
| Crear y actualizar lista de lectura | Hecho | `ReadingListAppService.CreateAsync` / `UpdateAsync` |
| Eliminar lista de lectura | Hecho | `ReadingListAppService.DeleteAsync` |
| Agregar noticia a lista de lectura | Hecho | `ReadingListAppService.AddItemAsync` |
| Crear alerta desde texto de busqueda | Hecho | `AlertAppService.CreateAsync` |
| Obtener notificaciones persistidas | Hecho | `AlertAppService.GetMyNotificationsAsync` |
| Ejecucion asincronica de alertas | Hecho | `AlertAppService.RunAlertsAsync` |
| Estadisticas de monitoreo de accesos | Hecho | `ApiMonitoringAppService.GetStatsAsync` |
| Pruebas unitarias / integracion | Hecho | `aspnet-core/test` |

## Punto importante: ejecucion asincronica de alertas

El metodo que representa la ejecucion asincronica pedida por la catedra es:

```txt
AlertAppService.RunAlertsAsync()
```

La idea es simple:

1. Lee las alertas activas guardadas en base de datos.
2. Por cada alerta busca noticias usando el texto de busqueda.
3. Guarda las noticias encontradas como notificaciones.
4. Registra el acceso a la API para el monitoreo.

No se agrego un background job automatico para mantener el TP simple y facil de explicar.
Como el enunciado lista metodos de AppServices, se dejo como metodo ejecutable desde Swagger o desde otro proceso.

## Como ejecutar en local

La guia completa esta en:

```txt
aspnet-core/docs/EJECUTAR_LOCAL_COMPLETO.md
```

Resumen rapido:

```bash
docker start utnnoticias-sql
```

Backend:

```bash
cd aspnet-core/src/UtnNoticias.HttpApi.Host
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' ASPNETCORE_ENVIRONMENT=Development ~/.dotnet/dotnet run --urls https://localhost:44311
```

Angular:

```bash
cd angular
pnpm install
pnpm ng serve --host localhost --port 4200
```

## NewsAPI key

Para que la busqueda real traiga noticias, configurar una key local:

```bash
export NEWS_API_KEY='tu_key_de_newsapi'
```

Si no se configura, el backend levanta igual, pero la busqueda devuelve una lista vacia.

## Pruebas

Backend:

```bash
cd aspnet-core
~/.dotnet/dotnet build UtnNoticias.sln
~/.dotnet/dotnet test UtnNoticias.sln --no-build
```

Angular:

```bash
cd angular
pnpm build
```

## Documentacion para la presentacion

- `aspnet-core/docs/TP_BACKEND_RESUMEN.md`: resumen funcional para explicar el TP.
- `aspnet-core/docs/RELACION_LIBRO_ASPNETCORE.md`: relacion con paginas del libro de Nate Barbettini.
- `aspnet-core/docs/EJECUTAR_LOCAL_COMPLETO.md`: pasos para correr todo local.

