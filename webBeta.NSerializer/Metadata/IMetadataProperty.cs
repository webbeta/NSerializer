using System.Collections.Generic;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Metadata.Model;

namespace webBeta.NSerializer.Metadata
{
    public interface IMetadataProperty
    {
        string GetPropertyName();
        bool HasAccessType();
        FieldAccessType GetAccessType();
        void SetAccessType(FieldAccessType accessType);
        MetadataPropertyAccessor GetAccessor();
        void SetAccessor(MetadataPropertyAccessor accessor);
        bool HasSerializedName();
        string GetSerializedName();
        void SetSerializedName(string serializedName);
        bool HasGroups();
        bool AppliesToGroups(List<string> wantedGroups);
        List<string> GetGroups();
        void SetGroups(List<string> groups);
    }
}