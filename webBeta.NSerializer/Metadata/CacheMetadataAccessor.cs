using System;
using webBeta.NSerializer.Base;

namespace webBeta.NSerializer.Metadata
{
    public class CacheMetadataAccessor : FileMetadataAccessor
    {
        private const string KeyTpl = "nserializer_metadata___{0}";

        private readonly ICache _cache;

        public CacheMetadataAccessor(ICache cache)
        {
            _cache = cache;
        }

        private string GenerateKey(Type klass)
        {
            return GenerateKey(klass.FullName);
        }

        private string GenerateKey(string canonical)
        {
            return string.Format(KeyTpl, canonical);
        }

        public new bool HasMetadata(Type klass)
        {
            return base.HasMetadata(klass);
        }

        public new string GetMetadataContent(Type klass)
        {
            var key = GenerateKey(klass);
            if (_cache.Get(key) != null)
                return _cache.Get(key);

            if (!base.HasMetadata(klass)) return null;

            var metadata = base.GetMetadataContent(klass);
            _cache.Set(key, metadata);
            return metadata;
        }
    }
}