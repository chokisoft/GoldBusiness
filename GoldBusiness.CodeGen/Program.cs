using System;
using System.IO;
using System.Linq;
using GoldBusiness.CodeGen;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🚀 GoldBusiness Code Generator");
        Console.WriteLine("================================\n");

        // ✅ Soporte para --preview opcional
        bool previewMode = args.Contains("--preview") || args.Contains("-p");
        args = args.Where(a => !a.StartsWith("--") && !a.StartsWith("-")).ToArray();

        if (args.Length < 2)
        {
            Console.WriteLine("Uso: dotnet run -- <ruta_entidad.cs> <NombreEntidad> [--preview]");
            Console.WriteLine("\nOpciones:");
            Console.WriteLine("  --preview, -p    Modo simulación (no crea archivos)");
            Console.WriteLine("\nEjemplo:");
            Console.WriteLine(@"  dotnet run -- ""F:\...\Entities\Producto.cs"" Producto");
            Console.WriteLine(@"  dotnet run -- ""F:\...\Entities\Producto.cs"" Producto --preview");
            return;
        }

        var entityPath = args[0];
        var entityName = args[1];

        if (!File.Exists(entityPath))
        {
            Console.WriteLine($"❌ Error: No se encontró el archivo: {entityPath}");
            return;
        }

        var clientRoot = @"F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.Client\src\app";

        try
        {
            Console.WriteLine($"📁 Ruta de entidad: {entityPath}");
            Console.WriteLine($"📂 Directorio de salida: {clientRoot}");
            if (previewMode) Console.WriteLine("🔍 MODO SIMULACIÓN - No se crearán archivos\n");
            else Console.WriteLine();

            // Analizar entidad
            var analyzer = new EntityAnalyzer();
            var metadata = analyzer.AnalyzeEntity(entityPath, entityName);

            Console.WriteLine($"📦 Metadata generada:");
            Console.WriteLine($"   Entity: {metadata.EntityName}");
            Console.WriteLine($"   Camel: {metadata.CamelCase}");
            Console.WriteLine($"   Kebab: {metadata.KebabCase}");
            Console.WriteLine($"   Plural: {metadata.PluralEntityName}\n");

            // ✅ VALIDAR si ya existen archivos
            var validator = new FileValidator(clientRoot, metadata);
            var existingFiles = validator.CheckExistingFiles();
            
            if (existingFiles.Any() && !previewMode)
            {
                Console.WriteLine("⚠️  Los siguientes archivos ya existen:");
                foreach (var file in existingFiles)
                {
                    Console.WriteLine($"   - {Path.GetFileName(file)}");
                }
                Console.Write("\n¿Desea sobrescribirlos? (s/N): ");
                var response = Console.ReadLine()?.Trim().ToLower();
                
                if (response != "s" && response != "si" && response != "yes")
                {
                    Console.WriteLine("\n❌ Operación cancelada.");
                    return;
                }
                Console.WriteLine();
            }

            // Generar archivos
            var generator = new CodeGenService(clientRoot, metadata, previewMode);
            generator.GenerateAngularFiles();

            if (previewMode)
            {
                Console.WriteLine($"\n✅ Simulación completada!");
                Console.WriteLine($"💡 Ejecuta sin --preview para crear los archivos");
            }
            else
            {
                Console.WriteLine($"\n✅ Generación completada exitosamente!");
                Console.WriteLine($"\n📝 Próximos pasos:");
                Console.WriteLine($"   1. git status  # Ver archivos creados");
                Console.WriteLine($"   2. Agregar rutas en app-routing.module.ts");
                Console.WriteLine($"   3. Declarar componentes en el módulo");
                Console.WriteLine($"   4. Agregar traducciones");
                Console.WriteLine($"   5. ng serve  # Probar en navegador");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error inesperado: {ex.Message}");
            Console.WriteLine($"\nStack trace:\n{ex.StackTrace}");
        }
    }
}