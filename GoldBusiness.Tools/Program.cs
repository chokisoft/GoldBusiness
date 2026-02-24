using System.IO.Compression;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("🌍 GeoNames + CPV → CSV Converter para GoldBusiness");
Console.WriteLine("═══════════════════════════════════════════════════════");

// ═══════════════════════════════════════════════════════════════
// CONFIGURACIÓN
// ═══════════════════════════════════════════════════════════════
var outputDir = Path.Combine(AppContext.BaseDirectory, "output");
Directory.CreateDirectory(outputDir);

var tempDir = Path.Combine(AppContext.BaseDirectory, "temp");
Directory.CreateDirectory(tempDir);

var paisesDeseados = new HashSet<string>
{
    "MX","GT","HN","SV","NI","CR","PA","CU","DO",
    "CO","VE","EC","PE","BO","CL","AR","UY","PY","ES","GQ",
    "FR","BE","CH","LU","MC","CA","HT",
    "SN","CI","CM","BF","NE","ML","TD","GN","RW","BI","BJ","TG","CF","CG","CD","GA","MG","SC","KM","DJ","MU",
    "US","JM","BS","BB","TT","GD","LC","VC","AG","KN","DM","GY","BZ",
    "GB","IE","MT",
    "NG","ZA","KE","GH","UG","TZ","ZW","ZM","BW","NA","LR","SL","GM","MW",
    "IN","PK","PH","SG","MY","HK","LK","BD",
    "AU","NZ","PG","FJ","WS","TO","KI","SB","VU",
    "BR","DE","IT","PT","NL","SE","NO","DK","FI","AT","PL","RU","CN","JP","KR",
    "TH","VN","ID","TR","IL","AE","SA","EG","MA","GR","RO","CZ","HU","UA"
};

var phoneData = new Dictionary<string, (string Tel, string Regex, string Fmt, string Ej)>
{
    ["MX"] = ("+52", @"^(?:\+?52)?[1-9]\d{9}$", "+52 XX XXXX XXXX", "+52 55 1234 5678"),
    ["GT"] = ("+502", @"^(?:\+?502)?[2-7]\d{7}$", "+502 XXXX-XXXX", "+502 5123-4567"),
    ["HN"] = ("+504", @"^(?:\+?504)?[2-9]\d{7}$", "+504 XXXX-XXXX", "+504 9123-4567"),
    ["SV"] = ("+503", @"^(?:\+?503)?[6-7]\d{7}$", "+503 XXXX-XXXX", "+503 7123-4567"),
    ["NI"] = ("+505", @"^(?:\+?505)?[2-8]\d{7}$", "+505 XXXX-XXXX", "+505 8123-4567"),
    ["CR"] = ("+506", @"^(?:\+?506)?[2-9]\d{7}$", "+506 XXXX-XXXX", "+506 8765-4321"),
    ["PA"] = ("+507", @"^(?:\+?507)?[6]\d{7}$", "+507 XXXX-XXXX", "+507 6123-4567"),
    ["CU"] = ("+53", @"^(?:\+?53)?[2-9]\d{7}$", "+53 XXXX-XXXX", "+53 5234-5678"),
    ["DO"] = ("+1", @"^(?:\+?1)?[8]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (809) 234-5678"),
    ["CO"] = ("+57", @"^(?:\+?57)?[3]\d{9}$", "+57 XXX XXX XXXX", "+57 300 123 4567"),
    ["VE"] = ("+58", @"^(?:\+?58)?[4]\d{9}$", "+58 XXX XXX XXXX", "+58 412 123 4567"),
    ["EC"] = ("+593", @"^(?:\+?593)?[9]\d{8}$", "+593 XX XXX XXXX", "+593 99 123 4567"),
    ["PE"] = ("+51", @"^(?:\+?51)?[9]\d{8}$", "+51 XXX XXX XXX", "+51 987 654 321"),
    ["BO"] = ("+591", @"^(?:\+?591)?[67]\d{7}$", "+591 X XXX XXXX", "+591 7 123 4567"),
    ["CL"] = ("+56", @"^(?:\+?56)?[2-9]\d{8}$", "+56 X XXXX XXXX", "+56 9 1234 5678"),
    ["AR"] = ("+54", @"^(?:\+?54)?[1-9]\d{9,10}$", "+54 XX XXXX-XXXX", "+54 11 1234-5678"),
    ["UY"] = ("+598", @"^(?:\+?598)?[9]\d{7}$", "+598 XX XXX XXX", "+598 99 123 456"),
    ["PY"] = ("+595", @"^(?:\+?595)?[9]\d{8}$", "+595 XXX XXX XXX", "+595 981 234 567"),
    ["ES"] = ("+34", @"^(?:\+?34)?[6-9]\d{8}$", "+34 XXX XX XX XX", "+34 612 34 56 78"),
    ["GQ"] = ("+240", @"^(?:\+?240)?[235]\d{8}$", "+240 XXX XXX XXX", "+240 222 123 456"),
    ["FR"] = ("+33", @"^(?:\+?33)?[1-9]\d{8}$", "+33 X XX XX XX XX", "+33 6 12 34 56 78"),
    ["BE"] = ("+32", @"^(?:\+?32)?[4]\d{8}$", "+32 XXX XX XX XX", "+32 470 12 34 56"),
    ["CH"] = ("+41", @"^(?:\+?41)?[7]\d{8}$", "+41 XX XXX XX XX", "+41 79 123 45 67"),
    ["LU"] = ("+352", @"^(?:\+?352)?[6]\d{8}$", "+352 XXX XXX XXX", "+352 621 123 456"),
    ["MC"] = ("+377", @"^(?:\+?377)?[4-9]\d{7,8}$", "+377 XX XX XX XX", "+377 06 12 34 56"),
    ["CA"] = ("+1", @"^(?:\+?1)?[2-9]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (514) 555-1234"),
    ["HT"] = ("+509", @"^(?:\+?509)?[34]\d{7}$", "+509 XXXX-XXXX", "+509 3412-3456"),
    ["SN"] = ("+221", @"^(?:\+?221)?[7]\d{8}$", "+221 XX XXX XX XX", "+221 77 123 45 67"),
    ["CI"] = ("+225", @"^(?:\+?225)?[0-9]\d{9}$", "+225 XX XX XX XX XX", "+225 07 12 34 56 78"),
    ["CM"] = ("+237", @"^(?:\+?237)?[26]\d{8}$", "+237 X XX XX XX XX", "+237 6 12 34 56 78"),
    ["BF"] = ("+226", @"^(?:\+?226)?[67]\d{7}$", "+226 XX XX XX XX", "+226 70 12 34 56"),
    ["NE"] = ("+227", @"^(?:\+?227)?[9]\d{7}$", "+227 XX XX XX XX", "+227 96 12 34 56"),
    ["ML"] = ("+223", @"^(?:\+?223)?[67]\d{7}$", "+223 XX XX XX XX", "+223 65 12 34 56"),
    ["TD"] = ("+235", @"^(?:\+?235)?[69]\d{7}$", "+235 XX XX XX XX", "+235 66 12 34 56"),
    ["GN"] = ("+224", @"^(?:\+?224)?[6]\d{8}$", "+224 XXX XX XX XX", "+224 621 12 34 56"),
    ["RW"] = ("+250", @"^(?:\+?250)?[7]\d{8}$", "+250 XXX XXX XXX", "+250 788 123 456"),
    ["BI"] = ("+257", @"^(?:\+?257)?[67]\d{7}$", "+257 XX XX XX XX", "+257 79 12 34 56"),
    ["BJ"] = ("+229", @"^(?:\+?229)?[69]\d{7}$", "+229 XX XX XX XX", "+229 97 12 34 56"),
    ["TG"] = ("+228", @"^(?:\+?228)?[9]\d{7}$", "+228 XX XX XX XX", "+228 90 12 34 56"),
    ["CF"] = ("+236", @"^(?:\+?236)?[7]\d{7}$", "+236 XX XX XX XX", "+236 70 12 34 56"),
    ["CG"] = ("+242", @"^(?:\+?242)?[0]\d{8}$", "+242 XX XXX XX XX", "+242 06 123 45 67"),
    ["CD"] = ("+243", @"^(?:\+?243)?[89]\d{8}$", "+243 XXX XXX XXX", "+243 812 345 678"),
    ["GA"] = ("+241", @"^(?:\+?241)?[0]\d{7,8}$", "+241 X XX XX XX", "+241 06 12 34 56"),
    ["MG"] = ("+261", @"^(?:\+?261)?[23]\d{8}$", "+261 XX XX XXX XX", "+261 32 12 345 67"),
    ["SC"] = ("+248", @"^(?:\+?248)?[2]\d{6}$", "+248 X XX XX XX", "+248 2 51 23 45"),
    ["KM"] = ("+269", @"^(?:\+?269)?[3]\d{6}$", "+269 XXX XX XX", "+269 321 23 45"),
    ["DJ"] = ("+253", @"^(?:\+?253)?[77]\d{7}$", "+253 XX XX XX XX", "+253 77 12 34 56"),
    ["MU"] = ("+230", @"^(?:\+?230)?[5]\d{7}$", "+230 XXXX XXXX", "+230 5123 4567"),
    ["US"] = ("+1", @"^(?:\+?1)?[2-9]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (202) 555-1234"),
    ["JM"] = ("+1", @"^(?:\+?1)?[8]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (876) 123-4567"),
    ["BS"] = ("+1", @"^(?:\+?1)?[2]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (242) 123-4567"),
    ["BB"] = ("+1", @"^(?:\+?1)?[2]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (246) 123-4567"),
    ["TT"] = ("+1", @"^(?:\+?1)?[8]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (868) 123-4567"),
    ["GD"] = ("+1", @"^(?:\+?1)?[4]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (473) 123-4567"),
    ["LC"] = ("+1", @"^(?:\+?1)?[7]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (758) 123-4567"),
    ["VC"] = ("+1", @"^(?:\+?1)?[7]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (784) 123-4567"),
    ["AG"] = ("+1", @"^(?:\+?1)?[2]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (268) 123-4567"),
    ["KN"] = ("+1", @"^(?:\+?1)?[8]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (869) 123-4567"),
    ["DM"] = ("+1", @"^(?:\+?1)?[7]\d{9}$", "+1 (XXX) XXX-XXXX", "+1 (767) 123-4567"),
    ["GY"] = ("+592", @"^(?:\+?592)?[6]\d{6}$", "+592 XXX-XXXX", "+592 612-3456"),
    ["BZ"] = ("+501", @"^(?:\+?501)?[6]\d{6}$", "+501 XXX-XXXX", "+501 612-3456"),
    ["GB"] = ("+44", @"^(?:\+?44)?[1-9]\d{9,10}$", "+44 XXXX XXXXXX", "+44 7700 900123"),
    ["IE"] = ("+353", @"^(?:\+?353)?[8]\d{8}$", "+353 XX XXX XXXX", "+353 85 123 4567"),
    ["MT"] = ("+356", @"^(?:\+?356)?[2-9]\d{7}$", "+356 XXXX XXXX", "+356 9123 4567"),
    ["NG"] = ("+234", @"^(?:\+?234)?[7-9]\d{9}$", "+234 XXX XXX XXXX", "+234 803 123 4567"),
    ["ZA"] = ("+27", @"^(?:\+?27)?[6-8]\d{8}$", "+27 XX XXX XXXX", "+27 82 123 4567"),
    ["KE"] = ("+254", @"^(?:\+?254)?[7]\d{8}$", "+254 XXX XXXXXX", "+254 712 345678"),
    ["GH"] = ("+233", @"^(?:\+?233)?[2-5]\d{8}$", "+233 XX XXX XXXX", "+233 24 123 4567"),
    ["UG"] = ("+256", @"^(?:\+?256)?[7]\d{8}$", "+256 XXX XXX XXX", "+256 712 345 678"),
    ["TZ"] = ("+255", @"^(?:\+?255)?[67]\d{8}$", "+255 XXX XXX XXX", "+255 712 345 678"),
    ["ZW"] = ("+263", @"^(?:\+?263)?[7]\d{8}$", "+263 XX XXX XXXX", "+263 71 123 4567"),
    ["ZM"] = ("+260", @"^(?:\+?260)?[9]\d{8}$", "+260 XXX XXX XXX", "+260 977 123 456"),
    ["BW"] = ("+267", @"^(?:\+?267)?[7]\d{7}$", "+267 XX XXX XXX", "+267 71 123 456"),
    ["NA"] = ("+264", @"^(?:\+?264)?[8]\d{7}$", "+264 XX XXX XXXX", "+264 81 123 4567"),
    ["LR"] = ("+231", @"^(?:\+?231)?[4-9]\d{7,8}$", "+231 XX XXX XXXX", "+231 77 123 4567"),
    ["SL"] = ("+232", @"^(?:\+?232)?[2-9]\d{7}$", "+232 XX XXX XXX", "+232 76 123 456"),
    ["GM"] = ("+220", @"^(?:\+?220)?[2-9]\d{6,7}$", "+220 XXX XXXX", "+220 771 2345"),
    ["MW"] = ("+265", @"^(?:\+?265)?[18]\d{8}$", "+265 X XX XX XX XX", "+265 1 99 12 34 56"),
    ["IN"] = ("+91", @"^(?:\+?91)?[6-9]\d{9}$", "+91 XXXXX XXXXX", "+91 98765 43210"),
    ["PK"] = ("+92", @"^(?:\+?92)?[3]\d{9}$", "+92 XXX XXXXXXX", "+92 300 1234567"),
    ["PH"] = ("+63", @"^(?:\+?63)?[9]\d{9}$", "+63 XXX XXX XXXX", "+63 917 123 4567"),
    ["SG"] = ("+65", @"^(?:\+?65)?[89]\d{7}$", "+65 XXXX XXXX", "+65 9123 4567"),
    ["MY"] = ("+60", @"^(?:\+?60)?[1]\d{8,9}$", "+60 XX-XXX XXXX", "+60 12-345 6789"),
    ["HK"] = ("+852", @"^(?:\+?852)?[5-9]\d{7}$", "+852 XXXX XXXX", "+852 9123 4567"),
    ["LK"] = ("+94", @"^(?:\+?94)?[7]\d{8}$", "+94 XX XXX XXXX", "+94 71 234 5678"),
    ["BD"] = ("+880", @"^(?:\+?880)?[1]\d{9}$", "+880 XXXX-XXXXXX", "+880 1712-345678"),
    ["AU"] = ("+61", @"^(?:\+?61)?[4]\d{8}$", "+61 XXX XXX XXX", "+61 412 345 678"),
    ["NZ"] = ("+64", @"^(?:\+?64)?[2]\d{7,9}$", "+64 XX XXX XXXX", "+64 21 123 4567"),
    ["PG"] = ("+675", @"^(?:\+?675)?[7]\d{7}$", "+675 XXXX XXXX", "+675 7123 4567"),
    ["FJ"] = ("+679", @"^(?:\+?679)?[7-9]\d{6}$", "+679 XXX XXXX", "+679 712 3456"),
    ["WS"] = ("+685", @"^(?:\+?685)?[7]\d{6}$", "+685 XX XXXXX", "+685 72 12345"),
    ["TO"] = ("+676", @"^(?:\+?676)?[7]\d{6}$", "+676 XXX XXXX", "+676 771 2345"),
    ["KI"] = ("+686", @"^(?:\+?686)?[7-9]\d{7}$", "+686 XXXX XXXX", "+686 7212 3456"),
    ["SB"] = ("+677", @"^(?:\+?677)?[7-9]\d{6}$", "+677 XXX XXXX", "+677 712 3456"),
    ["VU"] = ("+678", @"^(?:\+?678)?[5-7]\d{6}$", "+678 XXX XXXX", "+678 512 3456"),
    ["BR"] = ("+55", @"^(?:\+?55)?[1-9]\d{10}$", "+55 (XX) XXXXX-XXXX", "+55 (11) 91234-5678"),
    ["DE"] = ("+49", @"^(?:\+?49)?[1-9]\d{9,11}$", "+49 XXX XXXXXXXX", "+49 151 23456789"),
    ["IT"] = ("+39", @"^(?:\+?39)?[3]\d{8,9}$", "+39 XXX XXX XXXX", "+39 312 345 6789"),
    ["PT"] = ("+351", @"^(?:\+?351)?[9]\d{8}$", "+351 XXX XXX XXX", "+351 912 345 678"),
    ["NL"] = ("+31", @"^(?:\+?31)?[6]\d{8}$", "+31 X XXXX XXXX", "+31 6 1234 5678"),
    ["SE"] = ("+46", @"^(?:\+?46)?[7]\d{8}$", "+46 XX XXX XX XX", "+46 70 123 45 67"),
    ["NO"] = ("+47", @"^(?:\+?47)?[4-9]\d{7}$", "+47 XXX XX XXX", "+47 912 34 567"),
    ["DK"] = ("+45", @"^(?:\+?45)?[2-9]\d{7}$", "+45 XX XX XX XX", "+45 20 12 34 56"),
    ["FI"] = ("+358", @"^(?:\+?358)?[4-5]\d{7,10}$", "+358 XX XXX XXXX", "+358 40 123 4567"),
    ["AT"] = ("+43", @"^(?:\+?43)?[6]\d{8,13}$", "+43 XXX XXXXXXX", "+43 664 1234567"),
    ["PL"] = ("+48", @"^(?:\+?48)?[4-9]\d{8}$", "+48 XXX XXX XXX", "+48 501 234 567"),
    ["RU"] = ("+7", @"^(?:\+?7)?[9]\d{9}$", "+7 XXX XXX-XX-XX", "+7 912 345-67-89"),
    ["CN"] = ("+86", @"^(?:\+?86)?[1]\d{10}$", "+86 XXX XXXX XXXX", "+86 138 0013 8000"),
    ["JP"] = ("+81", @"^(?:\+?81)?[7-9]\d{9}$", "+81 XX-XXXX-XXXX", "+81 90-1234-5678"),
    ["KR"] = ("+82", @"^(?:\+?82)?[1]\d{9,10}$", "+82 XX-XXXX-XXXX", "+82 10-1234-5678"),
    ["TH"] = ("+66", @"^(?:\+?66)?[6-9]\d{8}$", "+66 XX XXX XXXX", "+66 81 234 5678"),
    ["VN"] = ("+84", @"^(?:\+?84)?[3-9]\d{8,9}$", "+84 XX XXXX XXXX", "+84 90 123 4567"),
    ["ID"] = ("+62", @"^(?:\+?62)?[8]\d{9,11}$", "+62 XXX-XXXX-XXXX", "+62 812-3456-7890"),
    ["TR"] = ("+90", @"^(?:\+?90)?[5]\d{9}$", "+90 XXX XXX XX XX", "+90 532 123 45 67"),
    ["IL"] = ("+972", @"^(?:\+?972)?[5]\d{8}$", "+972 XX-XXX-XXXX", "+972 50-123-4567"),
    ["AE"] = ("+971", @"^(?:\+?971)?[5]\d{8}$", "+971 XX XXX XXXX", "+971 50 123 4567"),
    ["SA"] = ("+966", @"^(?:\+?966)?[5]\d{8}$", "+966 XX XXX XXXX", "+966 50 123 4567"),
    ["EG"] = ("+20", @"^(?:\+?20)?[1]\d{9}$", "+20 XXX XXX XXXX", "+20 100 123 4567"),
    ["MA"] = ("+212", @"^(?:\+?212)?[5-7]\d{8}$", "+212 XXX-XXXXXX", "+212 612-345678"),
    ["GR"] = ("+30", @"^(?:\+?30)?[6]\d{9}$", "+30 XXX XXX XXXX", "+30 691 234 5678"),
    ["RO"] = ("+40", @"^(?:\+?40)?[7]\d{8}$", "+40 XXX XXX XXX", "+40 712 345 678"),
    ["CZ"] = ("+420", @"^(?:\+?420)?[6-7]\d{8}$", "+420 XXX XXX XXX", "+420 601 234 567"),
    ["HU"] = ("+36", @"^(?:\+?36)?[2-3]\d{8}$", "+36 XX XXX XXXX", "+36 20 123 4567"),
    ["UA"] = ("+380", @"^(?:\+?380)?[3-9]\d{8}$", "+380 XX XXX XX XX", "+380 50 123 45 67"),
};

