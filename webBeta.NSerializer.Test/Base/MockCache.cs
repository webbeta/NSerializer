using webBeta.NSerializer.Base;

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
}