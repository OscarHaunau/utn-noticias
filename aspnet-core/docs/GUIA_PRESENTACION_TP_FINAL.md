# Guia de presentacion TP Final - UtnNoticias

Esta guia es para preparar y defender el TP final. La idea es que puedas seguirla como un guion: que abrir, que mostrar, que tocar, que decir y como responder preguntas.

## Resumen de 30 segundos

> "Nuestro trabajo se centro en el backend, como pidio la catedra. Implementamos los AppServices pedidos: busqueda de noticias con NewsAPI, listas de lectura, alertas, notificaciones persistidas, ejecucion asincronica de alertas y monitoreo de accesos a la API. El frontend Angular levanta y permite login, pero la demo funcional del TP la mostramos desde Swagger porque el frontend no era obligatorio. Tambien agregamos pruebas automaticas y documentacion para correrlo con SQL Server en otras maquinas."

## Que conviene tener abierto antes de empezar

Abrir estas ventanas/tabs antes de presentar:

1. **GitHub del proyecto**
   - `https://github.com/OscarHaunau/utn-noticias`
   - Mostrar README y carpeta `aspnet-core/docs`.

2. **VS Code o editor** en la raiz del repo:
   - `/Users/oscarhaunau/Documents/asp-net-core/UtnNoticias`
   - Tener abiertos estos archivos:
     - `README.md`
     - `aspnet-core/docs/TP_BACKEND_RESUMEN.md`
     - `aspnet-core/docs/EJECUTAR_CON_SQL_SERVER.md`
     - `aspnet-core/src/UtnNoticias.Application/Alerts/AlertAppService.cs`
     - `aspnet-core/src/UtnNoticias.Application/ReadingLists/ReadingListAppService.cs`
     - `aspnet-core/src/UtnNoticias.Application/Monitoring/ApiMonitoringAppService.cs`
     - `aspnet-core/src/UtnNoticias.Domain/News/NewsApiService.cs`
     - `aspnet-core/test/UtnNoticias.Application.Tests/Alerts/AlertAppServiceTests.cs`

3. **Terminal 1: SQL Server / migraciones**
   - Para mostrar que corre con SQL Server.

4. **Terminal 2: Backend**
   - Backend en `https://localhost:44311`.

5. **Terminal 3: Angular**
   - Frontend en `http://localhost:4200`.

6. **Browser tabs**
   - `http://localhost:4200`
   - `https://localhost:44311/swagger`

## Preparacion tecnica antes de la demo

### 1. Verificar rama y repo

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias
git status -sb
```

Deberia verse algo como:

```txt
## main...origin/main
```

Decir:

> "La entrega esta subida en GitHub en la rama main."

### 2. Levantar SQL Server local

Si ya existe el contenedor:

```bash
docker start utnnoticias-sql
```

Si estas en otra maquina y no existe:

```bash
docker run -d --name utnnoticias-sql \
  -e 'ACCEPT_EULA=Y' \
  -e 'MSSQL_SA_PASSWORD=UtnNoticias_12345!' \
  -p 14333:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

Decir:

> "Usamos SQL Server. SQLite aparece solo en tests para correr rapido en memoria. La app real usa SQL Server con Entity Framework Core."

### 3. Ejecutar migraciones

```bash
cd aspnet-core/src/UtnNoticias.DbMigrator
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' ~/.dotnet/dotnet run
```

Decir:

> "El migrador crea la base, aplica migraciones y carga datos iniciales de ABP, como el usuario admin."

### 4. Levantar backend

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/aspnet-core/src/UtnNoticias.HttpApi.Host
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' \
ASPNETCORE_ENVIRONMENT=Development \
~/.dotnet/dotnet run --urls https://localhost:44311
```

Si vas a mostrar busqueda real en NewsAPI, antes configurar:

```bash
export NEWS_API_KEY='tu_key_de_newsapi'
```

Decir:

> "La key de NewsAPI no esta hardcodeada. Se lee desde una variable de entorno para no subir secretos a GitHub."

### 5. Levantar Angular

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/angular
pnpm install
pnpm ng serve --host localhost --port 4200
```

