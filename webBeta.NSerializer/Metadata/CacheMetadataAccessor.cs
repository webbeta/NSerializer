using System;
using webBeta.NSerializer.Base;

namespace webBeta.NSerializer.Metadata
{
    public class CacheMetadataAccessor : FileMetadataAccessor
    {
        private const string KEY_TPL = "serializer_metadata___%s";

        private ICache _cache;

        public CacheMetadataAccessor(ICache cache)
        {
            _cache = cache;
        }

        public new void SetMetadataPath(string path)
        {
            throw new NotImplementedException();
        }

        public new bool HasMetadata(Type klass)
        {
            throw new NotImplementedException();
        }

        public new string GetMetadataContent(Type klass)
        {
            throw new NotImplementedException();
        }
    }
}