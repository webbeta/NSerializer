using System;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Metadata.Provider
{
    public interface ISerializerMetadataProvider
    {
        bool CanProvide(Type klass);
        string[] GetPropertiesByGroup(Type klass, IParentFieldData parentData, params string[] group);
        string[] GetGroupsByFieldName(Type klass, string fieldName);
        bool HasPropertyAccessType(Type klass, string propertyName);
        FieldAccessType GetPropertyAccessType(Type klass, string propertyName);
        string GetPropertyCustomGetterName(Type klass, string propertyName);
        bool HasPropertySerializedName(Type klass, string propertyName);
        string GetPropertySerializedName(Type klass, string propertyName);
        bool IsVirtualProperty(Type klass, string propertyName);
        bool HasAccessType(Type klass);
        FieldAccessType GetAccessType(Type klass);
    }
}