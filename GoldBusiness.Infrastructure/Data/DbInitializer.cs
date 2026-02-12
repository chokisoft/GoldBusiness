using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("Iniciando seed de datos maestros...");

                // Plan de cuentas
                await SeedGrupoCuentaAsync(context, logger);
                await SeedSubGrupoCuentaAsync(context, logger);

                // SystemConfiguration
                await SeedSystemConfigurationAsync(context, logger);
                await SeedEstablecimientoAsync(context, logger);

                // Todas las cuentas reales
                await SeedCuentaAsync(context, logger);

                // Todas las localidades
                await SeedLocalidadAsync(context, logger);

                // Actualizar SystemConfiguration con cuentas reales
                await UpdateSystemConfigurationWithAccountsAsync(context, logger);

                // Otros seeds
                await SeedLineaAsync(context, logger);
                await SeedSubLineaAsync(context, logger);
                await SeedMonedaAsync(context, logger);
                await SeedConceptoAjusteAsync(context, logger);
                await SeedUnidadMedidaAsync(context, logger);
                await SeedTransaccionAsync(context, logger);

                logger.LogInformation("✅ Seed de datos maestros completado exitosamente!");
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
                new GrupoCuenta("10", "GRUPO DE ACTIVO", "system"),
                new GrupoCuenta("20", "GRUPO DE PASIVO", "system"),
                new GrupoCuenta("30", "GRUPO DE PATRIMONIO", "system"),
                new GrupoCuenta("40", "GRUPO DE CUENTAS NOMINALES", "system"),
                new GrupoCuenta("50", "CUENTA DE CIERRE", "system")
            };

            context.GrupoCuenta.AddRange(grupos);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<GrupoCuentaTranslation>
            {
                // ACTIVO
                new(grupos[0].Id, "es", "GRUPO DE ACTIVO", "system"),
                new(grupos[0].Id, "en", "ASSET GROUP", "system"),
                new(grupos[0].Id, "fr", "GROUPE D’ACTIFS", "system"),

                // PASIVO
                new(grupos[1].Id, "es", "GRUPO DE PASIVO", "system"),
                new(grupos[1].Id, "en", "LIABILITY GROUP", "system"),
                new(grupos[1].Id, "fr", "GROUPE DE PASSIF", "system"),

                // PATRIMONIO
                new(grupos[2].Id, "es", "GRUPO DE PATRIMONIO", "system"),
                new(grupos[2].Id, "en", "EQUITY GROUP", "system"),
                new(grupos[2].Id, "fr", "GROUPE DE PATRIMOINE", "system"),

                // INGRESOS
                new(grupos[3].Id, "es", "GRUPO DE CUENTAS NOMINALES", "system"),
                new(grupos[3].Id, "en", "NOMINAL ACCOUNTS GROUP", "system"),
                new(grupos[3].Id, "fr", "GROUPE DE COMPTES NOMINAUX", "system"),

                // GASTOS
                new(grupos[4].Id, "es", "CUENTA DE CIERRE", "system"),
                new(grupos[4].Id, "en", "CLOSING ACCOUNT", "system"),
                new(grupos[4].Id, "fr", "COMPTE DE CLÔTURE", "system")
            };

            context.GrupoCuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de GrupoCuenta completado: {Count} grupos agregados", grupos.Length);
        }

        #endregion

        #region SubGrupoCuenta

        private static async Task SeedSubGrupoCuentaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.SubGrupoCuenta.Any())
            {
                logger.LogInformation("SubGrupoCuenta ya tiene datos, omitiendo seed.");
                return;
            }

            // Obtener los GrupoCuenta para referenciarlos
            var grupoActivo = context.GrupoCuenta.First(g => g.Codigo == "10");
            var grupoPasivo = context.GrupoCuenta.First(g => g.Codigo == "20");
            var grupoPatrimonio = context.GrupoCuenta.First(g => g.Codigo == "30");
            var grupoNominales = context.GrupoCuenta.First(g => g.Codigo == "40");
            var grupoCierre = context.GrupoCuenta.First(g => g.Codigo == "50");

            var subgrupos = new[]
            {
                new SubGrupoCuenta("10101", "ACTIVO CIRCULANTE", grupoActivo.Id, deudora: true, "system"),
                new SubGrupoCuenta("10102", "ACTIVOS FIJOS", grupoActivo.Id, deudora: true, "system"),
                new SubGrupoCuenta("10103", "CUENTAS REGULADORAS DE ACTIVOS", grupoActivo.Id, deudora: false, "system"),
                new SubGrupoCuenta("20201", "PASIVOS CIRCULANTES", grupoPasivo.Id, deudora: false, "system"),
                new SubGrupoCuenta("30300", "GRUPO PATRIMONIO", grupoPatrimonio.Id, deudora: false, "system"),
                new SubGrupoCuenta("40401", "CUENTAS NOMINALES DEUDORAS", grupoNominales.Id, deudora: true, "system"),
                new SubGrupoCuenta("40402", "CUENTAS NOMINALES ACREEDORAS", grupoNominales.Id, deudora: false, "system"),
                new SubGrupoCuenta("50500", "CUENTA DE CIERRE", grupoCierre.Id, deudora: false, "system")
            };

            context.SubGrupoCuenta.AddRange(subgrupos);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<SubGrupoCuentaTranslation>
            {
                // 10101 - ACTIVO CIRCULANTE
                new(subgrupos[0].Id, "es", "ACTIVO CIRCULANTE", "system"),
                new(subgrupos[0].Id, "en", "CURRENT ASSETS", "system"),
                new(subgrupos[0].Id, "fr", "ACTIF CIRCULANT", "system"),

                // 10102 - ACTIVOS FIJOS
                new(subgrupos[1].Id, "es", "ACTIVOS FIJOS", "system"),
                new(subgrupos[1].Id, "en", "FIXED ASSETS", "system"),
                new(subgrupos[1].Id, "fr", "ACTIFS FIXES", "system"),

                // 10103 - CUENTAS REGULADORAS DE ACTIVOS
                new(subgrupos[2].Id, "es", "CUENTAS REGULADORAS DE ACTIVOS", "system"),
                new(subgrupos[2].Id, "en", "ASSET CONTRA ACCOUNTS", "system"),
                new(subgrupos[2].Id, "fr", "COMPTES CORRECTEURS D'ACTIFS", "system"),

                // 20201 - PASIVOS CIRCULANTES
                new(subgrupos[3].Id, "es", "PASIVOS CIRCULANTES", "system"),
                new(subgrupos[3].Id, "en", "CURRENT LIABILITIES", "system"),
                new(subgrupos[3].Id, "fr", "PASSIFS CIRCULANTS", "system"),

                // 30300 - GRUPO PATRIMONIO
                new(subgrupos[4].Id, "es", "GRUPO PATRIMONIO", "system"),
                new(subgrupos[4].Id, "en", "EQUITY GROUP", "system"),
                new(subgrupos[4].Id, "fr", "GROUPE PATRIMOINE", "system"),

                // 40401 - CUENTAS NOMINALES DEUDORAS
                new(subgrupos[5].Id, "es", "CUENTAS NOMINALES DEUDORAS", "system"),
                new(subgrupos[5].Id, "en", "DEBIT NOMINAL ACCOUNTS", "system"),
                new(subgrupos[5].Id, "fr", "COMPTES NOMINAUX DÉBITEURS", "system"),

                // 40402 - CUENTAS NOMINALES ACREEDORAS
                new(subgrupos[6].Id, "es", "CUENTAS NOMINALES ACREEDORAS", "system"),
                new(subgrupos[6].Id, "en", "CREDIT NOMINAL ACCOUNTS", "system"),
                new(subgrupos[6].Id, "fr", "COMPTES NOMINAUX CRÉDITEURS", "system"),

                // 50500 - CUENTA DE CIERRE
                new(subgrupos[7].Id, "es", "CUENTA DE CIERRE", "system"),
                new(subgrupos[7].Id, "en", "CLOSING ACCOUNT", "system"),
                new(subgrupos[7].Id, "fr", "COMPTE DE CLÔTURE", "system")
            };

            context.SubGrupoCuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de SubGrupoCuenta completado: {Count} subgrupos agregados", subgrupos.Length);
        }

        #endregion

        #region SystemConfiguration

        private static async Task SeedSystemConfigurationAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.SystemConfiguration.Any())
            {
                logger.LogInformation("SystemConfiguration ya tiene datos, omitiendo seed.");
                return;
            }

            // ✅ Crear SystemConfiguration SIN cuentas (gracias a nullable)
            var sysConfig = new SystemConfiguration(
                "CHK",
                "uxi/LeQnoZmyHjpkrS2J7RgiO6dKhwdapmg5r7TuwpnDzq2FPwwOWbLwRU6zUcRME2XktTsXkNmonkrYHFFPzg==",
                "CHOKISOFT SOLUCIONES TECNOLÓGICAS",
                "CALLE 172 #17830 E/ 180 y 182, REPARTO 1ERO DE MAYO",
                "BOYEROS",
                "LA HABANA",
                "10800",
                "http://localhost/imagen/imagen.jpg",
                "http://localhost/",
                "chokisoft@gmail.com",
                "+53 55152424",
                DateTime.UtcNow.AddYears(1),
                "system");

            context.SystemConfiguration.Add(sysConfig);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<SystemConfigurationTranslation>
            {
                new(sysConfig.Id, "es", "CHOKISOFT SOLUCIONES TECNOLÓGICAS", "CALLE 172 #17830 E/ 180 y 182, REPARTO 1ERO DE MAYO", "BOYEROS", "LA HABANA", "system"),
                new(sysConfig.Id, "en", "CHOKISOFT TECHNOLOGY SOLUTIONS", "172ND STREET #17830 BETWEEN 180TH AND 182ND, 1ST OF MAY NEIGHBORHOOD", "BOYEROS", "HAVANA", "system"),
                new(sysConfig.Id, "fr", "CHOKISOFT SOLUTIONS TECHNOLOGIQUES", "RUE 172 N°17830 ENTRE 180 ET 182, QUARTIER 1ER MAI", "BOYEROS", "LA HAVANE", "system")
            };

            context.SystemConfigurationTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ SystemConfiguration creado (Id={Id})", sysConfig.Id);
        }

        #endregion

        #region Cuenta

        private static async Task SeedCuentaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Cuenta.Any())
            {
                logger.LogInformation("Cuenta ya tiene datos, omitiendo seed.");
                return;
            }

            var sysConfig = context.SystemConfiguration.First();
            var subgrupos = context.SubGrupoCuenta.ToList();

            var sgActivoCirculante = subgrupos.First(s => s.Codigo == "10101");
            var sgActivosFijos = subgrupos.First(s => s.Codigo == "10102");
            var sgReguladoras = subgrupos.First(s => s.Codigo == "10103");
            var sgPasivoCirculante = subgrupos.First(s => s.Codigo == "20201");
            var sgPatrimonio = subgrupos.First(s => s.Codigo == "30300");
            var sgNominalesDeudoras = subgrupos.First(s => s.Codigo == "40401");
            var sgNominalesAcreedoras = subgrupos.First(s => s.Codigo == "40402");
            var sgCierre = subgrupos.First(s => s.Codigo == "50500");

            var cuentas = new[]
            {
                new Cuenta("10000010", "EFECTIVO EN CAJA", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("10101135", "CUENTAS POR COBRAR A CORTO PLAZO", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("11000010", "EFECTIVO EN BANCO", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("18800010", "PRODUCCIÓN TERMINADA", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("18900010", "MERCANCÍAS PARA LA VENTA", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("19200010", "VESTUARIO Y LENCERÍA", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("19300010", "INSUMOS", 1, sgActivoCirculante.Id, "system"),
                new Cuenta("20000010", "ACTIVOS FIJOS TANGIBLES", 1, sgActivosFijos.Id, "system"),
                new Cuenta("30000010", "DEPRECIACIÓN DE ACTIVOS FIJOS TANGIBLES", 1, sgReguladoras.Id, "system"),
                new Cuenta("40500000", "CUENTAS POR PAGAR A CORTO PLAZO", 1, sgPasivoCirculante.Id, "system"),
                new Cuenta("42100000", "CUENTAS POR PAGAR - ACTIVOS FIJOS TANGIBLES", 1, sgPasivoCirculante.Id, "system"),
                new Cuenta("47000010", "PRÉSTAMOS BANCARIOS A CORTO PLAZO", 1, sgPasivoCirculante.Id, "system"),
                new Cuenta("52000010", "PRÉSTAMOS BANCARIOS A LARGO PLAZO", 1, sgPasivoCirculante.Id, "system"),
                new Cuenta("60000010", "PATRIMONIO DEL TCP", 1, sgPatrimonio.Id, "system"),
                new Cuenta("60010010", "SALDO AL INICIO DEL EJERCICIO", 1, sgPatrimonio.Id, "system"),
                new Cuenta("60020010", "INCREMENTOS DE APORTES DEL TCP EN EL EJERCICIO CONTABLE", 1, sgPatrimonio.Id, "system"),
                new Cuenta("60030010", "EROGACIONES EFECTUADAS POR EL TCP EN EL EJERCICIO CONTABLE", 1, sgPatrimonio.Id, "system"),
                new Cuenta("60040010", "PAGOS DE CUOTAS DEL IMPUESTO SOBRE INGRESOS PERSONALES", 1, sgPatrimonio.Id, "system"),
                new Cuenta("60050010", "CONTRIBUCION A LA SEGURIDAD SOCIAL", 1, sgPatrimonio.Id, "system"),
                new Cuenta("61000010", "UTILIDADES RETENIDAS", 1, sgPatrimonio.Id, "system"),
                new Cuenta("62000010", "PÉRDIDA", 1, sgPatrimonio.Id, "system"),
                new Cuenta("80000010", "GASTOS DE OPERACIÓN", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("80400010", "DEVOLUCIONES Y REBAJAS EN VENTAS", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("80600010", "COSTO DE VENTA DE LA PRODUCCIÓN", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("80800010", "COSTO DE VENTA DE MERCANCÍAS", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("81000010", "IMPUESTOS Y TASAS", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("81010010", "IMPUESTOS SOBRE LAS VENTAS", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("81020010", "IMPUESTO SOBRE LOS SERVICIOS PÚBLICOS", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("81030010", "IMPUESTO POR LA UTILIZACIÓN DE LA FUERZA DE TRABAJO", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("81040010", "OTROS IMPUESTOS Y TASAS", 1, sgNominalesDeudoras.Id, "system"),
                new Cuenta("90000001", "VENTAS", 1, sgNominalesAcreedoras.Id, "system"),
                new Cuenta("99900001", "RESULTADOS", 1, sgCierre.Id, "system")
            };

            context.Cuenta.AddRange(cuentas);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<CuentaTranslation>
            {
                // 10000010 - EFECTIVO EN CAJA
                new(cuentas[0].Id, "es", "EFECTIVO EN CAJA", "system"),
                new(cuentas[0].Id, "en", "CASH ON HAND", "system"),
                new(cuentas[0].Id, "fr", "CAISSE", "system"),

                // 10101135 - CUENTAS POR COBRAR A CORTO PLAZO
                new(cuentas[1].Id, "es", "CUENTAS POR COBRAR A CORTO PLAZO", "system"),
                new(cuentas[1].Id, "en", "SHORT-TERM ACCOUNTS RECEIVABLE", "system"),
                new(cuentas[1].Id, "fr", "COMPTES À RECEVOIR À COURT TERME", "system"),

                // 11000010 - EFECTIVO EN BANCO
                new(cuentas[2].Id, "es", "EFECTIVO EN BANCO", "system"),
                new(cuentas[2].Id, "en", "CASH IN BANK", "system"),
                new(cuentas[2].Id, "fr", "BANQUE", "system"),

                // 18800010 - PRODUCCIÓN TERMINADA
                new(cuentas[3].Id, "es", "PRODUCCIÓN TERMINADA", "system"),
                new(cuentas[3].Id, "en", "FINISHED PRODUCTION", "system"),
                new(cuentas[3].Id, "fr", "PRODUCTION TERMINÉE", "system"),

                // 18900010 - MERCANCÍAS PARA LA VENTA
                new(cuentas[4].Id, "es", "MERCANCÍAS PARA LA VENTA", "system"),
                new(cuentas[4].Id, "en", "MERCHANDISE FOR SALE", "system"),
                new(cuentas[4].Id, "fr", "MARCHANDISES À VENDRE", "system"),

                // 19200010 - VESTUARIO Y LENCERÍA
                new(cuentas[5].Id, "es", "VESTUARIO Y LENCERÍA", "system"),
                new(cuentas[5].Id, "en", "CLOTHING AND LINGERIE", "system"),
                new(cuentas[5].Id, "fr", "VÊTEMENTS ET LINGERIE", "system"),

                // 19300010 - INSUMOS
                new(cuentas[6].Id, "es", "INSUMOS", "system"),
                new(cuentas[6].Id, "en", "SUPPLIES", "system"),
                new(cuentas[6].Id, "fr", "FOURNITURES", "system"),

                // 20000010 - ACTIVOS FIJOS TANGIBLES
                new(cuentas[7].Id, "es", "ACTIVOS FIJOS TANGIBLES", "system"),
                new(cuentas[7].Id, "en", "TANGIBLE FIXED ASSETS", "system"),
                new(cuentas[7].Id, "fr", "IMMOBILISATIONS CORPORELLES", "system"),

                // 30000010 - DEPRECIACIÓN DE ACTIVOS FIJOS TANGIBLES
                new(cuentas[8].Id, "es", "DEPRECIACIÓN DE ACTIVOS FIJOS TANGIBLES", "system"),
                new(cuentas[8].Id, "en", "DEPRECIATION OF TANGIBLE FIXED ASSETS", "system"),
                new(cuentas[8].Id, "fr", "AMORTISSEMENT DES IMMOBILISATIONS CORPORELLES", "system"),

                // 40500000 - CUENTAS POR PAGAR A CORTO PLAZO
                new(cuentas[9].Id, "es", "CUENTAS POR PAGAR A CORTO PLAZO", "system"),
                new(cuentas[9].Id, "en", "SHORT-TERM ACCOUNTS PAYABLE", "system"),
                new(cuentas[9].Id, "fr", "COMPTES À PAYER À COURT TERME", "system"),

                // 42100000 - CUENTAS POR PAGAR - ACTIVOS FIJOS TANGIBLES
                new(cuentas[10].Id, "es", "CUENTAS POR PAGAR - ACTIVOS FIJOS TANGIBLES", "system"),
                new(cuentas[10].Id, "en", "ACCOUNTS PAYABLE - TANGIBLE FIXED ASSETS", "system"),
                new(cuentas[10].Id, "fr", "COMPTES À PAYER - IMMOBILISATIONS CORPORELLES", "system"),

                // 47000010 - PRÉSTAMOS BANCARIOS A CORTO PLAZO
                new(cuentas[11].Id, "es", "PRÉSTAMOS BANCARIOS A CORTO PLAZO", "system"),
                new(cuentas[11].Id, "en", "SHORT-TERM BANK LOANS", "system"),
                new(cuentas[11].Id, "fr", "PRÊTS BANCAIRES À COURT TERME", "system"),

                // 52000010 - PRÉSTAMOS BANCARIOS A LARGO PLAZO
                new(cuentas[12].Id, "es", "PRÉSTAMOS BANCARIOS A LARGO PLAZO", "system"),
                new(cuentas[12].Id, "en", "LONG-TERM BANK LOANS", "system"),
                new(cuentas[12].Id, "fr", "PRÊTS BANCAIRES À LONG TERME", "system"),

                // 60000010 - PATRIMONIO DEL TCP
                new(cuentas[13].Id, "es", "PATRIMONIO DEL TCP", "system"),
                new(cuentas[13].Id, "en", "TCP EQUITY", "system"),
                new(cuentas[13].Id, "fr", "CAPITAUX PROPRES DU TCP", "system"),

                // 60010010 - SALDO AL INICIO DEL EJERCICIO
                new(cuentas[14].Id, "es", "SALDO AL INICIO DEL EJERCICIO", "system"),
                new(cuentas[14].Id, "en", "BALANCE AT THE BEGINNING OF THE PERIOD", "system"),
                new(cuentas[14].Id, "fr", "SOLDE AU DÉBUT DE L'EXERCICE", "system"),

                // 60020010 - INCREMENTOS DE APORTES DEL TCP EN EL EJERCICIO CONTABLE
                new(cuentas[15].Id, "es", "INCREMENTOS DE APORTES DEL TCP EN EL EJERCICIO CONTABLE", "system"),
                new(cuentas[15].Id, "en", "INCREASES IN TCP CONTRIBUTIONS DURING THE ACCOUNTING PERIOD", "system"),
                new(cuentas[15].Id, "fr", "AUGMENTATIONS DES APPORTS DU TCP PENDANT L'EXERCICE COMPTABLE", "system"),

                // 60030010 - EROGACIONES EFECTUADAS POR EL TCP EN EL EJERCICIO CONTABLE
                new(cuentas[16].Id, "es", "EROGACIONES EFECTUADAS POR EL TCP EN EL EJERCICIO CONTABLE", "system"),
                new(cuentas[16].Id, "en", "DISBURSEMENTS MADE BY TCP DURING THE ACCOUNTING PERIOD", "system"),
                new(cuentas[16].Id, "fr", "DÉBOURSEMENTS EFFECTUÉS PAR LE TCP PENDANT L'EXERCICE COMPTABLE", "system"),

                // 60040010 - PAGOS DE CUOTAS DEL IMPUESTO SOBRE INGRESOS PERSONALES
                new(cuentas[17].Id, "es", "PAGOS DE CUOTAS DEL IMPUESTO SOBRE INGRESOS PERSONALES", "system"),
                new(cuentas[17].Id, "en", "PERSONAL INCOME TAX INSTALLMENT PAYMENTS", "system"),
                new(cuentas[17].Id, "fr", "PAIEMENTS D'ACOMPTES DE L'IMPÔT SUR LE REVENU DES PERSONNES", "system"),

                // 60050010 - CONTRIBUCION A LA SEGURIDAD SOCIAL
                new(cuentas[18].Id, "es", "CONTRIBUCION A LA SEGURIDAD SOCIAL", "system"),
                new(cuentas[18].Id, "en", "SOCIAL SECURITY CONTRIBUTIONS", "system"),
                new(cuentas[18].Id, "fr", "COTISATIONS À LA SÉCURITÉ SOCIALE", "system"),

                // 61000010 - UTILIDADES RETENIDAS
                new(cuentas[19].Id, "es", "UTILIDADES RETENIDAS", "system"),
                new(cuentas[19].Id, "en", "RETAINED EARNINGS", "system"),
                new(cuentas[19].Id, "fr", "BÉNÉFICES NON RÉPARTIS", "system"),

                // 62000010 - PÉRDIDA
                new(cuentas[20].Id, "es", "PÉRDIDA", "system"),
                new(cuentas[20].Id, "en", "LOSS", "system"),
                new(cuentas[20].Id, "fr", "PERTE", "system"),

                // 80000010 - GASTOS DE OPERACIÓN
                new(cuentas[21].Id, "es", "GASTOS DE OPERACIÓN", "system"),
                new(cuentas[21].Id, "en", "OPERATING EXPENSES", "system"),
                new(cuentas[21].Id, "fr", "FRAIS D'EXPLOITATION", "system"),

                // 80400010 - DEVOLUCIONES Y REBAJAS EN VENTAS
                new(cuentas[22].Id, "es", "DEVOLUCIONES Y REBAJAS EN VENTAS", "system"),
                new(cuentas[22].Id, "en", "SALES RETURNS AND ALLOWANCES", "system"),
                new(cuentas[22].Id, "fr", "RETOURS ET RABAIS SUR VENTES", "system"),

                // 80600010 - COSTO DE VENTA DE LA PRODUCCIÓN
                new(cuentas[23].Id, "es", "COSTO DE VENTA DE LA PRODUCCIÓN", "system"),
                new(cuentas[23].Id, "en", "COST OF PRODUCTION SALES", "system"),
                new(cuentas[23].Id, "fr", "COÛT DES VENTES DE PRODUCTION", "system"),

                // 80800010 - COSTO DE VENTA DE MERCANCÍAS
                new(cuentas[24].Id, "es", "COSTO DE VENTA DE MERCANCÍAS", "system"),
                new(cuentas[24].Id, "en", "COST OF MERCHANDISE SOLD", "system"),
                new(cuentas[24].Id, "fr", "COÛT DES MARCHANDISES VENDUES", "system"),

                // 81000010 - IMPUESTOS Y TASAS
                new(cuentas[25].Id, "es", "IMPUESTOS Y TASAS", "system"),
                new(cuentas[25].Id, "en", "TAXES AND FEES", "system"),
                new(cuentas[25].Id, "fr", "IMPÔTS ET TAXES", "system"),

                // 81010010 - IMPUESTOS SOBRE LAS VENTAS
                new(cuentas[26].Id, "es", "IMPUESTOS SOBRE LAS VENTAS", "system"),
                new(cuentas[26].Id, "en", "SALES TAXES", "system"),
                new(cuentas[26].Id, "fr", "TAXES SUR LES VENTES", "system"),

                // 81020010 - IMPUESTO SOBRE LOS SERVICIOS PÚBLICOS
                new(cuentas[27].Id, "es", "IMPUESTO SOBRE LOS SERVICIOS PÚBLICOS", "system"),
                new(cuentas[27].Id, "en", "PUBLIC SERVICES TAX", "system"),
                new(cuentas[27].Id, "fr", "TAXE SUR LES SERVICES PUBLICS", "system"),

                // 81030010 - IMPUESTO POR LA UTILIZACIÓN DE LA FUERZA DE TRABAJO
                new(cuentas[28].Id, "es", "IMPUESTO POR LA UTILIZACIÓN DE LA FUERZA DE TRABAJO", "system"),
                new(cuentas[28].Id, "en", "LABOR UTILIZATION TAX", "system"),
                new(cuentas[28].Id, "fr", "TAXE SUR L'UTILISATION DE LA MAIN-D'ŒUVRE", "system"),

                // 81040010 - OTROS IMPUESTOS Y TASAS
                new(cuentas[29].Id, "es", "OTROS IMPUESTOS Y TASAS", "system"),
                new(cuentas[29].Id, "en", "OTHER TAXES AND FEES", "system"),
                new(cuentas[29].Id, "fr", "AUTRES IMPÔTS ET TAXES", "system"),

                // 90000001 - VENTAS
                new(cuentas[30].Id, "es", "VENTAS", "system"),
                new(cuentas[30].Id, "en", "SALES", "system"),
                new(cuentas[30].Id, "fr", "VENTES", "system"),

                // 99900001 - RESULTADOS
                new(cuentas[31].Id, "es", "RESULTADOS", "system"),
                new(cuentas[31].Id, "en", "RESULTS", "system"),
                new(cuentas[31].Id, "fr", "RÉSULTATS", "system")
            };

            context.CuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Cuenta completado: {Count} cuentas agregadas", cuentas.Length);
        }

        #endregion

        #region Update SystemConfiguration

        private static async Task UpdateSystemConfigurationWithAccountsAsync(ApplicationDbContext context, ILogger logger)
        {
            var sysConfig = context.SystemConfiguration.FirstOrDefault();
            if (sysConfig == null)
            {
                logger.LogWarning("⚠️ No se encontró SystemConfiguration");
                return;
            }

            // Buscar cuentas por pagar y cobrar
            var cuentaCobrar = context.Cuenta.FirstOrDefault(c => c.Codigo == "10101135");
            var cuentaPagar = context.Cuenta.FirstOrDefault(c => c.Codigo == "40500000");

            if (cuentaCobrar != null && cuentaPagar != null)
            {
                sysConfig.SetCuentas(cuentaPagar.Id, cuentaCobrar.Id);

                sysConfig.ActualizarAuditoria("system");

                context.SystemConfiguration.Update(sysConfig);
                await context.SaveChangesAsync();

                logger.LogInformation("✅ Cuentas asignadas → CuentaPagar: {CuentaPagar}, CuentaCobrar: {CuentaCobrar}",
                    cuentaPagar.Codigo, cuentaCobrar.Codigo);
            }
            else
            {
                logger.LogWarning("⚠️ No se encontraron las cuentas para asignar");
            }
        }

        #endregion

        #region Establecimiento

        private static async Task SeedEstablecimientoAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Establecimiento.Any())
            {
                logger.LogInformation("Establecimiento ya tiene datos, omitiendo seed.");
                return;
            }

            var establecimiento = new[]
            {
                new Establecimiento("CHK001", "DESARROLLO DE SOFTWARE", 1, "system"),
            };

            context.Establecimiento.AddRange(establecimiento);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<EstablecimientoTranslation>
            {
                new(establecimiento[0].Id, "es", "DESARROLLO DE SOFTWARE", "system"),
                new(establecimiento[0].Id, "en", "SOFTWARE DEVELOPMENT", "system"),
                new(establecimiento[0].Id, "fr", "DÉVELOPPEMENT LOGICIEL", "system"),
            };

            context.EstablecimientoTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Establecimiento completado: {Count} establecimientos agregadas", establecimiento.Length);
        }

        #endregion

        #region Localidad

        private static async Task SeedLocalidadAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Localidad.Any())
            {
                logger.LogInformation("Localidad ya tiene datos, omitiendo seed.");
                return;
            }

            var localidad = new[]
            {
                new Localidad("CHK001001", "GERENCIA GENERAL", 1,  5, 25, 31, 23, false, "system"),
                new Localidad("CHK001002", "ALMACÉN CENTRAL", 1,  5, 25, 31, 23, true, "system"),
                new Localidad("CHK001003", "DESARROLLO SOFTWARE", 1,  5, 25, 31, 23, false, "system"),
            };

            context.Localidad.AddRange(localidad);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<LocalidadTranslation>
            {
                new(localidad[0].Id, "es", "GERENCIA GENERAL", "system"),
                new(localidad[0].Id, "en", "GENERAL MANAGEMENT", "system"),
                new(localidad[0].Id, "fr", "DIRECTION GÉNÉRALE", "system"),

                new(localidad[1].Id, "es", "ALMACÉN CENTRAL", "system"),
                new(localidad[1].Id, "en", "CENTRAL WAREHOUSE", "system"),
                new(localidad[1].Id, "fr", "ENTREPÔT CENTRAL", "system"),

                new(localidad[2].Id, "es", "DESARROLLO SOFTWARE", "system"),
                new(localidad[2].Id, "en", "SOFTWARE DEVELOPMENT", "system"),
                new(localidad[2].Id, "fr", "DÉVELOPPEMENT LOGICIEL", "system"),
            };

            context.LocalidadTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Localidad completado: {Count} localidad agregadas", localidad.Length);
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
                new Linea("01", "CONFECCIONES PARA HOMBRE", "system"),
                new Linea("02", "CONFECCIONES NIÑOS Y JOVENCITOS", "system"),
                new Linea("03", "CONFECCIONES PARA DAMAS", "system"),
                new Linea("04", "CONFECCIONES NIÑAS Y JOVENCITAS", "system"),
                new Linea("05", "CANASTILLA", "system"),
                new Linea("06", "SEDERIA", "system"),
                new Linea("07", "TEJIDO P/VESTIR Y TAPIZAR", "system"),
                new Linea("08", "CALZADO", "system"),
                new Linea("09", "TALABARTERIA Y ACCESORIOS", "system"),
                new Linea("10", "PERFUMERÍA, ASEO Y FARMACIA", "system"),
                new Linea("11", "JOYERÍA, BISUTERÍA Y RELOJERÍA", "system"),
                new Linea("12", "ELECTRONICA", "system"),
                new Linea("13", "ELECTRODOMESTICOS", "system"),
                new Linea("14", "UTILES P/HOGAR FERRETERÍA DOMESTICA", "system"),
                new Linea("15", "ELEMENTOS Y UTÍLES DE FERRTERÍA GRUESA", "system"),
                new Linea("16", "AJUARES DE CASA", "system"),
                new Linea("17", "MUEBLES Y COLCHONES", "system"),
                new Linea("18", "EFECTOS DE OFICINA, LIBRERÍA", "system"),
                new Linea("19", "JUGUETES Y ATÍCULOS PARA FIESTAS", "system"),
                new Linea("20", "FOTOGRAFÍA", "system"),
                new Linea("21", "NUMISMATICA, FILATELIA", "system"),
                new Linea("22", "ARTÍCULOS DEPORTIVOS Y RECREATIVOS", "system"),
                new Linea("23", "ALIMENTOS, GOLOSINAS Y POSTRES", "system"),
                new Linea("24", "BEBIDAS, LICORES Y CREVEZAS", "system"),
                new Linea("25", "CIGARROS Y TABACOS", "system"),
                new Linea("26", "ARTÍCULOS DESECHABLES E INSUMOS", "system"),
                new Linea("27", "AUTOMOTRIZ Y COMBUSTIBLE", "system"),
                new Linea("28", "GASTRONOMÍA", "system"),
                new Linea("29", "SERVICIOS GENERALES MINORISTAS", "system")
            };

            context.Linea.AddRange(lineas);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<LineaTranslation>
            {
                // 01 - CONFECCIONES PARA HOMBRE
                new(lineas[0].Id, "es", "CONFECCIONES PARA HOMBRE", "system"),
                new(lineas[0].Id, "en", "MEN'S CLOTHING", "system"),
                new(lineas[0].Id, "fr", "CONFECTIONS POUR HOMMES", "system"),

                // 02 - CONFECCIONES NIÑOS Y JOVENCITOS
                new(lineas[1].Id, "es", "CONFECCIONES NIÑOS Y JOVENCITOS", "system"),
                new(lineas[1].Id, "en", "BOYS' AND YOUTH CLOTHING", "system"),
                new(lineas[1].Id, "fr", "CONFECTIONS GARÇONS ET JEUNES", "system"),

                // 03 - CONFECCIONES PARA DAMAS
                new(lineas[2].Id, "es", "CONFECCIONES PARA DAMAS", "system"),
                new(lineas[2].Id, "en", "WOMEN'S CLOTHING", "system"),
                new(lineas[2].Id, "fr", "CONFECTIONS POUR DAMES", "system"),

                // 04 - CONFECCIONES NIÑAS Y JOVENCITAS
                new(lineas[3].Id, "es", "CONFECCIONES NIÑAS Y JOVENCITAS", "system"),
                new(lineas[3].Id, "en", "GIRLS' AND YOUNG WOMEN'S CLOTHING", "system"),
                new(lineas[3].Id, "fr", "CONFECTIONS FILLES ET JEUNES FEMMES", "system"),

                // 05 - CANASTILLA
                new(lineas[4].Id, "es", "CANASTILLA", "system"),
                new(lineas[4].Id, "en", "LAYETTE", "system"),
                new(lineas[4].Id, "fr", "TROUSSEAU DE BÉBÉ", "system"),

                // 06 - SEDERIA
                new(lineas[5].Id, "es", "SEDERIA", "system"),
                new(lineas[5].Id, "en", "SILK GOODS", "system"),
                new(lineas[5].Id, "fr", "SOIERIE", "system"),

                // 07 - TEJIDO P/VESTIR Y TAPIZAR
                new(lineas[6].Id, "es", "TEJIDO P/VESTIR Y TAPIZAR", "system"),
                new(lineas[6].Id, "en", "CLOTHING AND UPHOLSTERY FABRICS", "system"),
                new(lineas[6].Id, "fr", "TISSUS POUR VÊTEMENTS ET TAPISSERIE", "system"),

                // 08 - CALZADO
                new(lineas[7].Id, "es", "CALZADO", "system"),
                new(lineas[7].Id, "en", "FOOTWEAR", "system"),
                new(lineas[7].Id, "fr", "CHAUSSURES", "system"),

                // 09 - TALABARTERIA Y ACCESORIOS
                new(lineas[8].Id, "es", "TALABARTERIA Y ACCESORIOS", "system"),
                new(lineas[8].Id, "en", "LEATHER GOODS AND ACCESSORIES", "system"),
                new(lineas[8].Id, "fr", "MAROQUINERIE ET ACCESSOIRES", "system"),

                // 10 - PERFUMERÍA, ASEO Y FARMACIA
                new(lineas[9].Id, "es", "PERFUMERÍA, ASEO Y FARMACIA", "system"),
                new(lineas[9].Id, "en", "PERFUMERY, TOILETRIES AND PHARMACY", "system"),
                new(lineas[9].Id, "fr", "PARFUMERIE, HYGIÈNE ET PHARMACIE", "system"),

                // 11 - JOYERÍA, BISUTERÍA Y RELOJERÍA
                new(lineas[10].Id, "es", "JOYERÍA, BISUTERÍA Y RELOJERÍA", "system"),
                new(lineas[10].Id, "en", "JEWELRY, COSTUME JEWELRY AND WATCHES", "system"),
                new(lineas[10].Id, "fr", "BIJOUTERIE, FANTAISIE ET HORLOGERIE", "system"),

                // 12 - ELECTRONICA
                new(lineas[11].Id, "es", "ELECTRONICA", "system"),
                new(lineas[11].Id, "en", "ELECTRONICS", "system"),
                new(lineas[11].Id, "fr", "ÉLECTRONIQUE", "system"),

                // 13 - ELECTRODOMESTICOS
                new(lineas[12].Id, "es", "ELECTRODOMESTICOS", "system"),
                new(lineas[12].Id, "en", "HOME APPLIANCES", "system"),
                new(lineas[12].Id, "fr", "ÉLECTROMÉNAGER", "system"),

                // 14 - UTILES P/HOGAR FERRETERÍA DOMESTICA
                new(lineas[13].Id, "es", "UTILES P/HOGAR FERRETERÍA DOMESTICA", "system"),
                new(lineas[13].Id, "en", "HOUSEHOLD ITEMS AND HARDWARE", "system"),
                new(lineas[13].Id, "fr", "ARTICLES MÉNAGERS ET QUINCAILLERIE", "system"),

                // 15 - ELEMENTOS Y UTÍLES DE FERRTERÍA GRUESA
                new(lineas[14].Id, "es", "ELEMENTOS Y UTÍLES DE FERRTERÍA GRUESA", "system"),
                new(lineas[14].Id, "en", "HEAVY HARDWARE TOOLS AND SUPPLIES", "system"),
                new(lineas[14].Id, "fr", "ÉLÉMENTS ET OUTILS DE QUINCAILLERIE LOURDE", "system"),

                // 16 - AJUARES DE CASA
                new(lineas[15].Id, "es", "AJUARES DE CASA", "system"),
                new(lineas[15].Id, "en", "HOUSEHOLD LINENS", "system"),
                new(lineas[15].Id, "fr", "TROUSSEAUX DE MAISON", "system"),

                // 17 - MUEBLES Y COLCHONES
                new(lineas[16].Id, "es", "MUEBLES Y COLCHONES", "system"),
                new(lineas[16].Id, "en", "FURNITURE AND MATTRESSES", "system"),
                new(lineas[16].Id, "fr", "MEUBLES ET MATELAS", "system"),

                // 18 - EFECTOS DE OFICINA, LIBRERÍA
                new(lineas[17].Id, "es", "EFECTOS DE OFICINA, LIBRERÍA", "system"),
                new(lineas[17].Id, "en", "OFFICE AND STATIONERY SUPPLIES", "system"),
                new(lineas[17].Id, "fr", "FOURNITURES DE BUREAU ET PAPETERIE", "system"),

                // 19 - JUGUETES Y ATÍCULOS PARA FIESTAS
                new(lineas[18].Id, "es", "JUGUETES Y ATÍCULOS PARA FIESTAS", "system"),
                new(lineas[18].Id, "en", "TOYS AND PARTY SUPPLIES", "system"),
                new(lineas[18].Id, "fr", "JOUETS ET ARTICLES DE FÊTE", "system"),

                // 20 - FOTOGRAFÍA
                new(lineas[19].Id, "es", "FOTOGRAFÍA", "system"),
                new(lineas[19].Id, "en", "PHOTOGRAPHY", "system"),
                new(lineas[19].Id, "fr", "PHOTOGRAPHIE", "system"),

                // 21 - NUMISMATICA, FILATELIA
                new(lineas[20].Id, "es", "NUMISMATICA, FILATELIA", "system"),
                new(lineas[20].Id, "en", "NUMISMATICS, PHILATELY", "system"),
                new(lineas[20].Id, "fr", "NUMISMATIQUE, PHILATÉLIE", "system"),

                // 22 - ARTÍCULOS DEPORTIVOS Y RECREATIVOS
                new(lineas[21].Id, "es", "ARTÍCULOS DEPORTIVOS Y RECREATIVOS", "system"),
                new(lineas[21].Id, "en", "SPORTS AND RECREATIONAL ITEMS", "system"),
                new(lineas[21].Id, "fr", "ARTICLES SPORTIFS ET RÉCRÉATIFS", "system"),

                // 23 - ALIMENTOS, GOLOSINAS Y POSTRES
                new(lineas[22].Id, "es", "ALIMENTOS, GOLOSINAS Y POSTRES", "system"),
                new(lineas[22].Id, "en", "FOOD, SWEETS AND DESSERTS", "system"),
                new(lineas[22].Id, "fr", "ALIMENTS, BONBONS ET DESSERTS", "system"),

                // 24 - BEBIDAS, LICORES Y CREVEZAS
                new(lineas[23].Id, "es", "BEBIDAS, LICORES Y CREVEZAS", "system"),
                new(lineas[23].Id, "en", "BEVERAGES, LIQUORS AND BEERS", "system"),
                new(lineas[23].Id, "fr", "BOISSONS, LIQUEURS ET BIÈRES", "system"),

                // 25 - CIGARROS Y TABACOS
                new(lineas[24].Id, "es", "CIGARROS Y TABACOS", "system"),
                new(lineas[24].Id, "en", "CIGARS AND TOBACCO", "system"),
                new(lineas[24].Id, "fr", "CIGARES ET TABAC", "system"),

                // 26 - ARTÍCULOS DESECHABLES E INSUMOS
                new(lineas[25].Id, "es", "ARTÍCULOS DESECHABLES E INSUMOS", "system"),
                new(lineas[25].Id, "en", "DISPOSABLE ITEMS AND SUPPLIES", "system"),
                new(lineas[25].Id, "fr", "ARTICLES JETABLES ET FOURNITURES", "system"),

                // 27 - AUTOMOTRIZ Y COMBUSTIBLE
                new(lineas[26].Id, "es", "AUTOMOTRIZ Y COMBUSTIBLE", "system"),
                new(lineas[26].Id, "en", "AUTOMOTIVE AND FUEL", "system"),
                new(lineas[26].Id, "fr", "AUTOMOBILE ET CARBURANT", "system"),

                // 28 - GASTRONOMÍA
                new(lineas[27].Id, "es", "GASTRONOMÍA", "system"),
                new(lineas[27].Id, "en", "GASTRONOMY", "system"),
                new(lineas[27].Id, "fr", "GASTRONOMIE", "system"),

                // 29 - SERVICIOS GENERALES MINORISTAS
                new(lineas[28].Id, "es", "SERVICIOS GENERALES MINORISTAS", "system"),
                new(lineas[28].Id, "en", "GENERAL RETAIL SERVICES", "system"),
                new(lineas[28].Id, "fr", "SERVICES GÉNÉRAUX DE DÉTAIL", "system")
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

            try
            {
                // Obtener las Líneas
                var lineas = context.Linea.ToList();
                var lineasDict = lineas.ToDictionary(l => l.Codigo, l => l);

                // Leer datos del CSV embebido
                var assembly = typeof(DbInitializer).Assembly;
                var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.SubLineas.csv";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    logger.LogWarning("No se encontró el archivo de datos SubLineas.csv. Recurso: {ResourceName}", resourceName);
                    logger.LogWarning("Recursos disponibles: {Resources}",
                        string.Join(", ", assembly.GetManifestResourceNames()));
                    return;
                }

                // ✅ UTF-8 explícito para manejar acentos correctamente
                using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);

                // Leer y validar encabezado
                var header = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(header))
                {
                    logger.LogWarning("Archivo CSV vacío o sin encabezado");
                    return;
                }

                logger.LogInformation("Encabezado CSV: {Header}", header);

                // Listas temporales para almacenar datos y traducciones
                var sublineasData = new List<(string Codigo, string Descripcion, int LineaId, string DescEN, string DescFR)>();

                string? line;
                int lineNumber = 1; // Ya leímos header

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var values = line.Split(',');

                    // ✅ Validar longitud mínima
                    if (values.Length < 3)
                    {
                        logger.LogWarning("Línea {LineNumber} inválida (tiene {Count} columnas, esperadas 5): {Line}",
                            lineNumber, values.Length, line);
                        continue;
                    }

                    // ✅ IMPORTANTE: NO hacer Trim() en codigo para preservar "01100"
                    var codigo = values[0];

                    // ✅ Validar que el código no esté vacío
                    if (string.IsNullOrWhiteSpace(codigo))
                    {
                        logger.LogWarning("Línea {LineNumber}: código vacío", lineNumber);
                        continue;
                    }

                    // ✅ Acceso seguro a índices con validación
                    var descripcion = values.Length > 1 ? values[1].Trim() : string.Empty;
                    var lineaCodigo = Convert.ToInt32(values[2].Trim());
                    var descripcionEN = values.Length > 3 ? values[3].Trim() : string.Empty;
                    var descripcionFR = values.Length > 4 ? values[4].Trim() : string.Empty;

                    // ✅ Validar que la descripción no esté vacía
                    if (string.IsNullOrWhiteSpace(descripcion))
                    {
                        logger.LogWarning("Línea {LineNumber}: descripción vacía para código {Codigo}", lineNumber, codigo);
                        continue;
                    }

                    // ✅ Log de debugging (solo primeras 10 líneas)
                    if (lineNumber <= 11)
                    {
                        logger.LogInformation("Línea {LineNumber}: Codigo='{Codigo}' (Length={Length}), Descripcion='{Descripcion}', LineaCodigo='{LineaCodigo}'",
                            lineNumber, codigo, codigo.Length, descripcion, lineaCodigo);
                    }

                    sublineasData.Add((codigo, descripcion, lineaCodigo, descripcionEN, descripcionFR));
                }

                logger.LogInformation("Datos del CSV leídos: {Count} sublíneas válidas de {TotalLines} líneas procesadas",
                    sublineasData.Count, lineNumber - 1);

                if (sublineasData.Count == 0)
                {
                    logger.LogWarning("No se cargaron datos del CSV. Verifica el formato del archivo.");
                    return;
                }

                // Segunda pasada: crear entidades SubLinea
                var sublineas = new List<SubLinea>();
                foreach (var data in sublineasData)
                {
                    try
                    {
                        var sublinea = new SubLinea(data.Codigo, data.Descripcion, data.LineaId, "system");
                        sublineas.Add(sublinea);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error al crear SubLinea: Codigo='{Codigo}' (Length={Length}), Descripcion='{Descripcion}'",
                            data.Codigo, data.Codigo.Length, data.Descripcion);
                    }
                }

                if (sublineas.Count == 0)
                {
                    logger.LogWarning("No se crearon entidades SubLinea válidas.");
                    return;
                }

                // Insertar SubLineas
                context.SubLinea.AddRange(sublineas);
                await context.SaveChangesAsync();

                logger.LogInformation("SubLineas insertadas: {Count}", sublineas.Count);

                // Tercera pasada: crear traducciones con los IDs generados
                var traducciones = new List<SubLineaTranslation>();
                for (int i = 0; i < sublineas.Count; i++)
                {
                    var sublinea = sublineas[i];
                    var data = sublineasData[i];

                    // Traducción en Español (ES)
                    traducciones.Add(new SubLineaTranslation(sublinea.Id, "es", data.Descripcion, "system"));

                    // Traducción en Inglés (EN)
                    if (!string.IsNullOrWhiteSpace(data.DescEN))
                    {
                        traducciones.Add(new SubLineaTranslation(sublinea.Id, "en", data.DescEN, "system"));
                    }

                    // Traducción en Francés (FR)
                    if (!string.IsNullOrWhiteSpace(data.DescFR))
                    {
                        traducciones.Add(new SubLineaTranslation(sublinea.Id, "fr", data.DescFR, "system"));
                    }
                }

                // Insertar traducciones
                context.SubLineaTranslation.AddRange(traducciones);
                await context.SaveChangesAsync();

                logger.LogInformation("Seed de SubLinea completado: {Count} sublíneas con {TransCount} traducciones",
                    sublineas.Count, traducciones.Count);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al cargar datos de SubLinea desde CSV");
                throw;
            }
        }

        #endregion

        #region Moneda

        private static async Task SeedMonedaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Moneda.Any())
            {
                logger.LogInformation("Moneda ya tiene datos, omitiendo seed.");
                return;
            }

            var moneda = new[]
            {
                new Moneda("CUP", "PESO CUBANO", "system"),
                new Moneda("MLC", "MONEDA LIBREMENTE CONVERTIBLE", "system"),
                new Moneda("USD", "DÓLAR ESTADOUNIDENSE", "system"),
                new Moneda("EUR", "EURO", "system"),
                new Moneda("JPY", "YEN JAPONÉS", "system"),
                new Moneda("GBP", "LIBRA ESTERLINA", "system"),
                new Moneda("AUD", "DÓLAR AUSTRALIANO", "system"),
                new Moneda("CAD", "DÓLAR CANADIENSE", "system"),
                new Moneda("CHF", "FRANCO SUIZO", "system"),
                new Moneda("CNY", "YUAN RENMINBI", "system"),
                new Moneda("HKD", "DÓLAR DE HONG KONG", "system"),
                new Moneda("NZD", "DÓLAR NEOZELANDÉS", "system")
            };

            context.Moneda.AddRange(moneda);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<MonedaTranslation>
            {
                new(moneda[0].Id, "es", "PESO CUBANO", "system"),
                new(moneda[0].Id, "en", "CUBAN PESO", "system"),
                new(moneda[0].Id, "fr", "PESO CUBAIN", "system"),

                new(moneda[1].Id, "es", "MONEDA LIBREMENTE CONVERTIBLE", "system"),
                new(moneda[1].Id, "en", "FREELY CONVERTIBLE CURRENCY", "system"),
                new(moneda[1].Id, "fr", "MONNAIE LIBREMENT CONVERTIBLE", "system"),

                new(moneda[2].Id, "es", "DÓLAR ESTADOUNIDENSE", "system"),
                new(moneda[2].Id, "en", "UNITED STATES DOLLAR", "system"),
                new(moneda[2].Id, "fr", "DOLLAR AMÉRICAIN", "system"),

                new(moneda[3].Id, "es", "EURO", "system"),
                new(moneda[3].Id, "en", "EURO", "system"),
                new(moneda[3].Id, "fr", "EURO", "system"),

                new(moneda[4].Id, "es", "YEN JAPONÉS", "system"),
                new(moneda[4].Id, "en", "JAPANESE YEN", "system"),
                new(moneda[4].Id, "fr", "YEN JAPONAIS", "system"),

                new(moneda[5].Id, "es", "LIBRA ESTERLINA", "system"),
                new(moneda[5].Id, "en", "POUND STERLING", "system"),
                new(moneda[5].Id, "fr", "LIVRE STERLING", "system"),

                new(moneda[6].Id, "es", "DÓLAR AUSTRALIANO", "system"),
                new(moneda[6].Id, "en", "AUSTRALIAN DOLLAR", "system"),
                new(moneda[6].Id, "fr", "DOLLAR AUSTRALIEN", "system"),

                new(moneda[7].Id, "es", "DÓLAR CANADIENSE", "system"),
                new(moneda[7].Id, "en", "CANADIAN DOLLAR", "system"),
                new(moneda[7].Id, "fr", "DOLLAR CANADIEN", "system"),

                new(moneda[8].Id, "es", "FRANCO SUIZO", "system"),
                new(moneda[8].Id, "en", "SWISS FRANC", "system"),
                new(moneda[8].Id, "fr", "FRANC SUISSE", "system"),

                new(moneda[9].Id, "es", "YUAN RENMINBI", "system"),
                new(moneda[9].Id, "en", "RENMINBI YUAN", "system"),
                new(moneda[9].Id, "fr", "YUAN RENMINBI", "system"),

                new(moneda[10].Id, "es", "DÓLAR DE HONG KONG", "system"),
                new(moneda[10].Id, "en", "HONG KONG DOLLAR", "system"),
                new(moneda[10].Id, "fr", "DOLLAR DE HONG KONG", "system"),

                new(moneda[11].Id, "es", "DÓLAR NEOZELANDÉS", "system"),
                new(moneda[11].Id, "en", "NEW ZEALAND DOLLAR", "system"),
                new(moneda[11].Id, "fr", "DOLLAR NÉO-ZÉLANDAIS", "system"),
            };

            context.MonedaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Moneda completado: {Count} monedas agregadas", moneda.Length);
        }

        #endregion

        #region ConceptoAjuste

        private static async Task SeedConceptoAjusteAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.ConceptoAjuste.Any())
            {
                logger.LogInformation("ConceptoAjuste ya tiene datos, omitiendo seed.");
                return;
            }

            var conceptoAjuste = new[]
            {
                new ConceptoAjuste("10", "CAMBIO DE PRECIO POSITIVO", 25, "system"),
                new ConceptoAjuste("11", "SOBRANTES", 21, "system"),
                new ConceptoAjuste("12", "MERCANCIA RECUPERADA", 25, "system"),
                new ConceptoAjuste("13", "ERROR EN VENTA (DE ENTRADA)", 25, "system"),
                new ConceptoAjuste("14", "MERMA ROTURA Y DETERIORO", 24, "system"),
                new ConceptoAjuste("17", "AUMENTO DE PRECIO PROMEDIO PONDERADO", 25, "system"),
                new ConceptoAjuste("18", "AJUSTE POSITIVO DE CONVERSIÓN", 25, "system"),
                new ConceptoAjuste("19", "OTRAS ENTRADAS", 25, "system"),

                new ConceptoAjuste("20", "CAMBIO DE PRECIO NEGATIVO", 25, "system"),
                new ConceptoAjuste("21", "FALTANTES", 21, "system"),
                new ConceptoAjuste("22", "MERCANCIA A RECUPERAR", 25, "system"),
                new ConceptoAjuste("23", "ERROR EN VENTA (DE SALIDA)", 25, "system"),
                new ConceptoAjuste("24", "MERMA ROTURA Y DETERIORO", 25, "system"),
                new ConceptoAjuste("27", "DISMINUCIÓN DE PRECIO PROMEDIO PONDERADO", 25, "system"),
                new ConceptoAjuste("28", "AJUSTE NEGATIVO DE CONVERSIÓN", 24, "system"),
                new ConceptoAjuste("29", "OTRAS SALIDAS", 25, "system"),
            };

            context.ConceptoAjuste.AddRange(conceptoAjuste);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<ConceptoAjusteTranslation>
            {
                new(conceptoAjuste[0].Id, "es", "CAMBIO DE PRECIO POSITIVO", "system"),
                new(conceptoAjuste[0].Id, "en", "POSITIVE PRICE CHANGE", "system"),
                new(conceptoAjuste[0].Id, "fr", "CHANGEMENT DE PRIX POSITIF", "system"),

                new(conceptoAjuste[1].Id, "es", "SOBRANTES", "system"),
                new(conceptoAjuste[1].Id, "en", "SURPLUSES", "system"),
                new(conceptoAjuste[1].Id, "fr", "SURPLUS", "system"),

                new(conceptoAjuste[2].Id, "es", "MERCANCÍA RECUPERADA", "system"),
                new(conceptoAjuste[2].Id, "en", "RECOVERED GOODS", "system"),
                new(conceptoAjuste[2].Id, "fr", "MARCHANDISE RÉCUPÉRÉE", "system"),

                new(conceptoAjuste[3].Id, "es", "ERROR EN VENTA (DE ENTRADA)", "system"),
                new(conceptoAjuste[3].Id, "en", "SALES ERROR (INBOUND)", "system"),
                new(conceptoAjuste[3].Id, "fr", "ERREUR DE VENTE (ENTRÉE)", "system"),

                new(conceptoAjuste[4].Id, "es", "MERMA ROTURA Y DETERIORO", "system"),
                new(conceptoAjuste[4].Id, "en", "BREAKAGE AND DETERIORATION LOSS", "system"),
                new(conceptoAjuste[4].Id, "fr", "PERTE PAR CASSURE ET DÉTÉRIORATION", "system"),

                new(conceptoAjuste[5].Id, "es", "AUMENTO DE PRECIO PROMEDIO PONDERADO", "system"),
                new(conceptoAjuste[5].Id, "en", "WEIGHTED AVERAGE PRICE INCREASE", "system"),
                new(conceptoAjuste[5].Id, "fr", "AUGMENTATION DU PRIX MOYEN PONDÉRÉ", "system"),

                new(conceptoAjuste[6].Id, "es", "AJUSTE POSITIVO DE CONVERSIÓN", "system"),
                new(conceptoAjuste[6].Id, "en", "POSITIVE CONVERSION ADJUSTMENT", "system"),
                new(conceptoAjuste[6].Id, "fr", "AJUSTEMENT POSITIF DE CONVERSION", "system"),

                new(conceptoAjuste[7].Id, "es", "OTRAS ENTRADAS", "system"),
                new(conceptoAjuste[7].Id, "en", "OTHER ENTRIES", "system"),
                new(conceptoAjuste[7].Id, "fr", "AUTRES ENTRÉES", "system"),

                new(conceptoAjuste[8].Id, "es", "CAMBIO DE PRECIO NEGATIVO", "system"),
                new(conceptoAjuste[8].Id, "en", "NEGATIVE PRICE CHANGE", "system"),
                new(conceptoAjuste[8].Id, "fr", "CHANGEMENT DE PRIX NÉGATIF", "system"),

                new(conceptoAjuste[9].Id, "es", "FALTANTES", "system"),
                new(conceptoAjuste[9].Id, "en", "SHORTAGES", "system"),
                new(conceptoAjuste[9].Id, "fr", "MANQUANTS", "system"),

                new(conceptoAjuste[10].Id, "es", "MERCANCÍA A RECUPERAR", "system"),
                new(conceptoAjuste[10].Id, "en", "GOODS TO BE RECOVERED", "system"),
                new(conceptoAjuste[10].Id, "fr", "MARCHANDISE À RÉCUPÉRER", "system"),

                new(conceptoAjuste[11].Id, "es", "ERROR EN VENTA (DE SALIDA)", "system"),
                new(conceptoAjuste[11].Id, "en", "SALES ERROR (OUTBOUND)", "system"),
                new(conceptoAjuste[11].Id, "fr", "ERREUR DE VENTE (SORTIE)", "system"),

                new(conceptoAjuste[12].Id, "es", "MERMA ROTURA Y DETERIORO", "system"),
                new(conceptoAjuste[12].Id, "en", "BREAKAGE AND DETERIORATION LOSS", "system"),
                new(conceptoAjuste[12].Id, "fr", "PERTE PAR CASSURE ET DÉTÉRIORATION", "system"),

                new(conceptoAjuste[13].Id, "es", "DISMINUCIÓN DE PRECIO PROMEDIO PONDERADO", "system"),
                new(conceptoAjuste[13].Id, "en", "WEIGHTED AVERAGE PRICE DECREASE", "system"),
                new(conceptoAjuste[13].Id, "fr", "DIMINUTION DU PRIX MOYEN PONDÉRÉ", "system"),

                new(conceptoAjuste[14].Id, "es", "AJUSTE NEGATIVO DE CONVERSIÓN", "system"),
                new(conceptoAjuste[14].Id, "en", "NEGATIVE CONVERSION ADJUSTMENT", "system"),
                new(conceptoAjuste[14].Id, "fr", "AJUSTEMENT NÉGATIF DE CONVERSION", "system"),

                new(conceptoAjuste[15].Id, "es", "OTRAS SALIDAS", "system"),
                new(conceptoAjuste[15].Id, "en", "OTHER OUTPUTS", "system"),
                new(conceptoAjuste[15].Id, "fr", "AUTRES SORTIES", "system"),

            };

            context.ConceptoAjusteTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Concepto Ajuste completado: {Count} coceptoAjuste agregados", conceptoAjuste.Length);
        }

        #endregion

        #region Unidad Medida

        private static async Task SeedUnidadMedidaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.UnidadMedida.Any())
            {
                logger.LogInformation("UnidadMedida ya tiene datos, omitiendo seed.");
                return;
            }

            var unidadMedida = new[]
            {
                new UnidadMedida("UNO", "UNIDAD", "system"),
                new UnidadMedida("PKG", "PAQUETE", "system"),
                new UnidadMedida("SET", "JUEGO", "system"),
                new UnidadMedida("BAG", "BOLSA", "system"),
                new UnidadMedida("BOX", "CAJA", "system"),
                new UnidadMedida("ROL", "ROLLO", "system"),
                new UnidadMedida("PLT", "TARIMA", "system"),
                new UnidadMedida("BND", "BANDA", "system"),

                new UnidadMedida("MTR", "METRO", "system"),
                new UnidadMedida("KMT", "KILÓMETRO", "system"),
                new UnidadMedida("CMT", "CENTÍMETRO", "system"),
                new UnidadMedida("MMT", "MILÍMETRO", "system"),
                new UnidadMedida("INH", "PULGADA", "system"),
                new UnidadMedida("FOT", "PIE", "system"),
                new UnidadMedida("YRD", "YARDA", "system"),

                new UnidadMedida("SQM", "METRO CUADRADO", "system"),
                new UnidadMedida("SFT", "PIE CUADRADO", "system"),
                new UnidadMedida("SYD", "YARDA CUADRADA", "system"),
                new UnidadMedida("HEC", "HECTÁREA", "system"),

                new UnidadMedida("LTR", "LITRO", "system"),
                new UnidadMedida("MLT", "MILILITRO", "system"),
                new UnidadMedida("GLN", "GALÓN", "system"),
                new UnidadMedida("PTA", "PINTA", "system"),
                new UnidadMedida("QRT", "CUARTO (QUART)", "system"),

                new UnidadMedida("KGM", "KILOGRAMO", "system"),
                new UnidadMedida("GRM", "GRAMO", "system"),
                new UnidadMedida("LBR", "LIBRA", "system"),
                new UnidadMedida("ONZ", "ONZA", "system"),
                new UnidadMedida("TON", "TONELADA", "system"),

                new UnidadMedida("HRS", "HORAS", "system"),
                new UnidadMedida("MIN", "MINUTOS", "system"),
                new UnidadMedida("SEC", "SEGUNDOS", "system"),
                new UnidadMedida("DAY", "DÍA", "system"),
                new UnidadMedida("WKS", "SEMANAS", "system"),
                new UnidadMedida("MTH", "MES", "system"),

                new UnidadMedida("KWH", "KILOVATIO-HORA", "system"),
                new UnidadMedida("KVA", "KILOVOLT-AMPERIO", "system"),
                new UnidadMedida("MAH", "MILIAMPERIO-HORA", "system"),
                new UnidadMedida("BTU", "UNIDAD TÉRMICA BRITÁNICA", "system"),

                new UnidadMedida("PSI", "LIBRA POR PULGADA CUADRADA", "system"),
                new UnidadMedida("BAR", "BAR", "system"),
            };

            context.UnidadMedida.AddRange(unidadMedida);
            await context.SaveChangesAsync();

            // ✅ TRADUCCIONES CORREGIDAS
            var traducciones = new List<UnidadMedidaTranslation>
            {
                // UNO - UNIDAD
                new(unidadMedida[0].Id, "es", "UNIDAD", "system"),
                new(unidadMedida[0].Id, "en", "UNIT", "system"),
                new(unidadMedida[0].Id, "fr", "UNITÉ", "system"),

                // PKG - PAQUETE
                new(unidadMedida[1].Id, "es", "PAQUETE", "system"),
                new(unidadMedida[1].Id, "en", "PACKAGE", "system"),
                new(unidadMedida[1].Id, "fr", "PAQUET", "system"),

                // SET - JUEGO
                new(unidadMedida[2].Id, "es", "JUEGO", "system"),
                new(unidadMedida[2].Id, "en", "SET", "system"),
                new(unidadMedida[2].Id, "fr", "ENSEMBLE", "system"),

                // BAG - BOLSA
                new(unidadMedida[3].Id, "es", "BOLSA", "system"),
                new(unidadMedida[3].Id, "en", "BAG", "system"),
                new(unidadMedida[3].Id, "fr", "SAC", "system"),

                // BOX - CAJA
                new(unidadMedida[4].Id, "es", "CAJA", "system"),
                new(unidadMedida[4].Id, "en", "BOX", "system"),
                new(unidadMedida[4].Id, "fr", "BOÎTE", "system"),

                // ROL - ROLLO
                new(unidadMedida[5].Id, "es", "ROLLO", "system"),
                new(unidadMedida[5].Id, "en", "ROLL", "system"),
                new(unidadMedida[5].Id, "fr", "ROULEAU", "system"),

                // PLT - TARIMA
                new(unidadMedida[6].Id, "es", "TARIMA", "system"),
                new(unidadMedida[6].Id, "en", "PALLET", "system"),
                new(unidadMedida[6].Id, "fr", "PALETTE", "system"),

                // BND - BANDA
                new(unidadMedida[7].Id, "es", "BANDA", "system"),
                new(unidadMedida[7].Id, "en", "BAND", "system"),
                new(unidadMedida[7].Id, "fr", "BANDE", "system"),

                // MTR - METRO
                new(unidadMedida[8].Id, "es", "METRO", "system"),
                new(unidadMedida[8].Id, "en", "METER", "system"),
                new(unidadMedida[8].Id, "fr", "MÈTRE", "system"),

                // KMT - KILÓMETRO
                new(unidadMedida[9].Id, "es", "KILÓMETRO", "system"),
                new(unidadMedida[9].Id, "en", "KILOMETER", "system"),
                new(unidadMedida[9].Id, "fr", "KILOMÈTRE", "system"),

                // CMT - CENTÍMETRO
                new(unidadMedida[10].Id, "es", "CENTÍMETRO", "system"),
                new(unidadMedida[10].Id, "en", "CENTIMETER", "system"),
                new(unidadMedida[10].Id, "fr", "CENTIMÈTRE", "system"),

                // MMT - MILÍMETRO
                new(unidadMedida[11].Id, "es", "MILÍMETRO", "system"),
                new(unidadMedida[11].Id, "en", "MILLIMETER", "system"),
                new(unidadMedida[11].Id, "fr", "MILLIMÈTRE", "system"),

                // INH - PULGADA
                new(unidadMedida[12].Id, "es", "PULGADA", "system"),
                new(unidadMedida[12].Id, "en", "INCH", "system"),
                new(unidadMedida[12].Id, "fr", "POUCE", "system"),

                // FOT - PIE
                new(unidadMedida[13].Id, "es", "PIE", "system"),
                new(unidadMedida[13].Id, "en", "FOOT", "system"),
                new(unidadMedida[13].Id, "fr", "PIED", "system"),

                // YRD - YARDA
                new(unidadMedida[14].Id, "es", "YARDA", "system"),
                new(unidadMedida[14].Id, "en", "YARD", "system"),
                new(unidadMedida[14].Id, "fr", "YARD", "system"),

                // SQM - METRO CUADRADO
                new(unidadMedida[15].Id, "es", "METRO CUADRADO", "system"),
                new(unidadMedida[15].Id, "en", "SQUARE METER", "system"),
                new(unidadMedida[15].Id, "fr", "MÈTRE CARRÉ", "system"),

                // SFT - PIE CUADRADO
                new(unidadMedida[16].Id, "es", "PIE CUADRADO", "system"),
                new(unidadMedida[16].Id, "en", "SQUARE FOOT", "system"),
                new(unidadMedida[16].Id, "fr", "PIED CARRÉ", "system"),

                // SYD - YARDA CUADRADA
                new(unidadMedida[17].Id, "es", "YARDA CUADRADA", "system"),
                new(unidadMedida[17].Id, "en", "SQUARE YARD", "system"),
                new(unidadMedida[17].Id, "fr", "YARD CARRÉ", "system"),

                // HEC - HECTÁREA
                new(unidadMedida[18].Id, "es", "HECTÁREA", "system"),
                new(unidadMedida[18].Id, "en", "HECTARE", "system"),
                new(unidadMedida[18].Id, "fr", "HECTARE", "system"),

                // LTR - LITRO
                new(unidadMedida[19].Id, "es", "LITRO", "system"),
                new(unidadMedida[19].Id, "en", "LITER", "system"),
                new(unidadMedida[19].Id, "fr", "LITRE", "system"),

                // MLT - MILILITRO
                new(unidadMedida[20].Id, "es", "MILILITRO", "system"),
                new(unidadMedida[20].Id, "en", "MILLILITER", "system"),
                new(unidadMedida[20].Id, "fr", "MILLILITRE", "system"),

                // GLN - GALÓN
                new(unidadMedida[21].Id, "es", "GALÓN", "system"),
                new(unidadMedida[21].Id, "en", "GALLON", "system"),
                new(unidadMedida[21].Id, "fr", "GALLON", "system"),

                // PTA - PINTA
                new(unidadMedida[22].Id, "es", "PINTA", "system"),
                new(unidadMedida[22].Id, "en", "PINT", "system"),
                new(unidadMedida[22].Id, "fr", "PINTE", "system"),

                // QRT - CUARTO (QUART)
                new(unidadMedida[23].Id, "es", "CUARTO (QUART)", "system"),
                new(unidadMedida[23].Id, "en", "QUART", "system"),
                new(unidadMedida[23].Id, "fr", "QUART", "system"),

                // KGM - KILOGRAMO
                new(unidadMedida[24].Id, "es", "KILOGRAMO", "system"),
                new(unidadMedida[24].Id, "en", "KILOGRAM", "system"),
                new(unidadMedida[24].Id, "fr", "KILOGRAMME", "system"),

                // GRM - GRAMO
                new(unidadMedida[25].Id, "es", "GRAMO", "system"),
                new(unidadMedida[25].Id, "en", "GRAM", "system"),
                new(unidadMedida[25].Id, "fr", "GRAMME", "system"),

                // LBR - LIBRA
                new(unidadMedida[26].Id, "es", "LIBRA", "system"),
                new(unidadMedida[26].Id, "en", "POUND", "system"),
                new(unidadMedida[26].Id, "fr", "LIVRE", "system"),

                // ONZ - ONZA
                new(unidadMedida[27].Id, "es", "ONZA", "system"),
                new(unidadMedida[27].Id, "en", "OUNCE", "system"),
                new(unidadMedida[27].Id, "fr", "ONCE", "system"),

                // TON - TONELADA
                new(unidadMedida[28].Id, "es", "TONELADA", "system"),
                new(unidadMedida[28].Id, "en", "TON", "system"),
                new(unidadMedida[28].Id, "fr", "TONNE", "system"),

                // HRS - HORAS
                new(unidadMedida[29].Id, "es", "HORAS", "system"),
                new(unidadMedida[29].Id, "en", "HOURS", "system"),
                new(unidadMedida[29].Id, "fr", "HEURES", "system"),

                // MIN - MINUTOS
                new(unidadMedida[30].Id, "es", "MINUTOS", "system"),
                new(unidadMedida[30].Id, "en", "MINUTES", "system"),
                new(unidadMedida[30].Id, "fr", "MINUTES", "system"),

                // SEC - SEGUNDOS
                new(unidadMedida[31].Id, "es", "SEGUNDOS", "system"),
                new(unidadMedida[31].Id, "en", "SECONDS", "system"),
                new(unidadMedida[31].Id, "fr", "SECONDES", "system"),

                // DAY - DÍA
                new(unidadMedida[32].Id, "es", "DÍA", "system"),
                new(unidadMedida[32].Id, "en", "DAY", "system"),
                new(unidadMedida[32].Id, "fr", "JOUR", "system"),

                // WKS - SEMANAS
                new(unidadMedida[33].Id, "es", "SEMANAS", "system"),
                new(unidadMedida[33].Id, "en", "WEEKS", "system"),
                new(unidadMedida[33].Id, "fr", "SEMAINES", "system"),

                // MTH - MES
                new(unidadMedida[34].Id, "es", "MES", "system"),
                new(unidadMedida[34].Id, "en", "MONTH", "system"),
                new(unidadMedida[34].Id, "fr", "MOIS", "system"),

                // KWH - KILOVATIO-HORA
                new(unidadMedida[35].Id, "es", "KILOVATIO-HORA", "system"),
                new(unidadMedida[35].Id, "en", "KILOWATT-HOUR", "system"),
                new(unidadMedida[35].Id, "fr", "KILOWATT-HEURE", "system"),

                // KVA - KILOVOLT-AMPERIO
                new(unidadMedida[36].Id, "es", "KILOVOLT-AMPERIO", "system"),
                new(unidadMedida[36].Id, "en", "KILOVOLT-AMPERE", "system"),
                new(unidadMedida[36].Id, "fr", "KILOVOLT-AMPÈRE", "system"),

                // MAH - MILIAMPERIO-HORA
                new(unidadMedida[37].Id, "es", "MILIAMPERIO-HORA", "system"),
                new(unidadMedida[37].Id, "en", "MILLIAMPERE-HOUR", "system"),
                new(unidadMedida[37].Id, "fr", "MILLIAMPÈRE-HEURE", "system"),

                // BTU - UNIDAD TÉRMICA BRITÁNICA
                new(unidadMedida[38].Id, "es", "UNIDAD TÉRMICA BRITÁNICA", "system"),
                new(unidadMedida[38].Id, "en", "BRITISH THERMAL UNIT", "system"),
                new(unidadMedida[38].Id, "fr", "UNITÉ THERMIQUE BRITANNIQUE", "system"),

                // PSI - LIBRA POR PULGADA CUADRADA
                new(unidadMedida[39].Id, "es", "LIBRA POR PULGADA CUADRADA", "system"),
                new(unidadMedida[39].Id, "en", "POUND PER SQUARE INCH", "system"),
                new(unidadMedida[39].Id, "fr", "LIVRE PAR POUCE CARRÉ", "system"),

                // BAR - BAR
                new(unidadMedida[40].Id, "es", "BAR", "system"),
                new(unidadMedida[40].Id, "en", "BAR", "system"),
                new(unidadMedida[40].Id, "fr", "BAR", "system")
            };

            context.UnidadMedidaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de UnidadMedida completado: {Count} unidades con {TransCount} traducciones agregadas",
                unidadMedida.Length, traducciones.Count);
        }

        #endregion

        #region Transaccion

        private static async Task SeedTransaccionAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Transaccion.Any())
            {
                logger.LogInformation("Transacción ya tiene datos, omitiendo seed.");
                return;
            }

            var transaccion = new[]
            {
                new Transaccion("11", "RECEPCIÓN", "system"),
                new Transaccion("13", "NOTA DE CRÉDITO EN VENTA", "system"),
                new Transaccion("14", "AJUSTE POSITIVO", "system"),
                new Transaccion("15", "DEVOLUCIÓN DE VENTA", "system"),
                new Transaccion("16", "DEVOLUCIÓN EN VALE DE ENTREGA", "system"),
                new Transaccion("17", "NOTA DE CRÉDITO EN COMPRA", "system"),
                new Transaccion("18", "TRANSFERENCIA INTERNA DE ENTRADA", "system"),
                
                new Transaccion("21", "FACTURA", "system"),
                new Transaccion("23", "NOTA DE DÉBITO EN VENTA", "system"),
                new Transaccion("24", "AJUSTE NEGATIVO", "system"),
                new Transaccion("25", "DEVOLUCIÓN DE COMPRA", "system"),
                new Transaccion("26", "VALE DE ENTREGA", "system"),
                new Transaccion("27", "NOTA DE DÉBITO EN COMPRA", "system"),
                new Transaccion("28", "TRANSFERENCIA INTERNA DE SALIDA", "system"),
                new Transaccion("35", "ORDEN DE SERVICIO", "system"),
            };

            context.Transaccion.AddRange(transaccion);
            await context.SaveChangesAsync();

            // Agregar traducciones
            var traducciones = new List<TransaccionTranslation>
            {
                // 11 - RECEPCIÓN
                new(transaccion[0].Id, "es", "RECEPCIÓN", "system"),
                new(transaccion[0].Id, "en", "RECEIPT", "system"),
                new(transaccion[0].Id, "fr", "RÉCEPTION", "system"),

                // 13 - NOTA DE CRÉDITO EN VENTA
                new(transaccion[1].Id, "es", "NOTA DE CRÉDITO EN VENTA", "system"),
                new(transaccion[1].Id, "en", "SALES CREDIT NOTE", "system"),
                new(transaccion[1].Id, "fr", "NOTE DE CRÉDIT EN VENTE", "system"),

                // 14 - AJUSTE POSITIVO
                new(transaccion[2].Id, "es", "AJUSTE POSITIVO", "system"),
                new(transaccion[2].Id, "en", "POSITIVE ADJUSTMENT", "system"),
                new(transaccion[2].Id, "fr", "AJUSTEMENT POSITIF", "system"),

                // 15 - DEVOLUCIÓN DE VENTA
                new(transaccion[3].Id, "es", "DEVOLUCIÓN DE VENTA", "system"),
                new(transaccion[3].Id, "en", "SALES RETURN", "system"),
                new(transaccion[3].Id, "fr", "RETOUR DE VENTE", "system"),

                // 16 - DEVOLUCIÓN EN VALE DE ENTREGA
                new(transaccion[4].Id, "es", "DEVOLUCIÓN EN VALE DE ENTREGA", "system"),
                new(transaccion[4].Id, "en", "DELIVERY NOTE RETURN", "system"),
                new(transaccion[4].Id, "fr", "RETOUR DE BON DE LIVRAISON", "system"),

                // 17 - NOTA DE CRÉDITO EN COMPRA
                new(transaccion[5].Id, "es", "NOTA DE CRÉDITO EN COMPRA", "system"),
                new(transaccion[5].Id, "en", "PURCHASE CREDIT NOTE", "system"),
                new(transaccion[5].Id, "fr", "NOTE DE CRÉDIT EN ACHAT", "system"),

                // 18 - TRANSFERENCIA INTERNA DE ENTRADA
                new(transaccion[6].Id, "es", "TRANSFERENCIA INTERNA DE ENTRADA", "system"),
                new(transaccion[6].Id, "en", "INTERNAL TRANSFER INBOUND", "system"),
                new(transaccion[6].Id, "fr", "TRANSFERT INTERNE D'ENTRÉE", "system"),

                // 21 - FACTURA
                new(transaccion[7].Id, "es", "FACTURA", "system"),
                new(transaccion[7].Id, "en", "INVOICE", "system"),
                new(transaccion[7].Id, "fr", "FACTURE", "system"),

                // 23 - NOTA DE DÉBITO EN VENTA
                new(transaccion[8].Id, "es", "NOTA DE DÉBITO EN VENTA", "system"),
                new(transaccion[8].Id, "en", "SALES DEBIT NOTE", "system"),
                new(transaccion[8].Id, "fr", "NOTE DE DÉBIT EN VENTE", "system"),

                // 24 - AJUSTE NEGATIVO
                new(transaccion[9].Id, "es", "AJUSTE NEGATIVO", "system"),
                new(transaccion[9].Id, "en", "NEGATIVE ADJUSTMENT", "system"),
                new(transaccion[9].Id, "fr", "AJUSTEMENT NÉGATIF", "system"),

                // 25 - DEVOLUCIÓN DE COMPRA
                new(transaccion[10].Id, "es", "DEVOLUCIÓN DE COMPRA", "system"),
                new(transaccion[10].Id, "en", "PURCHASE RETURN", "system"),
                new(transaccion[10].Id, "fr", "RETOUR D'ACHAT", "system"),

                // 26 - VALE DE ENTREGA
                new(transaccion[11].Id, "es", "VALE DE ENTREGA", "system"),
                new(transaccion[11].Id, "en", "DELIVERY NOTE", "system"),
                new(transaccion[11].Id, "fr", "BON DE LIVRAISON", "system"),

                // 27 - NOTA DE DÉBITO EN COMPRA
                new(transaccion[12].Id, "es", "NOTA DE DÉBITO EN COMPRA", "system"),
                new(transaccion[12].Id, "en", "PURCHASE DEBIT NOTE", "system"),
                new(transaccion[12].Id, "fr", "NOTE DE DÉBIT EN ACHAT", "system"),

                // 28 - TRANSFERENCIA INTERNA DE SALIDA
                new(transaccion[13].Id, "es", "TRANSFERENCIA INTERNA DE SALIDA", "system"),
                new(transaccion[13].Id, "en", "INTERNAL TRANSFER OUTBOUND", "system"),
                new(transaccion[13].Id, "fr", "TRANSFERT INTERNE DE SORTIE", "system"),

                // 35 - ORDEN DE SERVICIO
                new(transaccion[14].Id, "es", "ORDEN DE SERVICIO", "system"),
                new(transaccion[14].Id, "en", "SERVICE ORDER", "system"),
                new(transaccion[14].Id, "fr", "ORDRE DE SERVICE", "system"),
            };

            context.TransaccionTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("Seed de Transaccion completado: {Count} transaccion agregadas", transaccion.Length);
        }

        #endregion

    }
}