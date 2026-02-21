using System.IO.Compression;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("🌍 GeoNames → CSV Converter para GoldBusiness");
Console.WriteLine("═══════════════════════════════════════════════");

// ═══════════════════════════════════════════════════════════════
// CONFIGURACIÓN
// ═══════════════════════════════════════════════════════════════
var outputDir = Path.Combine(AppContext.BaseDirectory, "output");
Directory.CreateDirectory(outputDir);

var tempDir = Path.Combine(AppContext.BaseDirectory, "temp");
Directory.CreateDirectory(tempDir);

// Códigos ISO de los países que tienes en SeedPais
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

// ═══════════════════════════════════════════════════════════════
// DICCIONARIO DE DATOS TELEFÓNICOS (no disponible en GeoNames)
// ═══════════════════════════════════════════════════════════════
var phoneData = new Dictionary<string, (string Tel, string Regex, string Fmt, string Ej)>
{
    // Hispanoamérica
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
    // Francofonía
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
    // Anglofonía
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
    // Otros
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
// PASO 1: Descargar archivos de GeoNames
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📥 Descargando datos de GeoNames...");

using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromMinutes(30)
};

var countryInfoFile = Path.Combine(tempDir, "countryInfo.txt");
var admin1File = Path.Combine(tempDir, "admin1CodesASCII.txt");
var admin2File = Path.Combine(tempDir, "admin2Codes.txt");
var altNamesZip = Path.Combine(tempDir, "alternateNamesV2.zip");
var altNamesFile = Path.Combine(tempDir, "alternateNamesV2.txt");

if (!File.Exists(countryInfoFile))
{
    Console.Write("  → countryInfo.txt ... ");
    var data = await httpClient.GetByteArrayAsync(
        "https://download.geonames.org/export/dump/countryInfo.txt");
    await File.WriteAllBytesAsync(countryInfoFile, data);
    Console.WriteLine("✅");
}

if (!File.Exists(admin1File))
{
    Console.Write("  → admin1CodesASCII.txt ... ");
    var data = await httpClient.GetByteArrayAsync(
        "https://download.geonames.org/export/dump/admin1CodesASCII.txt");
    await File.WriteAllBytesAsync(admin1File, data);
    Console.WriteLine("✅");
}

if (!File.Exists(admin2File))
{
    Console.Write("  → admin2Codes.txt ... ");
    var data = await httpClient.GetByteArrayAsync(
        "https://download.geonames.org/export/dump/admin2Codes.txt");
    await File.WriteAllBytesAsync(admin2File, data);
    Console.WriteLine("✅");
}

if (!File.Exists(altNamesFile))
{
    Console.WriteLine("  → alternateNamesV2.zip (~300MB, puede tardar varios minutos)...");
    await DownloadWithProgressAsync(
        httpClient,
        "https://download.geonames.org/export/dump/alternateNamesV2.zip",
        altNamesZip);

    Console.Write("  → Descomprimiendo... ");
    ZipFile.ExtractToDirectory(altNamesZip, tempDir, overwriteFiles: true);
    Console.WriteLine("✅");
    File.Delete(altNamesZip);
    Console.WriteLine("  → ZIP eliminado para liberar espacio ✅");
}

// ═══════════════════════════════════════════════════════════════
// PASO 2: Cargar traducciones (alternateNames)
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
            traducciones[geoNameId] = new Dictionary<string, string>();

        traducciones[geoNameId].TryAdd(isoLang, altName.ToUpperInvariant());
    }

    Console.WriteLine($"\r  ✅ {count:N0} líneas procesadas, {traducciones.Count:N0} entidades con traducciones");
}

// ═══════════════════════════════════════════════════════════════
// PASO 3: Procesar países (countryInfo.txt)
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🌍 Procesando países...");

// Formato countryInfo.txt (tab-separated, # = comentarios):
// 0:ISO  1:ISO3  2:ISONumeric  3:fips  4:Country  5:Capital  6:Area
// 7:Population  8:Continent  9:tld  10:CurrencyCode  11:CurrencyName
// 12:Phone  13:PostalFormat  14:PostalRegex  15:Languages  16:geonameid

var paisesData = new List<(string Alpha3, string Alpha2, string Tel, string DescES, string DescEN, string DescFR, string Regex, string Fmt, string Ej)>();

