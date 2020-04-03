using webBeta.NSerializer.Base;

namespace webBeta.NSerializer.Test.Base
{
    public class MockEnvironment : IEnvironment
    {
        public bool IsProd()
        {
            return false;
        }
    }
}