using webBeta.NSerializer.Base;

namespace webBeta.NSerializer.Test.Base
{
    public class MockEnvironment : IEnvironment
    {
        private readonly bool _isProd = false;

        public MockEnvironment()
        {
        }

        public MockEnvironment(bool isProd)
        {
            _isProd = isProd;
        }
        
        public bool IsProd()
        {
            return _isProd;
        }
    }
}