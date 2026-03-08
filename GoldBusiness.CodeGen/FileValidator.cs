using System.Collections.Generic;
using System.IO;

namespace GoldBusiness.CodeGen
{
    public class FileValidator
    {
        private readonly string _clientRoot;
        private readonly EntityMetadata _metadata;

        public FileValidator(string clientRoot, EntityMetadata metadata)
        {
            _clientRoot = clientRoot;
            _metadata = metadata;
        }

        public List<string> CheckExistingFiles()
        {
            var existingFiles = new List<string>();

            // Service
            var servicePath = Path.Combine(_clientRoot, "services", $"{_metadata.KebabCase}.service.ts");
            if (File.Exists(servicePath)) existingFiles.Add(servicePath);

            // Componentes
            var componentsDir = Path.Combine(_clientRoot, "pages", _metadata.KebabCase);
            if (Directory.Exists(componentsDir))
            {
                var listDir = Path.Combine(componentsDir, $"{_metadata.KebabCase}-list");
                var formDir = Path.Combine(componentsDir, $"{_metadata.KebabCase}-form");
                var detailDir = Path.Combine(componentsDir, $"{_metadata.KebabCase}-detail");

                if (Directory.Exists(listDir)) existingFiles.Add(listDir);
                if (Directory.Exists(formDir)) existingFiles.Add(formDir);
                if (Directory.Exists(detailDir)) existingFiles.Add(detailDir);
            }

            return existingFiles;
        }
    }
}