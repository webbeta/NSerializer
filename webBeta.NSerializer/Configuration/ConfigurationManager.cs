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
        public const string METADATA_DIR_KEY = "serializer.metadata.dir";
        public const string INCLUDE_NULL_VALUES_KEY = "serializer.include_null_values";
        public const string FIELD_FORMATTING_METHOD_KEY = "serializer.field_formatting_method";
        public const string FIELD_ACCESS_TYPE_KEY = "serializer.access_type";
        public const string DATE_FORMAT_KEY = "serializer.date_format";
        private readonly FieldAccessType accessType;
        private readonly DateFormatType dateFormatType;
        private readonly FieldFormatterType formatterType;
        private readonly bool includeNullValues;

        private readonly IMetadataAccessor metadataAccessor;

        private readonly string metadataPath;

        public ConfigurationManager(
            IConfigurationProvider conf,
            IEnvironment environment,
            ICache cache
        )
        {
            metadataPath = conf.GetString(METADATA_DIR_KEY, "conf/serializer/");
            includeNullValues = conf.GetBoolean(INCLUDE_NULL_VALUES_KEY, false);
            Enum.TryParse(conf.GetString(FIELD_FORMATTING_METHOD_KEY, FieldFormatterType.LOWER_UNDERSCORE.ToString()),
                true, out formatterType);
            ;
            Enum.TryParse(conf.GetString(FIELD_ACCESS_TYPE_KEY, FieldAccessType.PROPERTY.ToString()), true,
                out accessType);
            ;
            Enum.TryParse(conf.GetString(DATE_FORMAT_KEY, DateFormatType.ISO8601.ToString()), true, out dateFormatType);

            if (environment.IsProd())
                metadataAccessor = new CacheMetadataAccessor(cache);
            else
                metadataAccessor = new FileMetadataAccessor();

            metadataAccessor.SetMetadataPath(metadataPath);
        }

        public string GetMetadataPath()
        {
            return metadataPath;
        }

        public bool GetIncludeNullValues()
        {
            return includeNullValues;
        }

        public IMetadataAccessor GetMetadataAccessor()
        {
            return metadataAccessor;
        }

        public ISerializerMetadataProvider NewMetadataProvider()
        {
            return new SerializerYamlMetadataProvider(metadataAccessor);
        }

        public IFieldFormatter GetFieldFormatter()
        {
            return new FieldFormatter(formatterType);
        }

        public FieldAccessType GetAccessType()
        {
            return accessType;
        }

        public DateFormatType GetDateFormatType()
        {
            return dateFormatType;
        }
    }
}