Decir:

> "Angular no era obligatorio, pero lo dejamos funcionando para mostrar que el proyecto completo levanta. Las funcionalidades del TP las mostramos desde Swagger porque el alcance obligatorio era backend."

## Orden recomendado de la presentacion

Duracion ideal: 12 a 18 minutos.

| Minuto | Que mostrar | Que decir |
|---:|---|---|
| 0-1 | GitHub / README | Entrega subida, backend completo, frontend base funcionando. |
| 1-3 | Requisitos del TP | Mostrar checklist de funcionalidades completas. |
| 3-5 | Arquitectura en carpetas | Explicar capas: Domain, Application, Contracts, EF Core, Host, Tests. |
| 5-9 | Swagger demo | Ejecutar endpoints principales. |
| 9-12 | Codigo clave | AppServices, entidades, DbContext, NewsApiService. |
| 12-15 | Tests | Mostrar tests y correr `dotnet test`. |
| 15-18 | Preguntas | Responder sobre SQL Server, async, frontend, patrones, seguridad. |

## Guion paso a paso

### Paso 1: mostrar GitHub

Abrir:

```txt
https://github.com/OscarHaunau/utn-noticias
```

Mostrar:

- `README.md`
- `aspnet-core/docs`
- `aspnet-core/docs/TP_BACKEND_RESUMEN.md`
- `aspnet-core/docs/EJECUTAR_CON_SQL_SERVER.md`

Decir:

> "Dejamos el proyecto subido a GitHub, publico, con documentacion para correrlo localmente, correrlo con SQL Server y preparar la presentacion."

### Paso 2: explicar que se pidio y que se hizo

Abrir:

```txt
aspnet-core/docs/TP_BACKEND_RESUMEN.md
```

Mostrar la lista de requisitos.

Decir:

> "La catedra redujo el alcance obligatorio al backend. Por eso implementamos los metodos de AppServices pedidos y los probamos con tests."

Checklist para decir:

- Busqueda en NewsAPI: hecho.
- Crear/actualizar listas: hecho.
- Eliminar listas: hecho.
- Agregar noticia a lista: hecho.
- Crear alerta: hecho.
- Obtener notificaciones persistidas: hecho.
- Ejecutar alertas de forma asincronica: hecho con `RunAlertsAsync`.
- Monitoreo de accesos: hecho con `ApiAccessLog`.
- Tests: hechos.

### Paso 3: mostrar Angular funcionando

Abrir:

```txt
http://localhost:4200
```

Tocar:

1. Ver que carga la pagina principal.
2. Hacer login:

```txt
Usuario: admin
Password: 1q2w3E*
```

3. Mostrar que aparece el usuario `admin` y el menu de administracion.

Decir:

> "El frontend levanta y permite login. No agregamos pantallas propias para cada caso porque el frontend no era obligatorio. Para no gastar tiempo en UI, priorizamos backend, persistencia y pruebas."

Si preguntan por que no hay pantallas del TP:

> "Porque la catedra aclaro que las funcionalidades obligatorias estaban vinculadas al backend. El frontend era bienvenido pero no obligatorio. Dejamos Angular funcionando como base y mostramos los casos de uso desde Swagger."

### Paso 4: abrir Swagger

Abrir:

```txt
https://localhost:44311/swagger
```

Mostrar grupos/endpoints relacionados con:

- `UtnNoticias` o `News`
- `ReadingList`
- `Alert`
- `ApiMonitoring`

Decir:

> "ABP expone los AppServices como endpoints. Por eso desde Swagger podemos probar los metodos que pidio la catedra."

Si Swagger pide autorizacion:

1. Click en **Authorize**.
2. Iniciar sesion con `admin` / `1q2w3E*` si redirige al login.
3. Si alguna operacion de lista devuelve 403, revisar permisos del rol `admin` desde Angular:
   - `Administration`
   - `Identity Management`
   - `Roles`
   - `admin`
   - `Permissions`
   - marcar permisos de `ReadingLists`