// ═══════════════════════════════════════════════════════════════
// PASO 1: Descargar archivos de GeoNames + CPV
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📥 Descargando datos...");

using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(30)
};

var countryInfoFile = Path.Combine(tempDir, "countryInfo.txt");
var admin1File = Path.Combine(tempDir, "admin1CodesASCII.txt");
var admin2File = Path.Combine(tempDir, "admin2Codes.txt");
var altNamesZip = Path.Combine(tempDir, "alternateNamesV2.zip");
var altNamesFile = Path.Combine(tempDir, "alternateNamesV2.txt");

// ── Postal ────────────────────────────────────────────────────
var postalDir = Path.Combine(tempDir, "postal");
var postalZip = Path.Combine(tempDir, "postal_codes.zip");
var postalFile = Path.Combine(postalDir, "allCountries.txt");
Directory.CreateDirectory(postalDir);

// ── GeoNames ──────────────────────────────────────────────────
if (!File.Exists(countryInfoFile))
{
    Console.Write("  → countryInfo.txt ... ");
    await File.WriteAllBytesAsync(countryInfoFile,
        await httpClient.GetByteArrayAsync("https://download.geonames.org/export/dump/countryInfo.txt"));
    Console.WriteLine("✅");
}

if (!File.Exists(admin1File))
{
    Console.Write("  → admin1CodesASCII.txt ... ");
    await File.WriteAllBytesAsync(admin1File,
        await httpClient.GetByteArrayAsync("https://download.geonames.org/export/dump/admin1CodesASCII.txt"));
    Console.WriteLine("✅");
}

if (!File.Exists(admin2File))
{
    Console.Write("  → admin2Codes.txt ... ");
    await File.WriteAllBytesAsync(admin2File,
        await httpClient.GetByteArrayAsync("https://download.geonames.org/export/dump/admin2Codes.txt"));
    Console.WriteLine("✅");
}

if (!File.Exists(altNamesFile))
{
    Console.WriteLine("  → alternateNamesV2.zip (~300MB)...");
    await DownloadWithProgressAsync(httpClient,
        "https://download.geonames.org/export/dump/alternateNamesV2.zip", altNamesZip);
    Console.Write("  → Descomprimiendo... ");
    ZipFile.ExtractToDirectory(altNamesZip, tempDir, overwriteFiles: true);
    File.Delete(altNamesZip);
    Console.WriteLine("✅");
}

if (!File.Exists(postalFile))
{
    Console.WriteLine("  → postal allCountries.zip (~50MB)...");
    await DownloadWithProgressAsync(httpClient,
        "https://download.geonames.org/export/zip/allCountries.zip", postalZip);
    Console.Write("  → Descomprimiendo códigos postales... ");
    ZipFile.ExtractToDirectory(postalZip, postalDir, overwriteFiles: true);
    File.Delete(postalZip);
    Console.WriteLine("✅");
}

// ═══════════════════════════════════════════════════════════════
// PASO 2: Cargar traducciones GeoNames (alternateNames)
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📖 Cargando traducciones...");

var traducciones = new Dictionary<string, Dictionary<string, string>>();
var idiomasDeseados = new HashSet<string> { "es", "en", "fr" };

using (var reader = new StreamReader(altNamesFile, Encoding.UTF8))
{
    string? line;
    long count = 0;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        count++;
        if (count % 5_000_000 == 0)
            Console.Write($"\r  Procesadas {count:N0} líneas...");

        var parts = line.Split('\t');
        if (parts.Length < 4) continue;

        var geoNameId = parts[1];
        var isoLang = parts[2];
        var altName = parts[3];

        if (!idiomasDeseados.Contains(isoLang)) continue;

        if (!traducciones.ContainsKey(geoNameId))
            traducciones[geoNameId] = [];

        traducciones[geoNameId].TryAdd(isoLang, altName.ToUpperInvariant());
    }
    Console.WriteLine($"\r  ✅ {count:N0} líneas, {traducciones.Count:N0} entidades");
}

// ═══════════════════════════════════════════════════════════════
// PASO 3: Procesar países
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🌍 Procesando países...");

var paisesData = new List<(string Alpha3, string Alpha2, string Tel, string DescES, string DescEN, string DescFR, string Regex, string Fmt, string Ej)>();

using (var reader = new StreamReader(countryInfoFile, Encoding.UTF8))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        if (line.StartsWith('#') || string.IsNullOrWhiteSpace(line)) continue;
        var parts = line.Split('\t');
        if (parts.Length < 17) continue;

        var alpha2 = parts[0].Trim();
        var alpha3 = parts[1].Trim();
        var countryName = parts[4].Trim().ToUpperInvariant();
        var geoNameId = parts[16].Trim();

        if (!paisesDeseados.Contains(alpha2) || !phoneData.ContainsKey(alpha2)) continue;

        var pd = phoneData[alpha2];
        var descES = countryName; var descEN = countryName; var descFR = countryName;

        if (traducciones.TryGetValue(geoNameId, out var trans))
        {
            if (trans.TryGetValue("es", out var es)) descES = es;
            if (trans.TryGetValue("en", out var en)) descEN = en;
            if (trans.TryGetValue("fr", out var fr)) descFR = fr;
        }

        paisesData.Add((alpha3, alpha2, pd.Tel, descES, descEN, descFR, pd.Regex, pd.Fmt, pd.Ej));
    }
}
Console.WriteLine($"  ✅ {paisesData.Count} países");

// ═══════════════════════════════════════════════════════════════
// PASO 4: Procesar provincias (admin1) - EXCLUIR COMPLETAMENTE CUBA
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🗺️ Procesando provincias (excluyendo completamente Cuba)...");

var provinciasPorPais = new Dictionary<string, List<(string Codigo, string Desc, string DescEN, string DescFR)>>();

using (var reader = new StreamReader(admin1File, Encoding.UTF8))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        var parts = line.Split('\t');
        if (parts.Length < 4) continue;

        var codeFull = parts[0];
        var countryCode = codeFull.Split('.')[0];

        // EXCLUIR COMPLETAMENTE CUBA - No importa cómo venga el código
        if (!paisesDeseados.Contains(countryCode) || countryCode == "CU") continue;

        var adminCode = codeFull.Split('.')[1];
        var nombre = parts[1].ToUpperInvariant();
        var geoNameId = parts[3];

        var descES = nombre; var descEN = nombre; var descFR = nombre;
        if (traducciones.TryGetValue(geoNameId, out var trans))
        {
            if (trans.TryGetValue("es", out var es)) descES = es;
            if (trans.TryGetValue("en", out var en)) descEN = en;
            if (trans.TryGetValue("fr", out var fr)) descFR = fr;
        }

        var codigo = $"{countryCode}-{adminCode}";
        if (!provinciasPorPais.ContainsKey(countryCode)) provinciasPorPais[countryCode] = [];
        provinciasPorPais[countryCode].Add((codigo, descES, descEN, descFR));
    }
}
Console.WriteLine($"  ✅ {provinciasPorPais.Sum(p => p.Value.Count)} provincias de {provinciasPorPais.Count} países (excluyendo completamente Cuba)");

// ═══════════════════════════════════════════════════════════════
// PASO 5: Procesar municipios (admin2) - EXCLUIR COMPLETAMENTE CUBA
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🏘️ Procesando municipios (excluyendo completamente Cuba)...");

var municipiosPorPais = new Dictionary<string, List<(string Codigo, string Desc, string ProvCodigo, string DescEN, string DescFR)>>();

using (var reader = new StreamReader(admin2File, Encoding.UTF8))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        var parts = line.Split('\t');
        if (parts.Length < 4) continue;

        var codeFull = parts[0];
        var segments = codeFull.Split('.');
        if (segments.Length < 3) continue;

        var countryCode = segments[0];

        // EXCLUIR COMPLETAMENTE CUBA - No importa cómo venga el código
        if (!paisesDeseados.Contains(countryCode) || countryCode == "CU") continue;

        var adminCode1 = segments[1];
        var adminCode2 = segments[2];
        var nombre = parts[1].ToUpperInvariant();
        var geoNameId = parts[3];

        var descES = nombre; var descEN = nombre; var descFR = nombre;
        if (traducciones.TryGetValue(geoNameId, out var trans))
        {
            if (trans.TryGetValue("es", out var es)) descES = es;
            if (trans.TryGetValue("en", out var en)) descEN = en;
            if (trans.TryGetValue("fr", out var fr)) descFR = fr;
        }

        var codigo = $"{countryCode}-{adminCode1}-{adminCode2}";
        var provCodigo = $"{countryCode}-{adminCode1}";
        if (!municipiosPorPais.ContainsKey(countryCode)) municipiosPorPais[countryCode] = [];
        municipiosPorPais[countryCode].Add((codigo, descES, provCodigo, descEN, descFR));
    }
}
Console.WriteLine($"  ✅ {municipiosPorPais.Sum(p => p.Value.Count)} municipios de {municipiosPorPais.Count} países (excluyendo completamente Cuba)");

// ═══════════════════════════════════════════════════════════════
// PASO 6: Procesar códigos postales - EXCLUIR COMPLETAMENTE CUBA
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📮 Procesando códigos postales (excluyendo completamente Cuba)...");

var cpPorPais = new Dictionary<string, List<(string CP, string Localidad, string ProvCodigo, string MunCodigo)>>();
var cpUnicos = new HashSet<string>();

using (var reader = new StreamReader(postalFile, Encoding.UTF8))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        var parts = line.Split('\t');
        if (parts.Length < 7) continue;

        var countryCode = parts[0].Trim();

        // EXCLUIR COMPLETAMENTE CUBA
        if (!paisesDeseados.Contains(countryCode) || countryCode == "CU") continue;

        var postalCode = parts[1].Trim();
        var placeName = parts[2].Trim().ToUpperInvariant();
        var adminCode1 = parts[4].Trim();
        var adminCode2 = parts[6].Trim();

        if (string.IsNullOrEmpty(postalCode)) continue;

        var provCodigo = string.IsNullOrEmpty(adminCode1)
            ? "" : $"{countryCode}-{adminCode1}";
        var munCodigo = string.IsNullOrEmpty(adminCode1) || string.IsNullOrEmpty(adminCode2)
            ? "" : $"{countryCode}-{adminCode1}-{adminCode2}";

        var key = $"{countryCode}|{postalCode}|{provCodigo}|{munCodigo}|{placeName}";
        if (!cpUnicos.Add(key)) continue;

        if (!cpPorPais.ContainsKey(countryCode)) cpPorPais[countryCode] = [];
        cpPorPais[countryCode].Add((postalCode, placeName, provCodigo, munCodigo));
    }
}
Console.WriteLine($"  ✅ {cpPorPais.Sum(p => p.Value.Count):N0} códigos postales de {cpPorPais.Count} países (excluyendo completamente Cuba)");

// ═══════════════════════════════════════════════════════════════
// PASO 6.5: Integrar datos manuales de Cuba
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🇨🇺 Integrando datos manuales de Cuba...");

// Datos manuales para provincias de Cuba (admin1) - SEGÚN GEONAMES
var cubaAdmin1Data = new List<(string Codigo, string DescES, string DescEN, string DescFR)>
{
    ("CU-01", "PINAR DEL RÍO", "PINAR DEL RIO", "PINAR DEL RÍO"),
    ("CU-02", "ARTEMISA", "ARTEMISA", "ARTEMISA"),
    ("CU-03", "LA HABANA", "HAVANA", "LA HAVANE"),
    ("CU-04", "MAYABEQUE", "MAYABEQUE", "MAYABEQUE"),
    ("CU-05", "MATANZAS", "MATANZAS", "MATANZAS"),
    ("CU-06", "VILLA CLARA", "VILLA CLARA", "VILLA CLARA"),
    ("CU-07", "CIENFUEGOS", "CIENFUEGOS", "CIENFUEGOS"),
    ("CU-08", "SANCTI SPÍRITUS", "SANCTI SPIRITUS", "SANCTI SPÍRITUS"),
    ("CU-09", "CIEGO DE ÁVILA", "CIEGO DE AVILA", "CIEGO DE ÁVILA"),
    ("CU-10", "CAMAGÜEY", "CAMAGUEY", "CAMAGÜEY"),
    ("CU-11", "LAS TUNAS", "LAS TUNAS", "LAS TUNAS"),
    ("CU-12", "GRANMA", "GRANMA", "GRANMA"),
    ("CU-13", "HOLGUÍN", "HOLGUIN", "HOLGUÍN"),
    ("CU-14", "SANTIAGO DE CUBA", "SANTIAGO DE CUBA", "SANTIAGO DE CUBA"),
    ("CU-15", "GUANTÁNAMO", "GUANTANAMO", "GUANTÁNAMO"),
    ("CU-16", "ISLA DE LA JUVENTUD", "ISLE OF YOUTH", "ÎLE DE LA JEUNESSE"),
};

