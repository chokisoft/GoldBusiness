using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GoldBusiness.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("Iniciando seed de datos maestros...");

                // Commons
                await SeedPaisAsync(context, logger);
                await SeedProvinciaAsync(context, logger);
                await SeedMunicipioAsync(context, logger);
                await SeedCodigoPostalAsync(context, logger);

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
            catch (DbUpdateException dbEx)
            {
                // ✅ Mostrar la inner exception real (SQL Server error)
                logger.LogError(dbEx, "❌ DbUpdateException durante seed");
                logger.LogError("Inner Exception: {Inner}", dbEx.InnerException?.Message);
                logger.LogError("Stack: {Stack}", dbEx.InnerException?.StackTrace);

                // Si hay entries, mostrar qué entidad falló
                foreach (var entry in dbEx.Entries)
                {
                    logger.LogError("Entidad fallida: {Entity} - Estado: {State}",
                        entry.Entity.GetType().Name, entry.State);
                }

                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error durante el seed de datos maestros");
                throw;
            }
        }

        #region Pais

        private static async Task SeedPaisAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Pais.Any())
            {
                logger.LogInformation("Pais ya tiene datos, omitiendo seed.");
                return;
            }

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.Paises.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró Paises.csv");
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado

            // Formato: CodigoAlpha3,CodigoAlpha2,CodigoTelefono,DescripcionES,DescripcionEN,DescripcionFR,RegexTelefono,FormatoTelefono,FormatoEjemplo
            var paisesData = new List<(string Alpha3, string Alpha2, string Tel, string DescES, string DescEN, string DescFR, string Regex, string Fmt, string Ej)>();

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = ParseCsvLine(line);
                if (values.Length < 9) continue;

                paisesData.Add((
                    values[0].Trim(),
                    values[1].Trim(),
                    values[2].Trim(),
                    values[3].Trim(),
                    values[4].Trim(),
                    values[5].Trim(),
                    values[6].Trim(),
                    values[7].Trim(),
                    values[8].Trim()
                ));
            }

            // ═══════════════════════════════════════════════════════════════
            // CREAR ENTIDADES PAIS
            // ═══════════════════════════════════════════════════════════════
            var paises = new List<Pais>();

            foreach (var d in paisesData)
            {
                try
                {
                    var pais = new Pais(d.Alpha3, d.Alpha2, d.Tel, d.DescES, d.Regex, d.Fmt, d.Ej, "system");
                    paises.Add(pais);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Error al crear país {Alpha2}: {Message}", d.Alpha2, ex.Message);
                }
            }

            context.Pais.AddRange(paises);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ {Count} países insertados desde CSV", paises.Count);

            // ═══════════════════════════════════════════════════════════════
            // TRADUCCIONES
            // ═══════════════════════════════════════════════════════════════
            var paisesDict = paises.ToDictionary(p => p.CodigoAlpha2, p => p.Id);
            var traducciones = new List<PaisTranslation>();

            foreach (var d in paisesData)
            {
                if (!paisesDict.TryGetValue(d.Alpha2, out var paisId)) continue;

                traducciones.Add(new PaisTranslation(paisId, "es", d.DescES, "system"));
                traducciones.Add(new PaisTranslation(paisId, "en", d.DescEN, "system"));
                traducciones.Add(new PaisTranslation(paisId, "fr", d.DescFR, "system"));
            }

            context.PaisTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ {Count} traducciones de países insertadas", traducciones.Count);
        }

        #endregion

        #region Provincia

        private static async Task SeedProvinciaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Provincia.Any())
            {
                logger.LogInformation("Provincia ya tiene datos, omitiendo seed.");
                return;
            }

            var paises = context.Pais.ToDictionary(p => p.CodigoAlpha2, p => p.Id);

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.Provincias.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró Provincias.csv");
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado

            var datosParaTraducciones = new List<(string Codigo, string DescES, string DescEN, string DescFR, int PaisId)>();
            var provincias = new List<Provincia>();
            var seen = new HashSet<(int PaisId, string Codigo, bool Cancelado)>();

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = ParseCsvLine(line);
                if (values.Length < 5) continue;

                var codigo = values[0].Trim();
                var paisCodigo = values[1].Trim();
                var descES = Truncar(values[2].Trim(), 150);
                var descEN = Truncar(values[3].Trim(), 150);
                var descFR = Truncar(values[4].Trim(), 150);

                if (codigo.Length > 20)
                {
                    logger.LogWarning("Provincia código demasiado largo, truncando: '{Codigo}'", codigo);
                    codigo = codigo[..20];
                }

                if (!paises.TryGetValue(paisCodigo, out var paisId)) continue;

                var key = (paisId, codigo, false); // Cancelado = false por defecto
                if (seen.Contains(key))
                {
                    logger.LogWarning("Provincia duplicada detectada: PaisId={PaisId}, Codigo={Codigo}", paisId, codigo);
                    continue;
                }

                seen.Add(key);
                provincias.Add(new Provincia(codigo, descES, paisId, "system"));
                datosParaTraducciones.Add((codigo, descES, descEN, descFR, paisId));
            }

            foreach (var batch in provincias.Chunk(1000))
            {
                context.Provincia.AddRange(batch);
                await context.SaveChangesAsync();
            }

            logger.LogInformation("✅ {Count} provincias insertadas desde CSV", provincias.Count);

            // Traducciones (usando datos almacenados, sin releer CSV)
            var provinciasDict = provincias.ToDictionary(p => (p.PaisId, p.Codigo), p => p.Id);
            var traducciones = new List<ProvinciaTranslation>();

            foreach (var (codigo, descES, descEN, descFR, paisId) in datosParaTraducciones)
            {
                if (!provinciasDict.TryGetValue((paisId, codigo), out var provId)) continue;

                traducciones.Add(new ProvinciaTranslation(provId, "es", descES, "system"));
                traducciones.Add(new ProvinciaTranslation(provId, "en", descEN, "system"));
                traducciones.Add(new ProvinciaTranslation(provId, "fr", descFR, "system"));
            }

            foreach (var batch in traducciones.Chunk(3000))
            {
                context.ProvinciaTranslation.AddRange(batch);
                await context.SaveChangesAsync();
            }

            logger.LogInformation("✅ {Count} traducciones de provincias insertadas", traducciones.Count);
        }

        #endregion

        #region Municipio

        private static async Task SeedMunicipioAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Municipio.Any())
            {
                logger.LogInformation("Municipio ya tiene datos, omitiendo seed.");
                return;
            }

            var provincias = context.Provincia.ToDictionary(p => p.Codigo, p => p.Id);

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.Municipios.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró Municipios.csv");
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado

            var datosParaTraducciones = new List<(string Codigo, string DescES, string DescEN, string DescFR)>();
            var municipios = new List<Municipio>();

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = ParseCsvLine(line);
                if (values.Length < 6) continue;

                var codigo = values[0].Trim();
                var provCodigo = values[1].Trim();
                var descES = Truncar(values[3].Trim(), 150);
                var descEN = Truncar(values[4].Trim(), 150);
                var descFR = Truncar(values[5].Trim(), 150);

                if (codigo.Length > 25)
                {
                    logger.LogWarning("Municipio código demasiado largo, truncando: '{Codigo}'", codigo);
                    codigo = codigo[..25];
                }

                if (!provincias.TryGetValue(provCodigo, out var provId)) continue;

                municipios.Add(new Municipio(codigo, descES, provId, "system"));
                datosParaTraducciones.Add((codigo, descES, descEN, descFR));
            }

            // ✅ Insertar en lotes de 1000 (municipios pueden ser 50,000+)
            var totalInserted = 0;
            foreach (var batch in municipios.Chunk(1000))
            {
                context.Municipio.AddRange(batch);
                await context.SaveChangesAsync();
                totalInserted += batch.Length;

                if (totalInserted % 5000 == 0)
                    logger.LogInformation("  Municipios insertados: {Count}/{Total}", totalInserted, municipios.Count);
            }

            logger.LogInformation("✅ {Count} municipios insertados desde CSV", municipios.Count);

            // ✅ Traducciones en lotes
            var municipiosDict = municipios.ToDictionary(m => m.Codigo, m => m.Id);
            var traducciones = new List<MunicipioTranslation>();

            foreach (var (codigo, descES, descEN, descFR) in datosParaTraducciones)
            {
                if (!municipiosDict.TryGetValue(codigo, out var munId)) continue;

                traducciones.Add(new MunicipioTranslation(munId, "es", descES, "system"));
                traducciones.Add(new MunicipioTranslation(munId, "en", descEN, "system"));
                traducciones.Add(new MunicipioTranslation(munId, "fr", descFR, "system"));
            }

            var totalTradInserted = 0;
            foreach (var batch in traducciones.Chunk(3000))
            {
                context.MunicipioTranslation.AddRange(batch);
                await context.SaveChangesAsync();
                totalTradInserted += batch.Length;

                if (totalTradInserted % 15000 == 0)
                    logger.LogInformation("  Traducciones municipio: {Count}/{Total}", totalTradInserted, traducciones.Count);
            }

            logger.LogInformation("✅ {Count} traducciones de municipios insertadas", traducciones.Count);
        }

        #endregion

        #region Codigo Postal

        private static async Task SeedCodigoPostalAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.CodigoPostal.Any())
            {
                logger.LogInformation("CodigoPostal ya tiene datos, omitiendo seed.");
                return;
            }

            var municipios = context.Municipio.ToDictionary(m => m.Codigo, m => m.Id);
            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.CodigosPostales.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró CodigosPostales.csv");
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            var header = await reader.ReadLineAsync(); // Saltar encabezado
            logger.LogInformation("Encabezado CSV: {Header}", header);

            var codigos = new List<CodigoPostal>();
            var seen = new HashSet<(int MunicipioId, string Codigo, bool Cancelado)>();

            string? line;
            int lineNumber = 1;
            int skipped = 0;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var values = ParseCsvLine(line);
                if (values.Length < 5)
                {
                    skipped++;
                    continue;
                }

                var codigoPostal = values[0].Trim();
                var municipioCodigo = values[3].Trim();

                if (!municipios.TryGetValue(municipioCodigo, out var municipioId))
                {
                    skipped++;
                    continue;
                }

                var key = (municipioId, codigoPostal, false); // Cancelado = false por defecto
                if (seen.Contains(key))
                {
                    skipped++;
                    continue;
                }

                seen.Add(key);
                codigos.Add(new CodigoPostal(codigoPostal, municipioId, "system"));
            }

            if (codigos.Count == 0)
            {
                logger.LogWarning("No se encontraron códigos postales válidos para insertar. Líneas saltadas: {Skipped}", skipped);
                return;
            }

            logger.LogInformation("Insertando {Count} códigos postales en lotes de 1000...", codigos.Count);

            foreach (var batch in codigos.Chunk(1000))
            {
                context.CodigoPostal.AddRange(batch);
                await context.SaveChangesAsync();
            }

            logger.LogInformation("✅ {Count} códigos postales insertados (saltados: {Skipped})", codigos.Count, skipped);
        }

        #endregion

        /// <summary>
        /// Trunca un string al largo máximo especificado
        /// </summary>
        private static string Truncar(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length > maxLength ? value[..maxLength] : value;
        }

        /// <summary>
        /// Parse CSV respetando comillas
        /// </summary>
        private static string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"') inQuotes = !inQuotes;
                else if (c == ',' && !inQuotes) { result.Add(current.ToString()); current.Clear(); }
                else current.Append(c);
            }
            result.Add(current.ToString());
            return result.ToArray();
        }

        #region GrupoCuenta

        private static async Task SeedGrupoCuentaAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.GrupoCuenta.Any())
            {
                logger.LogInformation("GrupoCuenta ya tiene datos, omitiendo seed.");
                return;
            }

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.GrupoCuentas.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró GrupoCuentas.csv. Recurso: {Resource}", resourceName);
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado: Codigo,DescripcionES,DescripcionEN,DescripcionFR

            var datos = new List<(string Codigo, string ES, string EN, string FR)>();
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var v = ParseCsvLine(line);
                if (v.Length < 4 || string.IsNullOrWhiteSpace(v[0])) continue;
                datos.Add((v[0].Trim(), v[1].Trim(), v[2].Trim(), v[3].Trim()));
            }

            if (datos.Count == 0)
            {
                logger.LogWarning("GrupoCuentas.csv no contiene datos válidos.");
                return;
            }

            var grupos = datos.Select(d => new GrupoCuenta(d.Codigo, d.ES, "system")).ToList();
            context.GrupoCuenta.AddRange(grupos);
            await context.SaveChangesAsync();

            var traducciones = new List<GrupoCuentaTranslation>();
            for (int i = 0; i < grupos.Count; i++)
            {
                var id = grupos[i].Id;
                var d = datos[i];
                traducciones.Add(new GrupoCuentaTranslation(id, "es", d.ES, "system"));
                traducciones.Add(new GrupoCuentaTranslation(id, "en", d.EN, "system"));
                traducciones.Add(new GrupoCuentaTranslation(id, "fr", d.FR, "system"));
            }

            context.GrupoCuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ Seed GrupoCuenta: {Count} grupos, {Trans} traducciones", grupos.Count, traducciones.Count);
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

            // ✅ int (no Guid) — GrupoCuenta.Id es int
            var gruposDict = context.GrupoCuenta.ToDictionary(g => g.Codigo, g => g.Id);

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.SubGruposCuentas.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró SubGruposCuentas.csv. Recurso: {Resource}", resourceName);
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado: Codigo,DescripcionES,GrupoCodigo,Deudora,DescripcionEN,DescripcionFR

            // ✅ GrupoId es int
            var datos = new List<(string Codigo, string ES, int GrupoId, bool Deudora, string EN, string FR)>();
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var v = ParseCsvLine(line);
                if (v.Length < 6 || string.IsNullOrWhiteSpace(v[0])) continue;

                var codigo = v[0].Trim();
                var descES = v[1].Trim();
                var grupoCodigo = v[2].Trim();
                var deudora = bool.TryParse(v[3].Trim(), out var b) && b;
                var descEN = v[4].Trim();
                var descFR = v[5].Trim();

                if (!gruposDict.TryGetValue(grupoCodigo, out var grupoId))
                {
                    logger.LogWarning("Grupo '{GrupoCodigo}' no encontrado para subgrupo '{Codigo}'", grupoCodigo, codigo);
                    continue;
                }

                datos.Add((codigo, descES, grupoId, deudora, descEN, descFR));
            }

            if (datos.Count == 0)
            {
                logger.LogWarning("SubGruposCuentas.csv no contiene datos válidos.");
                return;
            }

            var subgrupos = datos.Select(d => new SubGrupoCuenta(d.Codigo, d.ES, d.GrupoId, d.Deudora, "system")).ToList();
            context.SubGrupoCuenta.AddRange(subgrupos);
            await context.SaveChangesAsync();

            var traducciones = new List<SubGrupoCuentaTranslation>();
            for (int i = 0; i < subgrupos.Count; i++)
            {
                var id = subgrupos[i].Id;
                var d = datos[i];
                traducciones.Add(new SubGrupoCuentaTranslation(id, "es", d.ES, "system"));
                traducciones.Add(new SubGrupoCuentaTranslation(id, "en", d.EN, "system"));
                traducciones.Add(new SubGrupoCuentaTranslation(id, "fr", d.FR, "system"));
            }

            context.SubGrupoCuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ Seed SubGrupoCuenta: {Count} subgrupos, {Trans} traducciones", subgrupos.Count, traducciones.Count);
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

            // ✅ int (no Guid) — SubGrupoCuenta.Id es int
            var subgruposDict = context.SubGrupoCuenta.ToDictionary(s => s.Codigo, s => s.Id);

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.Cuentas.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró Cuentas.csv. Recurso: {Resource}", resourceName);
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado: Codigo,DescripcionES,Nivel,SubGrupoCodigo,DescripcionEN,DescripcionFR

            // ✅ SubGrupoId es int
            var datos = new List<(string Codigo, string ES, int Nivel, int SubGrupoId, string EN, string FR)>();
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var v = ParseCsvLine(line);
                if (v.Length < 6 || string.IsNullOrWhiteSpace(v[0])) continue;

                var codigo = v[0].Trim();
                var descES = v[1].Trim();
                var nivel = int.TryParse(v[2].Trim(), out var n) ? n : 1;
                var subGrupoCodigo = v[3].Trim();
                var descEN = v[4].Trim();
                var descFR = v[5].Trim();

                if (!subgruposDict.TryGetValue(subGrupoCodigo, out var subGrupoId))
                {
                    logger.LogWarning("SubGrupo '{SubGrupoCodigo}' no encontrado para cuenta '{Codigo}'", subGrupoCodigo, codigo);
                    continue;
                }

                datos.Add((codigo, descES, nivel, subGrupoId, descEN, descFR));
            }

            if (datos.Count == 0)
            {
                logger.LogWarning("Cuentas.csv no contiene datos válidos.");
                return;
            }

            var cuentas = datos.Select(d => new Cuenta(d.Codigo, d.ES, d.Nivel, d.SubGrupoId, "system")).ToList();
            context.Cuenta.AddRange(cuentas);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ {Count} cuentas insertadas", cuentas.Count);

            var traducciones = new List<CuentaTranslation>();
            for (int i = 0; i < cuentas.Count; i++)
            {
                var id = cuentas[i].Id;
                var d = datos[i];
                traducciones.Add(new CuentaTranslation(id, "es", d.ES, "system"));
                traducciones.Add(new CuentaTranslation(id, "en", d.EN, "system"));
                traducciones.Add(new CuentaTranslation(id, "fr", d.FR, "system"));
            }

            context.CuentaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ Seed Cuenta: {Count} cuentas, {Trans} traducciones", cuentas.Count, traducciones.Count);
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
            var cuentaCobrar = context.Cuenta.FirstOrDefault(c => c.Codigo == "10300001"); // CLIENTES NACIONALES
            var cuentaPagar = context.Cuenta.FirstOrDefault(c => c.Codigo == "20100001"); // PROVEEDORES NACIONALES

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

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.Lineas.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró Lineas.csv. Recurso: {Resource}", resourceName);
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            await reader.ReadLineAsync(); // Saltar encabezado: Codigo,DescripcionES,DescripcionEN,DescripcionFR

            var datos = new List<(string Codigo, string ES, string EN, string FR)>();
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var v = ParseCsvLine(line);
                if (v.Length < 4 || string.IsNullOrWhiteSpace(v[0])) continue;
                datos.Add((v[0].Trim(), v[1].Trim(), v[2].Trim(), v[3].Trim()));
            }

            if (datos.Count == 0)
            {
                logger.LogWarning("Lineas.csv no contiene datos válidos.");
                return;
            }

            var lineas = datos.Select(d => new Linea(d.Codigo, d.ES, "system")).ToList();
            context.Linea.AddRange(lineas);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ {Count} líneas insertadas desde CSV", lineas.Count);

            var traducciones = new List<LineaTranslation>();
            for (int i = 0; i < lineas.Count; i++)
            {
                var id = lineas[i].Id;
                var d = datos[i];
                traducciones.Add(new LineaTranslation(id, "es", d.ES, "system"));
                traducciones.Add(new LineaTranslation(id, "en", d.EN, "system"));
                traducciones.Add(new LineaTranslation(id, "fr", d.FR, "system"));
            }

            context.LineaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ Seed Linea: {Count} líneas, {Trans} traducciones", lineas.Count, traducciones.Count);
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

            // ✅ Lookup por Codigo string en lugar de asumir Id == número del código
            var lineasDict = context.Linea.ToDictionary(l => l.Codigo, l => l.Id);

            var assembly = typeof(DbInitializer).Assembly;
            var resourceName = "GoldBusiness.Infrastructure.Data.SeedData.SubLineas.csv";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                logger.LogWarning("No se encontró SubLineas.csv. Recurso: {Resource}", resourceName);
                logger.LogWarning("Recursos disponibles: {Resources}",
                    string.Join(", ", assembly.GetManifestResourceNames()));
                return;
            }

            using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
            var header = await reader.ReadLineAsync(); // Saltar encabezado: Codigo,DescripcionES,LineaCodigo,DescripcionEN,DescripcionFR
            logger.LogInformation("SubLineas.csv encabezado: {Header}", header);

            var datos = new List<(string Codigo, string ES, int LineaId, string EN, string FR)>();
            string? line;
            int lineNumber = 1;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                // ✅ ParseCsvLine en lugar de Split(',') para manejar campos con comas
                var v = ParseCsvLine(line);
                if (v.Length < 3 || string.IsNullOrWhiteSpace(v[0])) continue;

                var codigo = v[0].Trim();
                var descES = v.Length > 1 ? v[1].Trim() : string.Empty;
                var lineaCodigo = v.Length > 2 ? v[2].Trim() : string.Empty;
                var descEN = v.Length > 3 ? v[3].Trim() : string.Empty;
                var descFR = v.Length > 4 ? v[4].Trim() : string.Empty;

                if (string.IsNullOrWhiteSpace(descES)) continue;

                // ✅ Lookup real por código de línea
                if (!lineasDict.TryGetValue(lineaCodigo, out var lineaId))
                {
                    logger.LogWarning("Línea {Num}: código de línea '{LineaCodigo}' no encontrado para sublínea '{Codigo}'",
                        lineNumber, lineaCodigo, codigo);
                    continue;
                }

                datos.Add((codigo, descES, lineaId, descEN, descFR));
            }

            logger.LogInformation("SubLineas.csv: {Count} sublíneas válidas de {Total} líneas procesadas",
                datos.Count, lineNumber - 1);

            if (datos.Count == 0)
            {
                logger.LogWarning("No se cargaron datos de SubLineas.csv.");
                return;
            }

            var sublineas = new List<SubLinea>();
            foreach (var d in datos)
            {
                try
                {
                    sublineas.Add(new SubLinea(d.Codigo, d.ES, d.LineaId, "system"));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error al crear SubLinea '{Codigo}'", d.Codigo);
                }
            }

            context.SubLinea.AddRange(sublineas);
            await context.SaveChangesAsync();

            logger.LogInformation("SubLineas insertadas: {Count}", sublineas.Count);

            var traducciones = new List<SubLineaTranslation>();
            for (int i = 0; i < sublineas.Count; i++)
            {
                var id = sublineas[i].Id;
                var d = datos[i];
                traducciones.Add(new SubLineaTranslation(id, "es", d.ES, "system"));
                if (!string.IsNullOrWhiteSpace(d.EN)) traducciones.Add(new SubLineaTranslation(id, "en", d.EN, "system"));
                if (!string.IsNullOrWhiteSpace(d.FR)) traducciones.Add(new SubLineaTranslation(id, "fr", d.FR, "system"));
            }

            context.SubLineaTranslation.AddRange(traducciones);
            await context.SaveChangesAsync();

            logger.LogInformation("✅ Seed SubLinea: {Count} sublíneas, {Trans} traducciones",
                sublineas.Count, traducciones.Count);
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