Frase para explicar permisos:

> "Las listas de lectura tienen autorizacion porque pertenecen a un usuario. El servicio toma el usuario actual con `CurrentUser.Id`."

## Demo de Swagger: que tocar exactamente

> Consejo: si la API externa NewsAPI no responde o no hay key, no te trabes. Mostra que el backend no se rompe y despues mostra los tests con `FakeNewsService`.

### Demo A: busqueda en NewsAPI

En Swagger buscar el metodo de busqueda:

```txt
UtnNoticiasService.Search
```

o algun endpoint con nombre similar a:

```txt
Search
```

Parametro sugerido:

```txt
query = technology
```

o:

```txt
query = bitcoin
```

Decir:

> "Este metodo llama a NewsAPI y devuelve una lista de noticias. Tambien registra el acceso para despues poder calcular estadisticas de monitoreo."

Si devuelve lista vacia:

> "En esta maquina puede estar sin `NEWS_API_KEY` o la API puede limitar resultados. Por eso las pruebas automaticas usan un servicio falso para no depender de internet."

Archivo para mostrar despues:

```txt
aspnet-core/src/UtnNoticias.Domain/News/NewsApiService.cs
```

### Demo B: crear lista de lectura

Buscar en Swagger:

```txt
ReadingList - CreateAsync
```

Body:

```json
{
  "name": "Lista demo presentacion"
}
```

Copiar el `id` devuelto.

Decir:

> "La lista queda asociada al usuario logueado. No mostramos listas de otros usuarios."

Archivo para mostrar:

```txt
aspnet-core/src/UtnNoticias.Application/ReadingLists/ReadingListAppService.cs
```

### Demo C: actualizar lista de lectura

Buscar:

```txt
ReadingList - UpdateAsync
```

Usar el `id` de la lista creada.

Body:

```json
{
  "name": "Lista demo actualizada"
}
```

Decir:

> "La actualizacion primero busca una lista propia. Si el ID no existe o pertenece a otro usuario, se trata como no encontrada."

### Demo D: agregar una noticia a la lista

Buscar:

```txt
ReadingList - AddItemAsync
```

Usar el `id` de la lista.

Body:

```json
{
  "title": "Noticia de prueba para la presentacion",
  "url": "https://news.example/presentacion-utn",
  "author": "Autor demo",
  "description": "Descripcion corta de la noticia",
  "urlToImage": "https://news.example/imagen.jpg",
  "publishedAt": "2026-07-13T12:00:00Z",
  "content": "Contenido de ejemplo"
}
```

Decir:

> "Guardamos una noticia resultado de busqueda dentro de una lista de lectura. Tambien validamos que no se repita por URL."

Si te preguntan donde esta la regla de duplicado:

```txt
aspnet-core/src/UtnNoticias.Domain/ReadingLists/ReadingList.cs
```

Metodo:

```txt
AddItem(...)
```

### Demo E: crear alerta

Buscar:

```txt
Alert - CreateAsync
```

Body:

```json
{
  "searchText": "inteligencia artificial"
}
```

Decir:

> "Una alerta guarda el texto que el usuario quiere monitorear. Por ejemplo, `inteligencia artificial`."

Archivo para mostrar:

```txt
aspnet-core/src/UtnNoticias.Application/Alerts/AlertAppService.cs
```

### Demo F: ejecutar alertas

Buscar:

```txt
Alert - RunAlertsAsync
```

Ejecutar.

Decir:

> "Este es el punto de ejecucion asincronica pedido por la catedra. Recorre alertas activas, llama a la API externa, persiste notificaciones y registra monitoreo."

Importante para explicar:

- Es asincronico porque usa `async` / `await`.
- No bloquea el hilo esperando la API.
- No agregamos timer/background job para mantenerlo simple.
- Si mas adelante se quisiera automatico, este mismo metodo podria llamarse desde un background job.

