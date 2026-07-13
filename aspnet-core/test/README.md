# Ejecucion de tests

Este documento resume como ejecutar los tests automatizados incorporados para el modulo de listas de lectura.

## Requisitos previos

- Tener instalado .NET SDK 7.
- Ejecutar los comandos desde la carpeta `aspnet-core`.
- Si el backend esta levantado, detenerlo antes de correr los tests para evitar bloqueos de DLL.

```powershell
cd "C:\Users\eriic\OneDrive\Desktop\programacion\GitHub\UtnNoticias\aspnet-core"
```

Si aparece un error indicando que un archivo `.dll` esta siendo usado por otro proceso, detener los procesos `dotnet`:

```powershell
Get-Process dotnet -ErrorAction SilentlyContinue | Stop-Process -Force
```

## Tests de aplicacion

Ejecutan casos sobre `ReadingListAppService`.

```powershell
dotnet test test\UtnNoticias.Application.Tests\UtnNoticias.Application.Tests.csproj --no-restore
```

Resultado esperado:

```text
Correctas! - Con error: 0, Superado: 14, Omitido: 0, Total: 14
```

Casos principales cubiertos:

- Crear lista con nombre valido.
- Rechazar nombre vacio.
- Actualizar lista propia.
- Agregar noticia a una lista.
- Rechazar noticia duplicada por URL.
- Eliminar lista.
- Evitar operaciones sobre listas de otro usuario.
- Listar solo listas del usuario autenticado.

## Tests de dominio

Ejecutan reglas propias del aggregate `ReadingList`.

```powershell
dotnet test test\UtnNoticias.Domain.Tests\UtnNoticias.Domain.Tests.csproj --no-restore
```

Resultado esperado:

```text
Correctas! - Con error: 0, Superado: 5, Omitido: 0, Total: 5
```

Casos principales cubiertos:

- No crear lista con nombre vacio.
- No agregar items duplicados por URL.

## Notas

Durante la ejecucion pueden aparecer warnings relacionados con el paquete `NewsAPI 0.7.0` y advertencias de nullability en tests existentes. Esos warnings no bloquean la ejecucion si el resultado final indica `Con error: 0`.
