using System;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Metadata;
using webBeta.NSerializer.Test.Beans;

namespace webBeta.NSerializer.Test.Base
{
    public class MockCache : ICache
    {
        public string Get(string key)
        {
            return null;
        }

        public void Set(string key, string content)
        {
        }

        public void Remove(string key)
        {
        }
    }

    public class MockProdCache : ICache
    {
        private readonly FileMetadataAccessor _fileMetadataAccessor;

        public MockProdCache(FileMetadataAccessor fileMetadataAccessor)
        {
            _fileMetadataAccessor = fileMetadataAccessor;
        }

        public string Get(string key)
        {
            if (string.Equals(key, typeof(Foo).FullName, StringComparison.OrdinalIgnoreCase))
                return _fileMetadataAccessor.GetMetadataContent(typeof(Foo));
            if (string.Equals(key, typeof(Bar).FullName, StringComparison.OrdinalIgnoreCase))
                return _fileMetadataAccessor.GetMetadataContent(typeof(Bar));
            if (string.Equals(key, typeof(BeanWithWrongDefinedMetadata).FullName, StringComparison.OrdinalIgnoreCase))
                return _fileMetadataAccessor.GetMetadataContent(typeof(BeanWithWrongDefinedMetadata));
            return null;
        }

        public void Set(string key, string content)
        {
        }

        public void Remove(string key)
        {
        }
    }
}