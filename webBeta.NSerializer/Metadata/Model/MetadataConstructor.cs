using System;
using System.Collections.Generic;
using System.Linq;
using webBeta.NSerializer.Base.Types;
using YamlDotNet.RepresentationModel;

namespace webBeta.NSerializer.Metadata.Model
{
    public class MetadataConstructor
    {
        private const string KEY_VIRTUAL_PROPERTIES = "virtual_properties";
        private const string KEY_PROPERTIES = "properties";
        private const string KEY_ACCESS_TYPE = "access_type";
        private const string KEY_ACCESSOR = "accessor";
        private const string KEY_SERIALIZED_NAME = "serialized_name";

        private const string KEY_GROUPS = "groups";
        private const string KEY_GETTER = "getter";

        public static Metadata Build(YamlDocument document)
        {
            if (document == null) return null;

            var rootMapping = (YamlMappingNode) document.RootNode;
            var root = rootMapping.Children.Select(child => child.Key).FirstOrDefault();

            var canonicalName = root?.ToString();
            var modifiers = (YamlMappingNode) rootMapping.Children[new YamlScalarNode(canonicalName)];

            var metadata = new Metadata(canonicalName);

            foreach (var (key, value) in modifiers)
                switch (key.ToString())
                {
                    case KEY_PROPERTIES:
                    {
                        var properties =
                            BuildProperties((YamlMappingNode) value, false);
                        metadata.SetProperties(properties.Select(property => (MetadataProperty) property).ToList());
                        break;
                    }
                    case KEY_ACCESS_TYPE:
                        Enum.TryParse(value.ToString(), true, out FieldAccessType fieldAccessType);
                        metadata.SetAccessType(fieldAccessType);
                        break;
                    case KEY_VIRTUAL_PROPERTIES:
                    {
                        var properties =
                            BuildProperties((YamlMappingNode) value, true);
                        metadata.SetVirtualProperties(properties.Select(property => (MetadataVirtualProperty) property)
                            .ToList());
                        break;
                    }
                }

            return metadata;
        }

        private static List<IMetadataProperty> BuildProperties(YamlMappingNode map, bool asVirtualProperties)
        {
            var properties = new List<IMetadataProperty>();

            foreach (var (propertyName, args) in map)
            {
                var metadataProperty = asVirtualProperties
                    ? new MetadataVirtualProperty(propertyName.ToString())
                    : new MetadataProperty(propertyName.ToString());

                foreach (var (key, value) in (YamlMappingNode) args)
                {
                    switch (key.ToString())
                    {
                        case KEY_GROUPS:
                            var groups = (from @group in (YamlSequenceNode) value select @group.ToString()).ToList();
                            metadataProperty.SetGroups(groups);
                            break;
                        case KEY_SERIALIZED_NAME:
                            metadataProperty.SetSerializedName(value.ToString());
                            break;
                    }

                    if (!asVirtualProperties)
                        switch (key.ToString())
                        {
                            case KEY_ACCESS_TYPE:
                                Enum.TryParse(value.ToString(), true, out FieldAccessType fieldAccessType);
                                metadataProperty.SetAccessType(fieldAccessType);
                                break;
                            case KEY_ACCESSOR:
                                var accessor =
                                    BuildPropertyAccessor(metadataProperty, (YamlMappingNode) value);
                                metadataProperty.SetAccessor(accessor);
                                break;
                        }
                }

                properties.Add(metadataProperty);
            }

            return properties;
        }

        private static MetadataPropertyAccessor BuildPropertyAccessor(IMetadataProperty metadataProperty,
            YamlMappingNode map)
        {
            if (metadataProperty.GetAccessType() != FieldAccessType.PUBLIC_METHOD) return null;

            string getter = null;
            foreach (var (key, value) in map)
                if (key.ToString().Equals(KEY_GETTER))
                    getter = value.ToString();
            return getter == null ? null : new MetadataPropertyAccessor(getter);
        }
    }
}