using System;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Configuration;

namespace webBeta.NSerializer
{
    public class NSerializerBuilder
    {
        private ICache _cache;

        private IConfigurationProvider _configurationProvider;
        private IEnvironment _environment;
        private ILogger _logger;

        public static NSerializerBuilder Build()
        {
            return new NSerializerBuilder();
        }

        public NSerializerBuilder WithConfigurationProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;

            return this;
        }

        public NSerializerBuilder WithEnvironment(IEnvironment environment)
        {
            _environment = environment;

            return this;
        }

        public NSerializerBuilder WithCache(ICache cache)
        {
            _cache = cache;

            return this;
        }

        public NSerializerBuilder WithLogger(ILogger logger)
        {
            _logger = logger;

            return this;
        }

        public NSerializer Get()
        {
            if (_configurationProvider == null || _environment == null || _cache == null)
                throw new ArgumentException("All services must be settled.");

            var configurationManager =
                new ConfigurationManager(_configurationProvider, _environment, _cache);

            var serializer = new NSerializer(configurationManager);
            serializer.SetLogger(_logger);

            return serializer;
        }
    }
}