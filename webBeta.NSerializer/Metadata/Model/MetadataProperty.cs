using System.Collections.Generic;
using System.Linq;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Metadata.Model
{
    public class MetadataProperty : IMetadataProperty
    {
        protected MetadataPropertyAccessor Accessor;
        protected FieldAccessType? AccessType;
        private List<string> _groups;

        private readonly string _propertyName;
        private string _serializedName;

        public MetadataProperty(string propertyName)
        {
            _propertyName = propertyName;
        }

        public string GetPropertyName()
        {
            return _propertyName;
        }

        public bool HasAccessType()
        {
            return AccessType != null;
        }

        public FieldAccessType GetAccessType()
        {
            return AccessType.Value;
        }

        public void SetAccessType(FieldAccessType accessType)
        {
            AccessType = accessType;
        }

        public MetadataPropertyAccessor GetAccessor()
        {
            return Accessor;
        }

        public void SetAccessor(MetadataPropertyAccessor accessor)
        {
            Accessor = accessor;
        }

        public bool HasSerializedName()
        {
            return _serializedName != null;
        }

        public string GetSerializedName()
        {
            return _serializedName;
        }

        public void SetSerializedName(string serializedName)
        {
            _serializedName = serializedName;
        }

        public bool HasGroups()
        {
            return _groups.Count > 0;
        }

        public bool AppliesToGroups(List<string> wantedGroups)
        {
            return _groups.Intersect(wantedGroups).Any();
        }

        public List<string> GetGroups()
        {
            return _groups;
        }

        public void SetGroups(List<string> groups)
        {
            _groups = groups;
        }
    }
}