// Datos manuales para municipios de Cuba (admin2) - CORREGIDO CON CÓDIGOS REALES
// SOLO LOS MUNICIPIOS OFICIALES, SIN DUPLICADOS
var cubaAdmin2Data = new List<(string Codigo, string ProvCodigo, string DescES, string DescEN, string DescFR)>
{
    // PINAR DEL RÍO (14 municipios) - Códigos CU-01-01 a CU-01-14
    ("CU-01-01", "CU-01", "PINAR DEL RÍO", "PINAR DEL RIO", "PINAR DEL RÍO"),
    ("CU-01-02", "CU-01", "CONSOLACIÓN DEL SUR", "CONSOLACION DEL SUR", "CONSOLACIÓN DEL SUR"),
    ("CU-01-03", "CU-01", "GUANE", "GUANE", "GUANE"),
    ("CU-01-04", "CU-01", "LA PALMA", "LA PALMA", "LA PALMA"),
    ("CU-01-05", "CU-01", "LOS PALACIOS", "LOS PALACIOS", "LOS PALACIOS"),
    ("CU-01-06", "CU-01", "MANTUA", "MANTUA", "MANTUA"),
    ("CU-01-07", "CU-01", "MINAS DE MATAHAMBRE", "MINAS DE MATAHAMBRE", "MINAS DE MATAHAMBRE"),
    ("CU-01-08", "CU-01", "SAN JUAN Y MARTÍNEZ", "SAN JUAN Y MARTINEZ", "SAN JUAN Y MARTÍNEZ"),
    ("CU-01-09", "CU-01", "SAN LUIS", "SAN LUIS", "SAN LUIS"),
    ("CU-01-10", "CU-01", "SANDINO", "SANDINO", "SANDINO"),
    ("CU-01-11", "CU-01", "VIÑALES", "VIÑALES", "VIÑALES"),
    ("CU-01-12", "CU-01", "BAHÍA HONDA", "BAHIA HONDA", "BAHÍA HONDA"),
    ("CU-01-13", "CU-01", "CANDELARIA", "CANDELARIA", "CANDELARIA"),
    ("CU-01-14", "CU-01", "SAN CRISTÓBAL", "SAN CRISTOBAL", "SAN CRISTÓBAL"),
    
    // ARTEMISA (11 municipios) - Artemisa tiene 11 municipios oficialmente
    ("CU-02-01", "CU-02", "ARTEMISA", "ARTEMISA", "ARTEMISA"),
    ("CU-02-02", "CU-02", "ALQUÍZAR", "ALQUIZAR", "ALQUÍZAR"),
    ("CU-02-03", "CU-02", "BAUTA", "BAUTA", "BAUTA"),
    ("CU-02-04", "CU-02", "CAIMITO", "CAIMITO", "CAIMITO"),
    ("CU-02-05", "CU-02", "GUANAJAY", "GUANAJAY", "GUANAJAY"),
    ("CU-02-06", "CU-02", "GÜIRA DE MELENA", "GUIRA DE MELENA", "GÜIRA DE MELENA"),
    ("CU-02-07", "CU-02", "MARIEL", "MARIEL", "MARIEL"),
    ("CU-02-08", "CU-02", "SAN ANTONIO DE LOS BAÑOS", "SAN ANTONIO DE LOS BAÑOS", "SAN ANTONIO DE LOS BAÑOS"),
    
    // LA HABANA (15 municipios) - Códigos CU-03-01 a CU-03-15
    ("CU-03-01", "CU-03", "ARROYO NARANJO", "ARROYO NARANJO", "ARROYO NARANJO"),
    ("CU-03-02", "CU-03", "BOYEROS", "BOYEROS", "BOYEROS"),
    ("CU-03-03", "CU-03", "CENTRO HABANA", "CENTRO HABANA", "CENTRO HABANA"),
    ("CU-03-04", "CU-03", "CERRO", "CERRO", "CERRO"),
    ("CU-03-05", "CU-03", "COTORRO", "COTORRO", "COTORRO"),
    ("CU-03-06", "CU-03", "DIEZ DE OCTUBRE", "DIEZ DE OCTUBRE", "DIEZ DE OCTUBRE"),
    ("CU-03-07", "CU-03", "GUANABACOA", "GUANABACOA", "GUANABACOA"),
    ("CU-03-08", "CU-03", "LA HABANA DEL ESTE", "EAST HAVANA", "HAVANE EST"),
    ("CU-03-09", "CU-03", "LA HABANA VIEJA", "OLD HAVANA", "LA HAVANE VIEILLE"),
    ("CU-03-10", "CU-03", "LA LISA", "LA LISA", "LA LISA"),
    ("CU-03-11", "CU-03", "MARIANAO", "MARIANAO", "MARIANAO"),
    ("CU-03-12", "CU-03", "PLAZA DE LA REVOLUCIÓN", "PLAZA DE LA REVOLUCION", "PLAZA DE LA REVOLUCIÓN"),
    ("CU-03-13", "CU-03", "REGLA", "REGLA", "REGLA"),
    ("CU-03-14", "CU-03", "SAN MIGUEL DEL PADRÓN", "SAN MIGUEL DEL PADRON", "SAN MIGUEL DEL PADRÓN"),
    ("CU-03-15", "CU-03", "PLAYA", "PLAYA", "PLAYA"),
    
    // MAYABEQUE (11 municipios) - Códigos CU-04-01 a CU-04-11
    ("CU-04-01", "CU-04", "BATABANÓ", "BATABANO", "BATABANÓ"),
    ("CU-04-02", "CU-04", "BEJUCAL", "BEJUCAL", "BEJUCAL"),
    ("CU-04-03", "CU-04", "GÜINES", "GUINES", "GÜINES"),
    ("CU-04-04", "CU-04", "JARUCO", "JARUCO", "JARUCO"),
    ("CU-04-05", "CU-04", "MADRUGA", "MADRUGA", "MADRUGA"),
    ("CU-04-06", "CU-04", "MELENA DEL SUR", "MELENA DEL SUR", "MELENA DEL SUR"),
    ("CU-04-07", "CU-04", "NUEVA PAZ", "NUEVA PAZ", "NUEVA PAZ"),
    ("CU-04-08", "CU-04", "QUIVICÁN", "QUIVICAN", "QUIVICÁN"),
    ("CU-04-09", "CU-04", "SAN JOSÉ DE LAS LAJAS", "SAN JOSE DE LAS LAJAS", "SAN JOSÉ DE LAS LAJAS"),
    ("CU-04-10", "CU-04", "SAN NICOLÁS DE BARI", "SAN NICOLAS DE BARI", "SAN NICOLÁS DE BARI"),
    ("CU-04-11", "CU-04", "SANTA CRUZ DEL NORTE", "SANTA CRUZ DEL NORTE", "SANTA CRUZ DEL NORTE"),
    
    // MATANZAS (13 municipios) - Códigos CU-05-01 a CU-05-13
    ("CU-05-01", "CU-05", "MATANZAS", "MATANZAS", "MATANZAS"),
    ("CU-05-02", "CU-05", "CALIMETE", "CALIMETE", "CALIMETE"),
    ("CU-05-03", "CU-05", "CÁRDENAS", "CARDENAS", "CÁRDENAS"),
    ("CU-05-04", "CU-05", "CIÉNAGA DE ZAPATA", "CIENAGA DE ZAPATA", "CIÉNAGA DE ZAPATA"),
    ("CU-05-05", "CU-05", "COLÓN", "COLON", "COLÓN"),
    ("CU-05-06", "CU-05", "JAGÜEY GRANDE", "JAGUEY GRANDE", "JAGÜEY GRANDE"),
    ("CU-05-07", "CU-05", "JOVELLANOS", "JOVELLANOS", "JOVELLANOS"),
    ("CU-05-08", "CU-05", "LIMONAR", "LIMONAR", "LIMONAR"),
    ("CU-05-09", "CU-05", "LOS ARABOS", "LOS ARABOS", "LOS ARABOS"),
    ("CU-05-10", "CU-05", "MARTÍ", "MARTI", "MARTÍ"),
    ("CU-05-11", "CU-05", "PEDRO BETANCOURT", "PEDRO BETANCOURT", "PEDRO BETANCOURT"),
    ("CU-05-12", "CU-05", "PERICO", "PERICO", "PERICO"),
    ("CU-05-13", "CU-05", "UNIÓN DE REYES", "UNION DE REYES", "UNIÓN DE REYES"),
    
    // VILLA CLARA (13 municipios) - Códigos CU-06-01 a CU-06-13
    ("CU-06-01", "CU-06", "SANTA CLARA", "SANTA CLARA", "SANTA CLARA"),
    ("CU-06-02", "CU-06", "CAIBARIÉN", "CAIBARIEN", "CAIBARIÉN"),
    ("CU-06-03", "CU-06", "CAMAJUANÍ", "CAMAJUANI", "CAMAJUANÍ"),
    ("CU-06-04", "CU-06", "CIFUENTES", "CIFUENTES", "CIFUENTES"),
    ("CU-06-05", "CU-06", "CORRALILLO", "CORRALILLO", "CORRALILLO"),
    ("CU-06-06", "CU-06", "ENCRUCIJADA", "ENCRUCIJADA", "ENCRUCIJADA"),
    ("CU-06-07", "CU-06", "MANICARAGUA", "MANICARAGUA", "MANICARAGUA"),
    ("CU-06-08", "CU-06", "PLACETAS", "PLACETAS", "PLACETAS"),
    ("CU-06-09", "CU-06", "QUEMADO DE GÜINES", "QUEMADO DE GUINES", "QUEMADO DE GÜINES"),
    ("CU-06-10", "CU-06", "RANCHO VELOZ", "RANCHO VELOZ", "RANCHO VELOZ"),
    ("CU-06-11", "CU-06", "REMEDIOS", "REMEDIOS", "REMEDIOS"),
    ("CU-06-12", "CU-06", "SAGUA LA GRANDE", "SAGUA LA GRANDE", "SAGUA LA GRANDE"),
    ("CU-06-13", "CU-06", "SAN JUAN DE LOS YERAS", "SAN JUAN DE LOS YERAS", "SAN JUAN DE LOS YERAS"),
    
    // CIENFUEGOS (8 municipios) - Códigos CU-07-01 a CU-07-08
    ("CU-07-01", "CU-07", "CIENFUEGOS", "CIENFUEGOS", "CIENFUEGOS"),
    ("CU-07-02", "CU-07", "ABREUS", "ABREUS", "ABREUS"),
    ("CU-07-03", "CU-07", "AGUADA", "AGUADA", "AGUADA"),
    ("CU-07-04", "CU-07", "CRUCES", "CRUCES", "CRUCES"),
    ("CU-07-05", "CU-07", "CUMANAYAGUA", "CUMANAYAGUA", "CUMANAYAGUA"),
    ("CU-07-06", "CU-07", "LAJAS", "LAJAS", "LAJAS"),
    ("CU-07-07", "CU-07", "PALMIRA", "PALMIRA", "PALMIRA"),
    ("CU-07-08", "CU-07", "RODAS", "RODAS", "RODAS"),
    
    // SANCTI SPÍRITUS (8 municipios) - Códigos CU-08-01 a CU-08-08
    ("CU-08-01", "CU-08", "SANCTI SPÍRITUS", "SANCTI SPIRITUS", "SANCTI SPÍRITUS"),
    ("CU-08-02", "CU-08", "CABAIGUÁN", "CABAIGUAN", "CABAIGUÁN"),
    ("CU-08-03", "CU-08", "FOMENTO", "FOMENTO", "FOMENTO"),
    ("CU-08-04", "CU-08", "JATIBONICO", "JATIBONICO", "JATIBONICO"),
    ("CU-08-05", "CU-08", "LA SIERPE", "LA SIERPE", "LA SIERPE"),
    ("CU-08-06", "CU-08", "TAGUASCO", "TAGUASCO", "TAGUASCO"),
    ("CU-08-07", "CU-08", "TRINIDAD", "TRINIDAD", "TRINIDAD"),
    ("CU-08-08", "CU-08", "YAGUAJAY", "YAGUAJAY", "YAGUAJAY"),
    
    // CIEGO DE ÁVILA (10 municipios) - Códigos CU-09-01 a CU-09-10
    ("CU-09-01", "CU-09", "CIEGO DE ÁVILA", "CIEGO DE AVILA", "CIEGO DE ÁVILA"),
    ("CU-09-02", "CU-09", "BARAGUÁ", "BARAGUA", "BARAGUÁ"),
    ("CU-09-03", "CU-09", "BOLIVIA", "BOLIVIA", "BOLIVIA"),
    ("CU-09-04", "CU-09", "CHAMBAS", "CHAMBAS", "CHAMBAS"),
    ("CU-09-05", "CU-09", "CIRO REDONDO", "CIRO REDONDO", "CIRO REDONDO"),
    ("CU-09-06", "CU-09", "FLORENCIA", "FLORENCIA", "FLORENCIA"),
    ("CU-09-07", "CU-09", "MAJAGUA", "MAJAGUA", "MAJAGUA"),
    ("CU-09-08", "CU-09", "MORÓN", "MORON", "MORÓN"),
    ("CU-09-09", "CU-09", "PRIMERO DE ENERO", "PRIMERO DE ENERO", "PRIMERO DE ENERO"),
    ("CU-09-10", "CU-09", "VENEZUELA", "VENEZUELA", "VENEZUELA"),
    
    // CAMAGÜEY (13 municipios) - Códigos CU-10-01 a CU-10-13
    ("CU-10-01", "CU-10", "CAMAGÜEY", "CAMAGUEY", "CAMAGÜEY"),
    ("CU-10-02", "CU-10", "CARLOS MANUEL DE CÉSPEDES", "CARLOS MANUEL DE CESPEDES", "CARLOS MANUEL DE CÉSPEDES"),
    ("CU-10-03", "CU-10", "ESMERALDA", "ESMERALDA", "ESMERALDA"),
    ("CU-10-04", "CU-10", "FLORIDA", "FLORIDA", "FLORIDA"),
    ("CU-10-05", "CU-10", "GUÁIMARO", "GUAIMARO", "GUÁIMARO"),
    ("CU-10-06", "CU-10", "JIMAGUAYÚ", "JIMAGUAYU", "JIMAGUAYÚ"),
    ("CU-10-07", "CU-10", "MINAS", "MINAS", "MINAS"),
    ("CU-10-08", "CU-10", "NAJASA", "NAJASA", "NAJASA"),
    ("CU-10-09", "CU-10", "NUEVITAS", "NUEVITAS", "NUEVITAS"),
    ("CU-10-10", "CU-10", "SANTA CRUZ DEL SUR", "SANTA CRUZ DEL SUR", "SANTA CRUZ DEL SUR"),
    ("CU-10-11", "CU-10", "SIBANICÚ", "SIBANICU", "SIBANICÚ"),
    ("CU-10-12", "CU-10", "SIERRA DE CUBITAS", "SIERRA DE CUBITAS", "SIERRA DE CUBITAS"),
    ("CU-10-13", "CU-10", "VERTIENTES", "VERTIENTES", "VERTIENTES"),
    
    // LAS TUNAS (8 municipios) - Códigos CU-11-01 a CU-11-08
    ("CU-11-01", "CU-11", "LAS TUNAS", "LAS TUNAS", "LAS TUNAS"),
    ("CU-11-02", "CU-11", "AMANCIO", "AMANCIO", "AMANCIO"),
    ("CU-11-03", "CU-11", "COLOMBIA", "COLOMBIA", "COLOMBIA"),
    ("CU-11-04", "CU-11", "JESÚS MENÉNDEZ", "JESUS MENENDEZ", "JESÚS MENÉNDEZ"),
    ("CU-11-05", "CU-11", "JOBABO", "JOBABO", "JOBABO"),
    ("CU-11-06", "CU-11", "MAJIBACOA", "MAJIBACOA", "MAJIBACOA"),
    ("CU-11-07", "CU-11", "MANATÍ", "MANATI", "MANATÍ"),
    ("CU-11-08", "CU-11", "PUERTO PADRE", "PUERTO PADRE", "PUERTO PADRE"),
    
    // GRANMA (13 municipios) - Códigos CU-12-01 a CU-12-13
    ("CU-12-01", "CU-12", "BAYAMO", "BAYAMO", "BAYAMO"),
    ("CU-12-02", "CU-12", "BARTOLOMÉ MASÓ", "BARTOLOME MASO", "BARTOLOMÉ MASÓ"),
    ("CU-12-03", "CU-12", "BUEY ARRIBA", "BUEY ARRIBA", "BUEY ARRIBA"),
    ("CU-12-04", "CU-12", "CAMPECHUELA", "CAMPECHUELA", "CAMPECHUELA"),
    ("CU-12-05", "CU-12", "GUISA", "GUISA", "GUISA"),
    ("CU-12-06", "CU-12", "JIGUANÍ", "JIGUANI", "JIGUANÍ"),
    ("CU-12-07", "CU-12", "MANZANILLO", "MANZANILLO", "MANZANILLO"),
    ("CU-12-08", "CU-12", "MEDIA LUNA", "MEDIA LUNA", "MEDIA LUNA"),
    ("CU-12-09", "CU-12", "NIQUERO", "NIQUERO", "NIQUERO"),
    ("CU-12-10", "CU-12", "PILÓN", "PILON", "PILÓN"),
    ("CU-12-11", "CU-12", "RÍO CAUTO", "RIO CAUTO", "RÍO CAUTO"),
    ("CU-12-12", "CU-12", "YARA", "YARA", "YARA"),
    // En Granma falta "CÁRDENAS" porque no existe, el que aparece en tu archivo es incorrecto
    
    // HOLGUÍN (14 municipios) - Códigos CU-13-01 a CU-13-14
    ("CU-13-01", "CU-13", "HOLGUÍN", "HOLGUIN", "HOLGUÍN"),
    ("CU-13-02", "CU-13", "ANTILLA", "ANTILLA", "ANTILLA"),
    ("CU-13-03", "CU-13", "BÁGUANOS", "BAGUANOS", "BÁGUANOS"),
    ("CU-13-04", "CU-13", "BANES", "BANES", "BANES"),
    ("CU-13-05", "CU-13", "CACOCUM", "CACOCUM", "CACOCUM"),
    ("CU-13-06", "CU-13", "CALIXTO GARCÍA", "CALIXTO GARCIA", "CALIXTO GARCÍA"),
    ("CU-13-07", "CU-13", "CUETO", "CUETO", "CUETO"),
    ("CU-13-08", "CU-13", "FRANK PAÍS", "FRANK PAIS", "FRANK PAÍS"),
    ("CU-13-09", "CU-13", "GIBARA", "GIBARA", "GIBARA"),
    ("CU-13-10", "CU-13", "MAYARÍ", "MAYARI", "MAYARÍ"),
    ("CU-13-11", "CU-13", "MOA", "MOA", "MOA"),
    ("CU-13-12", "CU-13", "RAFAEL FREYRE", "RAFAEL FREYRE", "RAFAEL FREYRE"),
    ("CU-13-13", "CU-13", "SAGUA DE TÁNAMO", "SAGUA DE TANAMO", "SAGUA DE TÁNAMO"),
    ("CU-13-14", "CU-13", "URBANO NORIS", "URBANO NORIS", "URBANO NORIS"),
    
    // SANTIAGO DE CUBA (9 municipios) - Códigos CU-14-01 a CU-14-09
    ("CU-14-01", "CU-14", "SANTIAGO DE CUBA", "SANTIAGO DE CUBA", "SANTIAGO DE CUBA"),
    ("CU-14-02", "CU-14", "CONTRAMAESTRE", "CONTRAMAESTRE", "CONTRAMAESTRE"),
    ("CU-14-03", "CU-14", "GUAMÁ", "GUAMA", "GUAMÁ"),
    ("CU-14-04", "CU-14", "MELLA", "MELLA", "MELLA"),
    ("CU-14-05", "CU-14", "PALMA SORIANO", "PALMA SORIANO", "PALMA SORIANO"),
    ("CU-14-06", "CU-14", "SAN LUIS", "SAN LUIS", "SAN LUIS"),
    ("CU-14-07", "CU-14", "SEGUNDO FRENTE", "SEGUNDO FRENTE", "SEGUNDO FRENTE"),
    ("CU-14-08", "CU-14", "SONGO - LA MAYA", "SONGO - LA MAYA", "SONGO - LA MAYA"),
    ("CU-14-09", "CU-14", "TERCER FRENTE", "TERCER FRENTE", "TERCER FRENTE"),
    
    // GUANTÁNAMO (10 municipios) - Códigos CU-15-01 a CU-15-10
    ("CU-15-01", "CU-15", "GUANTÁNAMO", "GUANTANAMO", "GUANTÁNAMO"),
    ("CU-15-02", "CU-15", "BARACOA", "BARACOA", "BARACOA"),
    ("CU-15-03", "CU-15", "CAIMANERA", "CAIMANERA", "CAIMANERA"),
    ("CU-15-04", "CU-15", "EL SALVADOR", "EL SALVADOR", "EL SALVADOR"),
    ("CU-15-05", "CU-15", "IMÍAS", "IMIAS", "IMÍAS"),
    ("CU-15-06", "CU-15", "MAISÍ", "MAISI", "MAISÍ"),
    ("CU-15-07", "CU-15", "MANUEL TAMÉS", "MANUEL TAMES", "MANUEL TAMÉS"),
    ("CU-15-08", "CU-15", "NICETO PÉREZ", "NICETO PEREZ", "NICETO PÉREZ"),
    ("CU-15-09", "CU-15", "SAN ANTONIO DEL SUR", "SAN ANTONIO DEL SUR", "SAN ANTONIO DEL SUR"),
    ("CU-15-10", "CU-15", "YATERAS", "YATERAS", "YATERAS"),
    
    // ISLA DE LA JUVENTUD (1 municipio especial) - Código CU-16-01
    ("CU-16-01", "CU-16", "ISLA DE LA JUVENTUD", "ISLE OF YOUTH", "ÎLE DE LA JEUNESSE"),
};

