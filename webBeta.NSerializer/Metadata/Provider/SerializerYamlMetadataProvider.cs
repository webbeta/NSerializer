using System;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Metadata.Provider
{
    public class SerializerYamlMetadataProvider : ISerializerMetadataProvider
    {
        
        private readonly IMetadataAccessor _metadataAccessor;

        public SerializerYamlMetadataProvider(IMetadataAccessor metadataAccessor)
        {
            _metadataAccessor = metadataAccessor;
        }

        public bool CanProvide(Type klass)
        {
            throw new NotImplementedException();
        }

        public string[] GetPropertiesByGroup(Type klass, IParentFieldData parentData, params string[] @group)
        {
            throw new NotImplementedException();
        }

        public string[] GetGroupsByFieldName(Type klass, string fieldName)
        {
            throw new NotImplementedException();
        }

        public bool HasPropertyAccessType(Type klass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public FieldAccessType GetPropertyAccessType(Type klass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public string GetPropertyCustomGetterName(Type klass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasPropertySerializedName(Type klass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public string GetPropertySerializedName(Type klass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool IsVirtualProperty(Type klass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool HasAccessType(Type klass)
        {
            throw new NotImplementedException();
        }

        public FieldAccessType GetAccessType(Type klass)
        {
            throw new NotImplementedException();
        }
    }
}