Si preguntan: "Pero asincronico no significa automatico?"

Responder:

> "No necesariamente. Asincronico significa que el metodo espera operaciones externas sin bloquear, usando `Task`, `async` y `await`. Automatico seria agregar un scheduler o background job. Como el enunciado pidio metodos de AppServices, lo dejamos como metodo de AppService."

### Demo G: ver notificaciones persistidas

Buscar:

```txt
Alert - GetMyNotificationsAsync
```

Ejecutar.

Decir:

> "Este metodo no llama a NewsAPI. Solo lee notificaciones ya guardadas en base de datos. Eso era una aclaracion explicita del enunciado."

Si no hay notificaciones:

> "Si no hay key de NewsAPI o no hubo resultados, puede estar vacio. El comportamiento importante es que lee de la base y no dispara otra busqueda. Eso esta cubierto por tests."

Test relacionado:

```txt
Debe_Devolver_Notificaciones_Persistidas_Sin_Buscar_De_Nuevo
```

Archivo:

```txt
aspnet-core/test/UtnNoticias.Application.Tests/Alerts/AlertAppServiceTests.cs
```

### Demo H: monitoreo de accesos

Buscar:

```txt
ApiMonitoring - GetStatsAsync
```

Ejecutar.

Decir:

> "Cada busqueda o ejecucion de alerta guarda un `ApiAccessLog`. Este endpoint resume total de accesos, errores y tiempo promedio."

Archivo:

```txt
aspnet-core/src/UtnNoticias.Application/Monitoring/ApiMonitoringAppService.cs
```

## Arquitectura: como explicarla simple

Usar esta frase:

> "Seguimos una arquitectura por capas tipica de ABP/ASP.NET Core. La UI o Swagger entra por HTTP, llama a AppServices, los AppServices coordinan casos de uso, el dominio contiene entidades y reglas simples, y EF Core persiste en SQL Server."

### Capas del proyecto

| Capa | Carpeta | Que hace | Como explicarlo |
|---|---|---|---|
| Host/API | `UtnNoticias.HttpApi.Host` | Levanta la app, Swagger, auth, config | "Es el punto de entrada web." |
| Contratos | `UtnNoticias.Application.Contracts` | Interfaces y DTOs | "Define que datos entran y salen." |
| Aplicacion | `UtnNoticias.Application` | AppServices / casos de uso | "Aca esta lo que pidio la catedra." |
| Dominio | `UtnNoticias.Domain` | Entidades y reglas | "Aca viven objetos como ReadingList y NewsAlert." |
| EF Core | `UtnNoticias.EntityFrameworkCore` | DbContext, tablas, migraciones | "Conecta las entidades con SQL Server." |
| Tests | `test` | Pruebas automaticas | "Verifica que lo pedido funcione." |
| Angular | `angular` | Frontend base | "Levanta y permite login, pero no era obligatorio." |

### Flujo de una request

Ejemplo: agregar noticia a lista.

```txt
Swagger / Angular
   -> ReadingListAppService.AddItemAsync
      -> ReadingList.AddItem
         -> IRepository<ReadingList>
            -> EF Core
               -> SQL Server
```

Explicacion:

> "El AppService no escribe SQL manual. Usa repositorios de ABP, que por debajo usan EF Core y SQL Server."

## Patrones y conceptos usados

### 1. AppService / Service Layer

Archivos:

```txt
UtnNoticiasService.cs
ReadingListAppService.cs
AlertAppService.cs
ApiMonitoringAppService.cs
```

Decir:

> "Separamos la logica de negocio en servicios de aplicacion, en vez de poner todo en controladores. Eso hace mas facil probar y explicar cada caso de uso."

### 2. DTOs

Archivos:

```txt
Application.Contracts/ReadingLists
Application.Contracts/Alerts
Application.Contracts/Monitoring
```

Decir:

