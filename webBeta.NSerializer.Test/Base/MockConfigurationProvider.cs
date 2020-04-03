using System.Collections.Generic;
using webBeta.NSerializer.Base;

namespace webBeta.NSerializer.Test.Base
{
    public class MockConfigurationProvider : IConfigurationProvider
    {
        private readonly Dictionary<string, object> _conf;

        public MockConfigurationProvider(Dictionary<string, object> conf)
        {
            _conf = conf;
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            return _conf[key] == null ? defaultValue : bool.Parse(_conf[key].ToString());
        }

        public string GetString(string key, string defaultValue)
        {
            return _conf[key] == null ? defaultValue : _conf[key].ToString();
        }
    }
}