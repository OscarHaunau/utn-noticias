# Resumen Backend TP Final

Este resumen es para explicar la entrega de forma simple en la presentacion.

## Que se pidio

La catedra pidio cubrir estas funciones de backend:

1. Buscar noticias en NewsAPI y devolver resultados.
2. Crear y actualizar listas de lectura.
3. Eliminar listas de lectura.
4. Agregar una noticia de busqueda a una lista.
5. Crear una alerta a partir de un texto de busqueda.
6. Obtener notificaciones persistidas del usuario.
7. Ejecutar una busqueda de alertas y guardar notificaciones.
8. Obtener estadisticas de monitoreo de accesos a la API.

## Como se resolvio

La solucion se hizo basica, separada en AppServices:

- `UtnNoticiasService`: busca noticias y guarda un log de acceso.
- `ReadingListAppService`: maneja listas de lectura.
- `AlertAppService`: crea alertas, ejecuta alertas y devuelve notificaciones.
- `ApiMonitoringAppService`: devuelve cantidad de accesos, errores y tiempo promedio.

## Flujo de alertas

1. El usuario crea una alerta con un texto, por ejemplo `bitcoin`.
2. `RunAlertsAsync` toma las alertas activas.
3. Por cada alerta llama al servicio de noticias.
4. Cada noticia encontrada se guarda como `NewsNotification`.
5. La pantalla de notificaciones puede usar `GetMyNotificationsAsync`.

Importante: `GetMyNotificationsAsync` no llama a NewsAPI. Solo lee datos ya guardados.

## Monitoreo

Cada busqueda guarda un `ApiAccessLog` con:

- texto buscado,
- fecha de inicio,
- fecha de fin,
- duracion,
- si hubo error.

Con esos registros se calculan estadisticas simples.

## Pruebas

Los tests no llaman a NewsAPI real. Usan `FakeNewsService` para que las pruebas sean rapidas y no dependan de internet ni de una API key.

Comandos usados:

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/aspnet-core
dotnet build UtnNoticias.sln
dotnet test UtnNoticias.sln --no-build
```