> "Los DTOs son objetos simples para entrada y salida. Evitan exponer directamente las entidades de base de datos."

### 3. Repository Pattern

Ejemplo:

```txt
IRepository<ReadingList, Guid>
IRepository<NewsAlert, Guid>
IRepository<ApiAccessLog, Guid>
```

Decir:

> "Usamos repositorios de ABP para consultar y guardar entidades sin escribir SQL manual."

### 4. Dependency Injection

Ejemplo:

```txt
AlertAppService recibe INewsService e IRepository por constructor.
```

Decir:

> "ASP.NET Core inyecta dependencias. Eso nos permite cambiar NewsAPI real por un FakeNewsService en tests."

### 5. Async / Await

Ejemplos:

```txt
Search(...)
RunAlertsAsync()
GetMyNotificationsAsync()
GetStatsAsync()
```

Decir:

> "Las llamadas a base de datos y API externa son asincronicas para no bloquear la aplicacion."

### 6. Entidades con reglas simples

Ejemplo:

```txt
ReadingList.AddItem
NewsAlert.AddNotification
```

Decir:

> "Algunas reglas estan en el dominio, como normalizar nombres, evitar URLs duplicadas o evitar notificaciones repetidas."

### 7. EF Core Code First / Migrations

Archivos:

```txt
UtnNoticiasDbContext.cs
Migrations/202607070001_AddAlertsAndMonitoring.cs
```

Decir:

> "Definimos entidades en C# y EF Core crea o actualiza tablas con migraciones."

Tablas importantes:

```txt
AppReadingLists
AppReadingListItems
AppNewsAlerts
AppNewsNotifications
AppApiAccessLogs
```

### 8. Fake para tests

Archivo:

```txt
test/UtnNoticias.Application.Tests/News/FakeNewsService.cs
```

Decir:

> "En tests no llamamos a NewsAPI real. Usamos un fake para que los tests no dependan de internet ni de una key."

## Como explicar cada requisito con archivo y test

| Requisito | Codigo | Test para mostrar | Explicacion corta |
|---|---|---|---|
| Buscar NewsAPI | `UtnNoticiasService.Search` / `NewsApiService.GetNewsAsync` | `ApiMonitoringAppServiceTests` usa busquedas | "Busca noticias y registra acceso." |
| Crear lista | `ReadingListAppService.CreateAsync` | `Debe_Crear_Lista_Con_Nombre_Valido` | "Crea lista propia del usuario." |
| Actualizar lista | `ReadingListAppService.UpdateAsync` | `Debe_Actualizar_Lista_Propia` | "Actualiza solo si pertenece al usuario." |
| Eliminar lista | `ReadingListAppService.DeleteAsync` | `Debe_Eliminar_Lista` | "Borra la lista y sus items." |
| Agregar noticia | `ReadingListAppService.AddItemAsync` | `Debe_Agregar_Noticia_A_Lista` | "Persiste una noticia dentro de una lista." |
| Crear alerta | `AlertAppService.CreateAsync` | `Debe_Crear_Alerta_De_Noticias` | "Guarda texto a monitorear." |
| Obtener notificaciones | `AlertAppService.GetMyNotificationsAsync` | `Debe_Devolver_Notificaciones_Persistidas_Sin_Buscar_De_Nuevo` | "Lee solo lo persistido." |
| Ejecutar alertas | `AlertAppService.RunAlertsAsync` | `Debe_Ejecutar_Alertas_Y_Guardar_Notificaciones` | "Busca alertas y guarda notificaciones." |
| Monitoreo | `ApiMonitoringAppService.GetStatsAsync` | `Debe_Mostrar_Estadisticas_De_Accesos_A_La_Api` | "Resume accesos y errores." |

## Como explicar los tests

Frase inicial:

> "Agregamos pruebas automaticas para verificar los casos de uso principales. Algunas son mas unitarias sobre reglas simples y otras son de integracion liviana porque levantan el contexto de ABP con repositorios."

### Tests de listas de lectura

