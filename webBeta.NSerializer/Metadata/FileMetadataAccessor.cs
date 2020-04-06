using System;
using System.IO;

namespace webBeta.NSerializer.Metadata
{
    public class FileMetadataAccessor : IMetadataAccessor
    {
        private const string YamlExt = ".yaml";
        private const string YmlExt = ".yml";

        protected string MetadataPath;

        public void SetMetadataPath(string path)
        {
            MetadataPath = path;
        }

        public bool HasMetadata(Type klass)
        {
            return HasYmlMetadata(klass) ||
                   HasYamlMetadata(klass);
        }

        public string GetMetadataContent(Type klass)
        {
            if (HasYmlMetadata(klass))
                return File.ReadAllText(BuildPath(klass, YmlExt));
            if (HasYamlMetadata(klass))
                return File.ReadAllText(BuildPath(klass, YamlExt));
            return null;
        }

        private string BuildPath(Type klass, string extension)
        {
            return Path.Combine(MetadataPath, klass.FullName + extension);
        }

        private bool HasYmlMetadata(Type klass)
        {
            return File.Exists(BuildPath(klass, YmlExt));
        }

        private bool HasYamlMetadata(Type klass)
        {
            return File.Exists(BuildPath(klass, YamlExt));
        }
    }
}