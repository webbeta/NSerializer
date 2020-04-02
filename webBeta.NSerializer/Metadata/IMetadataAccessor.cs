using System;

namespace webBeta.NSerializer.Metadata
{
    public interface IMetadataAccessor
    {
        void SetMetadataPath(string path);
        bool HasMetadata(Type klass);
        string GetMetadataContent(Type klass);
    }
}