Archivo:

```txt
aspnet-core/test/UtnNoticias.Application.Tests/ReadingLists/ReadingListAppServiceTests.cs
```

Que cubren:

- Crear lista con nombre valido.
- Rechazar nombre vacio.
- Actualizar lista propia.
- Agregar noticia.
- Rechazar noticia duplicada por URL.
- Eliminar lista.
- Rechazar operaciones sobre lista ajena.
- Listar solo listas del usuario actual.

Como decirlo:

> "Aca probamos no solo el camino feliz, sino reglas de seguridad y errores: nombre vacio, duplicados y lista ajena."

### Tests de alertas

Archivo:

```txt
aspnet-core/test/UtnNoticias.Application.Tests/Alerts/AlertAppServiceTests.cs
```

Que cubren:

- Crear alerta.
- Ejecutar alertas y guardar notificaciones.
- Obtener notificaciones persistidas sin volver a buscar.

Como decirlo:

> "El test mas importante es el que verifica que `GetMyNotificationsAsync` no vuelve a llamar a NewsAPI. Compara el monitoreo antes y despues."

### Tests de monitoreo

Archivo:

```txt
aspnet-core/test/UtnNoticias.Application.Tests/Monitoring/ApiMonitoringAppServiceTests.cs
```

Que cubre:

- Hacer busquedas.
- Ver que aumenta el total de accesos.
- Ver que no haya errores.
- Ver que el promedio sea valido.

Como decirlo:

> "El monitoreo se calcula desde registros persistidos, no es un valor inventado."

### FakeNewsService

Archivo:

```txt
aspnet-core/test/UtnNoticias.Application.Tests/News/FakeNewsService.cs
```

Como decirlo:

> "Para no depender de internet, los tests usan un FakeNewsService que devuelve noticias fijas. Asi los tests son repetibles."

## Como correr pruebas durante la presentacion

Desde la raiz del backend:

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/aspnet-core
~/.dotnet/dotnet build UtnNoticias.sln
~/.dotnet/dotnet test UtnNoticias.sln --no-build
```

Resultado esperado:

- Build correcto.
- Tests de Application pasan.
- Tests de Domain pasan.
- Tests de EntityFrameworkCore pasan.
- `TestBase` puede decir que no hay pruebas disponibles; eso es normal porque es proyecto base de testing.

Decir:

> "Los tests principales estan en Application.Tests, Domain.Tests y EntityFrameworkCore.Tests. TestBase es infraestructura y no contiene tests propios."

Para Angular:

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/angular
pnpm build
```

Decir:

> "El build de Angular pasa, entonces el frontend base esta sano."

## Como explicar SQL Server vs SQLite

Pregunta posible:

> "Por que aparece SQLite en el proyecto si dicen que usan SQL Server?"

Respuesta:

> "La aplicacion real usa SQL Server. Se ve en `UtnNoticiasEntityFrameworkCoreModule.cs` con `options.UseSqlServer()`. SQLite aparece solo en los proyectos de test para tener una base en memoria rapida y no depender de una instalacion externa."

Archivo para mostrar:

```txt
aspnet-core/src/UtnNoticias.EntityFrameworkCore/EntityFrameworkCore/UtnNoticiasEntityFrameworkCoreModule.cs
```

Documento para mostrar:

```txt
aspnet-core/docs/EJECUTAR_CON_SQL_SERVER.md
```

## Como explicar la seguridad

Puntos para decir:

- Las listas pertenecen a un usuario (`OwnerId`).
- El AppService usa `CurrentUser.Id`.
- Las operaciones de listas tienen `[Authorize]`.
- Si una lista es de otro usuario, no se devuelve.
- Hay tests que intentan operar sobre una lista ajena y esperan error.

Frase:

> "La seguridad basica esta en que cada usuario opera sobre sus propias listas. No alcanza con pasar un ID: el servicio filtra tambien por OwnerId."

## Como explicar monitoreo

Puntos:

- Cada busqueda registra un `ApiAccessLog`.
- Cada ejecucion de alerta tambien registra acceso.
- Se guarda:
  - texto buscado,
  - inicio,
  - fin,
  - duracion,
  - si hubo error,
  - mensaje de error si corresponde.
- `ApiMonitoringAppService.GetStatsAsync` calcula estadisticas.

Frase:

> "No es monitoreo avanzado, pero cumple el requisito: nos permite saber cuantos accesos a la API hubo, cuantos fallaron y cuanto tardaron en promedio."

## Como explicar la ejecucion asincronica

Punto pedido:

```txt
Ejecucion asincronica que busque los textos de las alertas en la API y persista la informacion de las notificaciones.
```

Metodo:

```txt
AlertAppService.RunAlertsAsync()
```

Explicacion:

> "El metodo busca alertas activas, llama a NewsAPI con `await`, guarda notificaciones y actualiza la fecha de ultima ejecucion. Es asincronico por el uso de `Task`, `async` y `await`."

Si preguntan por background job:

> "No agregamos un background job automatico para mantener el alcance simple. Pero el metodo ya esta separado y podria ser llamado desde un job de ABP en una mejora futura."

## Como explicar el frontend

Frase recomendada:

> "El frontend Angular funciona y permite login. No hicimos pantallas propias para cada metodo porque la catedra aclaro que el frontend no era obligatorio. Entonces usamos Swagger para mostrar los AppServices del backend."

Si preguntan si se podria hacer:

> "Si, se podria agregar una pantalla simple que consuma los endpoints de News, ReadingList, Alert y Monitoring. Pero para esta entrega priorizamos backend, persistencia y tests."

## Relacion con el libro de ASP.NET Core

Documento:

```txt
aspnet-core/docs/RELACION_LIBRO_ASPNETCORE.md
```

Frase:

> "Relacionamos partes del codigo con conceptos del libro: servicios, inyeccion de dependencias, async/await, EF Core, LINQ y pruebas automaticas. Aunque el libro usa otra version, los conceptos siguen vigentes."

Si te piden ejemplos:

| Concepto | Donde se ve |
|---|---|
| Servicios | `AlertAppService`, `ReadingListAppService` |
| Inyeccion de dependencias | constructores con `IRepository` / `INewsService` |
| Async/await | metodos que devuelven `Task` |
| EF Core | `UtnNoticiasDbContext` y migraciones |
| LINQ | filtros `Where`, `OrderBy`, `Average` |
| Tests | carpeta `aspnet-core/test` |

## Preguntas posibles y respuestas cortas

### 1. Que parte del TP esta completa?

> "Todo lo obligatorio del backend: busqueda, listas, alertas, notificaciones, ejecucion asincronica, monitoreo y tests."

### 2. El frontend esta completo?

> "No como UI del TP. Funciona como app Angular base con login, pero las pantallas del TP no eran obligatorias. La demo funcional del backend se hace por Swagger."

### 3. Por que usaron Swagger para mostrar funcionalidades?

> "Porque la catedra pidio metodos de AppServices. Swagger nos permite invocarlos directamente y ver request/response sin construir UI adicional."

### 4. Que es un AppService?

> "Es una clase de la capa de aplicacion que representa casos de uso. Por ejemplo crear lista, crear alerta o ejecutar alertas."

### 5. Por que usan DTOs?

> "Para definir datos de entrada y salida sin exponer entidades internas."

### 6. Donde se guarda la informacion?

> "En SQL Server mediante Entity Framework Core. Las tablas nuevas principales son `AppNewsAlerts`, `AppNewsNotifications` y `AppApiAccessLogs`, ademas de listas de lectura."

### 7. Como evitan duplicados?

> "En listas, se evita duplicar noticias por URL. En alertas, se evita duplicar notificaciones por URL dentro de una alerta."

### 8. Que pasa si NewsAPI falla?

