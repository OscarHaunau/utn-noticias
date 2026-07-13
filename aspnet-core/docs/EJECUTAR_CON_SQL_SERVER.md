# Ejecutar UtnNoticias con SQL Server

Esta guia explica como correr el proyecto en otra maquina usando **SQL Server**.

La aplicacion real ya esta configurada para SQL Server. SQLite se usa solo en los tests para correr rapido en memoria.

## Resumen rapido

1. Levantar o instalar SQL Server.
2. Ejecutar el migrador para crear la base `UtnNoticias`.
3. Levantar el backend usando el mismo connection string.
4. Levantar Angular.

## Opcion recomendada: SQL Server con Docker

Esta opcion funciona igual en Windows, Mac y Linux si la maquina tiene Docker.

```bash
docker run -d --name utnnoticias-sql \
  -e 'ACCEPT_EULA=Y' \
  -e 'MSSQL_SA_PASSWORD=UtnNoticias_12345!' \
  -p 14333:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

Si el contenedor ya existe pero esta detenido:

```bash
docker start utnnoticias-sql
```

Connection string para esta opcion:

```txt
Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True
```

## Opcion Windows: SQL Server instalado

Si la maquina tiene SQL Server instalado localmente, se puede usar autenticacion de Windows:

```txt
Server=localhost;Database=UtnNoticias;Trusted_Connection=True;TrustServerCertificate=True
```

Si usa SQL Server Express:

```txt
Server=localhost\SQLEXPRESS;Database=UtnNoticias;Trusted_Connection=True;TrustServerCertificate=True
```

Si usa usuario y password:

```txt
Server=localhost;Database=UtnNoticias;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True
```

## Crear la base con el migrador

Desde la carpeta del migrador:

```bash
cd aspnet-core/src/UtnNoticias.DbMigrator
```

Con Docker SQL Server:

```bash
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' \
~/.dotnet/dotnet run
```

En Windows PowerShell, el mismo comando se puede escribir asi:

```powershell
$env:ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True'
dotnet run
```

Que hace el migrador:

- crea la base de datos si no existe,
- aplica las migraciones de Entity Framework Core,
- carga datos iniciales de ABP,
- crea el usuario administrador.

## Levantar el backend

Desde el host web:

```bash
cd aspnet-core/src/UtnNoticias.HttpApi.Host
```

Con Docker SQL Server:

```bash
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' \
ASPNETCORE_ENVIRONMENT=Development \
~/.dotnet/dotnet run --urls https://localhost:44311
```

En Windows PowerShell:

```powershell
$env:ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True'
$env:ASPNETCORE_ENVIRONMENT='Development'
dotnet run --urls https://localhost:44311
```

Swagger queda en:

```txt
https://localhost:44311/swagger
```

## Levantar Angular

En otra terminal:

```bash
cd angular
pnpm install
pnpm ng serve --host localhost --port 4200
```

Frontend:

```txt
http://localhost:4200
```

Login inicial:

```txt
Usuario: admin
Password: 1q2w3E*
```

## NewsAPI

Para que la busqueda real devuelva noticias, configurar la key localmente:

```bash
export NEWS_API_KEY='tu_key_de_newsapi'
```

En Windows PowerShell:

```powershell
$env:NEWS_API_KEY='tu_key_de_newsapi'
```

Si no se configura, el backend levanta igual, pero la busqueda real devuelve una lista vacia.

## Aclaracion sobre SQLite

En el proyecto aparece SQLite en esta carpeta:

```txt
aspnet-core/test/UtnNoticias.EntityFrameworkCore.Tests
```

Eso es solo para pruebas automaticas. Los tests usan SQLite en memoria para no depender de una base real.

La aplicacion principal usa SQL Server porque en el modulo de Entity Framework esta configurado:

```txt
options.UseSqlServer();
```

Archivo:

```txt
aspnet-core/src/UtnNoticias.EntityFrameworkCore/EntityFrameworkCore/UtnNoticiasEntityFrameworkCoreModule.cs
```

## Checklist para otra maquina

- [ ] Tiene .NET 7 instalado.
- [ ] Tiene Node/pnpm para Angular.
- [ ] Tiene SQL Server o Docker.
- [ ] Corrio el migrador sin errores.
- [ ] El backend abre Swagger.
- [ ] Angular abre en `http://localhost:4200`.
- [ ] Puede entrar con `admin` / `1q2w3E*`.

## Problemas comunes

| Problema | Solucion |
|---|---|
| `Login failed for user sa` | Revisar password y que el contenedor este usando el mismo puerto. |
| `A network-related or instance-specific error` | Revisar que SQL Server este levantado y que el `Server=` sea correcto. |
| Puerto `14333` ocupado | Cambiar el puerto externo, por ejemplo `-p 14334:1433`, y actualizar el connection string. |
| Certificado SQL Server | Dejar `TrustServerCertificate=True` para desarrollo local. |
| No aparecen tablas | Ejecutar primero `UtnNoticias.DbMigrator`. |
