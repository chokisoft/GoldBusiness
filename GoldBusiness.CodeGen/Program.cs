using System;
using System.IO;
using GoldBusiness.CodeGen;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Argumentos recibidos: {string.Join(", ", args)}");

        if (args.Length < 2)
        {
            Console.WriteLine("Uso: dotnet run -- <ruta_modelo.cs> <NombreEntidad>");
            return;
        }

        var modelPath = args[0];
        var entityName = args[1];

        if (!File.Exists(modelPath))
        {
            Console.WriteLine($"La ruta de acceso al archivo proporcionada no existe: {modelPath}");
            return;
        }

        var clientRoot = @"F:\Documents\Visual Studio 18\Projects\GoldBusiness\GoldBusiness.Client\src\app";
        try
        {
            Console.WriteLine("Analizando modelo...");
            var analyzer = new ModelAnalyzer();
            var properties = analyzer.GetProperties(modelPath);
            Console.WriteLine($"Propiedades encontradas: {properties.Count}");

            Console.WriteLine("Generando archivos...");
            var generator = new CodeGenService(clientRoot);
            generator.GenerateAngularFiles(entityName, properties);

            Console.WriteLine($"Componentes y servicios generados para {entityName}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}

public class ModelAnalyzer
{
    public List<(string Name, string Type)> GetProperties(string modelPath)
    {
        var lines = File.ReadAllLines(modelPath);
        var properties = new List<(string, string)>();
        var regex = new Regex(@"public\s+(\w+)\s+(\w+)\s*\{");
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            if (match.Success)
            {
                properties.Add((match.Groups[2].Value, match.Groups[1].Value));
            }
        }
        return properties;
    }
}