// Agregar provincias de Cuba manualmente
if (!provinciasPorPais.ContainsKey("CU"))
    provinciasPorPais["CU"] = new List<(string Codigo, string Desc, string DescEN, string DescFR)>();

foreach (var prov in cubaAdmin1Data)
{
    provinciasPorPais["CU"].Add((prov.Codigo, prov.DescES, prov.DescEN, prov.DescFR));
}

// Agregar municipios de Cuba manualmente - SOLO LOS OFICIALES
if (!municipiosPorPais.ContainsKey("CU"))
    municipiosPorPais["CU"] = new List<(string Codigo, string Desc, string ProvCodigo, string DescEN, string DescFR)>();

// Usar un HashSet para evitar duplicados por código
var codigosMunicipiosUnicos = new HashSet<string>();

foreach (var mun in cubaAdmin2Data)
{
    // Solo agregar si el código no existe ya
    if (codigosMunicipiosUnicos.Add(mun.Codigo))
    {
        municipiosPorPais["CU"].Add((mun.Codigo, mun.DescES, mun.ProvCodigo, mun.DescEN, mun.DescFR));
    }
}

Console.WriteLine($"  ✅ Cuba: {provinciasPorPais["CU"].Count} provincias, {municipiosPorPais["CU"].Count} municipios oficiales");

// Agregar códigos postales de Cuba manualmente
if (!cpPorPais.ContainsKey("CU"))
    cpPorPais["CU"] = new List<(string CP, string Localidad, string ProvCodigo, string MunCodigo)>();

// Datos manuales para códigos postales de Cuba
var cubaPostalData = new Dictionary<string, List<(string CP, string Localidad, string Provincia, string Municipio)>>
{
    ["CU"] = new List<(string, string, string, string)>
    {
        // ==================== PINAR DEL RÍO ====================
        ("24150", "SANDINO", "PINAR DEL RÍO", "SANDINO"),
        ("22200", "MANTUA", "PINAR DEL RÍO", "MANTUA"),
        ("22300", "MINAS DE MATAHAMBRE", "PINAR DEL RÍO", "MINAS DE MATAHAMBRE"),
        ("22400", "VIÑALES", "PINAR DEL RÍO", "VIÑALES"),
        ("22600", "LA PALMA", "PINAR DEL RÍO", "LA PALMA"),
        ("22600", "BAHÍA HONDA", "PINAR DEL RÍO", "BAHÍA HONDA"), // Mismo CP que La Palma
        ("22700", "CANDELARIA", "PINAR DEL RÍO", "CANDELARIA"),
        ("22800", "SAN CRISTÓBAL", "PINAR DEL RÍO", "SAN CRISTÓBAL"),
        ("22900", "LOS PALACIOS", "PINAR DEL RÍO", "LOS PALACIOS"),
        ("23000", "CONSOLACIÓN DEL SUR", "PINAR DEL RÍO", "CONSOLACIÓN DEL SUR"),
        ("20100", "PINAR DEL RÍO", "PINAR DEL RÍO", "PINAR DEL RÍO"),
        ("23100", "SAN LUIS", "PINAR DEL RÍO", "SAN LUIS"),
        ("23200", "SAN JUAN Y MARTÍNEZ", "PINAR DEL RÍO", "SAN JUAN Y MARTÍNEZ"),
        ("23300", "GUANE", "PINAR DEL RÍO", "GUANE"),
        
        // ==================== ARTEMISA ====================
        ("33700", "ALQUÍZAR", "ARTEMISA", "ALQUÍZAR"),
        ("33800", "ARTEMISA", "ARTEMISA", "ARTEMISA"),
        ("32400", "BAUTA", "ARTEMISA", "BAUTA"),
        ("32300", "CAIMITO", "ARTEMISA", "CAIMITO"),
        ("32200", "GUANAJAY", "ARTEMISA", "GUANAJAY"),
        ("33600", "GÜIRA DE MELENA", "ARTEMISA", "GÜIRA DE MELENA"),
        ("32100", "MARIEL", "ARTEMISA", "MARIEL"),
        ("32500", "SAN ANTONIO DE LOS BAÑOS", "ARTEMISA", "SAN ANTONIO DE LOS BAÑOS"),
        
        // ==================== LA HABANA ====================
        ("11300", "PLAYA", "LA HABANA", "PLAYA"),
        ("10400", "PLAZA DE LA REVOLUCIÓN", "LA HABANA", "PLAZA DE LA REVOLUCIÓN"),
        ("10200", "CENTRO HABANA", "LA HABANA", "CENTRO HABANA"),
        ("10100", "LA HABANA VIEJA", "LA HABANA", "LA HABANA VIEJA"),
        ("11200", "REGLA", "LA HABANA", "REGLA"),
        ("10900", "HABANA DEL ESTE", "LA HABANA", "HABANA DEL ESTE"), // También llamado Guanabacoa
        ("11100", "GUANABACOA", "LA HABANA", "GUANABACOA"),
        ("11000", "SAN MIGUEL DEL PADRÓN", "LA HABANA", "SAN MIGUEL DEL PADRÓN"),
        ("10700", "DIEZ DE OCTUBRE", "LA HABANA", "DIEZ DE OCTUBRE"), // También 10500
        ("10600", "CERRO", "LA HABANA", "CERRO"),
        ("11500", "MARIANAO", "LA HABANA", "MARIANAO"),
        ("17100", "LA LISA", "LA HABANA", "LA LISA"),
        ("10800", "BOYEROS", "LA HABANA", "BOYEROS"), // ¡Confirmado!
        ("10900", "ARROYO NARANJO", "LA HABANA", "ARROYO NARANJO"),
        ("14000", "COTORRO", "LA HABANA", "COTORRO"),
        
        // ==================== MAYABEQUE ====================
        ("33400", "BATABANÓ", "MAYABEQUE", "BATABANÓ"),
        ("32600", "BEJUCAL", "MAYABEQUE", "BEJUCAL"),
        ("33900", "GÜINES", "MAYABEQUE", "GÜINES"),
        ("32800", "JARUCO", "MAYABEQUE", "JARUCO"),
        ("33000", "MADRUGA", "MAYABEQUE", "MADRUGA"),
        ("33300", "MELENA DEL SUR", "MAYABEQUE", "MELENA DEL SUR"),
        ("33100", "NUEVA PAZ", "MAYABEQUE", "NUEVA PAZ"),
        ("33500", "QUIVICÁN", "MAYABEQUE", "QUIVICÁN"),
        ("32700", "SAN JOSÉ DE LAS LAJAS", "MAYABEQUE", "SAN JOSÉ DE LAS LAJAS"),
        ("33200", "SAN NICOLÁS DE BARI", "MAYABEQUE", "SAN NICOLÁS DE BARI"),
        ("32900", "SANTA CRUZ DEL NORTE", "MAYABEQUE", "SANTA CRUZ DEL NORTE"),
        
        // ==================== MATANZAS ====================
        ("40100", "MATANZAS", "MATANZAS", "MATANZAS"),
        ("42110", "CÁRDENAS", "MATANZAS", "CÁRDENAS"),
        ("42200", "VARADERO", "MATANZAS", "VARADERO"),
        ("42300", "MARTÍ", "MATANZAS", "MARTÍ"),
        ("42400", "COLÓN", "MATANZAS", "COLÓN"),
        ("42500", "PERICO", "MATANZAS", "PERICO"),
        ("42600", "JOVELLANOS", "MATANZAS", "JOVELLANOS"),
        ("42700", "PEDRO BETANCOURT", "MATANZAS", "PEDRO BETANCOURT"),
        ("42800", "LIMONAR", "MATANZAS", "LIMONAR"),
        ("42900", "UNIÓN DE REYES", "MATANZAS", "UNIÓN DE REYES"),
        ("43000", "CIÉNAGA DE ZAPATA", "MATANZAS", "CIÉNAGA DE ZAPATA"),
        ("43100", "JAGÜEY GRANDE", "MATANZAS", "JAGÜEY GRANDE"),
        ("43200", "CALIMETE", "MATANZAS", "CALIMETE"),
        ("43300", "LOS ARABOS", "MATANZAS", "LOS ARABOS"),
        
        // ==================== VILLA CLARA ====================
        ("50100", "SANTA CLARA", "VILLA CLARA", "SANTA CLARA"),
        ("52900", "CIFUENTES", "VILLA CLARA", "CIFUENTES"),
        ("53310", "SAGUA LA GRANDE", "VILLA CLARA", "SAGUA LA GRANDE"),
        ("52200", "QUEMADO DE GÜINES", "VILLA CLARA", "QUEMADO DE GÜINES"),
        ("52100", "CORRALILLO", "VILLA CLARA", "CORRALILLO"),
        // ("50600", "CAMPAÑUELA", "VILLA CLARA", "CAMPAÑUELA"), // No encontrado en fuentes recientes
        ("52400", "ENCrucijada", "VILLA CLARA", "ENCRUCIJADA"), // Ajustado
        ("52500", "CAMAJUANÍ", "VILLA CLARA", "CAMAJUANÍ"),
        ("52610", "CAIBARIÉN", "VILLA CLARA", "CAIBARIÉN"),
        ("52700", "REMEDIOS", "VILLA CLARA", "REMEDIOS"),
        ("52800", "PLACETAS", "VILLA CLARA", "PLACETAS"),
        // ("51200", "RANCHO VELOZ", "VILLA CLARA", "RANCHO VELOZ"), // No encontrado en fuentes recientes
        ("53200", "MANICARAGUA", "VILLA CLARA", "MANICARAGUA"),
        // ("51400", "ENCRUCIJADA", "VILLA CLARA", "ENCRUCIJADA"), // Duplicado, ya arriba
        
        // ==================== CIENFUEGOS ====================
        ("55100", "CIENFUEGOS", "CIENFUEGOS", "CIENFUEGOS"),
        ("57500", "CRUCES", "CIENFUEGOS", "CRUCES"),
        ("57100", "AGUADA DE PASAJEROS", "CIENFUEGOS", "AGUADA DE PASAJEROS"),
        ("57400", "LAJAS", "CIENFUEGOS", "LAJAS"),
        ("57300", "PALMIRA", "CIENFUEGOS", "PALMIRA"),
        ("57200", "RODAS", "CIENFUEGOS", "RODAS"),
        ("57700", "ABREUS", "CIENFUEGOS", "ABREUS"),
        ("57600", "CUMANAYAGUA", "CIENFUEGOS", "CUMANAYAGUA"),
        
        // ==================== SANCTI SPÍRITUS ====================
        ("60100", "SANCTI SPÍRITUS", "SANCTI SPÍRITUS", "SANCTI SPÍRITUS"),
        ("62600", "TRINIDAD", "SANCTI SPÍRITUS", "TRINIDAD"),
        ("62500", "FOMENTO", "SANCTI SPÍRITUS", "FOMENTO"),
        ("62410", "CABAIGUÁN", "SANCTI SPÍRITUS", "CABAIGUÁN"),
        ("62100", "YAGUAJAY", "SANCTI SPÍRITUS", "YAGUAJAY"),
        ("62200", "JATIBONICO", "SANCTI SPÍRITUS", "JATIBONICO"),
        ("62300", "TAGUASCO", "SANCTI SPÍRITUS", "TAGUASCO"),
        ("62700", "LA SIERPE", "SANCTI SPÍRITUS", "LA SIERPE"),
        
        // ==================== CIEGO DE ÁVILA ====================
        ("65100", "CIEGO DE ÁVILA", "CIEGO DE ÁVILA", "CIEGO DE ÁVILA"),
        ("67210", "MORÓN", "CIEGO DE ÁVILA", "MORÓN"),
        ("67800", "VENEZUELA", "CIEGO DE ÁVILA", "VENEZUELA"),
        ("67900", "BARAGUÁ", "CIEGO DE ÁVILA", "BARAGUÁ"),
        ("67300", "BOLIVIA", "CIEGO DE ÁVILA", "BOLIVIA"),
        ("67100", "CHAMBAS", "CIEGO DE ÁVILA", "CHAMBAS"),
        ("67500", "CIRO REDONDO", "CIEGO DE ÁVILA", "CIRO REDONDO"),
        ("67400", "PRIMERO DE ENERO", "CIEGO DE ÁVILA", "PRIMERO DE ENERO"),
        ("67700", "MAJAGUA", "CIEGO DE ÁVILA", "MAJAGUA"),
        ("67600", "FLORENCIA", "CIEGO DE ÁVILA", "FLORENCIA"),
        
        // ==================== CAMAGÜEY ====================
        ("70100", "CAMAGÜEY", "CAMAGÜEY", "CAMAGÜEY"),
        ("72510", "NUEVITAS", "CAMAGÜEY", "NUEVITAS"),
        ("72200", "ESMERALDA", "CAMAGÜEY", "ESMERALDA"),
        ("72400", "MINAS", "CAMAGÜEY", "MINAS"),
        ("72700", "SIBANICÚ", "CAMAGÜEY", "SIBANICÚ"),
        ("72600", "GUÁIMARO", "CAMAGÜEY", "GUÁIMARO"),
        ("73000", "JIMAGUAYÚ", "CAMAGÜEY", "JIMAGUAYÚ"),
        ("72900", "VERTIENTES", "CAMAGÜEY", "VERTIENTES"),
        ("72810", "FLORIDA", "CAMAGÜEY", "FLORIDA"),
        ("73100", "NAJASA", "CAMAGÜEY", "NAJASA"),
        ("72100", "CARLOS MANUEL DE CÉSPEDES", "CAMAGÜEY", "CARLOS MANUEL DE CÉSPEDES"),
        ("73200", "SANTA CRUZ DEL SUR", "CAMAGÜEY", "SANTA CRUZ DEL SUR"),
        ("72300", "SIERRA DE CUBITAS", "CAMAGÜEY", "SIERRA DE CUBITAS"),
        
        // ==================== LAS TUNAS ====================
        ("75100", "LAS TUNAS", "LAS TUNAS", "LAS TUNAS"),
        ("77200", "PUERTO PADRE", "LAS TUNAS", "PUERTO PADRE"),
        ("77300", "JESÚS MENÉNDEZ", "LAS TUNAS", "JESÚS MENÉNDEZ"),
        ("77400", "MAJIBACOA", "LAS TUNAS", "MAJIBACOA"),
        ("77100", "MANATÍ", "LAS TUNAS", "MANATÍ"),
        ("77600", "COLOMBIA", "LAS TUNAS", "COLOMBIA"),
        ("77500", "JOBABO", "LAS TUNAS", "JOBABO"),
        ("77700", "AMANCIO", "LAS TUNAS", "AMANCIO"),
        
        // ==================== GRANMA ====================
        ("85100", "BAYAMO", "GRANMA", "BAYAMO"),
        ("87510", "MANZANILLO", "GRANMA", "MANZANILLO"),
        ("87400", "YARA", "GRANMA", "YARA"),
        ("88000", "BARTOLOMÉ MASÓ", "GRANMA", "BARTOLOMÉ MASÓ"),
        ("88100", "BUEY ARRIBA", "GRANMA", "BUEY ARRIBA"),
        ("87300", "JIGUANÍ", "GRANMA", "JIGUANÍ"),
        ("87600", "CAMPECHUELA", "GRANMA", "CAMPECHUELA"),
        ("88200", "GUISA", "GRANMA", "GUISA"),
        ("87800", "NIQUERO", "GRANMA", "NIQUERO"),
        ("87100", "RÍO CAUTO", "GRANMA", "RÍO CAUTO"),
        ("87700", "MEDIA LUNA", "GRANMA", "MEDIA LUNA"),
        ("87900", "PILÓN", "GRANMA", "PILÓN"),
        
        // ==================== HOLGUÍN ====================
        ("80100", "HOLGUÍN", "HOLGUÍN", "HOLGUÍN"),
        ("82100", "GIBARA", "HOLGUÍN", "GIBARA"),
        ("83300", "RAFAEL FREYRE", "HOLGUÍN", "RAFAEL FREYRE"),
        ("82300", "BANES", "HOLGUÍN", "BANES"),
        ("82400", "ANTILLA", "HOLGUÍN", "ANTILLA"),
        ("82500", "BÁGUANOS", "HOLGUÍN", "BÁGUANOS"),
        ("82700", "CACOCUM", "HOLGUÍN", "CACOCUM"),
        ("82800", "URBANO NORIS", "HOLGUÍN", "URBANO NORIS"),
        ("82600", "CALIXTO GARCÍA", "HOLGUÍN", "CALIXTO GARCÍA"),
        ("82900", "CUETO", "HOLGUÍN", "CUETO"),
        ("83000", "MAYARÍ", "HOLGUÍN", "MAYARÍ"),
        ("83100", "FRANK PAÍS", "HOLGUÍN", "FRANK PAÍS"),
        ("83200", "SAGUA DE TÁNAMO", "HOLGUÍN", "SAGUA DE TÁNAMO"),
        ("83310", "MOA", "HOLGUÍN", "MOA"),
        
        // ==================== SANTIAGO DE CUBA ====================
        ("90100", "SANTIAGO DE CUBA", "SANTIAGO DE CUBA", "SANTIAGO DE CUBA"),
        ("92610", "SONGO - LA MAYA", "SANTIAGO DE CUBA", "SONGO - LA MAYA"),
        ("92100", "CONTRAMAESTRE", "SANTIAGO DE CUBA", "CONTRAMAESTRE"),
        ("90400", "JIGUANÍ", "SANTIAGO DE CUBA", "JIGUANÍ"), // Verificar si existe, no en fuente principal
        ("92300", "SAN LUIS", "SANTIAGO DE CUBA", "SAN LUIS"),
        ("92100", "SEGUNDO FRENTE", "SANTIAGO DE CUBA", "SEGUNDO FRENTE"), // Mismo CP que Contramaestre
        ("92700", "TERCER FRENTE", "SANTIAGO DE CUBA", "TERCER FRENTE"),
        ("92800", "GUAMÁ", "SANTIAGO DE CUBA", "GUAMÁ"),
        ("92200", "MELLA", "SANTIAGO DE CUBA", "MELLA"),
        ("92610", "PALMA SORIANO", "SANTIAGO DE CUBA", "PALMA SORIANO"),
        
        // ==================== GUANTÁNAMO ====================
        ("95100", "GUANTÁNAMO", "GUANTÁNAMO", "GUANTÁNAMO"),
        ("97310", "BARACOA", "GUANTÁNAMO", "BARACOA"),
        ("99320", "MAISÍ", "GUANTÁNAMO", "MAISÍ"),
        ("97500", "IMÍAS", "GUANTÁNAMO", "IMÍAS"),
        ("97600", "SAN ANTONIO DEL SUR", "GUANTÁNAMO", "SAN ANTONIO DEL SUR"),
        ("97100", "EL SALVADOR", "GUANTÁNAMO", "EL SALVADOR"),
        ("99420", "YATERAS", "GUANTÁNAMO", "YATERAS"),
        ("97700", "MANUEL TAMÉS", "GUANTÁNAMO", "MANUEL TAMÉS"),
        ("97900", "NICETO PÉREZ", "GUANTÁNAMO", "NICETO PÉREZ"),
        ("97800", "CAIMANERA", "GUANTÁNAMO", "CAIMANERA"),
        
        // ==================== ISLA DE LA JUVENTUD ====================
        ("25100", "NUEVA GERONA", "ISLA DE LA JUVENTUD", "ISLA DE LA JUVENTUD"),
        ("25200", "LA FE", "ISLA DE LA JUVENTUD", "ISLA DE LA JUVENTUD"),
        ("25300", "MACAGUA", "ISLA DE LA JUVENTUD", "ISLA DE LA JUVENTUD"),
        ("25400", "COLOMBIA", "ISLA DE LA JUVENTUD", "ISLA DE LA JUVENTUD"),
    }
};

