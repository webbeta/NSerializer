using System.Collections.Generic;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Metadata.Model
{
    public class Metadata
    {
        private readonly string _canonicalName;
        private FieldAccessType? _accessType;
        private List<MetadataProperty> _properties = new List<MetadataProperty>();
        private List<MetadataVirtualProperty> _virtualProperties = new List<MetadataVirtualProperty>();

        public Metadata(string canonicalName)
        {
            _canonicalName = canonicalName;
        }

        public bool AppliesTo(string canonicalName)
        {
            return _canonicalName.Equals(canonicalName);
        }

        public string GetCanonicalName()
        {
            return _canonicalName;
        }

        public bool HasAccessType()
        {
            return _accessType != null;
        }

        public FieldAccessType GetAccessType()
        {
            return _accessType.Value;
        }

        public void SetAccessType(FieldAccessType accessType)
        {
            _accessType = accessType;
        }

        public bool HasVirtualProperties()
        {
            return _virtualProperties.Count > 0;
        }

        public List<MetadataVirtualProperty> GetVirtualProperties()
        {
            return _virtualProperties;
        }

        public bool HasVirtualProperty(string name)
        {
            return GetVirtualProperty(name) != null;
        }

        public MetadataVirtualProperty GetVirtualProperty(string name)
        {
            var props = _virtualProperties
                .FindAll(prop => prop.GetPropertyName().Equals(name));

            return props.Count == 0 ? null : props[0];
        }

        public void SetVirtualProperties(List<MetadataVirtualProperty> virtualProperties)
        {
            _virtualProperties = virtualProperties;
        }

        public bool HasProperties()
        {
            return _properties.Count > 0;
        }

        public List<MetadataProperty> GetProperties()
        {
            return _properties;
        }

        public bool HasProperty(string name)
        {
            return GetProperty(name) != null;
        }

        public MetadataProperty GetProperty(string name)
        {
            var props = _properties
                .FindAll(prop => prop.GetPropertyName().Equals(name));

            return props.Count == 0 ? null : props[0];
        }

        public void SetProperties(List<MetadataProperty> properties)
        {
            _properties = properties;
        }

        public bool HasMixedProperties()
        {
            return HasProperties() || HasVirtualProperties();
        }

        public List<MetadataProperty> GetMixedProperties()
        {
            var merged = new List<MetadataProperty>();
            merged.AddRange(_properties);
            merged.AddRange(_virtualProperties);
            return merged;
        }

        public bool HasMixedProperty(string name)
        {
            return GetMixedProperty(name) != null;
        }

        public MetadataProperty GetMixedProperty(string name)
        {
            var mixedProps = GetMixedProperties();

            var props = mixedProps
                .FindAll(prop => prop.GetPropertyName().Equals(name));

            return props.Count == 0 ? null : props[0];
        }
    }
}