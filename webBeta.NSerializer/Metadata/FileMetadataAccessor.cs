using System;
using System.IO;

namespace webBeta.NSerializer.Metadata
{
    public class FileMetadataAccessor : IMetadataAccessor
    {
        private const string YAML_EXT = ".yaml";
        private const string YML_EXT = ".yml";

        protected string metadataPath;

        public void SetMetadataPath(string path)
        {
            metadataPath = path;
        }

        public bool HasMetadata(Type klass)
        {
            return HasYmlMetadata(klass) ||
                   HasYamlMetadata(klass);
        }

        public string GetMetadataContent(Type klass)
        {
            if (HasYmlMetadata(klass))
                return File.ReadAllText(BuildPath(klass, YML_EXT));
            if (HasYamlMetadata(klass))
                return File.ReadAllText(BuildPath(klass, YAML_EXT));
            return null;
        }

        private string BuildPath(Type klass, string extension)
        {
            return Path.Combine(metadataPath, klass.FullName + extension);
        }

        private bool HasYmlMetadata(Type klass)
        {
            return File.Exists(BuildPath(klass, YML_EXT));
        }

        private bool HasYamlMetadata(Type klass)
        {
            return File.Exists(BuildPath(klass, YAML_EXT));
        }
    }
}