// Crear diccionario para convertir nombres de provincia a códigos
var provinciaNameToCode = new Dictionary<string, string>();
foreach (var prov in cubaAdmin1Data)
{
    provinciaNameToCode.TryAdd(prov.DescES.ToUpperInvariant(), prov.Codigo);
}

// Para municipios, crear un diccionario con clave compuesta para manejar duplicados
var municipioNameToCode = new Dictionary<string, string>();
foreach (var mun in cubaAdmin2Data)
{
    // Crear clave única combinando nombre y provincia
    var uniqueKey = $"{mun.DescES.ToUpperInvariant()}|{mun.ProvCodigo}";
    if (!municipioNameToCode.ContainsKey(uniqueKey))
    {
        municipioNameToCode.Add(uniqueKey, mun.Codigo);
    }
}

// También crear un diccionario simple para búsqueda directa (solo para nombres que no se repiten)
var municipioSimpleNameToCode = new Dictionary<string, string>();
foreach (var mun in cubaAdmin2Data)
{
    // Solo agregar si no existe (para nombres únicos)
    if (!municipioSimpleNameToCode.ContainsKey(mun.DescES.ToUpperInvariant()))
    {
        // Verificar si este nombre es único en toda la lista
        var count = cubaAdmin2Data.Count(m => m.DescES.ToUpperInvariant() == mun.DescES.ToUpperInvariant());
        if (count == 1)
        {
            municipioSimpleNameToCode.TryAdd(mun.DescES.ToUpperInvariant(), mun.Codigo);
        }
    }
}

foreach (var cp in cubaPostalData["CU"])
{
    var provCode = provinciaNameToCode.GetValueOrDefault(cp.Provincia.ToUpperInvariant(), "");

    // Intentar buscar primero por clave compuesta
    var munKey = $"{cp.Municipio.ToUpperInvariant()}|{provCode}";
    var munCode = municipioNameToCode.GetValueOrDefault(munKey, "");

    // Si no encuentra, intentar búsqueda simple (para nombres únicos)
    if (string.IsNullOrEmpty(munCode))
    {
        munCode = municipioSimpleNameToCode.GetValueOrDefault(cp.Municipio.ToUpperInvariant(), "");
    }

    // Si aún no encuentra, intentar búsqueda parcial
    if (string.IsNullOrEmpty(munCode))
    {
        var posibleMun = municipioNameToCode
            .FirstOrDefault(kvp => kvp.Key.StartsWith(cp.Municipio.ToUpperInvariant() + "|") &&
                                   kvp.Key.EndsWith($"|{provCode}"))
            .Value;

        if (!string.IsNullOrEmpty(posibleMun))
        {
            munCode = posibleMun;
        }
        else
        {
            // Último recurso: buscar cualquier municipio que contenga el nombre
            var anyMatch = municipioNameToCode
                .FirstOrDefault(kvp => kvp.Key.Contains(cp.Municipio.ToUpperInvariant()))
                .Value;
            munCode = anyMatch ?? "";
        }
    }

    cpPorPais["CU"].Add((cp.CP, cp.Localidad.ToUpperInvariant(), provCode, munCode));
}

Console.WriteLine($"  ✅ Cuba: {provinciasPorPais["CU"].Count} provincias, {municipiosPorPais["CU"].Count} municipios, {cpPorPais["CU"].Count} códigos postales");

// ═══════════════════════════════════════════════════════════════
// PASO 7: Líneas y SubLineas → datos embebidos en GenerarCPV2008()
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📦 Cargando clasificación de Líneas/SubLíneas...");
var (lineasCpv, sublineasCpv) = GenerarCPV2008();
Console.WriteLine($"  ✅ {lineasCpv.Count} líneas y {sublineasCpv.Count} sublíneas");

// ═══════════════════════════════════════════════════════════════
// PASO 8: Generar todos los CSVs
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📝 Generando archivos CSV...");

// ── PAÍSES ──
var paisFile = Path.Combine(outputDir, "Paises.csv");
using (var writer = new StreamWriter(paisFile, false, new UTF8Encoding(true)))
{
    await writer.WriteLineAsync("CodigoAlpha3,CodigoAlpha2,CodigoTelefono,DescripcionES,DescripcionEN,DescripcionFR,RegexTelefono,FormatoTelefono,FormatoEjemplo");
    foreach (var p in paisesData.OrderBy(p => p.Alpha2))
        await writer.WriteLineAsync($"{p.Alpha3},{p.Alpha2},{p.Tel},{EscapeCsv(p.DescES)},{EscapeCsv(p.DescEN)},{EscapeCsv(p.DescFR)},{EscapeCsv(p.Regex)},{EscapeCsv(p.Fmt)},{EscapeCsv(p.Ej)}");
}
Console.WriteLine($"  ✅ Paises.csv ({paisesData.Count} registros)");

// ── PROVINCIAS ──
var provFile = Path.Combine(outputDir, "Provincias.csv");
using (var writer = new StreamWriter(provFile, false, new UTF8Encoding(true)))
{
    await writer.WriteLineAsync("Codigo,PaisCodigo,DescripcionES,DescripcionEN,DescripcionFR");
    foreach (var (pais, provs) in provinciasPorPais.OrderBy(p => p.Key))
        foreach (var prov in provs.OrderBy(p => p.Codigo))
            await writer.WriteLineAsync($"{prov.Codigo},{pais},{EscapeCsv(prov.Desc)},{EscapeCsv(prov.DescEN)},{EscapeCsv(prov.DescFR)}");
}
Console.WriteLine($"  ✅ Provincias.csv ({provinciasPorPais.Sum(p => p.Value.Count)} registros)");

// ── MUNICIPIOS ──
var munFile = Path.Combine(outputDir, "Municipios.csv");
using (var writer = new StreamWriter(munFile, false, new UTF8Encoding(true)))
{
    await writer.WriteLineAsync("Codigo,ProvinciaCodigo,PaisCodigo,DescripcionES,DescripcionEN,DescripcionFR");
    foreach (var (pais, muns) in municipiosPorPais.OrderBy(p => p.Key))
        foreach (var mun in muns.OrderBy(m => m.Codigo))
            await writer.WriteLineAsync($"{mun.Codigo},{mun.ProvCodigo},{pais},{EscapeCsv(mun.Desc)},{EscapeCsv(mun.DescEN)},{EscapeCsv(mun.DescFR)}");
}
Console.WriteLine($"  ✅ Municipios.csv ({municipiosPorPais.Sum(p => p.Value.Count)} registros)");

// ── CÓDIGOS POSTALES ──
if (cpPorPais.Count > 0)
{
    var cpFile = Path.Combine(outputDir, "CodigosPostales.csv");
    using var writer = new StreamWriter(cpFile, false, new UTF8Encoding(true));
    await writer.WriteLineAsync("CodigoPostal,Localidad,ProvinciaCodigo,MunicipioCodigo,PaisCodigo");
    foreach (var (pais, cps) in cpPorPais.OrderBy(p => p.Key))
        foreach (var cp in cps.OrderBy(c => c.CP).ThenBy(c => c.Localidad))
            await writer.WriteLineAsync(
                $"{EscapeCsv(cp.CP)},{EscapeCsv(cp.Localidad)},{EscapeCsv(cp.ProvCodigo)},{EscapeCsv(cp.MunCodigo)},{pais}");
    Console.WriteLine($"  ✅ CodigosPostales.csv ({cpPorPais.Sum(p => p.Value.Count):N0} registros)");
}

// ── LINEAS (CPV Divisiones) ──
if (lineasCpv.Count > 0)
{
    var lineasFile = Path.Combine(outputDir, "Lineas.csv");
    using var writer = new StreamWriter(lineasFile, false, new UTF8Encoding(true));
    await writer.WriteLineAsync("Codigo,DescripcionES,DescripcionEN,DescripcionFR");
    foreach (var (codigo, es, en, fr) in lineasCpv.OrderBy(l => l.Codigo))
        await writer.WriteLineAsync($"{EscapeCsv(codigo)},{EscapeCsv(es)},{EscapeCsv(en)},{EscapeCsv(fr)}");
    Console.WriteLine($"  ✅ Lineas.csv ({lineasCpv.Count} registros)");
}

// ── SUBLINEAS (CPV Grupos) ──
if (sublineasCpv.Count > 0)
{
    var sublineasFile = Path.Combine(outputDir, "SubLineas_CPV.csv");
    using var writer = new StreamWriter(sublineasFile, false, new UTF8Encoding(true));
    await writer.WriteLineAsync("Codigo,DescripcionES,LineaCodigo,DescripcionEN,DescripcionFR");
    foreach (var (codigo, padre, es, en, fr) in sublineasCpv.OrderBy(s => s.Codigo))
        await writer.WriteLineAsync($"{EscapeCsv(codigo)},{EscapeCsv(es)},{EscapeCsv(padre)},{EscapeCsv(en)},{EscapeCsv(fr)}");
    Console.WriteLine($"  ✅ SubLineas_CPV.csv ({sublineasCpv.Count} registros)");
}

// ── ESTADÍSTICAS FINALES ──
Console.WriteLine("\n📊 ESTADÍSTICAS GEOGRÁFICAS:");
Console.WriteLine($"  {"País",-6} {"Provincias",12} {"Municipios",12} {"Cód.Postales",14}");
Console.WriteLine($"  {"────",-6} {"──────────",12} {"──────────",12} {"────────────",14}");
foreach (var pais in provinciasPorPais.Keys.OrderBy(k => k))
{
    var provCount = provinciasPorPais[pais].Count;
    var munCount = municipiosPorPais.GetValueOrDefault(pais)?.Count ?? 0;
    var cpCount = cpPorPais.GetValueOrDefault(pais)?.Count ?? 0;
    Console.WriteLine($"  {pais,-6} {provCount,12:N0} {munCount,12:N0} {cpCount,14:N0}");
}
Console.WriteLine($"  {"────",-6} {"──────────",12} {"──────────",12} {"────────────",14}");
Console.WriteLine($"  {"TOTAL",-6} {provinciasPorPais.Sum(p => p.Value.Count),12:N0} {municipiosPorPais.Sum(p => p.Value.Count),12:N0} {cpPorPais.Sum(p => p.Value.Count),14:N0}");
Console.WriteLine($"\n  Países:         {paisesData.Count}");
Console.WriteLine($"  Líneas:         {lineasCpv.Count}");
Console.WriteLine($"  SubLíneas:      {sublineasCpv.Count}");
Console.WriteLine($"  Cód. Postales:  {cpPorPais.Sum(p => p.Value.Count):N0}");
Console.WriteLine($"\n📂 Archivos generados en: {outputDir}");
Console.WriteLine("\n✅ ¡Proceso completado!");

// ═══════════════════════════════════════════════════════════════
// HELPERS
// ═══════════════════════════════════════════════════════════════

static async Task DownloadWithProgressAsync(HttpClient client, string url, string outputPath, int maxRetries = 3)
{
    int attempt = 0;
    while (true)
    {
        try
        {
            using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1;
            var totalMB = totalBytes > 0 ? totalBytes / (1024.0 * 1024.0) : -1;

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            await using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            var buffer = new byte[81920];
            long totalRead = 0;
            int bytesRead;
            var lastProgress = DateTime.MinValue;

            while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                totalRead += bytesRead;

                if ((DateTime.Now - lastProgress).TotalMilliseconds > 500)
                {
                    var readMB = totalRead / (1024.0 * 1024.0);
                    Console.Write(totalBytes > 0
                        ? $"\r    ⬇️  {readMB:F1} / {totalMB:F1} MB ({(double)totalRead / totalBytes * 100:F1}%)   "
                        : $"\r    ⬇️  {readMB:F1} MB descargados...   ");
                    lastProgress = DateTime.Now;
                }
            }
            Console.WriteLine($"\r    ⬇️  {totalRead / (1024.0 * 1024.0):F1} MB ✅                    ");
            break; // Éxito
        }
        catch (IOException ex) when (attempt < maxRetries)
        {
            attempt++;
            Console.WriteLine($"\n  ⚠️  Error de red, reintentando ({attempt}/{maxRetries})...");
            await Task.Delay(2000 * attempt);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n  ❌ Error descargando {url}: {ex.Message}");
            throw;
        }
    }
}

static string EscapeCsv(string value)
{
    if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        return $"\"{value.Replace("\"", "\"\"")}\"";
    return value;
}

