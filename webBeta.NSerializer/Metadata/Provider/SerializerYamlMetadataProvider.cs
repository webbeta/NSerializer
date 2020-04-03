using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Metadata.Model;
using YamlDotNet.RepresentationModel;

namespace webBeta.NSerializer.Metadata.Provider
{
    public class SerializerYamlMetadataProvider : ISerializerMetadataProvider
    {
        private readonly IMetadataAccessor _metadataAccessor;
        private readonly Dictionary<string, Model.Metadata> _metadatas;

        public SerializerYamlMetadataProvider(IMetadataAccessor metadataAccessor)
        {
            _metadataAccessor = metadataAccessor;
            _metadatas = new Dictionary<string, Model.Metadata>();
        }

        public bool CanProvide(Type klass)
        {
            InitializeMetadata(klass);

            return _metadataAccessor.HasMetadata(klass) &&
                   ContainsMetadata(klass);
        }

        public string[] GetPropertiesByGroup(Type klass, IParentFieldData parentData, params string[] group)
        {
            InitializeMetadata(klass);

            if (!ContainsMetadata(klass))
                return new string[0];

            var properties = new List<string>();

            var metadata = GetMetadata(klass);

            if (metadata.HasProperties())
                properties.AddRange(from property in metadata.GetProperties()
                    where parentData == null || !parentData.IsRecursive(klass, property.GetPropertyName())
                    where property.HasGroups()
                    where property.AppliesToGroups(new List<string>(@group))
                    select property.GetPropertyName());

            if (metadata.HasVirtualProperties())
                properties.AddRange(from property in metadata.GetVirtualProperties()
                    where parentData == null || !parentData.IsRecursive(klass, property.GetPropertyName())
                    where property.HasGroups()
                    where property.AppliesToGroups(new List<string>(@group))
                    select property.GetPropertyName());

            return properties.ToArray();
        }

        public string[] GetGroupsByFieldName(Type klass, string fieldName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(fieldName);
            return property.GetGroups().ToArray();
        }

        public bool HasPropertyAccessType(Type klass, string propertyName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(propertyName);
            return property.HasAccessType();
        }

        public FieldAccessType GetPropertyAccessType(Type klass, string propertyName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(propertyName);
            return property.GetAccessType();
        }

        public string GetPropertyCustomGetterName(Type klass, string propertyName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(propertyName);
            var accessor = property.GetAccessor();
            if (accessor == null || !accessor.hasGetter())
                return null;
            return accessor.getGetter();
        }

        public bool HasPropertySerializedName(Type klass, string propertyName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(propertyName);
            return property.HasSerializedName();
        }

        public string GetPropertySerializedName(Type klass, string propertyName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(propertyName);
            return property.GetSerializedName();
        }

        public bool IsVirtualProperty(Type klass, string propertyName)
        {
            var metadata = GetMetadata(klass);
            var property = metadata.GetMixedProperty(propertyName);
            return property is MetadataVirtualProperty;
        }

        public bool HasAccessType(Type klass)
        {
            var metadata = GetMetadata(klass);
            return metadata.HasAccessType();
        }

        public FieldAccessType GetAccessType(Type klass)
        {
            var metadata = GetMetadata(klass);
            return metadata.GetAccessType();
        }

        private void InitializeMetadata(Type klass)
        {
            if (_metadatas.ContainsKey(klass.FullName))
                return;

            var yaml = new YamlStream();

            if (!_metadataAccessor.HasMetadata(klass))
            {
                PutNullMetadata(klass);
                return;
            }

            var yamlFileContents = _metadataAccessor.GetMetadataContent(klass);
            yaml.Load(new StringReader(yamlFileContents));

            var metadata = yaml.Documents.Count > 0 ? MetadataConstructor.Build(yaml.Documents.First()) : null;

            if (metadata == null || !metadata.AppliesTo(klass.FullName))
            {
                PutNullMetadata(klass);
                return;
            }

            _metadatas.Add(klass.FullName, metadata);
        }

        private void PutNullMetadata(Type klass)
        {
            _metadatas.Add(klass.FullName, null);
        }

        private bool ContainsMetadata(Type klass)
        {
            return _metadatas.ContainsKey(klass.FullName) &&
                   _metadatas[klass.FullName] != null;
        }

        private Model.Metadata GetMetadata(Type klass)
        {
            return _metadatas[klass.FullName];
        }
    }
}