using (var reader = new StreamReader(countryInfoFile, Encoding.UTF8))
{
    string? line;
    while ((line = await reader.ReadLineAsync()) != null)
    {
        if (line.StartsWith('#')) continue;
        if (string.IsNullOrWhiteSpace(line)) continue;

        var parts = line.Split('\t');
        if (parts.Length < 17) continue;

        var alpha2 = parts[0].Trim();
        var alpha3 = parts[1].Trim();
        var countryName = parts[4].Trim().ToUpperInvariant();
        var geoNameId = parts[16].Trim();

        if (!paisesDeseados.Contains(alpha2)) continue;
        if (!phoneData.ContainsKey(alpha2)) continue;

        var pd = phoneData[alpha2];

        // Traducciones desde alternateNames
        var descES = countryName;
        var descEN = countryName;
        var descFR = countryName;

        if (traducciones.TryGetValue(geoNameId, out var trans))
        {
            if (trans.TryGetValue("es", out var es)) descES = es;
            if (trans.TryGetValue("en", out var en)) descEN = en;
            if (trans.TryGetValue("fr", out var fr)) descFR = fr;
        }

        paisesData.Add((alpha3, alpha2, pd.Tel, descES, descEN, descFR, pd.Regex, pd.Fmt, pd.Ej));
    }
}

Console.WriteLine($"  ✅ {paisesData.Count} países procesados");

// ═══════════════════════════════════════════════════════════════
// PASO 4: Procesar provincias (admin1)
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🗺️ Procesando provincias/estados (admin1)...");

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

        if (!paisesDeseados.Contains(countryCode)) continue;

        var adminCode = codeFull.Split('.')[1];
        var nombre = parts[1].ToUpperInvariant();
        var geoNameId = parts[3];

        var descES = nombre;
        var descEN = nombre;
        var descFR = nombre;

        if (traducciones.TryGetValue(geoNameId, out var trans))
        {
            if (trans.TryGetValue("es", out var es)) descES = es;
            if (trans.TryGetValue("en", out var en)) descEN = en;
            if (trans.TryGetValue("fr", out var fr)) descFR = fr;
        }

        var codigo = $"{countryCode}-{adminCode}";

        if (!provinciasPorPais.ContainsKey(countryCode))
            provinciasPorPais[countryCode] = [];

        provinciasPorPais[countryCode].Add((codigo, descES, descEN, descFR));
    }
}

Console.WriteLine($"  ✅ {provinciasPorPais.Sum(p => p.Value.Count)} provincias de {provinciasPorPais.Count} países");

// ═══════════════════════════════════════════════════════════════
// PASO 5: Procesar municipios (admin2)
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n🏘️ Procesando municipios (admin2)...");

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
        if (!paisesDeseados.Contains(countryCode)) continue;

        var adminCode1 = segments[1];
        var adminCode2 = segments[2];
        var nombre = parts[1].ToUpperInvariant();
        var geoNameId = parts[3];

        var descES = nombre;
        var descEN = nombre;
        var descFR = nombre;

        if (traducciones.TryGetValue(geoNameId, out var trans))
        {
            if (trans.TryGetValue("es", out var es)) descES = es;
            if (trans.TryGetValue("en", out var en)) descEN = en;
            if (trans.TryGetValue("fr", out var fr)) descFR = fr;
        }

        var codigo = $"{countryCode}-{adminCode1}-{adminCode2}";
        var provCodigo = $"{countryCode}-{adminCode1}";

        if (!municipiosPorPais.ContainsKey(countryCode))
            municipiosPorPais[countryCode] = [];

        municipiosPorPais[countryCode].Add((codigo, descES, provCodigo, descEN, descFR));
    }
}

Console.WriteLine($"  ✅ {municipiosPorPais.Sum(p => p.Value.Count)} municipios de {municipiosPorPais.Count} países");

// ═══════════════════════════════════════════════════════════════
// PASO 6: Generar CSVs
// ═══════════════════════════════════════════════════════════════
Console.WriteLine("\n📝 Generando archivos CSV...");

