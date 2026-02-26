using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace GoldBusiness.CodeGen
{
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
}