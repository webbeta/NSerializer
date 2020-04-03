using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Configuration;
using webBeta.NSerializer.Test.Base;
using webBeta.NSerializer.Test.Beans;
using Xunit;

namespace webBeta.NSerializer.Test
{
    public class NSerializerTest
    {
        private static NSerializer GetSerializerAs(FieldFormatterType type, FieldAccessType accessType,
            DateFormatType dateFormatType, bool withNulls)
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            var config = new Dictionary<string, object>
            {
                {ConfigurationManager.METADATA_DIR_KEY, $"{currentDirectory.Parent?.Parent}\\Resources"},
                {ConfigurationManager.INCLUDE_NULL_VALUES_KEY, withNulls},
                {ConfigurationManager.FIELD_FORMATTING_METHOD_KEY, type.ToString()},
                {ConfigurationManager.FIELD_ACCESS_TYPE_KEY, accessType.ToString()},
                {ConfigurationManager.DATE_FORMAT_KEY, dateFormatType.ToString()}
            };

            var configurationProvider = new MockConfigurationProvider(config);
            var environment = new MockEnvironment();
            var cache = new MockCache();

            return NSerializerBuilder.Build()
                .WithCache(cache)
                .WithConfigurationProvider(configurationProvider)
                .WithEnvironment(environment)
                .Get();
        }

