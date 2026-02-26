using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoldBusiness.CodeGen
{
    public class CodeGenService
    {
        private readonly string _clientRoot;

        public CodeGenService(string clientRoot)
        {
            _clientRoot = clientRoot;
        }

        public void GenerateAngularFiles(string entity, List<(string Name, string Type)> properties)
        {
            // 1. Generar interface
            var interfaceContent = GenerateInterface(entity, properties);
            var interfacePath = Path.Combine(_clientRoot, "interfaces", $"{entity.ToLower()}.interface.ts");
            WriteFile(interfacePath, interfaceContent);

            // 2. Generar servicio
            var serviceContent = GenerateService(entity);
            var servicePath = Path.Combine(_clientRoot, "services", $"{entity.ToLower()}.service.ts");
            WriteFile(servicePath, serviceContent);

            // 3. Generar componentes (list, form, detail)
            GenerateComponent(entity, "list", properties);
            GenerateComponent(entity, "form", properties);
            GenerateComponent(entity, "detail", properties);
        }

        private void GenerateComponent(string entity, string type, List<(string Name, string Type)> properties)
        {
            var componentDir = Path.Combine(_clientRoot, "pages", entity.ToLower(), $"{entity.ToLower()}-{type}");
            Directory.CreateDirectory(componentDir);

            // .ts
            var tsContent = GenerateComponentTs(entity, type, properties);
            WriteFile(Path.Combine(componentDir, $"{entity.ToLower()}-{type}.component.ts"), tsContent);

            // .html
            var htmlContent = $"<!-- {entity} {type} component HTML -->";
            WriteFile(Path.Combine(componentDir, $"{entity.ToLower()}-{type}.component.html"), htmlContent);

            // .css (puedes copiar de un componente base si lo deseas)
            var cssContent = $"/* Estilos base para {entity} {type} */";
            WriteFile(Path.Combine(componentDir, $"{entity.ToLower()}-{type}.component.css"), cssContent);
        }

        private string GenerateInterface(string entity, List<(string Name, string Type)> properties)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"export interface {entity} {{");
            foreach (var prop in properties)
            {
                sb.AppendLine($"  {ToCamelCase(prop.Name)}: {MapToTypeScriptType(prop.Type)};");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        private string GenerateService(string entity)
        {
            var lcEntity = entity.ToLower();
            return
$@"import {{ Injectable }} from '@angular/core';
import {{ HttpClient }} from '@angular/common/http';
import {{ Observable }} from 'rxjs';
import {{ {entity} }} from '../interfaces/{lcEntity}.interface';

@Injectable({{ providedIn: 'root' }})
export class {entity}Service {{
  private apiUrl = '/api/{lcEntity}s';

  constructor(private http: HttpClient) {{ }}

  getAll(): Observable<{entity}[]> {{
    return this.http.get<{entity}[]>(this.apiUrl);
  }}

  getById(id: number): Observable<{entity}> {{
    return this.http.get<{entity}>(`${{this.apiUrl}}/${{id}}`);
  }}

  create(item: {entity}): Observable<{entity}> {{
    return this.http.post<{entity}>(this.apiUrl, item);
  }}

  update(id: number, item: {entity}): Observable<{entity}> {{
    return this.http.put<{entity}>(`${{this.apiUrl}}/${{id}}`, item);
  }}

  delete(id: number): Observable<void> {{
    return this.http.delete<void>(`${{this.apiUrl}}/${{id}}`);
  }}
}}
";
        }

        private string GenerateComponentTs(string entity, string type, List<(string Name, string Type)> properties)
        {
            // Puedes personalizar según el tipo de componente
            return
$@"import {{ Component }} from '@angular/core';

@Component({{
  selector: 'app-{entity.ToLower()}-{type}',
  templateUrl: './{entity.ToLower()}-{type}.component.html',
  styleUrls: ['./{entity.ToLower()}-{type}.component.css']
}})
export class {entity}{ToPascalCase(type)}Component {{

}}
";
        }

        private void WriteFile(string path, string content)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content, Encoding.UTF8);
        }

        private string ToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Length < 2)
                return input.ToLower();
            return char.ToLower(input[0]) + input.Substring(1);
        }

        private string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        private string MapToTypeScriptType(string csharpType)
        {
            return csharpType switch
            {
                "int" or "long" or "double" or "decimal" or "float" => "number",
                "string" => "string",
                "bool" or "boolean" => "boolean",
                "DateTime" => "Date",
                _ => "any"
            };
        }
    }
}