> "Se registra el acceso con error en `ApiAccessLog`. En busqueda se propaga el error si ocurre una excepcion. Si no hay key, el servicio devuelve lista vacia para no romper la app local."

### 9. Por que la key no esta en GitHub?

> "Porque es un secreto. Se configura localmente con `NEWS_API_KEY`."

### 10. Como prueban algo que depende de NewsAPI?

> "En tests usamos `FakeNewsService`, que devuelve noticias fijas. Asi no dependemos de internet ni de una key."

### 11. Son pruebas unitarias o de integracion?

> "Hay pruebas de comportamiento de AppServices usando el entorno de test de ABP, que son mas de integracion liviana. Tambien hay reglas de dominio probadas indirectamente, como duplicados y validaciones."

### 12. Por que SQLite en tests?

> "Para tener una base en memoria rapida. La app real usa SQL Server."

### 13. Que patron de arquitectura usaron?

> "Capas: Host/API, Application, Domain, EntityFrameworkCore y Tests. Ademas usamos Repository, DTOs, Dependency Injection y Service Layer."

### 14. Como se controla que un usuario no vea listas de otro?

> "El servicio filtra por `OwnerId == CurrentUser.Id`. Hay un test que intenta operar sobre una lista ajena y falla."

### 15. Por que `RunAlertsAsync` no corre solo cada cierto tiempo?

> "Porque el enunciado pidio metodos de AppServices. Lo dejamos como metodo ejecutable. Como mejora futura podria invocarse desde un background job."

### 16. Que mejora harian si tuvieran mas tiempo?

> "Agregar una UI Angular especifica para el TP y un background job programado para ejecutar alertas automaticamente."

### 17. Que pasa si hay muchas alertas?

> "La version actual es simple y suficiente para el TP. Si creciera, se podria paginar alertas, procesar en lotes y agregar reintentos/background jobs."

### 18. Como se relaciona con el libro?

> "Usamos conceptos del libro: servicios, DI, async/await, EF Core, LINQ y tests. Dejamos un documento con referencias por pagina."

## Plan B si algo falla en vivo

### Si no levanta SQL Server

Mostrar:

```txt
aspnet-core/docs/EJECUTAR_CON_SQL_SERVER.md
```

Decir:

> "La guia documenta como levantar SQL Server con Docker o con SQL Server instalado."

### Si NewsAPI no devuelve resultados

Decir:

> "La API externa puede depender de la key o internet. Por eso los tests usan FakeNewsService."

Mostrar:

```txt
test/UtnNoticias.Application.Tests/News/FakeNewsService.cs
```

Y correr:

```bash
~/.dotnet/dotnet test UtnNoticias.sln --no-build
```

### Si Swagger pide permisos

Decir:

> "Los endpoints protegidos requieren usuario y permisos. Podemos mostrar el login en Angular y los tests que validan la logica."

### Si Angular no carga

Decir:

> "Angular no era obligatorio. El backend se puede demostrar por Swagger y tests. Igualmente el build de Angular pasa con `pnpm build`."

## Checklist final antes de presentar

- [ ] Repo abierto en GitHub.
- [ ] SQL Server levantado.
- [ ] Migrador ejecutado.
- [ ] Backend levantado.
- [ ] Angular levantado.
- [ ] Swagger abierto.
- [ ] `NEWS_API_KEY` configurada si se quiere mostrar busqueda real.
- [ ] Terminal preparada para `dotnet test`.
- [ ] Archivos clave abiertos en VS Code.
- [ ] Tener copiado un body JSON para Swagger.

## Mini discurso final

> "En resumen, completamos el alcance obligatorio de backend. Organizamos la solucion por capas, usamos AppServices para cada caso de uso, EF Core con SQL Server para persistencia, NewsAPI como API externa, y tests automaticos para validar el comportamiento. El frontend Angular levanta y permite login, pero la demostracion principal la hacemos con Swagger porque lo obligatorio eran los metodos backend. La solucion es simple, entendible y pensada para poder explicarla como trabajo de alumnos que estan empezando."
