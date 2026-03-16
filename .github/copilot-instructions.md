# Copilot Instructions

## General Guidelines
- Prefiere respuestas en español.
- Mantén la encapsulación DDD y la lógica de solución existente (usa setters privados y métodos de dominio).
- Mantén la compatibilidad con EF Core.
- Al recargar datos de lista, no restablezcas la paginación; preserva currentPage a menos que quede fuera de rango.
- Los proyectos del workspace deben estar orientados a .NET 10.
- Prefiere un color de encabezado más suave y menos blanco en el inicio de sesión (por ejemplo, gris cálido suave #d6cfa9) para reducir la fatiga visual y coincidir con la marca GoldBusiness.
- Los proyectos están ubicados en el repositorio `F:\Documents\Visual Studio 18\Projects\GoldBusiness` en la rama `staging` y se despliegan en la Azure Web App 'goldbusinesswebapi-dev' en el grupo de recursos 'rg-goldbusiness-dev'.
- Excluye el proyecto del cliente Angular (GoldBusiness.Client) de la construcción/publicación al desplegar la WebApi; restaura y publica solo el proyecto GoldBusiness.WebApi (GoldBusiness.WebApi/GoldBusiness.WebApi.csproj).
- Prefiere un script de despliegue en PowerShell que construya la solución y use Azure CLI para desplegar.
- El `CuentaController` debe permitir tanto `ERPFullAccess` como `ERPAdminAccess` (permitir ambas políticas).
- Los workflows deben construir el cliente Angular desde `GoldBusiness.Client/dist/gold-business.client/browser` y subirlo a `$web`.
- Usa el principal de servicio `AZURE_CREDENTIALS` para GitHub Actions; asegúrate de que el SP tenga el rol de Storage Blob Data Contributor.
- Coloca el botón de alternar la barra lateral en la barra de navegación junto al título/subtítulo en escritorio y móvil; en móvil, abre la barra lateral como superposición, y en escritorio, alterna el colapso a través de SidebarService. Asegúrate de que el color del botón de alternar la barra lateral coincida con el gradiente de la barra de navegación y que los botones de alternar sean visualmente coherentes en móvil y escritorio. Prefiere botones circulares consistentes en móvil.
  - Recuerda que el color del botón toggle móvil debe coincidir con el theme del navbar (usar gradiente del navbar) y los botones toggle deben mantener estilo consistente entre móvil y escritorio.
- El título de la marca en la barra de navegación debe permanecer visible, pero truncado (con elipsis) en móvil para que los botones de la barra de navegación no se desplacen fuera de la pantalla; no ocultes el título de la marca únicamente basado en el ancho de la ventana.
- Previene que el menú central (Inicio/Acerca) se superponga con el selector de idioma y el área de usuario en anchos medios; prefiere que el menú central se alinee a la izquierda o sea desplazable cuando el espacio sea limitado.
- Prefiere que el selector de idioma permanezca visible en móvil como compacto/sólo icono en lugar de estar oculto. Al actualizar la UI, preserva la temática de color original de los componentes para los selects; prefiere un selector de idioma solo con icono en móvil mientras mantienes los colores de escritorio intactos. Además, prefiere que el popover del selector de idioma en móvil coincida visualmente con la apariencia del select en escritorio, usando un fondo oscuro sólido para las opciones del popover, texto blanco, bordes coincidentes y sombra para mantener el contraste.
- Haz que los botones de la barra de navegación (menú, idioma, usuario, cerrar sesión) sean ligeramente más pequeños en móvil (por ejemplo, 34px) para un mejor ajuste.
- Implementa la misma experiencia de generación de código para `Cuenta` como para `SubGrupoCuenta`: usa el dropdown de `SubGrupoCuenta` más un campo de entrada de código de usuario de 3 dígitos para componer el `Cuenta.codigo` de 8 dígitos (prefijo = `subgrupo.codigo` seleccionado (5) + 3 dígitos de usuario).
- Usa una UI de cascada consistente en los formularios de Plan Cuenta: muestra 'Prefijo seleccionado' solo cuando exista un prefijo, desactiva la entrada de código de usuario cuando no se seleccione un prefijo, y coincide con el diseño/comportamiento del formulario de SubGrupoCuenta.
- Alinear verticalmente el checkbox 'Deudora' en formularios del Plan de Cuenta y en SubGrupoCuenta para que quede centrado con respecto a los inputs en .form-row-three.

## Project-Specific Rules
- Los endpoints deben ser paginados: Cliente y Proveedor deben implementar `GetPagedAsync`; los repositorios deben exponer `Query()`. Se requiere un DTO `PagedResult<T>`; los servicios del frontend deben tener `getPaged` y evitar faltas de importación de modelos.
- El tipo de modelo Cliente está definido dentro de 'cliente.service.ts' (no 'cliente.model.ts'). No asumas un archivo separado cliente.model.ts; actualiza las importaciones para consumir la interfaz desde el archivo del servicio o crea un archivo de modelo adecuado si se prefiere.

## Code Style
- Usa reglas de formato específicas.
- Sigue las convenciones de nomenclatura.

## Memory
-  Prefiere respuestas en español (es).
- Alinear verticalmente el checkbox 'Deudora' en SubGrupoCuenta: centrar el bloque de checkbox con respecto a los inputs en .form-row-three (alineación vertical preferida).
- No asumas que existen archivos de modelo TypeScript para Cliente; el proyecto define la interfaz Cliente dentro de 'cliente.service.ts'. Las recomendaciones futuras deben respetar eso y evitar sugerir 'cliente.model.ts' a menos que se solicite.