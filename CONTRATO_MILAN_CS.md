# Contrato de implementación — MILAN C# (milan-cs)

## 1. Objetivo

Desarrolla una versión web ligera, funcional y autónoma de **MILAN — Mercado Interactivo en Línea de Artículos y Novedades**, utilizando C# y ASP.NET Core.

MILAN es una aplicación educativa de comercio electrónico que se utiliza como proyecto de referencia para el Workshop CODE.

La aplicación deberá reproducir el alcance funcional básico de las versiones de MILAN implementadas en C, Android e iOS, sin incorporar la complejidad de una solución empresarial de comercio electrónico.

El resultado deberá ser un proyecto completo, comprensible, fácil de ejecutar, depurar, probar y mantener.

**Prioriza la simplicidad arquitectónica, la calidad del código y la claridad didáctica sobre la incorporación de funcionalidades.**

## 2. Revisión inicial obligatoria

Antes de modificar archivos:

1. Inspecciona la estructura y el estado actual del repositorio `milan-cs`.
2. Identifica los proyectos, dependencias, recursos y componentes que ya existen.
3. Determina qué elementos pueden reutilizarse y cuáles deben simplificarse o eliminarse.
4. Si tienes acceso a `milan-c`, `milan-android-mct` o `milan-ios-mct`, utilízalos únicamente como referencias funcionales.
5. Identifica los artículos, nombres, imágenes, precios y reglas de negocio que deben conservarse.
6. Presenta un plan de implementación breve antes de efectuar cambios sustanciales.

No modifiques otros repositorios. No elimines archivos sin verificar primero sus dependencias y su utilidad.

Respeta el trabajo existente que sea compatible con el nuevo alcance.

## 3. Stack tecnológico

Utiliza preferentemente:

- C# y .NET 10 LTS.
- ASP.NET Core con Razor Pages.
- HTML5 y CSS3.
- JavaScript mínimo, únicamente cuando sea necesario.
- Inyección de dependencias integrada de ASP.NET Core.
- Repositorios en memoria mediante colecciones C#.
- Autenticación de demostración mediante cookies.
- xUnit para las pruebas automatizadas.

No utilices Entity Framework Core, ASP.NET Core Identity, SQL Server, SQLite, servicios externos, API REST, Blazor Server ni Blazor WebAssembly.

No introduzcas bibliotecas de terceros si el framework puede resolver la necesidad de manera sencilla.

## 4. Alcance funcional

### RF-01. Catálogo de artículos

La página principal deberá mostrar el catálogo de productos disponibles.

Cada artículo deberá presentar:

- Imagen.
- Nombre.
- Descripción breve.
- Precio en MXN.
- Acción para agregar al carrito.

Reutiliza los artículos e imágenes originales de MILAN cuando estén disponibles.

El catálogo deberá ser visible sin autenticación.

### RF-02. Carrito de compras

Al seleccionar un artículo, deberá agregarse al carrito.

El carrito permitirá:

- Consultar los artículos seleccionados.
- Incrementar y disminuir cantidades.
- Eliminar artículos.
- Consultar precios y subtotales.
- Calcular el importe total.
- Continuar comprando.
- Iniciar el proceso de compra.

Utiliza `decimal` para todos los importes monetarios y aplica un formato consistente en pesos mexicanos.

El carrito deberá estar aislado por sesión y conservarse cuando el cliente sea redirigido al inicio de sesión.

No compartas carritos entre usuarios ni sesiones de navegador.

### RF-03. Autenticación de clientes

El inicio de sesión será necesario únicamente para procesar la compra o consultar pedidos.

Implementa dos cuentas de demostración predefinidas, con credenciales ficticias documentadas.

Ambas tendrán exactamente el mismo perfil: Cliente.

No implementes roles administrativos.

La autenticación deberá permitir:

- Iniciar sesión.
- Cerrar sesión.
- Identificar al cliente activo.
- Proteger las páginas de pedidos y confirmación de compra.
- Regresar al proceso de compra después de autenticarse.

Las credenciales serán exclusivamente para demostración y no tendrán ninguna relación con usuarios reales.

Utiliza cookies de autenticación de ASP.NET Core, no una variable global para indicar qué usuario inició sesión.

No se requiere registro de usuarios, recuperación de contraseña, cambio de contraseña, MFA, edición de perfil ni administración de cuentas.

### RF-04. Confirmación de compra

El cliente autenticado deberá visualizar el resumen de su compra antes de finalizarla.

Presenta:

