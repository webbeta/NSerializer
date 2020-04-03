using System;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Metadata.Model
{
    public class MetadataVirtualProperty : MetadataProperty, IMetadataProperty
    {
        public MetadataVirtualProperty(string virtualPropertyName) : base(virtualPropertyName)
        {
            AccessType = FieldAccessType.PUBLIC_METHOD;
            Accessor = new MetadataPropertyAccessor(virtualPropertyName);
        }

        public new void SetAccessType(FieldAccessType accessType)
        {
            throw new ArgumentException("Virtual properties have preset access type as PUBLIC_METHOD.");
        }

        public new void SetAccessor(MetadataPropertyAccessor accessor)
        {
            throw new ArgumentException("Virtual properties have preset accessor as its name.");
        }
    }
}