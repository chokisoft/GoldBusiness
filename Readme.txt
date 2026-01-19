* Agregar una migración
dotnet ef migrations add AddGrupo_SubGrupo_Cuenta --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

* Aplicar migraciones a la base de datos
dotnet ef database update --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

