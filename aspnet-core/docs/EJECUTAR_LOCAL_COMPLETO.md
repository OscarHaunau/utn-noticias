# Ejecutar UtnNoticias completo en local

Esta guia deja el proyecto funcionando completo en local: SQL Server + Backend + Angular.

## 1. Base de datos local con Docker

```bash
docker start utnnoticias-sql
```

Si el contenedor no existe:

```bash
docker run -d --name utnnoticias-sql \
  -e 'ACCEPT_EULA=Y' \
  -e 'MSSQL_SA_PASSWORD=UtnNoticias_12345!' \
  -p 14333:1433 \
  mcr.microsoft.com/mssql/server:2022-latest
```

## 2. Usar .NET 7 local

El proyecto ABP 7.3 funciona correctamente con .NET 7.

```bash
~/.dotnet/dotnet --list-sdks
```

Debe aparecer `7.0.410` o similar.

## 3. Migrar/seed de base de datos

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/aspnet-core/src/UtnNoticias.DbMigrator
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' ~/.dotnet/dotnet run
```

## 4. Levantar backend

Para que funcione la busqueda real en NewsAPI, antes se puede configurar la key localmente:

```bash
export NEWS_API_KEY='tu_key_de_newsapi'
```

Si no se configura, el backend igual levanta, pero la busqueda real devuelve una lista vacia.

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/aspnet-core/src/UtnNoticias.HttpApi.Host
ConnectionStrings__Default='Server=localhost,14333;Database=UtnNoticias;User Id=sa;Password=UtnNoticias_12345!;TrustServerCertificate=True' ASPNETCORE_ENVIRONMENT=Development ~/.dotnet/dotnet run --urls https://localhost:44311
```

Swagger:

```txt
https://localhost:44311/swagger
```

## 5. Levantar Angular

```bash
cd /Users/oscarhaunau/Documents/asp-net-core/UtnNoticias/angular
pnpm install
pnpm ng serve --host localhost --port 4200
```

Frontend:

```txt
http://localhost:4200
```

## Login

Usuario:

```txt
admin
```

Password:

```txt
1q2w3E*
```

## Verificacion realizada

- Backend levanta en `https://localhost:44311`.
- Swagger muestra endpoints del TP.
- Angular levanta en `http://localhost:4200`.
- Login con `admin` funciona.
- Captura visual guardada en `/tmp/utn-angular-after-login.png`.
