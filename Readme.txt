* Iniciar migración

dotnet ef migrations add InitialCreate --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

* Agregar una migración

dotnet ef migrations add AddIdentityTable --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext
dotnet ef migrations add AddBusinessTable --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext

* Aplicar migraciones a la base de datos
dotnet ef database update --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext



dotnet ef migrations remove --project .\GoldBusiness.Infrastructure\GoldBusiness.Infrastructure.csproj --startup-project .\GoldBusiness.WebApi\GoldBusiness.WebApi.csproj --context ApplicationDbContext





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








using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Infrastructure.Context;
using Microsoft.Extensions.Logging;

namespace GoldBusiness.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            ILogger logger)
        {
            try
            {
                logger.LogInformation("Iniciando seed de datos maestros...");

                // Asegurar que la base de datos existe
                await context.Database.EnsureCreatedAsync();

                // Seed de GrupoCuenta
                await SeedGrupoCuentaAsync(context, logger);

                // Seed de Linea
                await SeedLineaAsync(context, logger);

                // Seed de SubLinea
                await SeedSubLineaAsync(context, logger);

                // Seed de UnidadMedida
                await SeedUnidadMedidaAsync(context, logger);

                logger.LogInformation("Seed de datos maestros completado exitosamente!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error durante el seed de datos maestros");
                throw;
            }
        }

        #region GrupoCuenta

        private static async Task SeedGrupoCuentaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.GrupoCuenta.Any())
            {
                logger.LogInformation("GrupoCuenta ya tiene datos, omitiendo seed.");
                return;
            }

            var grupos = new[]
            {
                new GrupoCuenta("01", "ACTIVO", "system"),
                new GrupoCuenta("02", "PASIVO", "system"),
                new GrupoCuenta("03", "PATRIMONIO", "system"),
                new GrupoCuenta("04", "INGRESOS", "system"),
                new GrupoCuenta("05", "GASTOS", "system")
            };

            context.GrupoCuenta.AddRange(grupos);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<GrupoCuentaTranslation>
            {
                // ACTIVO
                new GrupoCuentaTranslation(grupos[0].Id, "es", "ACTIVO", "system"),
                new GrupoCuentaTranslation(grupos[0].Id, "en", "ASSETS", "system"),
                new GrupoCuentaTranslation(grupos[0].Id, "fr", "ACTIF", "system"),

                // PASIVO
                new GrupoCuentaTranslation(grupos[1].Id, "es", "PASIVO", "system"),
                new GrupoCuentaTranslation(grupos[1].Id, "en", "LIABILITIES", "system"),
                new GrupoCuentaTranslation(grupos[1].Id, "fr", "PASSIF", "system"),

                // PATRIMONIO
                new GrupoCuentaTranslation(grupos[2].Id, "es", "PATRIMONIO", "system"),
                new GrupoCuentaTranslation(grupos[2].Id, "en", "EQUITY", "system"),
                new GrupoCuentaTranslation(grupos[2].Id, "fr", "CAPITAUX PROPRES", "system"),

                // INGRESOS
                new GrupoCuentaTranslation(grupos[3].Id, "es", "INGRESOS", "system"),
                new GrupoCuentaTranslation(grupos[3].Id, "en", "INCOME", "system"),
                new GrupoCuentaTranslation(grupos[3].Id, "fr", "REVENUS", "system"),

                // GASTOS
                new GrupoCuentaTranslation(grupos[4].Id, "es", "GASTOS", "system"),
                new GrupoCuentaTranslation(grupos[4].Id, "en", "EXPENSES", "system"),
                new GrupoCuentaTranslation(grupos[4].Id, "fr", "DÉPENSES", "system")
            };

            context.GrupoCuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de GrupoCuenta completado: {Count} grupos agregados", grupos.Length);
        }

        #endregion

        #region Linea

        private static async Task SeedLineaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Linea.Any())
            {
                logger.LogInformation("Linea ya tiene datos, omitiendo seed.");
                return;
            }

            var lineas = new[]
            {
                new Linea("AL", "ALIMENTOS", "system"),
                new Linea("BE", "BEBIDAS", "system"),
                new Linea("LI", "LIMPIEZA", "system"),
                new Linea("EL", "ELECTRÓNICA", "system"),
                new Linea("TE", "TEXTIL", "system"),
                new Linea("FE", "FERRETERÍA", "system"),
                new Linea("JU", "JUGUETERÍA", "system"),
                new Linea("LB", "LIBRERÍA", "system")
            };

            context.Linea.AddRange(lineas);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<LineaTranslation>
            {
                // ALIMENTOS
                new LineaTranslation(lineas[0].Id, "es", "ALIMENTOS", "system"),
                new LineaTranslation(lineas[0].Id, "en", "FOOD", "system"),
                new LineaTranslation(lineas[0].Id, "fr", "ALIMENTS", "system"),

                // BEBIDAS
                new LineaTranslation(lineas[1].Id, "es", "BEBIDAS", "system"),
                new LineaTranslation(lineas[1].Id, "en", "BEVERAGES", "system"),
                new LineaTranslation(lineas[1].Id, "fr", "BOISSONS", "system"),

                // LIMPIEZA
                new LineaTranslation(lineas[2].Id, "es", "LIMPIEZA", "system"),
                new LineaTranslation(lineas[2].Id, "en", "CLEANING", "system"),
                new LineaTranslation(lineas[2].Id, "fr", "NETTOYAGE", "system"),

                // ELECTRÓNICA
                new LineaTranslation(lineas[3].Id, "es", "ELECTRÓNICA", "system"),
                new LineaTranslation(lineas[3].Id, "en", "ELECTRONICS", "system"),
                new LineaTranslation(lineas[3].Id, "fr", "ÉLECTRONIQUE", "system"),

                // TEXTIL
                new LineaTranslation(lineas[4].Id, "es", "TEXTIL", "system"),
                new LineaTranslation(lineas[4].Id, "en", "TEXTILE", "system"),
                new LineaTranslation(lineas[4].Id, "fr", "TEXTILE", "system"),

                // FERRETERÍA
                new LineaTranslation(lineas[5].Id, "es", "FERRETERÍA", "system"),
                new LineaTranslation(lineas[5].Id, "en", "HARDWARE", "system"),
                new LineaTranslation(lineas[5].Id, "fr", "QUINCAILLERIE", "system"),

                // JUGUETERÍA
                new LineaTranslation(lineas[6].Id, "es", "JUGUETERÍA", "system"),
                new LineaTranslation(lineas[6].Id, "en", "TOY STORE", "system"),
                new LineaTranslation(lineas[6].Id, "fr", "JOUETS", "system"),

                // LIBRERÍA
                new LineaTranslation(lineas[7].Id, "es", "LIBRERÍA", "system"),
                new LineaTranslation(lineas[7].Id, "en", "BOOKSTORE", "system"),
                new LineaTranslation(lineas[7].Id, "fr", "LIBRAIRIE", "system")
            };

            context.LineaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Linea completado: {Count} líneas agregadas", lineas.Length);
        }

        #endregion

        #region SubLinea

        private static async Task SeedSubLineaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.SubLinea.Any())
            {
                logger.LogInformation("SubLinea ya tiene datos, omitiendo seed.");
                return;
            }

            // Obtener las líneas creadas
            var lineaAlimentos = context.Linea.First(l => l.Codigo == "AL");
            var lineaBebidas = context.Linea.First(l => l.Codigo == "BE");

            var sublineas = new[]
            {
                // Alimentos
                new SubLinea("ALCON", "CONSERVAS", lineaAlimentos.Id, "system"),
                new SubLinea("ALFRE", "PRODUCTOS FRESCOS", lineaAlimentos.Id, "system"),
                new SubLinea("ALCON", "CONGELADOS", lineaAlimentos.Id, "system"),

                // Bebidas
                new SubLinea("BEREF", "REFRESCOS", lineaBebidas.Id, "system"),
                new SubLinea("BEALC", "BEBIDAS ALCOHÓLICAS", lineaBebidas.Id, "system"),
                new SubLinea("BECAF", "CAFÉ Y TÉ", lineaBebidas.Id, "system")
            };

            context.SubLinea.AddRange(sublineas);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<SubLineaTranslation>
            {
                // CONSERVAS
                new SubLineaTranslation(sublineas[0].Id, "es", "CONSERVAS", "system"),
                new SubLineaTranslation(sublineas[0].Id, "en", "CANNED FOOD", "system"),
                new SubLineaTranslation(sublineas[0].Id, "fr", "CONSERVES", "system"),

                // PRODUCTOS FRESCOS
                new SubLineaTranslation(sublineas[1].Id, "es", "PRODUCTOS FRESCOS", "system"),
                new SubLineaTranslation(sublineas[1].Id, "en", "FRESH PRODUCTS", "system"),
                new SubLineaTranslation(sublineas[1].Id, "fr", "PRODUITS FRAIS", "system"),

                // CONGELADOS
                new SubLineaTranslation(sublineas[2].Id, "es", "CONGELADOS", "system"),
                new SubLineaTranslation(sublineas[2].Id, "en", "FROZEN", "system"),
                new SubLineaTranslation(sublineas[2].Id, "fr", "SURGELÉS", "system"),

                // REFRESCOS
                new SubLineaTranslation(sublineas[3].Id, "es", "REFRESCOS", "system"),
                new SubLineaTranslation(sublineas[3].Id, "en", "SOFT DRINKS", "system"),
                new SubLineaTranslation(sublineas[3].Id, "fr", "BOISSONS GAZEUSES", "system"),

                // BEBIDAS ALCOHÓLICAS
                new SubLineaTranslation(sublineas[4].Id, "es", "BEBIDAS ALCOHÓLICAS", "system"),
                new SubLineaTranslation(sublineas[4].Id, "en", "ALCOHOLIC BEVERAGES", "system"),
                new SubLineaTranslation(sublineas[4].Id, "fr", "BOISSONS ALCOOLISÉES", "system"),

                // CAFÉ Y TÉ
                new SubLineaTranslation(sublineas[5].Id, "es", "CAFÉ Y TÉ", "system"),
                new SubLineaTranslation(sublineas[5].Id, "en", "COFFEE & TEA", "system"),
                new SubLineaTranslation(sublineas[5].Id, "fr", "CAFÉ ET THÉ", "system")
            };

            context.SubLineaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de SubLinea completado: {Count} sublíneas agregadas", sublineas.Length);
        }

        #endregion

        #region UnidadMedida

        private static async Task SeedUnidadMedidaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.UnidadMedida.Any())
            {
                logger.LogInformation("UnidadMedida ya tiene datos, omitiendo seed.");
                return;
            }

            var unidades = new[]
            {
                new UnidadMedida("UND", "UNIDAD", "system"),
                new UnidadMedida("KG", "KILOGRAMO", "system"),
                new UnidadMedida("LT", "LITRO", "system"),
                new UnidadMedida("MT", "METRO", "system"),
                new UnidadMedida("CJ", "CAJA", "system"),
                new UnidadMedida("PAQ", "PAQUETE", "system")
            };

            context.UnidadMedida.AddRange(unidades);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<UnidadMedidaTranslation>
            {
                // UNIDAD
                new UnidadMedidaTranslation(unidades[0].Id, "es", "UNIDAD", "system"),
                new UnidadMedidaTranslation(unidades[0].Id, "en", "UNIT", "system"),
                new UnidadMedidaTranslation(unidades[0].Id, "fr", "UNITÉ", "system"),

                // KILOGRAMO
                new UnidadMedidaTranslation(unidades[1].Id, "es", "KILOGRAMO", "system"),
                new UnidadMedidaTranslation(unidades[1].Id, "en", "KILOGRAM", "system"),
                new UnidadMedidaTranslation(unidades[1].Id, "fr", "KILOGRAMME", "system"),

                // LITRO
                new UnidadMedidaTranslation(unidades[2].Id, "es", "LITRO", "system"),
                new UnidadMedidaTranslation(unidades[2].Id, "en", "LITER", "system"),
                new UnidadMedidaTranslation(unidades[2].Id, "fr", "LITRE", "system"),

                // METRO
                new UnidadMedidaTranslation(unidades[3].Id, "es", "METRO", "system"),
                new UnidadMedidaTranslation(unidades[3].Id, "en", "METER", "system"),
                new UnidadMedidaTranslation(unidades[3].Id, "fr", "MÈTRE", "system"),

                // CAJA
                new UnidadMedidaTranslation(unidades[4].Id, "es", "CAJA", "system"),
                new UnidadMedidaTranslation(unidades[4].Id, "en", "BOX", "system"),
                new UnidadMedidaTranslation(unidades[4].Id, "fr", "BOÎTE", "system"),

                // PAQUETE
                new UnidadMedidaTranslation(unidades[5].Id, "es", "PAQUETE", "system"),
                new UnidadMedidaTranslation(unidades[5].Id, "en", "PACKAGE", "system"),
                new UnidadMedidaTranslation(unidades[5].Id, "fr", "PAQUET", "system")
            };

            context.UnidadMedidaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de UnidadMedida completado: {Count} unidades agregadas", unidades.Length);
        }

        #endregion
    }
}