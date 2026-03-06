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

## Code Style
- Usa reglas de formato específicas.
- Sigue las convenciones de nomenclatura.