        [Fact]
        public void test_bean_fields_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "c");

            var mockMap = new JObject {{"fooField", "Random string"}};

            var mockSubMap = new JObject {{"barField", "Random string"}};

            mockMap.Add("bar", mockSubMap);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_bean_fields_prints_correct_serialization_excluding_child_beans_with_wrong_metadata()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "only_correct_metadata");

            var mockMap = new JObject {{"id", 654}};

            var mockSubMap = new JObject {{"barField", "Random string"}};

            mockMap.Add("bar", mockSubMap);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_bean_fields_prints_correct_serialization_including_null_values()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "c", "null_group");

            var mockMap = new JObject {{"fooField", "Random string"}};

            var mockSubMap = new JObject {{"barField", "Random string"}, {"nullField", null}};

            mockMap.Add("bar", mockSubMap);

            mockMap.Add("nullField", null);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_bean_fields_prints_correct_serialization_including_null_values_only_if_field_exists()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "c", "null_group_unexistent");

            var mockMap = new JObject {{"fooField", "Random string"}};

            var mockSubMap = new JObject {{"barField", "Random string"}};

            mockMap.Add("bar", mockSubMap);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_bean_fields_prints_correct_serialization_NOT_including_null_values()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, false);

            var parsed = serializer.Serialize(new Foo(), "c", "null_group");

            var mockMap = new JObject {{"fooField", "Random string"}};

            var mockSubMap = new JObject {{"barField", "Random string"}};

            mockMap.Add("bar", mockSubMap);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_bean_with_metadata_file_but_no_content_returns_empty_object()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new EmptyMetadata(), "a");

            parsed.Should().BeEquivalentTo("{}");
        }

        [Fact]
        public void test_bean_with_wrong_defined_metadata_returns_empty_object()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new BeanWithWrongDefinedMetadata(), "a");

            parsed.Should().BeEquivalentTo("{}");
        }

        [Fact]
        public void test_bean_without_metadata_returns_empty_object()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new BeanWithoutMetadata(), "a");

            parsed.Should().BeEquivalentTo("{}");
        }

        [Fact]
        public void test_check_boolean_primitive_types()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new TypeChecks();

            var parsed = serializer.Serialize(foo, "boolean");

            var mock = new JObject {{"primitiveBoolean", false}, {"objectBoolean", true}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_check_date_primitive_types_USING_ISO_DATE_FORMAT()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new TypeChecks();

            var parsed = serializer.Serialize(foo, "date");

            var mock = new JObject {{"fooDate", foo.getFooDate().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'")}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_check_date_primitive_types_USING_UNIX_TIMESTAMP_DATE_FORMAT()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.UNIX_TIMESTAMP, true);

            var foo = new TypeChecks();

            var parsed = serializer.Serialize(foo, "date");

            var mock = new JObject
            {
                {"fooDate", 1503396672112}
            };

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_check_numeric_primitive_types()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new TypeChecks();

            var parsed = serializer.Serialize(foo, "numerics");

            var mock = new JObject
            {
                {"primitiveByte", 0},
                {"objectByte", 1},
                {"primitiveShort", 50},
                {"objectShort", 50},
                {"primitiveInt", 500},
                {"objectInt", 500},
                {"bigInt", 500},
                {"primitiveLong", 500L},
                {"objectLong", 500L},
                {"primitiveDouble", 500.1},
                {"objectDouble", 500.1},
                {"primitiveFloat", 500.1F},
                {"objectFloat", 500.1F}
            };

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_check_string_primitive_types()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new TypeChecks();

            var parsed = serializer.Serialize(foo, "string");

            var mock = new JObject
            {
                {"fooString", "Foo"},
                {"fooEnum", "A"},
                {"fooUUID", "8c748766-df38-421b-9eb2-0f0d2ffa2299"}
            };

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_list_of_bean_fields_prints_correct_serialization_with_upper_underscore()
        {
            var serializer = GetSerializerAs(FieldFormatterType.UPPER_UNDERSCORE, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "list_of_bean", "b");

            var mockMap = new JObject {{"ID", 654}, {"FOO_FIELD", "Random string"}};

            var mockArray = new JArray();

            var mockSubMap1 = new JObject {{"ID", 111}};
            mockArray.Add(mockSubMap1);

            var mockSubMap2 = new JObject {{"ID", 112}};
            mockArray.Add(mockSubMap2);

            mockMap.Add("LIST_OF_BEAN", mockArray);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_list_of_scalar_at_top_using_beans_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var listOfBeans = new List<Foo> {new Foo()};

            var foo = new Foo();
            foo.setId(800);
            listOfBeans.Add(foo);

            var parsed = serializer.Serialize(listOfBeans, "a");

            var mockArray = new JArray();

            var mockMap1 = new JObject {{"id", 654}};

            mockArray.Add(mockMap1);

            var mockMap2 = new JObject {{"id", 800}};

            mockArray.Add(mockMap2);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockArray.ToString(Formatting.None));
        }

        [Fact]
        public void test_list_of_scalar_at_top_using_beans_with_folding_prints_correct_serialization_with_lower_hyphen()
        {
            var serializer = GetSerializerAs(FieldFormatterType.LOWER_HYPHEN, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var listOfBeans = new List<Foo> {new Foo()};

            var foo = new Foo {fooField = "Another string"};
            listOfBeans.Add(foo);

            var parsed = serializer.Serialize(listOfBeans, "c");

            var mockArray = new JArray();

            var mockMap1 = new JObject {{"foo-field", "Random string"}};

            var mockSubMap1 = new JObject {{"bar-field", "Random string"}};

            mockMap1.Add("bar", mockSubMap1);

            mockArray.Add(mockMap1);

            var mockMap2 = new JObject {{"foo-field", "Another string"}};

            var mockSubMap2 = new JObject {{"bar-field", "Random string"}};

            mockMap2.Add("bar", mockSubMap2);

            mockArray.Add(mockMap2);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockArray.ToString(Formatting.None));
        }

        [Fact]
        public void test_list_of_scalar_at_top_using_scalar_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var listOfScalar = new List<string> {"A", "B"};

            var parsed = serializer.Serialize(listOfScalar);

            var mockArray = new JArray {"A", "B"};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockArray.ToString(Formatting.None));
        }

        [Fact]
        public void test_list_of_scalar_fields_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "list");

            var mockMap = new JObject();
            var mockArray = new JArray {"A", "B"};
            mockMap.Add("list", mockArray);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_map_of_scalar_fields_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "map");

            var mockMap = new JObject();
            var mockMapValue = new JObject
            {
                {"bar", 60.12},
                {"foo", 50.1}
            };
            mockMap.Add("map", mockMapValue);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_null_objects_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize<object>(null, "a_group");

            parsed.Should().BeNull();
        }

        [Fact]
        public void test_object_with_list_of_same_object_serializes_children_map_using_parent_groups()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new Foo();

            var childFoo1 = new Foo();
            childFoo1.setId(1);

            foo.addRecursiveMapItem("random_key_1", childFoo1);

            var childFoo2 = new Foo();
            childFoo2.setId(2);

            foo.addRecursiveMapItem("random_key_2", childFoo2);

            var parsed = serializer.Serialize(foo, "a", "recursive_foo_map");

            var childsJson = new JObject();

            var childMock1 = new JObject {{"fooField", "Random string"}};

            childsJson.Add("random_key_1", childMock1);

            var childMock2 = new JObject {{"fooField", "Random string"}};

            var mock = new JObject {{"id", 654}, {"fooField", "Random string"}, {"recursiveMap", childsJson}};

            childsJson.Add("random_key_2", childMock2);

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_object_with_list_of_same_object_serializes_children_using_parent_groups()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new Foo();

            var childFoo1 = new Foo();
            childFoo1.setId(1);

            foo.addRecursiveListItem(childFoo1);

            var childFoo2 = new Foo();
            childFoo2.setId(2);

            foo.addRecursiveListItem(childFoo2);

            var parsed = serializer.Serialize(foo, "a", "recursive_foo");

            var childsJson = new JArray();

            var childMock1 = new JObject {{"fooField", "Random string"}};

            childsJson.Add(childMock1);

            var childMock2 = new JObject {{"fooField", "Random string"}};

            childsJson.Add(childMock2);

            var mock = new JObject {{"id", 654}, {"fooField", "Random string"}, {"recursiveList", childsJson}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_scalar_fields_prints_correct_serialization()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var parsed = serializer.Serialize(new Foo(), "a");

            var mockMap = new JObject {{"id", 654}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mockMap.ToString(Formatting.None));
        }

        [Fact]
        public void test_simple_serialization_using_getters()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PUBLIC_METHOD,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters();

            var parsed = serializer.Serialize(foo, "a");

            var mock = new JObject {{"foo", "Hello world!"}, {"bar", true}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void
            test_simple_serialization_using_PROPERTY_accessor_at_global_config_but_GETTERS_in_metadata_head_config()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters2();

            var parsed = serializer.Serialize(foo, "a");

            var mock = new JObject {{"foo", "Hello world2!"}, {"bar", false}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void
            test_simple_serialization_using_PROPERTY_accessor_at_global_config_but_GETTERS_in_metadata_head_config_and_PROPERTY_over_property_config()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters3();

            var parsed = serializer.Serialize(foo, "a");

            var mock = new JObject {{"id", 200}, {"foo", "Hello world2!"}, {"bar", false}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_simple_serialization_using_PROPERTY_accessor_at_global_config_but_GETTERS_in_property_config()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters();

            var parsed = serializer.Serialize(foo, "a", "b");

            var mock = new JObject {{"id", 500}, {"foo", "Hello world!"}, {"bar", true}, {"barMethod", "Foo Bar"}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void
            test_simple_serialization_using_PROPERTY_accessor_at_global_config_but_GETTERS_in_property_config_with_custom_accessor()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters();

            var parsed = serializer.Serialize(foo, "a", "c");

            var mock = new JObject
            {
                {"id", 500}, {"foo", "Hello world!"}, {"bar", true}, {"barMethod2", "Foo Bar Foo Bar"}
            };

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_simple_serialization_using_the_SERIALIZED_NAME_property_modifier_over_a_field()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new Foo();

            var parsed = serializer.Serialize(foo, "a", "truncated_name");

            var mock = new JObject {{"id", 654}, {"anotherName", "Hello world!"}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_simple_serialization_using_the_SERIALIZED_NAME_property_modifier_over_a_method()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters2();

            var parsed = serializer.Serialize(foo, "a", "truncated_name");

            var mock = new JObject {{"foo", "Hello world2!"}, {"bar", false}, {"anotherName", "Hello world!"}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void test_simple_serialization_using_virtual_properties()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters();

            var parsed = serializer.Serialize(foo, "fake_group");

            var mock = new JObject {{"id", 500}, {"fake", "I'm fake!"}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }

        [Fact]
        public void
            test_simple_serialization_using_virtual_properties_without_serialized_name_will_get_method_name_as_key()
        {
            var serializer = GetSerializerAs(FieldFormatterType.INHERITED, FieldAccessType.PROPERTY,
                DateFormatType.ISO8601, true);

            var foo = new ClassWithGetters();

            var parsed = serializer.Serialize(foo, "fake_group2");

            var mock = new JObject {{"getGetterWithoutProperty2", "I'm fake!"}};

            parsed.Should().NotBeEmpty();
            parsed.Should().BeEquivalentTo(mock.ToString(Formatting.None));
        }
    }
}