- Artículos.
- Cantidades.
- Precios unitarios.
- Subtotales.
- Total a pagar.

Verifica que el carrito no esté vacío y que las cantidades sean válidas.

El cliente deberá confirmar expresamente que desea continuar con la compra.

### RF-05. Pago simulado

El pago será completamente ficticio.

No integres pasarelas de pago, servicios bancarios, tarjetas reales ni proveedores externos.

Al confirmar un pago simulado exitoso:

1. Genera un identificador único de pedido.
2. Registra la fecha de compra.
3. Guarda una copia de los artículos, cantidades y precios del pedido.
4. Asocia el pedido al cliente autenticado.
5. Registra el total pagado.
6. Marca el pedido como pagado.
7. Vacía únicamente el carrito correspondiente.
8. Muestra una confirmación de compra exitosa.

Evita registrar duplicadamente una compra por doble clic o reenvío accidental del formulario.

Los importes de los pedidos deberán calcularse en el servidor y nunca confiar exclusivamente en datos enviados por el navegador.

### RF-06. Historial de pedidos

Implementa una página denominada **Mis pedidos**.

El cliente deberá poder consultar sus compras realizadas durante la ejecución de la aplicación.

Muestra:

- Número de pedido.
- Fecha de compra.
- Artículos adquiridos.
- Cantidades.
- Total.
- Estado del pedido.

Cada cliente deberá consultar exclusivamente sus propios pedidos.

No implementes cancelaciones, devoluciones, facturación, envíos ni seguimiento logístico.

## 5. Almacenamiento en memoria

Toda la información deberá mantenerse exclusivamente en la memoria del proceso o, para el carrito, en el estado de sesión correspondiente.

No debe existir ninguna conexión a motores de bases de datos.

No implementes persistencia en archivos JSON, XML, CSV ni bases de datos embebidas.

Puedes utilizar datos iniciales definidos en código o recursos del proyecto, pero los cambios realizados durante la ejecución no deberán escribirse en almacenamiento permanente.

El catálogo será inicializado al arrancar la aplicación.

Los pedidos permanecerán disponibles mientras el proceso esté activo y se perderán al reiniciarlo.

Implementa mecanismos sencillos de sincronización para proteger las colecciones compartidas cuando existan solicitudes concurrentes.

Documenta claramente este comportamiento.

## 6. Arquitectura y estructura

Propón una organización pequeña y coherente, preferentemente con un único proyecto web y un proyecto de pruebas.

Como referencia:

```text
milan-cs/
├── src/
│   └── Milan.Web/
│       ├── Models/
│       ├── Services/
│       ├── Repositories/
│       ├── Pages/
│       ├── wwwroot/
│       │   ├── css/
│       │   ├── js/
│       │   └── images/
│       ├── Program.cs
│       └── Milan.Web.csproj
├── tests/
│   └── Milan.Tests/
├── milan.sln
├── README.md
└── .gitignore
```

Esta estructura es orientativa. Adáptala cuando exista una alternativa más simple.

Utiliza modelos claros para Producto, Cliente, Carrito, Artículo del carrito, Pedido y Detalle del pedido.

Separa las reglas de negocio de las páginas y de la presentación.

No implementes CQRS, MediatR, microservicios, buses de eventos, arquitecturas distribuidas ni abstracciones que no estén justificadas por el alcance.

Evita crear clases, interfaces o capas que no aporten una responsabilidad identificable.

## 7. Interfaz y experiencia del usuario

La interfaz deberá estar completamente en español de México.

La aplicación deberá utilizar el nombre MILAN y su denominación completa.

Diseña una apariencia limpia, moderna, sencilla y adaptable a computadoras y dispositivos móviles.

Conserva, siempre que sea posible, la identidad visual y los artículos utilizados en las demás implementaciones.

La navegación principal incluirá:

- Catálogo.
- Carrito.
- Mis pedidos.
- Iniciar sesión o Cerrar sesión, según corresponda.

Presenta mensajes claros para operaciones exitosas, errores de validación y carritos vacíos.

No incorpores pantallas administrativas ni funciones que no estén incluidas en los requisitos.

## 8. Seguridad proporcional al entorno educativo

Aunque la aplicación es una demostración, evita prácticas que puedan confundir a los estudiantes.

Aplica:

- Autorización de acceso a pedidos y confirmación de compra.
- Validación del lado del servidor.
- Protección antifalsificación en formularios.
- Codificación segura de los textos mostrados.
- Aislamiento de pedidos por identificador de cliente.
- Cookies de autenticación configuradas apropiadamente.
- Validación del destino de redirección después del inicio de sesión.

