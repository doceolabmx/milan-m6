# MILAN C# Lightweight

Versión educativa y autónoma de **MILAN — Mercado Interactivo en Línea de Artículos y Novedades**. Conserva el catálogo y recursos gráficos de MILAN C# MCT `v1.0.0`; implementa solo catálogo, carrito, autenticación de demostración, compra simulada e historial.

## Requisitos

- SDK de .NET 10.
- Visual Studio 2026 o CLI de .NET.
- No requiere conexión a servicios externos, Docker ni servidor de base de datos.

## Estructura

```text
src/Milan.Web/       Aplicación ASP.NET Core Razor Pages
  Models/            Producto, carrito y pedido
  Services/           Catálogo, sesión, cuentas y pedidos
  Pages/              Catálogo, carrito, compra, acceso e historial
  wwwroot/             CSS e imágenes seleccionadas de MILAN v1.0.0
tests/Milan.Tests/    Pruebas unitarias xUnit
docs/                 Notas de documentación
  funcionalidades-futuras.md  Pasarela de pago no integrada
  operacion.md        Cierre de compra sin envío ni logística
```

## Ejecutar

Desde la raíz del repositorio:

```powershell
dotnet restore Milan.sln
dotnet build Milan.sln
dotnet run --project src/Milan.Web
```

Abre la URL local que indique ASP.NET Core.

## Cuentas de demostración

| Cliente | Correo | Contraseña |
|---|---|---|
| Cliente MILAN | `cliente@doceolab.com` | `Pass@word1` |
| Cliente MILAN 2 | `admin@doceolab.com` | `Pass@word1` |

Las dos cuentas, incluida `admin@doceolab.com`, tienen exclusivamente el perfil **Cliente**; no existen privilegios ni pantallas administrativas.

Son credenciales ficticias incluidas en código para aprendizaje. El inicio de sesión utiliza cookies de ASP.NET Core, pero no es un mecanismo apto para producción.

## Flujo de compra

1. Explora los 12 drones del catálogo y agrega productos al carrito.
2. Ajusta cantidades (0 elimina el artículo; máximo 99) o quítalos.
3. Continúa con la compra. Si aún no iniciaste sesión, accede con una cuenta de demostración; el carrito de esa sesión se conserva.
4. Revisa el resumen y confirma el pago simulado.
5. Consulta el pedido en **Mis pedidos**. Las cuentas solo ven sus propios pedidos.

No se solicitan ni procesan datos financieros reales. El servidor obtiene precios del catálogo y calcula los totales; el formulario solo confirma la intención de compra.

## Almacenamiento y límites

- El catálogo es una lista inmutable inicializada en código.
- El carrito se serializa dentro de la sesión ASP.NET Core con almacenamiento distribuido en memoria.
- Los pedidos se conservan en colecciones sincronizadas mientras el proceso está activo.
- El reinicio del proceso elimina carritos y pedidos. No existe persistencia en archivos ni conexión a una base de datos.
- No hay registro, recuperación de contraseña, MFA, administración, inventario, API, pagos reales, facturación ni logística.
- La aplicación es educativa y no debe exponerse como tienda de producción.

## Pruebas

```powershell
dotnet test Milan.sln
```

Las pruebas verifican catálogo, carrito, validaciones, importes, pedidos, idempotencia y privacidad por cliente.

## Repositorios por stack

- [MILAN C](https://github.com/vicasso-mx/milan-c)
- [MILAN C#](https://github.com/vicasso-mx/milan-cs)
- [MILAN Android](https://github.com/vicasso-mx/milan-android)
- [MILAN iOS](https://github.com/vicasso-mx/milan-ios)