// ── CSV DE PAÍSES ──
var paisFile = Path.Combine(outputDir, "Paises.csv");
using (var writer = new StreamWriter(paisFile, false, new UTF8Encoding(true)))
{
    await writer.WriteLineAsync("CodigoAlpha3,CodigoAlpha2,CodigoTelefono,DescripcionES,DescripcionEN,DescripcionFR,RegexTelefono,FormatoTelefono,FormatoEjemplo");

    foreach (var p in paisesData.OrderBy(p => p.Alpha2))
    {
        var descES = EscapeCsv(p.DescES);
        var descEN = EscapeCsv(p.DescEN);
        var descFR = EscapeCsv(p.DescFR);
        var regex = EscapeCsv(p.Regex);
        var fmt = EscapeCsv(p.Fmt);
        var ej = EscapeCsv(p.Ej);
        await writer.WriteLineAsync($"{p.Alpha3},{p.Alpha2},{p.Tel},{descES},{descEN},{descFR},{regex},{fmt},{ej}");
    }
}

Console.WriteLine($"  ✅ {paisFile}");

// ── CSV DE PROVINCIAS ──
var provFile = Path.Combine(outputDir, "Provincias.csv");
using (var writer = new StreamWriter(provFile, false, new UTF8Encoding(true)))
{
    await writer.WriteLineAsync("Codigo,PaisCodigo,DescripcionES,DescripcionEN,DescripcionFR");

    foreach (var (pais, provs) in provinciasPorPais.OrderBy(p => p.Key))
    {
        foreach (var prov in provs.OrderBy(p => p.Codigo))
        {
            var es = EscapeCsv(prov.Desc);
            var en = EscapeCsv(prov.DescEN);
            var fr = EscapeCsv(prov.DescFR);
            await writer.WriteLineAsync($"{prov.Codigo},{pais},{es},{en},{fr}");
        }
    }
}

Console.WriteLine($"  ✅ {provFile}");

// ── CSV DE MUNICIPIOS ──
var munFile = Path.Combine(outputDir, "Municipios.csv");
using (var writer = new StreamWriter(munFile, false, new UTF8Encoding(true)))
{
    await writer.WriteLineAsync("Codigo,ProvinciaCodigo,PaisCodigo,DescripcionES,DescripcionEN,DescripcionFR");

    foreach (var (pais, muns) in municipiosPorPais.OrderBy(p => p.Key))
    {
        foreach (var mun in muns.OrderBy(m => m.Codigo))
        {
            var es = EscapeCsv(mun.Desc);
            var en = EscapeCsv(mun.DescEN);
            var fr = EscapeCsv(mun.DescFR);
            await writer.WriteLineAsync($"{mun.Codigo},{mun.ProvCodigo},{pais},{es},{en},{fr}");
        }
    }
}

Console.WriteLine($"  ✅ {munFile}");

// ── ESTADÍSTICAS ──
Console.WriteLine("\n📊 ESTADÍSTICAS:");
Console.WriteLine($"  {"País",-6} {"Provincias",12} {"Municipios",12}");
Console.WriteLine($"  {"────",-6} {"──────────",12} {"──────────",12}");

foreach (var pais in provinciasPorPais.Keys.OrderBy(k => k))
{
    var provCount = provinciasPorPais[pais].Count;
    var munCount = municipiosPorPais.GetValueOrDefault(pais)?.Count ?? 0;
    Console.WriteLine($"  {pais,-6} {provCount,12:N0} {munCount,12:N0}");
}

Console.WriteLine($"  {"────",-6} {"──────────",12} {"──────────",12}");
Console.WriteLine($"  {"TOTAL",-6} {provinciasPorPais.Sum(p => p.Value.Count),12:N0} {municipiosPorPais.Sum(p => p.Value.Count),12:N0}");
Console.WriteLine($"  Países: {paisesData.Count}");

Console.WriteLine($"\n📂 Archivos generados en: {outputDir}");
Console.WriteLine("\n✅ ¡Proceso completado!");

// ═══════════════════════════════════════════════════════════════
// HELPERS
// ═══════════════════════════════════════════════════════════════

static async Task DownloadWithProgressAsync(HttpClient client, string url, string outputPath)
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

            if (totalBytes > 0)
            {
                var percent = (double)totalRead / totalBytes * 100;
                Console.Write($"\r    ⬇️  {readMB:F1} / {totalMB:F1} MB ({percent:F1}%)   ");
            }
            else
            {
                Console.Write($"\r    ⬇️  {readMB:F1} MB descargados...   ");
            }

            lastProgress = DateTime.Now;
        }
    }

    var finalMB = totalRead / (1024.0 * 1024.0);
    Console.WriteLine($"\r    ⬇️  {finalMB:F1} MB descargados ✅                    ");
}

static string EscapeCsv(string value)
{
    if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        return $"\"{value.Replace("\"", "\"\"")}\"";
    return value;
}