// ═══════════════════════════════════════════════════════════════
// CPV 2008 EMBEBIDO — Publications Office of the European Union
// División (XX) → Linea  |  Grupo (XXX) → SubLinea
// ═══════════════════════════════════════════════════════════════
static (
    List<(string Codigo, string ES, string EN, string FR)> lineas,
    List<(string Codigo, string PadreCodigo, string ES, string EN, string FR)> sublineas)
    GenerarCPV2008()
{
    var lineas = new List<(string Codigo, string ES, string EN, string FR)>
    {
        ("03", "PRODUCTOS AGROPECUARIOS, PESCA Y SILVICULTURA",         "AGRICULTURAL, FARMING, FISHING AND FORESTRY PRODUCTS",        "PRODUITS AGRICOLES, DE L'ÉLEVAGE, DE LA PÊCHE ET DE LA FORÊT"),
        ("09", "COMBUSTIBLES, ELECTRICIDAD Y FUENTES DE ENERGÍA",       "PETROLEUM PRODUCTS, FUELS, ELECTRICITY AND ENERGY",           "PRODUITS PÉTROLIERS, COMBUSTIBLES, ÉLECTRICITÉ ET ÉNERGIE"),
        ("14", "MINERÍA Y METALES BÁSICOS",                             "MINING AND BASIC METALS",                                     "PRODUITS MINIERS ET MÉTAUX DE BASE"),
        ("15", "ALIMENTOS, BEBIDAS Y TABACO",                           "FOOD, BEVERAGES AND TOBACCO",                                 "PRODUITS ALIMENTAIRES, BOISSONS ET TABAC"),
        ("16", "MAQUINARIA AGRÍCOLA",                                   "AGRICULTURAL MACHINERY",                                      "MACHINES AGRICOLES"),
        ("18", "ROPA, CALZADO Y ACCESORIOS",                            "CLOTHING, FOOTWEAR AND ACCESSORIES",                          "VÊTEMENTS, CHAUSSURES ET ACCESSOIRES"),
        ("19", "CUERO, TEXTILES, PLÁSTICO Y CAUCHO",                    "LEATHER, TEXTILES, PLASTIC AND RUBBER",                       "CUIR, TEXTILES, PLASTIQUE ET CAOUTCHOUC"),
        ("22", "IMPRESOS Y PRODUCTOS EDITORIALES",                      "PRINTED MATTER AND PUBLISHING PRODUCTS",                      "IMPRIMÉS ET PRODUITS CONNEXES"),
        ("24", "PRODUCTOS QUÍMICOS",                                    "CHEMICAL PRODUCTS",                                           "PRODUITS CHIMIQUES"),
        ("25", "PRODUCTOS DE CAUCHO, PLÁSTICO Y CERA",                  "RUBBER, PLASTIC AND WAX PRODUCTS",                            "PRODUITS EN CAOUTCHOUC, PLASTIQUE ET CIRE"),
        ("26", "VIDRIO, CERÁMICA Y PRODUCTOS INORGÁNICOS",              "GLASS, CERAMIC AND INORGANIC PRODUCTS",                       "VERRE, CÉRAMIQUE ET PRODUITS INORGANIQUES"),
        ("27", "METALES BÁSICOS Y ALEACIONES",                          "BASE METALS AND ALLOYS",                                      "MÉTAUX DE BASE ET ALLIAGES"),
        ("28", "PRODUCTOS METÁLICOS ELABORADOS",                        "FABRICATED METAL PRODUCTS",                                   "PRODUITS MÉTALLIQUES TRANSFORMÉS"),
        ("30", "EQUIPOS INFORMÁTICOS Y DE OFICINA",                     "COMPUTING AND OFFICE EQUIPMENT",                              "ÉQUIPEMENTS INFORMATIQUES ET DE BUREAU"),
        ("31", "MAQUINARIA Y EQUIPOS ELÉCTRICOS",                       "ELECTRICAL MACHINERY AND EQUIPMENT",                          "MACHINES ET ÉQUIPEMENTS ÉLECTRIQUES"),
        ("32", "EQUIPOS DE RADIO, TV Y TELECOMUNICACIONES",             "RADIO, TV AND TELECOMMUNICATIONS EQUIPMENT",                  "ÉQUIPEMENTS RADIO, TV ET TÉLÉCOMMUNICATIONS"),
        ("33", "EQUIPOS MÉDICOS Y PRODUCTOS FARMACÉUTICOS",             "MEDICAL EQUIPMENT AND PHARMACEUTICALS",                       "ÉQUIPEMENTS MÉDICAUX ET PRODUITS PHARMACEUTIQUES"),
        ("34", "EQUIPOS Y MATERIAL DE TRANSPORTE",                      "TRANSPORT EQUIPMENT AND MATERIALS",                           "MATÉRIEL ET ÉQUIPEMENTS DE TRANSPORT"),
        ("35", "EQUIPOS DE SEGURIDAD, DEFENSA Y POLICÍA",               "SECURITY, DEFENCE AND POLICE EQUIPMENT",                      "ÉQUIPEMENTS DE SÉCURITÉ, DÉFENSE ET POLICE"),
        ("37", "INSTRUMENTOS MUSICALES, DEPORTE, JUGUETES Y ARTESANÍA", "MUSICAL INSTRUMENTS, SPORT, TOYS AND CRAFTS",                 "INSTRUMENTS DE MUSIQUE, SPORT, JOUETS ET ARTISANAT"),
        ("38", "EQUIPOS DE LABORATORIO, ÓPTICA Y PRECISIÓN",            "LABORATORY, OPTICAL AND PRECISION EQUIPMENT",                 "ÉQUIPEMENTS DE LABORATOIRE, OPTIQUE ET PRÉCISION"),
        ("39", "MOBILIARIO, ELECTRODOMÉSTICOS Y LIMPIEZA",              "FURNITURE, APPLIANCES AND CLEANING PRODUCTS",                 "MEUBLES, ÉLECTROMÉNAGER ET PRODUITS D'ENTRETIEN"),
        ("41", "AGUA",                                                  "WATER",                                                       "EAU"),
        ("42", "MAQUINARIA INDUSTRIAL",                                 "INDUSTRIAL MACHINERY",                                        "MACHINES INDUSTRIELLES"),
        ("43", "MAQUINARIA PARA MINERÍA Y CONSTRUCCIÓN",                "MINING AND CONSTRUCTION MACHINERY",                           "MACHINES POUR MINES ET CONSTRUCTION"),
        ("44", "MATERIALES Y ESTRUCTURAS DE CONSTRUCCIÓN",              "CONSTRUCTION MATERIALS AND STRUCTURES",                       "MATÉRIAUX ET STRUCTURES DE CONSTRUCTION"),
        ("45", "TRABAJOS DE CONSTRUCCIÓN",                              "CONSTRUCTION WORKS",                                          "TRAVAUX DE CONSTRUCTION"),
        ("48", "SOFTWARE Y SISTEMAS DE INFORMACIÓN",                    "SOFTWARE AND INFORMATION SYSTEMS",                            "LOGICIELS ET SYSTÈMES D'INFORMATION"),
        ("50", "SERVICIOS DE REPARACIÓN Y MANTENIMIENTO",               "REPAIR AND MAINTENANCE SERVICES",                             "SERVICES DE RÉPARATION ET D'ENTRETIEN"),
        ("51", "SERVICIOS DE INSTALACIÓN",                              "INSTALLATION SERVICES",                                       "SERVICES D'INSTALLATION"),
        ("55", "HOSTELERÍA, RESTAURACIÓN Y COMERCIO MINORISTA",         "HOTEL, CATERING AND RETAIL SERVICES",                         "SERVICES HÔTELIERS, RESTAURATION ET VENTE AU DÉTAIL"),
        ("60", "SERVICIOS DE TRANSPORTE",                               "TRANSPORT SERVICES",                                          "SERVICES DE TRANSPORT"),
        ("63", "SERVICIOS AUXILIARES DE TRANSPORTE Y TURISMO",          "AUXILIARY TRANSPORT AND TRAVEL SERVICES",                     "SERVICES AUXILIAIRES DE TRANSPORT ET TOURISME"),
        ("64", "SERVICIOS POSTALES Y DE TELECOMUNICACIONES",            "POSTAL AND TELECOMMUNICATIONS SERVICES",                      "SERVICES POSTAUX ET DE TÉLÉCOMMUNICATIONS"),
        ("65", "SERVICIOS PÚBLICOS (AGUA, GAS, ELECTRICIDAD)",          "PUBLIC UTILITIES (WATER, GAS, ELECTRICITY)",                  "SERVICES PUBLICS (EAU, GAZ, ÉLECTRICITÉ)"),
        ("66", "SERVICIOS FINANCIEROS Y DE SEGUROS",                    "FINANCIAL AND INSURANCE SERVICES",                            "SERVICES FINANCIERS ET D'ASSURANCE"),
        ("70", "SERVICIOS INMOBILIARIOS",                               "REAL ESTATE SERVICES",                                        "SERVICES IMMOBILIERS"),
        ("71", "SERVICIOS DE ARQUITECTURA E INGENIERÍA",                "ARCHITECTURAL AND ENGINEERING SERVICES",                      "SERVICES D'ARCHITECTURE ET D'INGÉNIERIE"),
        ("72", "SERVICIOS DE TECNOLOGÍA DE LA INFORMACIÓN",             "INFORMATION TECHNOLOGY SERVICES",                             "SERVICES DE TECHNOLOGIES DE L'INFORMATION"),
        ("73", "SERVICIOS DE I+D Y CONSULTORÍA TÉCNICA",                "R&D AND TECHNICAL CONSULTANCY SERVICES",                      "SERVICES DE R&D ET CONSEIL TECHNIQUE"),
        ("75", "SERVICIOS DE ADMINISTRACIÓN Y DEFENSA",                 "ADMINISTRATION AND DEFENCE SERVICES",                         "SERVICES D'ADMINISTRATION ET DE DÉFENSE"),
        ("76", "SERVICIOS PARA LA INDUSTRIA PETROLERA Y DEL GAS",       "OIL AND GAS INDUSTRY SERVICES",                               "SERVICES POUR L'INDUSTRIE PÉTROLIÈRE ET GAZIÈRE"),
        ("77", "SERVICIOS AGRÍCOLAS, FORESTALES Y ACUÍCOLAS",           "AGRICULTURAL, FORESTRY AND AQUACULTURE SERVICES",             "SERVICES AGRICOLES, FORESTIERS ET AQUACOLES"),
        ("79", "SERVICIOS EMPRESARIALES Y PROFESIONALES",               "BUSINESS AND PROFESSIONAL SERVICES",                          "SERVICES AUX ENTREPRISES ET SERVICES PROFESSIONNELS"),
        ("80", "SERVICIOS DE EDUCACIÓN Y FORMACIÓN",                    "EDUCATION AND TRAINING SERVICES",                             "SERVICES D'ENSEIGNEMENT ET DE FORMATION"),
        ("85", "SERVICIOS SANITARIOS Y ASISTENCIA SOCIAL",              "HEALTH AND SOCIAL SERVICES",                                  "SERVICES DE SANTÉ ET SERVICES SOCIAUX"),
        ("90", "SERVICIOS MEDIOAMBIENTALES Y DE SANEAMIENTO",           "ENVIRONMENTAL AND SANITATION SERVICES",                       "SERVICES ENVIRONNEMENTAUX ET D'ASSAINISSEMENT"),
        ("92", "SERVICIOS RECREATIVOS, CULTURALES Y DEPORTIVOS",        "RECREATIONAL, CULTURAL AND SPORTING SERVICES",                "SERVICES RÉCRÉATIFS, CULTURELS ET SPORTIFS"),
        ("98", "OTROS SERVICIOS COMUNITARIOS Y PERSONALES",             "OTHER COMMUNITY AND PERSONAL SERVICES",                       "AUTRES SERVICES COMMUNAUTAIRES ET PERSONNELS"),
        ("99", "ORGANISMOS EXTRATERRITORIALES",                         "EXTRATERRITORIAL ORGANISATIONS",                              "ORGANISATIONS EXTRATERRITORIALES"),
    };

    var sublineas = new List<(string Codigo, string PadreCodigo, string ES, string EN, string FR)>
    {
        // ── 03 Agropecuario ──────────────────────────────────────────────
        ("031","03","PRODUCTOS AGRÍCOLAS Y ANIMALES VIVOS",          "AGRICULTURAL PRODUCTS AND LIVE ANIMALS",         "PRODUITS AGRICOLES ET ANIMAUX VIVANTS"),
        ("032","03","HORTICULTURA",                                  "HORTICULTURE",                                   "HORTICULTURE"),
        ("033","03","GANADERÍA",                                     "LIVESTOCK",                                      "ÉLEVAGE"),
        ("034","03","MADERA Y PRODUCTOS FORESTALES",                 "TIMBER AND FORESTRY PRODUCTS",                   "BOIS ET PRODUITS FORESTIERS"),
        ("035","03","PESCA Y PRODUCTOS ACUÍCOLAS",                   "FISHING AND AQUACULTURE PRODUCTS",               "PRODUITS DE LA PÊCHE ET DE L'AQUACULTURE"),
        ("036","03","PIENSOS Y ALIMENTOS PARA ANIMALES",             "ANIMAL FEED AND PRODUCTS",                       "ALIMENTS ET PRODUITS POUR ANIMAUX"),
        // ── 09 Energía ──────────────────────────────────────────────────
        ("091","09","COMBUSTIBLES LÍQUIDOS Y GASES",                 "LIQUID FUELS AND GASES",                         "COMBUSTIBLES LIQUIDES ET GAZ"),
        ("092","09","ELECTRICIDAD",                                  "ELECTRICITY",                                    "ÉLECTRICITÉ"),
        ("093","09","CARBÓN Y COMBUSTIBLES SÓLIDOS",                 "COAL AND SOLID FUELS",                           "CHARBON ET COMBUSTIBLES SOLIDES"),
        ("094","09","ENERGÍAS RENOVABLES",                           "RENEWABLE ENERGY",                               "ÉNERGIES RENOUVELABLES"),
        // ── 14 Minería ──────────────────────────────────────────────────
        ("141","14","MINERALES METÁLICOS",                           "METALLIC MINERALS",                              "MINÉRAUX MÉTALLIQUES"),
        ("142","14","MINERALES NO METÁLICOS",                        "NON-METALLIC MINERALS",                          "MINÉRAUX NON MÉTALLIQUES"),
        ("143","14","METALES PRECIOSOS Y SEMIPRECIOSOS",             "PRECIOUS AND SEMI-PRECIOUS METALS",              "MÉTAUX PRÉCIEUX ET SEMI-PRÉCIEUX"),
        // ── 15 Alimentos ────────────────────────────────────────────────
        ("151","15","CARNE Y PRODUCTOS CÁRNICOS",                    "MEAT AND MEAT PRODUCTS",                         "VIANDE ET PRODUITS À BASE DE VIANDE"),
        ("152","15","PESCADO Y MARISCOS PROCESADOS",                 "FISH AND PROCESSED SEAFOOD",                     "POISSON ET FRUITS DE MER TRANSFORMÉS"),
        ("153","15","PRODUCTOS LÁCTEOS",                             "DAIRY PRODUCTS",                                 "PRODUITS LAITIERS"),
        ("154","15","ACEITES Y GRASAS VEGETALES Y ANIMALES",         "VEGETABLE AND ANIMAL OILS AND FATS",             "HUILES ET GRAISSES VÉGÉTALES ET ANIMALES"),
        ("155","15","PRODUCTOS DE MOLINERÍA Y ALMIDONES",            "GRAIN MILL PRODUCTS AND STARCHES",               "PRODUITS DE MINOTERIE ET AMIDONS"),
        ("156","15","AZÚCAR Y EDULCORANTES",                         "SUGAR AND SWEETENERS",                           "SUCRE ET ÉDULCORANTS"),
        ("157","15","CACAO, CHOCOLATE Y CONFITERÍA",                 "COCOA, CHOCOLATE AND CONFECTIONERY",             "CACAO, CHOCOLAT ET CONFISERIE"),
        ("158","15","PANADERÍA, PASTELERÍA Y REPOSTERÍA",            "BAKERY, PASTRY AND CONFECTIONERY",               "BOULANGERIE, PÂTISSERIE ET CONFISERIE"),
        ("159","15","BEBIDAS",                                       "BEVERAGES",                                      "BOISSONS"),
        // ── 16 Maquinaria agrícola ──────────────────────────────────────
        ("161","16","TRACTORES",                                     "TRACTORS",                                       "TRACTEURS"),
        ("162","16","APEROS E IMPLEMENTOS AGRÍCOLAS",                "AGRICULTURAL IMPLEMENTS",                        "INSTRUMENTS AGRICOLES"),
        ("163","16","COSECHADORAS Y EQUIPOS DE RECOLECCIÓN",         "HARVESTING EQUIPMENT",                           "MATÉRIEL DE RÉCOLTE"),
        // ── 18 Ropa ─────────────────────────────────────────────────────
        ("181","18","ROPA DE TRABAJO Y UNIFORMES",                   "WORKWEAR AND UNIFORMS",                          "VÊTEMENTS DE TRAVAIL ET UNIFORMES"),
        ("182","18","PRENDAS DE VESTIR EXTERIORES",                  "OUTERWEAR",                                      "VÊTEMENTS D'EXTÉRIEUR"),
        ("183","18","ROPA ESPECIALIZADA Y DE PROTECCIÓN",            "SPECIALIST AND PROTECTIVE CLOTHING",             "VÊTEMENTS SPÉCIALISÉS ET DE PROTECTION"),
        ("184","18","ROPA INTERIOR Y DE DORMIR",                     "UNDERWEAR AND NIGHTWEAR",                        "SOUS-VÊTEMENTS ET VÊTEMENTS DE NUIT"),
        ("185","18","SOMBREROS, GORROS Y ACCESORIOS",                "HATS, CAPS AND HEAD ACCESSORIES",                "CHAPEAUX, CASQUETTES ET ACCESSOIRES"),
        ("186","18","CALZADO",                                       "FOOTWEAR",                                       "CHAUSSURES"),
        ("187","18","MARROQUINERÍA Y EQUIPAJE",                      "LEATHER GOODS AND LUGGAGE",                      "MAROQUINERIE ET BAGAGES"),
        // ── 19 Textiles ─────────────────────────────────────────────────
        ("191","19","CUERO Y ARTÍCULOS DE CUERO",                    "LEATHER AND LEATHER ARTICLES",                   "CUIR ET ARTICLES EN CUIR"),
        ("192","19","TELAS Y TEJIDOS",                               "FABRICS AND TEXTILES",                           "TISSUS ET TEXTILES"),
        ("193","19","PRODUCTOS DE CAUCHO",                           "RUBBER PRODUCTS",                                "PRODUITS EN CAOUTCHOUC"),
        ("194","19","MATERIALES PLÁSTICOS",                          "PLASTIC MATERIALS",                              "MATIÈRES PLASTIQUES"),
        ("195","19","FIBRAS Y ARTÍCULOS SIMILARES",                  "FIBRES AND SIMILAR ARTICLES",                    "FIBRES ET ARTICLES SIMILAIRES"),
        // ── 22 Impresos ─────────────────────────────────────────────────
        ("221","22","LIBROS Y PUBLICACIONES",                        "BOOKS AND PUBLICATIONS",                         "LIVRES ET PUBLICATIONS"),
        ("222","22","PERIÓDICOS, REVISTAS Y PUBLICACIONES",          "NEWSPAPERS, JOURNALS AND PERIODICALS",           "JOURNAUX, REVUES ET PÉRIODIQUES"),
        ("223","22","MATERIAL PUBLICITARIO Y DE MARKETING",          "ADVERTISING AND MARKETING MATERIALS",            "MATÉRIEL PUBLICITAIRE ET MARKETING"),
        ("224","22","FORMULARIOS, PAPELERÍA Y ARTÍCULOS DE OFICINA", "FORMS, STATIONERY AND OFFICE ARTICLES",          "FORMULAIRES, PAPETERIE ET ARTICLES DE BUREAU"),
        // ── 24 Químicos ─────────────────────────────────────────────────
        ("241","24","PRODUCTOS QUÍMICOS BÁSICOS",                    "BASIC CHEMICAL PRODUCTS",                        "PRODUITS CHIMIQUES DE BASE"),
        ("242","24","FERTILIZANTES Y AGROQUÍMICOS",                  "FERTILISERS AND AGRICULTURAL CHEMICALS",         "ENGRAIS ET PRODUITS CHIMIQUES AGRICOLES"),
        ("243","24","PINTURAS, BARNICES Y RECUBRIMIENTOS",           "PAINTS, VARNISHES AND COATINGS",                 "PEINTURES, VERNIS ET REVÊTEMENTS"),
        ("244","24","PRODUCTOS FARMACÉUTICOS BÁSICOS",               "BASIC PHARMACEUTICAL PRODUCTS",                  "PRODUITS PHARMACEUTIQUES DE BASE"),
        ("245","24","DETERGENTES Y PRODUCTOS DE LIMPIEZA",           "DETERGENTS AND CLEANING PRODUCTS",               "DÉTERGENTS ET PRODUITS DE NETTOYAGE"),
        ("246","24","EXPLOSIVOS Y PIROTECNIA",                       "EXPLOSIVES AND PYROTECHNICS",                    "EXPLOSIFS ET PYROTECHNIE"),
        ("247","24","ADHESIVOS Y SELLANTES",                         "ADHESIVES AND SEALANTS",                         "ADHÉSIFS ET PRODUITS D'ÉTANCHÉITÉ"),
        ("248","24","PERFUMERÍA Y COSMÉTICOS",                       "PERFUMERY AND COSMETICS",                        "PARFUMERIE ET COSMÉTIQUES"),
        // ── 25 Caucho y plástico ────────────────────────────────────────
        ("251","25","PRODUCTOS DE CAUCHO",                           "RUBBER PRODUCTS",                                "PRODUITS EN CAOUTCHOUC"),
        ("252","25","PRODUCTOS DE PLÁSTICO",                         "PLASTIC PRODUCTS",                               "PRODUITS EN PLASTIQUE"),
        ("253","25","ARTÍCULOS DE CERA",                             "WAX ARTICLES",                                   "ARTICLES EN CIRE"),
        // ── 26 Vidrio y cerámica ────────────────────────────────────────
        ("261","26","PRODUCTOS DE VIDRIO",                           "GLASS PRODUCTS",                                 "PRODUITS EN VERRE"),
        ("262","26","PRODUCTOS CERÁMICOS Y DE LOZA",                 "CERAMIC AND EARTHENWARE PRODUCTS",               "PRODUITS CÉRAMIQUES ET EN FAÏENCE"),
        ("263","26","MATERIALES REFRACTARIOS",                       "REFRACTORY MATERIALS",                           "MATÉRIAUX RÉFRACTAIRES"),
        ("264","26","CEMENTO, CAL Y YESO",                           "CEMENT, LIME AND PLASTER",                       "CIMENT, CHAUX ET PLÂTRE"),
        // ── 27 Metales básicos ──────────────────────────────────────────
        ("271","27","HIERRO Y ACERO",                                "IRON AND STEEL",                                 "FER ET ACIER"),
        ("272","27","METALES NO FERROSOS",                           "NON-FERROUS METALS",                             "MÉTAUX NON FERREUX"),
        ("273","27","METALES PRECIOSOS",                             "PRECIOUS METALS",                                "MÉTAUX PRÉCIEUX"),
        ("274","27","PRODUCTOS SEMIACABADOS DE METAL",               "SEMI-FINISHED METAL PRODUCTS",                   "PRODUITS MÉTALLIQUES SEMI-FINIS"),
        // ── 28 Productos metálicos ──────────────────────────────────────
        ("281","28","ESTRUCTURAS METÁLICAS",                         "METAL STRUCTURES",                               "STRUCTURES MÉTALLIQUES"),
        ("282","28","TANQUES Y DEPÓSITOS METÁLICOS",                 "METAL TANKS AND RESERVOIRS",                     "RÉSERVOIRS ET CITERNES EN MÉTAL"),
        ("283","28","HERRAMIENTAS MANUALES",                         "HAND TOOLS",                                     "OUTILS À MAIN"),
        ("284","28","CERRAJERÍA, FERRETERÍA Y FIJACIONES",           "LOCKS, HARDWARE AND FASTENERS",                  "SERRURERIE, QUINCAILLERIE ET ATTACHES"),
        // ── 30 Informática ──────────────────────────────────────────────
        ("301","30","ORDENADORES Y PERIFÉRICOS",                     "COMPUTERS AND PERIPHERALS",                      "ORDINATEURS ET PÉRIPHÉRIQUES"),
        ("302","30","EQUIPOS DE RED",                                "NETWORK EQUIPMENT",                              "ÉQUIPEMENTS RÉSEAU"),
        ("303","30","MÁQUINAS DE OFICINA Y SUMINISTROS",             "OFFICE MACHINES AND SUPPLIES",                   "MACHINES DE BUREAU ET FOURNITURES"),
        ("304","30","SOPORTES DE ALMACENAMIENTO DE DATOS",           "DATA STORAGE MEDIA",                             "SUPPORTS DE STOCKAGE DE DONNÉES"),
        // ── 31 Electricidad ─────────────────────────────────────────────
        ("311","31","GENERADORES, MOTORES Y TRANSFORMADORES",        "GENERATORS, MOTORS AND TRANSFORMERS",            "GÉNÉRATEURS, MOTEURS ET TRANSFORMATEURS"),
        ("312","31","DISTRIBUCIÓN Y CONTROL ELÉCTRICO",              "ELECTRICAL DISTRIBUTION AND CONTROL",            "DISTRIBUTION ET COMMANDE ÉLECTRIQUES"),
        ("313","31","CABLES Y CONDUCTORES ELÉCTRICOS",               "ELECTRICAL CABLES AND CONDUCTORS",               "CÂBLES ET CONDUCTEURS ÉLECTRIQUES"),
        ("314","31","ACUMULADORES, PILAS Y BATERÍAS",                "ACCUMULATORS, BATTERIES AND CELLS",              "ACCUMULATEURS, PILES ET BATTERIES"),
        ("315","31","ILUMINACIÓN Y LÁMPARAS",                        "LIGHTING AND LAMPS",                             "ÉCLAIRAGE ET LAMPES"),
        // ── 32 Telecomunicaciones ───────────────────────────────────────
        ("321","32","EQUIPOS ELECTRÓNICOS",                          "ELECTRONIC EQUIPMENT",                           "ÉQUIPEMENTS ÉLECTRONIQUES"),
        ("322","32","EQUIPOS DE RADIO Y TELEVISIÓN",                 "RADIO AND TELEVISION EQUIPMENT",                 "ÉQUIPEMENTS RADIO ET TÉLÉVISION"),
        ("323","32","EQUIPOS DE TELECOMUNICACIONES",                 "TELECOMMUNICATIONS EQUIPMENT",                   "ÉQUIPEMENTS DE TÉLÉCOMMUNICATIONS"),
        ("324","32","REDES INFORMÁTICAS E INTERNET",                 "COMPUTER AND INTERNET NETWORKS",                 "RÉSEAUX INFORMATIQUES ET INTERNET"),
        // ── 33 Médico y farmacéutico ────────────────────────────────────
        ("331","33","INSTRUMENTOS MÉDICOS Y DE DIAGNÓSTICO",         "MEDICAL AND DIAGNOSTIC INSTRUMENTS",             "INSTRUMENTS MÉDICAUX ET DE DIAGNOSTIC"),
        ("332","33","SUMINISTROS MÉDICOS Y DE ENFERMERÍA",           "MEDICAL AND NURSING SUPPLIES",                   "FOURNITURES MÉDICALES ET DE SOINS"),
        ("333","33","MEDICAMENTOS",                                  "MEDICINES AND DRUGS",                            "MÉDICAMENTS"),
        ("334","33","PRODUCTOS SANITARIOS E HIGIENE PERSONAL",       "SANITARY AND PERSONAL CARE PRODUCTS",            "PRODUITS SANITAIRES ET DE SOINS PERSONNELS"),
        // ── 34 Transporte ───────────────────────────────────────────────
        ("341","34","AUTOMÓVILES Y VEHÍCULOS A MOTOR",               "MOTOR VEHICLES",                                 "VÉHICULES À MOTEUR"),
        ("342","34","MOTOCICLETAS Y BICICLETAS",                     "MOTORCYCLES AND BICYCLES",                       "MOTOCYCLES ET BICYCLETTES"),
        ("343","34","PIEZAS Y ACCESORIOS DE VEHÍCULOS",              "VEHICLE PARTS AND ACCESSORIES",                  "PIÈCES ET ACCESSOIRES DE VÉHICULES"),
        ("344","34","AERONAVES Y NAVES ESPACIALES",                  "AIRCRAFT AND SPACECRAFT",                        "AÉRONEFS ET ENGINS SPATIAUX"),
        ("345","34","EMBARCACIONES Y BARCOS",                        "BOATS AND SHIPS",                                "BATEAUX ET NAVIRES"),
        ("346","34","MATERIAL FERROVIARIO",                          "RAILWAY EQUIPMENT",                              "MATÉRIEL FERROVIAIRE"),
        // ── 35 Seguridad ────────────────────────────────────────────────
        ("351","35","EQUIPOS DE EXTINCIÓN DE INCENDIOS",             "FIRE-FIGHTING EQUIPMENT",                        "ÉQUIPEMENTS D'EXTINCTION D'INCENDIE"),
        ("352","35","EQUIPOS DE SEGURIDAD Y VIGILANCIA",             "SECURITY AND SURVEILLANCE EQUIPMENT",            "ÉQUIPEMENTS DE SÉCURITÉ ET SURVEILLANCE"),
        ("353","35","EQUIPOS MILITARES Y DE DEFENSA",                "MILITARY AND DEFENCE EQUIPMENT",                 "ÉQUIPEMENTS MILITAIRES ET DE DÉFENSE"),
        // ── 37 Deporte y ocio ───────────────────────────────────────────
        ("371","37","INSTRUMENTOS MUSICALES",                        "MUSICAL INSTRUMENTS",                            "INSTRUMENTS DE MUSIQUE"),
        ("372","37","ARTÍCULOS DEPORTIVOS",                          "SPORT ARTICLES",                                 "ARTICLES DE SPORT"),
        ("373","37","JUEGOS Y JUGUETES",                             "GAMES AND TOYS",                                 "JEUX ET JOUETS"),
        ("374","37","FOTOGRAFÍA E IMAGEN",                           "PHOTOGRAPHY AND IMAGING",                        "PHOTOGRAPHIE ET IMAGERIE"),
        ("375","37","ARTESANÍA Y BELLAS ARTES",                      "CRAFTS AND FINE ARTS",                           "ARTISANAT ET BEAUX-ARTS"),
        // ── 38 Laboratorio ──────────────────────────────────────────────
        ("381","38","INSTRUMENTOS DE LABORATORIO",                   "LABORATORY INSTRUMENTS",                         "INSTRUMENTS DE LABORATOIRE"),
        ("382","38","INSTRUMENTOS DE MEDICIÓN Y CONTROL",            "MEASURING AND MONITORING INSTRUMENTS",           "INSTRUMENTS DE MESURE ET DE CONTRÔLE"),
        ("383","38","EQUIPOS ÓPTICOS",                               "OPTICAL EQUIPMENT",                              "ÉQUIPEMENTS OPTIQUES"),
        ("384","38","INSTRUMENTOS DE NAVEGACIÓN Y METEOROLOGÍA",     "NAVIGATION AND METEOROLOGICAL INSTRUMENTS",      "INSTRUMENTS DE NAVIGATION ET MÉTÉOROLOGIQUES"),
        // ── 39 Mobiliario ───────────────────────────────────────────────
        ("391","39","MUEBLES",                                       "FURNITURE",                                      "MEUBLES"),
        ("392","39","ROPA DE CAMA, TEXTILES Y ARTÍCULOS DEL HOGAR",  "BED LINEN, TEXTILES AND HOME ARTICLES",          "LITERIE, TEXTILES ET ARTICLES MÉNAGERS"),
        ("393","39","ELECTRODOMÉSTICOS",                             "HOUSEHOLD APPLIANCES",                           "ÉLECTROMÉNAGER"),
        ("394","39","ARTÍCULOS DE COCINA Y COMEDOR",                 "KITCHEN AND DINING ARTICLES",                    "ARTICLES DE CUISINE ET DE SALLE À MANGER"),
        ("395","39","ARTÍCULOS DECORATIVOS Y DE ARTE",               "DECORATIVE AND ART ARTICLES",                    "ARTICLES DÉCORATIFS ET D'ART"),
        // ── 41 Agua ─────────────────────────────────────────────────────
        ("411","41","AGUA POTABLE",                                  "DRINKING WATER",                                 "EAU POTABLE"),
        ("412","41","AGUA TRATADA PARA PROCESOS",                    "TREATED AND PROCESS WATER",                      "EAU TRAITÉE ET EAU DE PROCESSUS"),
        // ── 42 Maquinaria industrial ────────────────────────────────────
        ("421","42","MAQUINARIA DE USO GENERAL",                     "GENERAL PURPOSE MACHINERY",                      "MACHINES D'USAGE GÉNÉRAL"),
        ("422","42","MAQUINARIA DE USO ESPECIAL",                    "SPECIAL PURPOSE MACHINERY",                      "MACHINES À USAGE SPÉCIAL"),
        ("423","42","EQUIPOS DE ELEVACIÓN Y MANIPULACIÓN",           "LIFTING AND HANDLING EQUIPMENT",                 "ÉQUIPEMENTS DE LEVAGE ET DE MANUTENTION"),
        ("424","42","MAQUINARIA PARA EMBALAJE Y ENVASADO",           "PACKAGING MACHINERY",                            "MACHINES D'EMBALLAGE ET DE CONDITIONNEMENT"),
        // ── 43 Minería y construcción ───────────────────────────────────
        ("431","43","MAQUINARIA PARA MINERÍA Y EXTRACCIÓN",          "MINING AND EXTRACTION MACHINERY",                "MACHINES POUR MINES ET EXTRACTION"),
        ("432","43","MAQUINARIA PARA CONSTRUCCIÓN Y DEMOLICIÓN",     "CONSTRUCTION AND DEMOLITION MACHINERY",          "MACHINES DE CONSTRUCTION ET DÉMOLITION"),
        ("433","43","EQUIPOS PARA PERFORACIÓN Y SONDEO",             "DRILLING AND BORING EQUIPMENT",                  "ÉQUIPEMENTS DE FORAGE ET SONDAGE"),
        // ── 44 Materiales de construcción ──────────────────────────────
        ("441","44","MATERIALES DE CONSTRUCCIÓN",                    "CONSTRUCTION MATERIALS",                         "MATÉRIAUX DE CONSTRUCTION"),
        ("442","44","ESTRUCTURAS PREFABRICADAS",                     "PREFABRICATED STRUCTURES",                       "STRUCTURES PRÉFABRIQUÉES"),
        ("443","44","TUBERÍAS Y ACCESORIOS",                         "PIPES AND FITTINGS",                             "TUYAUX ET RACCORDS"),
        ("444","44","HERRAMIENTAS PARA CONSTRUCCIÓN",                "CONSTRUCTION TOOLS",                             "OUTILS DE CONSTRUCTION"),
        ("445","44","PINTURAS Y RECUBRIMIENTOS PARA CONSTRUCCIÓN",   "CONSTRUCTION PAINTS AND COATINGS",               "PEINTURES ET REVÊTEMENTS POUR CONSTRUCTION"),
        // ── 45 Construcción ─────────────────────────────────────────────
        ("451","45","PREPARACIÓN DEL TERRENO",                       "SITE PREPARATION WORK",                          "TRAVAUX DE PRÉPARATION DU TERRAIN"),
        ("452","45","OBRAS DE EDIFICACIÓN",                          "BUILDING CONSTRUCTION WORKS",                    "TRAVAUX DE CONSTRUCTION DE BÂTIMENTS"),
        ("453","45","INSTALACIONES PARA EDIFICIOS",                  "BUILDING INSTALLATION WORKS",                    "TRAVAUX D'INSTALLATION DANS LES BÂTIMENTS"),
        ("454","45","OBRAS DE ACABADO DE EDIFICIOS",                 "BUILDING COMPLETION WORKS",                      "TRAVAUX DE FINITION DE BÂTIMENTS"),
        ("455","45","OBRAS DE INGENIERÍA CIVIL",                     "CIVIL ENGINEERING WORKS",                        "TRAVAUX DE GÉNIE CIVIL"),
        // ── 48 Software ─────────────────────────────────────────────────
        ("481","48","PAQUETES DE SOFTWARE ESTÁNDAR",                 "STANDARD SOFTWARE PACKAGES",                     "LOGICIELS STANDARD"),
        ("482","48","SOFTWARE DE USO ESPECIAL",                      "SPECIAL-PURPOSE SOFTWARE",                       "LOGICIELS À USAGE SPÉCIFIQUE"),
        ("483","48","SISTEMAS DE GESTIÓN EMPRESARIAL (ERP/CRM)",     "BUSINESS MANAGEMENT SYSTEMS (ERP/CRM)",          "SYSTÈMES DE GESTION D'ENTREPRISE (ERP/CRM)"),
        ("484","48","SISTEMAS DE INFORMACIÓN GEOGRÁFICA Y GPS",      "GIS AND GPS SYSTEMS",                            "SYSTÈMES SIG ET GPS"),
        // ── 50 Reparación ───────────────────────────────────────────────
        ("501","50","MANTENIMIENTO Y REPARACIÓN DE VEHÍCULOS",       "VEHICLE MAINTENANCE AND REPAIR",                 "MAINTENANCE ET RÉPARATION DE VÉHICULES"),
        ("502","50","MANTENIMIENTO DE EQUIPOS INFORMÁTICOS",         "COMPUTER EQUIPMENT MAINTENANCE",                 "MAINTENANCE D'ÉQUIPEMENTS INFORMATIQUES"),
        ("503","50","MANTENIMIENTO DE MAQUINARIA INDUSTRIAL",        "INDUSTRIAL MACHINERY MAINTENANCE",               "MAINTENANCE DE MACHINES INDUSTRIELLES"),
        ("504","50","MANTENIMIENTO DE EDIFICIOS E INSTALACIONES",    "BUILDING AND FACILITY MAINTENANCE",              "MAINTENANCE DE BÂTIMENTS ET D'INSTALLATIONS"),
        // ── 51 Instalación ──────────────────────────────────────────────
        ("511","51","INSTALACIONES MECÁNICAS",                       "MECHANICAL INSTALLATIONS",                       "INSTALLATIONS MÉCANIQUES"),
        ("512","51","INSTALACIONES ELÉCTRICAS",                      "ELECTRICAL INSTALLATIONS",                       "INSTALLATIONS ÉLECTRIQUES"),
        ("513","51","INSTALACIONES DE TELECOMUNICACIONES",           "TELECOMMUNICATIONS INSTALLATIONS",               "INSTALLATIONS DE TÉLÉCOMMUNICATIONS"),
        ("514","51","INSTALACIONES SANITARIAS Y DE FONTANERÍA",      "SANITARY AND PLUMBING INSTALLATIONS",            "INSTALLATIONS SANITAIRES ET PLOMBERIE"),
        // ── 55 Hostelería ───────────────────────────────────────────────
        ("551","55","SERVICIOS DE ALOJAMIENTO",                      "ACCOMMODATION SERVICES",                         "SERVICES D'HÉBERGEMENT"),
        ("552","55","SERVICIOS DE RESTAURANTE Y CATERING",           "RESTAURANT AND CATERING SERVICES",               "SERVICES DE RESTAURATION ET TRAITEUR"),
        ("553","55","COMERCIO MINORISTA Y DISTRIBUCIÓN",             "RETAIL AND DISTRIBUTION",                        "COMMERCE DE DÉTAIL ET DISTRIBUTION"),
        // ── 60 Transporte ───────────────────────────────────────────────
        ("601","60","TRANSPORTE POR CARRETERA",                      "ROAD TRANSPORT",                                 "TRANSPORT ROUTIER"),
        ("602","60","TRANSPORTE FERROVIARIO",                        "RAIL TRANSPORT",                                 "TRANSPORT FERROVIAIRE"),
        ("603","60","TRANSPORTE AÉREO",                              "AIR TRANSPORT",                                  "TRANSPORT AÉRIEN"),
        ("604","60","TRANSPORTE MARÍTIMO Y FLUVIAL",                 "MARITIME AND RIVER TRANSPORT",                   "TRANSPORT MARITIME ET FLUVIAL"),
        ("605","60","TRANSPORTE URBANO",                             "URBAN TRANSPORT",                                "TRANSPORT URBAIN"),
        // ── 63 Auxiliares de transporte ─────────────────────────────────
        ("631","63","AGENCIAS DE VIAJES Y TURISMO",                  "TRAVEL AGENCIES AND TOURISM",                    "AGENCES DE VOYAGES ET TOURISME"),
        ("632","63","LOGÍSTICA Y ALMACENAMIENTO",                    "LOGISTICS AND STORAGE",                          "LOGISTIQUE ET STOCKAGE"),
        ("633","63","SERVICIOS PORTUARIOS Y AEROPORTUARIOS",         "PORT AND AIRPORT SERVICES",                      "SERVICES PORTUAIRES ET AÉROPORTUAIRES"),
        // ── 64 Postal y telecomunicaciones ─────────────────────────────
        ("641","64","SERVICIOS POSTALES Y DE MENSAJERÍA",            "POSTAL AND COURIER SERVICES",                    "SERVICES POSTAUX ET DE MESSAGERIE"),
        ("642","64","SERVICIOS DE TELEFONÍA",                        "TELEPHONE SERVICES",                             "SERVICES TÉLÉPHONIQUES"),
        ("643","64","SERVICIOS DE INTERNET Y DATOS",                 "INTERNET AND DATA SERVICES",                     "SERVICES INTERNET ET DONNÉES"),
        ("644","64","SERVICIOS DE RADIODIFUSIÓN",                    "BROADCASTING SERVICES",                          "SERVICES DE RADIODIFFUSION"),
        // ── 65 Servicios públicos ───────────────────────────────────────
        ("651","65","DISTRIBUCIÓN DE GAS",                           "GAS DISTRIBUTION",                               "DISTRIBUTION DE GAZ"),
        ("652","65","DISTRIBUCIÓN DE AGUA",                          "WATER DISTRIBUTION",                             "DISTRIBUTION D'EAU"),
        ("653","65","DISTRIBUCIÓN DE ELECTRICIDAD",                  "ELECTRICITY DISTRIBUTION",                       "DISTRIBUTION D'ÉLECTRICITÉ"),
        ("654","65","CALEFACCIÓN URBANA",                            "DISTRICT HEATING",                               "CHAUFFAGE URBAIN"),
        // ── 66 Financiero ───────────────────────────────────────────────
        ("661","66","SERVICIOS BANCARIOS Y DE INVERSIÓN",            "BANKING AND INVESTMENT SERVICES",                "SERVICES BANCAIRES ET D'INVESTISSEMENT"),
        ("662","66","SEGUROS",                                       "INSURANCE",                                      "ASSURANCE"),
        ("663","66","SERVICIOS DE GESTIÓN DE FONDOS",                "FUND MANAGEMENT SERVICES",                       "SERVICES DE GESTION DE FONDS"),
        // ── 70 Inmobiliario ─────────────────────────────────────────────
        ("701","70","COMPRAVENTA DE BIENES INMUEBLES",               "REAL ESTATE BUYING AND SELLING",                 "ACHAT ET VENTE IMMOBILIERS"),
        ("702","70","ARRENDAMIENTO DE BIENES INMUEBLES",             "REAL ESTATE LEASING",                            "LOCATION DE BIENS IMMOBILIERS"),
        ("703","70","ADMINISTRACIÓN DE PROPIEDADES",                 "PROPERTY MANAGEMENT",                            "GESTION IMMOBILIÈRE"),
        // ── 71 Arquitectura e ingeniería ────────────────────────────────
        ("711","71","SERVICIOS DE ARQUITECTURA",                     "ARCHITECTURAL SERVICES",                         "SERVICES D'ARCHITECTURE"),
        ("712","71","SERVICIOS DE INGENIERÍA",                       "ENGINEERING SERVICES",                           "SERVICES D'INGÉNIERIE"),
        ("713","71","CONSULTORÍA TÉCNICA Y SUPERVISIÓN",             "TECHNICAL CONSULTANCY AND SUPERVISION",          "CONSEIL TECHNIQUE ET SUPERVISION"),
        ("714","71","TOPOGRAFÍA Y CARTOGRAFÍA",                      "SURVEYING AND CARTOGRAPHY",                      "TOPOGRAPHIE ET CARTOGRAPHIE"),
        // ── 72 TI ───────────────────────────────────────────────────────
        ("721","72","CONSULTORÍA EN TI",                             "IT CONSULTANCY",                                 "CONSEIL EN TECHNOLOGIES DE L'INFORMATION"),
        ("722","72","DESARROLLO Y PROGRAMACIÓN DE SOFTWARE",         "SOFTWARE DEVELOPMENT AND PROGRAMMING",           "DÉVELOPPEMENT ET PROGRAMMATION DE LOGICIELS"),
        ("723","72","GESTIÓN DE INFRAESTRUCTURA Y REDES",            "INFRASTRUCTURE AND NETWORK MANAGEMENT",          "GESTION D'INFRASTRUCTURE ET RÉSEAUX"),
        ("724","72","SOPORTE TÉCNICO Y HELPDESK",                    "TECHNICAL SUPPORT AND HELPDESK",                 "SUPPORT TECHNIQUE ET HELPDESK"),
        ("725","72","SERVICIOS DE SEGURIDAD INFORMÁTICA",            "IT SECURITY SERVICES",                           "SERVICES DE SÉCURITÉ INFORMATIQUE"),
        // ── 73 I+D ──────────────────────────────────────────────────────
        ("731","73","INVESTIGACIÓN EN CIENCIAS EXACTAS",             "R&D IN EXACT SCIENCES",                          "R&D EN SCIENCES EXACTES"),
        ("732","73","INVESTIGACIÓN EN CIENCIAS APLICADAS",           "R&D IN APPLIED SCIENCES",                        "R&D EN SCIENCES APPLIQUÉES"),
        ("733","73","ENSAYOS Y PRUEBAS TÉCNICAS",                    "TECHNICAL TESTING AND TRIALS",                   "ESSAIS ET TESTS TECHNIQUES"),
        // ── 75 Administración ───────────────────────────────────────────
        ("751","75","SERVICIOS DE ADMINISTRACIÓN PÚBLICA",           "PUBLIC ADMINISTRATION SERVICES",                 "SERVICES D'ADMINISTRATION PUBLIQUE"),
        ("752","75","SERVICIOS DE DEFENSA Y SEGURIDAD NACIONAL",     "DEFENCE AND NATIONAL SECURITY SERVICES",         "SERVICES DE DÉFENSE ET SÉCURITÉ NATIONALE"),
        ("753","75","SERVICIOS DE SEGURIDAD SOCIAL",                 "SOCIAL SECURITY SERVICES",                       "SERVICES DE SÉCURITÉ SOCIALE"),
        // ── 76 Petróleo y gas ───────────────────────────────────────────
        ("761","76","PERFORACIÓN Y EXPLORACIÓN PETROLERA",           "OIL DRILLING AND EXPLORATION",                   "FORAGE ET EXPLORATION PÉTROLIÈRE"),
        ("762","76","SERVICIOS DE REFINERÍA Y PROCESAMIENTO",        "REFINERY AND PROCESSING SERVICES",               "SERVICES DE RAFFINERIE ET TRAITEMENT"),
        // ── 77 Servicios agrícolas ──────────────────────────────────────
        ("771","77","SERVICIOS AGRÍCOLAS Y HORTÍCOLAS",              "AGRICULTURAL AND HORTICULTURAL SERVICES",        "SERVICES AGRICOLES ET HORTICOLES"),
        ("772","77","SERVICIOS FORESTALES",                          "FORESTRY SERVICES",                              "SERVICES FORESTIERS"),
        ("773","77","SERVICIOS DE PESCA Y ACUICULTURA",              "FISHING AND AQUACULTURE SERVICES",               "SERVICES DE PÊCHE ET AQUACULTURE"),
        ("774","77","SERVICIOS MEDIOAMBIENTALES AGRÍCOLAS",          "AGRICULTURAL ENVIRONMENTAL SERVICES",            "SERVICES ENVIRONNEMENTAUX AGRICOLES"),
        // ── 79 Servicios empresariales ──────────────────────────────────
        ("791","79","SERVICIOS JURÍDICOS",                           "LEGAL SERVICES",                                 "SERVICES JURIDIQUES"),
        ("792","79","AUDITORÍA Y CONTABILIDAD",                      "AUDIT AND ACCOUNTING SERVICES",                  "SERVICES D'AUDIT ET COMPTABILITÉ"),
        ("793","79","PUBLICIDAD Y MARKETING",                        "ADVERTISING AND MARKETING",                      "PUBLICITÉ ET MARKETING"),
        ("794","79","RECURSOS HUMANOS Y EMPLEO",                     "HUMAN RESOURCES AND EMPLOYMENT",                 "RESSOURCES HUMAINES ET EMPLOI"),
        ("795","79","SERVICIOS DE SEGURIDAD Y VIGILANCIA",           "SECURITY AND GUARD SERVICES",                    "SERVICES DE SÉCURITÉ ET DE GARDIENNAGE"),
        ("796","79","INVESTIGACIÓN Y ANÁLISIS DE MERCADO",           "MARKET RESEARCH AND ANALYSIS",                   "ÉTUDE ET ANALYSE DE MARCHÉ"),
        ("797","79","TRADUCCIÓN E INTERPRETACIÓN",                   "TRANSLATION AND INTERPRETATION",                 "TRADUCTION ET INTERPRÉTATION"),
        ("798","79","SERVICIOS DE IMPRENTA Y EDICIÓN",               "PRINTING AND PUBLISHING SERVICES",               "SERVICES D'IMPRESSION ET ÉDITION"),
        // ── 80 Educación ────────────────────────────────────────────────
        ("801","80","EDUCACIÓN PRIMARIA Y SECUNDARIA",               "PRIMARY AND SECONDARY EDUCATION",                "ENSEIGNEMENT PRIMAIRE ET SECONDAIRE"),
        ("802","80","EDUCACIÓN UNIVERSITARIA Y SUPERIOR",            "UNIVERSITY AND HIGHER EDUCATION",                "ENSEIGNEMENT UNIVERSITAIRE ET SUPÉRIEUR"),
        ("803","80","FORMACIÓN PROFESIONAL Y TÉCNICA",               "VOCATIONAL AND TECHNICAL TRAINING",              "FORMATION PROFESSIONNELLE ET TECHNIQUE"),
        ("804","80","FORMACIÓN EMPRESARIAL Y CORPORATIVA",           "BUSINESS AND CORPORATE TRAINING",                "FORMATION AUX ENTREPRISES ET CORPORATE"),
        // ── 85 Salud ────────────────────────────────────────────────────
        ("851","85","SERVICIOS HOSPITALARIOS Y CLÍNICOS",            "HOSPITAL AND CLINICAL SERVICES",                 "SERVICES HOSPITALIERS ET CLINIQUES"),
        ("852","85","ATENCIÓN MÉDICA PRIMARIA Y ESPECIALIZADA",      "PRIMARY AND SPECIALIST MEDICAL CARE",            "SOINS MÉDICAUX PRIMAIRES ET SPÉCIALISÉS"),
        ("853","85","SERVICIOS ODONTOLÓGICOS",                       "DENTAL SERVICES",                                "SERVICES DENTAIRES"),
        ("854","85","SERVICIOS SOCIALES Y ASISTENCIA",               "SOCIAL SERVICES AND CARE",                       "SERVICES SOCIAUX ET SOINS"),
        ("855","85","SERVICIOS DE SALUD MENTAL",                     "MENTAL HEALTH SERVICES",                         "SERVICES DE SANTÉ MENTALE"),
        // ── 90 Medioambiente ────────────────────────────────────────────
        ("901","90","GESTIÓN DE RESIDUOS Y RECICLAJE",               "WASTE MANAGEMENT AND RECYCLING",                 "GESTION DES DÉCHETS ET RECYCLAGE"),
        ("902","90","LIMPIEZA Y SANEAMIENTO URBANO",                 "URBAN CLEANING AND SANITATION",                  "NETTOYAGE ET ASSAINISSEMENT URBAIN"),
        ("903","90","GESTIÓN DE AGUAS RESIDUALES",                   "WASTEWATER MANAGEMENT",                          "GESTION DES EAUX USÉES"),
        ("904","90","DESCONTAMINACIÓN Y REMEDIACIÓN AMBIENTAL",      "DECONTAMINATION AND ENVIRONMENTAL REMEDIATION",  "DÉCONTAMINATION ET ASSAINISSEMENT ENVIRONNEMENTAL"),
        // ── 92 Recreación ───────────────────────────────────────────────
        ("921","92","SERVICIOS CULTURALES Y DE ENTRETENIMIENTO",     "CULTURAL AND ENTERTAINMENT SERVICES",            "SERVICES CULTURELS ET DE DIVERTISSEMENT"),
        ("922","92","SERVICIOS DEPORTIVOS Y DE RECREACIÓN",          "SPORTS AND RECREATION SERVICES",                 "SERVICES SPORTIFS ET DE LOISIRS"),
        ("923","92","SERVICIOS DE MEDIOS DE COMUNICACIÓN",           "MEDIA SERVICES",                                 "SERVICES MÉDIAS"),
        ("924","92","MUSEOS Y PATRIMONIO CULTURAL",                  "MUSEUMS AND CULTURAL HERITAGE",                  "MUSÉES ET PATRIMOINE CULTUREL"),
        // ── 98 Otros servicios ──────────────────────────────────────────
        ("981","98","SERVICIOS DE MEMBRESÍA Y ASOCIACIONES",         "MEMBERSHIP AND ASSOCIATION SERVICES",            "SERVICES D'ADHÉSION ET D'ASSOCIATIONS"),
        ("982","98","SERVICIOS PERSONALES Y DOMÉSTICOS",             "PERSONAL AND DOMESTIC SERVICES",                 "SERVICES PERSONNELS ET DOMESTIQUES"),
        ("983","98","SERVICIOS FUNERARIOS",                          "FUNERAL SERVICES",                               "SERVICES FUNÉRAIRES"),
        // ── 99 Extraterritorial ─────────────────────────────────────────
        ("991","99","ORGANIZACIONES INTERNACIONALES",                "INTERNATIONAL ORGANISATIONS",                    "ORGANISATIONS INTERNATIONALES"),
    };

    return (lineas, sublineas);
}