* Iniciar migración

dotnet ef migrations add InitialCreate --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext
dotnet ef migrations add AddRefreshTokens --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext
dotnet ef migrations add AddActivoSystemConfiguration --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

* Agregar una migración

dotnet ef migrations add AddIdentityTable --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext
dotnet ef migrations add AddBusinessTable --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

* Aplicar migraciones a la base de datos
dotnet ef database update --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext



dotnet ef migrations remove --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext


* Generador de codigo 

// Ejemplos
# Ruta completa (larga)
dotnet run -- "F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.Domain\Entities\Producto.cs" Producto

# Modo normal (igual que antes - RECOMENDADO)
dotnet run -- "..\GoldBusiness.Domain\Entities\Proveedor.cs" Proveedor

# Modo preview (solo simula, no crea archivos)
dotnet run -- "..\GoldBusiness.Domain\Entities\Proveedor.cs" Proveedor --preview

# Versión corta del preview
dotnet run -- "..\GoldBusiness.Domain\Entities\Proveedor.cs" Proveedor -p






Despelgar en Azure   

dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
Compress-Archive -Path .\* -DestinationPath ..\api.zip -Force

 az webapp deploy --resource-group rg-goldbusiness-dev --name goldbusinesswebapi-dev --src-path /home/rolando/api.zip --type zip


 az storage blob upload-batch --account-name goldbusinessstorage `
  -s "F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.Client\dist\gold-business.client\browser" `
  -d '$web'

ng build --configuration production

az storage blob upload-batch --account-name goldbusinessstorage `
  -s "F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.Client\dist\gold-business.client\browser" `
  -d '$web' `
  --overwrite true



# genera una clave segura base64 (≈64 chars)
openssl rand -base64 48

Ejecuta exactamente esto en tu Cloud Shell (PowerShell). Sustituye el valor de $jwtKey si quieres regenerar la clave:
# define la clave en una variable PowerShell (no queda en historial de comandos si no la vuelcas)
$jwtKey = 'zTNcIFotKzhTzVoTxckrPCCYDYw59K1kTvRyDzqEwvmkoZzcj466vTurPxakxe2D'

# establece las app settings usando doble guion bajo
az webapp config appsettings set `
  --resource-group rg-goldbusiness-dev `
  --name goldbusinesswebapi-dev `
  --settings Jwt__Issuer='https://goldbusinesswebapi-dev.azurewebsites.net' Jwt__Key=$jwtKey

# verificar que se guardaron
az webapp config appsettings list -g rg-goldbusiness-dev -n goldbusinesswebapi-dev --query "[?starts_with(name,'Jwt')]" -o table

# reiniciar y vigilar logs
az webapp restart -g rg-goldbusiness-dev -n goldbusinesswebapi-dev
az webapp log tail -g rg-goldbusiness-dev -n goldbusinesswebapi-dev







Acceso al sitio 


https://goldbusinessstorage.z19.web.core.windows.net/login

https://goldbusinesswebapi-dev.azurewebsites.net/swagger/index.html

<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
<environmentVariable name="ConnectionStrings__DefaultConnection" value="Server=localhost;Database=GoldBusiness;User Id=sa;Password=C3r4p10*;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True" />
<environmentVariable name="Jwt__Key" value="Pr0d_C!av3S3gur@2024#Meg4L0ng4&amp;Extr4F0rt3&amp;PRODUCCION-KEY-67890!!" />

Copy-Item "F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.WebApi\web.config" -Destination "C:\inetpub\wwwroot\GoldBusinessWebApi\web.config" -Force


Configurar User Secrets
cd "F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.WebApi"

# Configurar Connection String de desarrollo
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=GoldBusiness;User Id=sa;MultipleActiveResultSets=true;Password=C3r4p10*;Encrypt=False;TrustServerCertificate=True"

# Configurar JWT Key de desarrollo
dotnet user-secrets set "Jwt:Key" "C!av3S3gur@2024#Meg4L0ng4&Extr4F0rt3&12345!!"

# Ver los secrets configurados
dotnet user-secrets list


