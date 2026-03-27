# Copilot Instructions

## General Guidelines
- Prefiere respuestas en español (es).
- Mantén la encapsulación DDD y la lógica de solución existente (usa setters privados y métodos de dominio).
- Mantén la compatibilidad con EF Core.
- Al recargar datos de lista, no restablezcas la paginación; preserva currentPage a menos que quede fuera de rango.
- Los proyectos del workspace deben estar orientados a .NET 10.
- Prefiere un color de encabezado más suave y menos blanco en el inicio de sesión (por ejemplo, gris cálido suave #d6cfa9) para reducir la fatiga visual y coincidir con la marca GoldBusiness.
- Excluye el proyecto del cliente Angular (GoldBusiness.Client) de la construcción/publicación al desplegar la WebApi; restaura y publica solo el proyecto GoldBusiness.WebApi (GoldBusiness.WebApi/GoldBusiness.WebApi.csproj).
- Prefiere un script de despliegue en PowerShell que construya la solución y use Azure CLI para desplegar.
- El `CuentaController` debe permitir tanto `ERPFullAccess` como `ERPAdminAccess` (permitir ambas políticas).
- Los workflows deben construir el cliente Angular desde `GoldBusiness.Client/dist/gold-business.client/browser` y subirlo a `$web`.
- Usa el principal de servicio `AZURE_CREDENTIALS` para GitHub Actions; asegúrate de que el SP tenga el rol de Storage Blob Data Contributor.
- Los mensajes de validación residen en GoldBusiness.Domain (recurso ValidationMessages); las traducciones del cliente se encuentran en GoldBusiness.Client/src/app/services/translation.service.ts. Mantenlos sincronizados y prefiere generar traducciones del cliente a partir de recursos del servidor o mapeándolos explícitamente.
- Usa validación numérica/normalizada para números de teléfono en Establecimiento, Cliente y Proveedor. Extrae la normalización/validación de teléfonos a un servicio o utilidad compartida para su reutilización. Mantén el prefijo '+' visible en la UI; usa `normalizePhone` para preservar el '+' al enviar y `normalizePhoneForValidation` para eliminar el '+' en la validación regex. Elimina los helpers de depuración (logFormState y console.logs) antes de producción y reutiliza `shared phone.util` (normalizePhone, phoneValidator, PHONE_MAX_LENGTH) en todos los componentes.

## UI/UX Guidelines
- Coloca el botón de alternar la barra lateral en la barra de navegación junto al título/subtítulo en escritorio y móvil; en móvil, abre la barra lateral como superposición, y en escritorio, alterna el colapso a través de SidebarService. Asegúrate de que el color del botón de alternar la barra lateral coincida con el gradiente de la barra de navegación y que los botones de alternar sean visualmente coherentes en móvil y escritorio. Prefiere botones circulares consistentes en móvil.
  - Recuerda que el color del botón toggle móvil debe coincidir con el theme del navbar (usar gradiente del navbar) y los botones toggle deben mantener estilo consistente entre móvil y escritorio.
- El título de la marca en la barra de navegación debe permanecer visible, pero truncado (con elipsis) en móvil para que los botones de la barra de navegación no se desplacen fuera de la pantalla; no ocultes el título de la marca únicamente basado en el ancho de la ventana.
- Previene que el menú central (Inicio/Acerca) se superponga con el selector de idioma y el área de usuario en anchos medios; prefiere que el menú central se alinee a la izquierda o sea desplazable cuando el espacio sea limitado.
- Prefiere que el selector de idioma permanezca visible en móvil como compacto/sólo icono en lugar de estar oculto. Al actualizar la UI, preserva la temática de color original de los componentes para los selects; prefiere un selector de idioma solo con icono en móvil mientras mantienes los colores de escritorio intactos. Además, prefiere que el popover del selector de idioma en móvil coincida visualmente con la apariencia del select en escritorio, usando un fondo oscuro sólido para las opciones del popover, texto blanco, bordes coincidentes y sombra para mantener el contraste.
- Haz que los botones de la barra de navegación (menú, idioma, usuario, cerrar sesión) sean ligeramente más pequeños en móvil (por ejemplo, 34px) para un mejor ajuste.
- En el formulario de establecimiento, restaura la flecha nativa del select sobreescribiendo los estilos de .shared-select localmente, ya que el problema de la flecha del select aparece solo en ese componente.
- Prefiere que los inputs de inicio de sesión no se muevan al recibir foco (eliminar translateY); usa background-clip: padding-box y box-shadow controlado para evitar una línea de borde visible.
- Prefiere la alineación vertical global de los checkboxes a través de shared style-form.css: labels inline-flex, altura 48px, checkbox centrado y pequeño margen superior si es necesario.
- Muestra un checkbox 'Activo' en los formularios frontend para las entidades que lo requieran, mientras que el backend utiliza 'Cancelado' para marcar registros eliminados suavemente. El frontend debe mapear 'activo' = !'cancelado'. Reutiliza los estilos del checkbox de Establecimiento (clases 'checkbox-group', 'form-checkbox') para consistencia. **No se debe agregar la propiedad 'Activo' a GrupoCuenta, SubGrupoCuenta o Cuenta. Los formularios frontend para estas entidades no deben incluir 'Activo'.** 'Cancelado' se mantiene como la bandera de soft-delete utilizada para reactivación y estado interno.

## Formulario y Validaciones
- Implementa la misma experiencia de generación de código para `Cuenta` como para `SubGrupoCuenta`: usa el dropdown de `SubGrupoCuenta` más un campo de entrada de código de usuario de 3 dígitos para componer el `Cuenta.codigo` de 8 dígitos (prefijo = `subgrupo.codigo` seleccionado (5) + 3 dígitos de usuario).
- Usa una UI de cascada consistente en los formularios de Plan Cuenta: muestra 'Prefijo seleccionado' solo cuando exista un prefijo, desactiva la entrada de código de usuario cuando no se seleccione un prefijo, y coincide con el diseño/comportamiento del formulario de SubGrupoCuenta.
- Alinear verticalmente el checkbox 'Deudora' en formularios del Plan de Cuenta y en SubGrupoCuenta para que quede centrado con respecto a los inputs en .form-row-three.
- Prefiere que los selects en formularios de país/provincia/municipio/código postal permanezcan deshabilitados hasta seleccionar el padre; la cascada se maneja mediante valueChanges y los controles reactivos deben habilitarse/deshabilitarse con control.enable()/control.disable(); usar clases shared-select/shared-disabled y [attr.aria-disabled] para accesibilidad.
- Al poblar los selects de país/provincia/municipio/código postal, habilita/deshabilita los Angular FormControls aguas abajo usando control.enable()/control.disable() para que los selects aparezcan inactivos hasta que se seleccione el padre.

## Configuración del Sistema
- El término "negocio" en la UI se refiere a SystemConfiguration (SystemConfigurationDTO.nombreNegocio) y no a una entidad Negocio separada; poblar el select de negocio desde SystemConfigurationService y asegurar que el componente use DTOs de SystemConfiguration.
- Prefiere el diseño del formulario de SystemConfiguration: vista previa del logo ligeramente más grande (aprox. 180x110), etiqueta 'URL Logo/Imagen' mostrada encima de la vista previa, controles del logo (elegir archivo, nombre de archivo, eliminar) mostrados a la derecha de la vista previa en línea, área de texto de licencia más estrecha en altura (~72-88px) y ocupando el ancho restante; los estilos deben estar limitados bajo .system-config-form y ser responsivos en móvil.

## Navegación
- Al cerrar la edición/detalle, navega de regreso a la vista de lista correspondiente (Cliente -> /nomencladores/clientes, Proveedor -> /nomencladores/proveedor).