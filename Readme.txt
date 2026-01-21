* Agregar una migración
dotnet ef migrations add AddIdentityTable --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext
dotnet ef migrations add AddBusinessTable --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

* Aplicar migraciones a la base de datos
dotnet ef database update --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext



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