No almacenes contraseñas reales ni información financiera sensible.

Documenta que el sistema es educativo, utiliza credenciales ficticias y no constituye un mecanismo de autenticación apto para producción.

## 9. Pruebas automatizadas

Implementa pruebas unitarias que cubran como mínimo:

1. Recuperación del catálogo.
2. Agregar un artículo al carrito.
3. Agregar varias unidades del mismo artículo.
4. Actualizar cantidades.
5. Eliminar artículos.
6. Calcular subtotales y total correctamente.
7. Rechazar cantidades inválidas.
8. Impedir compras con el carrito vacío.
9. Crear un pedido después del pago simulado.
10. Conservar los precios y detalles históricos del pedido.
11. Evitar duplicar un pedido al repetir su confirmación.
12. Verificar que cada cliente únicamente pueda consultar sus propios pedidos.

Cuando resulte viable, incorpora pruebas de integración para la autenticación y las páginas protegidas.

No utilices servicios externos para ejecutar las pruebas.

## 10. Exclusiones obligatorias

No implementes:

- Panel de administración.
- Gestión administrativa de productos.
- Gestión de inventarios.
- API REST.
- Swagger u OpenAPI.
- Entity Framework Core.
- SQL Server, SQLite u otra base de datos.
- Servicios en la nube.
- Registro público de usuarios.
- Recuperación o cambio de contraseñas.
- Autenticación multifactor.
- Gestión de perfiles.
- Roles administrativos.
- Pagos reales.
- Facturación electrónica.
- Envíos.
- Promociones o cupones.
- Integración con correo electrónico.
- Telemetría externa.
- Infraestructura empresarial innecesaria.

No agregues funcionalidades adicionales por iniciativa propia.

## 11. Calidad del código

Utiliza nombres significativos y convenciones idiomáticas de C#.

Conserva en inglés los elementos propios de .NET, palabras reservadas y nombres técnicos que deben permanecer en ese idioma.

La interfaz, los mensajes y la documentación dirigida al usuario deberán estar en español.

Aplica formateo coherente, archivos legibles, métodos pequeños y responsabilidades claras.

Evita archivos artificialmente minificados y código concentrado en métodos excesivamente extensos.

Utiliza comentarios únicamente cuando expliquen decisiones o reglas que no sean evidentes.

## 12. Ejecución y documentación

Incluye un README.md en español que explique:

- Propósito educativo de MILAN.
- Requisitos del entorno.
- Estructura del proyecto.
- Compilación y ejecución local.
- Cuentas de demostración.
- Flujo de compra.
- Ejecución de pruebas.
- Funcionamiento del almacenamiento en memoria.
- Limitaciones deliberadas de la aplicación.

La solución deberá poder restaurarse, compilarse, ejecutarse y probarse desde Visual Studio o mediante la CLI de .NET.

No deberá requerir Docker, servidores de bases de datos, credenciales externas ni infraestructura adicional.

## 13. Validación y entrega

Antes de concluir:

1. Restaura las dependencias.
2. Compila la solución completa.
3. Ejecuta las pruebas automatizadas.
4. Verifica el funcionamiento de las páginas.
5. Comprueba el flujo completo de compra.
6. Comprueba el aislamiento de pedidos entre las dos cuentas.
7. Revisa que no existan dependencias innecesarias.
8. Confirma que no se utilicen conexiones a bases de datos ni servicios externos.
9. Verifica que el árbol de trabajo solo contenga los cambios previstos.

Entrega un resumen con los componentes creados, archivos modificados, decisiones arquitectónicas, resultados de compilación y pruebas, y cualquier limitación pendiente.

No declares pruebas exitosas si no fueron ejecutadas.

No realices commits, pushes, creación de tags ni publicaciones sin autorización explícita.

## 14. Criterio final de aceptación

La implementación será aceptada cuando un usuario pueda:

1. Abrir MILAN.
2. Examinar el catálogo.
3. Agregar artículos al carrito.
4. Modificar su compra.
5. Iniciar sesión cuando sea necesario.
6. Confirmar el pedido.
7. Ejecutar un pago simulado.
8. Recibir una confirmación.
9. Consultar su historial de pedidos.

Todo deberá ejecutarse localmente sin conexión a servidores externos y sin utilizar una base de datos.

**Principio rector: MILAN C# deberá ser una aplicación pequeña, funcional, legible, didáctica y equivalente en alcance a MILAN C, Android e iOS. No deberá ser una reproducción completa de eShopOnWeb.**