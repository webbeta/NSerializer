using System;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Formatter;
using webBeta.NSerializer.Metadata;
using webBeta.NSerializer.Metadata.Provider;

namespace webBeta.NSerializer.Configuration
{
    public class ConfigurationManager
    {
        public const string MetadataDirKey = "serializer.metadata.dir";
        public const string IncludeNullValuesKey = "serializer.include_null_values";
        public const string FieldFormattingMethodKey = "serializer.field_formatting_method";
        public const string FieldAccessTypeKey = "serializer.access_type";
        public const string DateFormatKey = "serializer.date_format";

        private readonly FieldAccessType _accessType;
        private readonly DateFormatType _dateFormatType;
        private readonly FieldFormatterType _formatterType;
        private readonly bool _includeNullValues;

        private readonly IMetadataAccessor _metadataAccessor;

        private readonly string _metadataPath;

        public ConfigurationManager(
            IConfigurationProvider conf,
            IEnvironment environment,
            ICache cache
        )
        {
            _metadataPath = conf.GetString(MetadataDirKey, "conf/serializer/");
            _includeNullValues = conf.GetBoolean(IncludeNullValuesKey, false);
            Enum.TryParse(conf.GetString(FieldFormattingMethodKey, FieldFormatterType.LOWER_UNDERSCORE.ToString()),
                true, out _formatterType);
            Enum.TryParse(conf.GetString(FieldAccessTypeKey, FieldAccessType.PROPERTY.ToString()), true,
                out _accessType);
            Enum.TryParse(conf.GetString(DateFormatKey, DateFormatType.ISO8601.ToString()), true, out _dateFormatType);

            _metadataAccessor = environment.IsProd() ? new CacheMetadataAccessor(cache) : new FileMetadataAccessor();

            _metadataAccessor.SetMetadataPath(_metadataPath);
        }

        public string GetMetadataPath()
        {
            return _metadataPath;
        }

        public bool GetIncludeNullValues()
        {
            return _includeNullValues;
        }

        public IMetadataAccessor GetMetadataAccessor()
        {
            return _metadataAccessor;
        }

        public ISerializerMetadataProvider NewMetadataProvider()
        {
            return new SerializerYamlMetadataProvider(_metadataAccessor);
        }

        public IFieldFormatter GetFieldFormatter()
        {
            return new FieldFormatter(_formatterType);
        }

        public FieldAccessType GetAccessType()
        {
            return _accessType;
        }

        public DateFormatType GetDateFormatType()
        {
            return _dateFormatType;
        }
    }
}