using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using FluentAssertions;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Configuration;
using webBeta.NSerializer.Metadata;
using webBeta.NSerializer.Metadata.Provider;
using webBeta.NSerializer.Test.Base;
using webBeta.NSerializer.Test.Beans;
using Xunit;

namespace webBeta.NSerializer.Test
{
    public class TypeCheckerTest
    {
        public TypeCheckerTest()
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            var config = new Dictionary<string, object>
            {
                {
                    ConfigurationManager.MetadataDirKey,
                    Path.Combine(currentDirectory.Parent?.Parent?.ToString(), "Resources")
                }
            };

            var configurationProvider = new MockConfigurationProvider(config);

            var environment = new MockEnvironment();

            var fileMetadataAccessor = new FileMetadataAccessor();
            fileMetadataAccessor.SetMetadataPath((string) config[ConfigurationManager.MetadataDirKey]);

            var cache = new MockCache();

            var configurationManager = new ConfigurationManager(configurationProvider, environment, cache);
            _metadataProvider = configurationManager.NewMetadataProvider();
        }

        private static ISerializerMetadataProvider _metadataProvider;

        private enum Sample
        {
            READ,
            WRITE
        }

        [Fact]
        public void test_it_detects_biginteger_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = new BigInteger(130);

            typeChecker.IsBigInteger(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_boolean()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Boolean type = true;
            bool typePrimitive = false;

            typeChecker.IsBool(type).Should().BeTrue();
            typeChecker.IsBool(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_byte_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Byte type = 1;
            byte typePrimitive = 1;

            typeChecker.IsByte(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
            typeChecker.IsByte(typePrimitive).Should().BeTrue();
            typeChecker.IsNumeric(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_date()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = new DateTime();

            typeChecker.IsDate(type).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_double_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Double type = 1.0;
            double typePrimitive = 1.0;

            typeChecker.IsDouble(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
            typeChecker.IsDouble(typePrimitive).Should().BeTrue();
            typeChecker.IsNumeric(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_float_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Single type = 1F;
            float typePrimitive = 1F;

            typeChecker.IsFloat(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
            typeChecker.IsFloat(typePrimitive).Should().BeTrue();
            typeChecker.IsNumeric(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_integer_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Int32 type = 1;
            int typePrimitive = 1;

            typeChecker.IsInteger(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
            typeChecker.IsInteger(typePrimitive).Should().BeTrue();
            typeChecker.IsNumeric(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_iterable()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var typeList = new List<int>();
            string[] typeStringArr = {"a", "b"};

            typeChecker.IsIterable(typeList).Should().BeTrue();
            typeChecker.IsIterable(typeStringArr).Should().BeFalse();
        }

        [Fact]
        public void test_it_detects_long_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Int64 type = 1L;
            long typePrimitive = 1L;

            typeChecker.IsLong(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
            typeChecker.IsLong(typePrimitive).Should().BeTrue();
            typeChecker.IsNumeric(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_map()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = new Dictionary<int, int>();

            typeChecker.IsMap(type).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_serializable_object()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = new Foo();

            typeChecker.IsSerializableObject(type).Should().BeTrue();
            typeChecker.IsUnserializableObject(type).Should().BeFalse();
        }

        [Fact]
        public void test_it_detects_short_as_numeric()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            Int16 type = 1;
            short typePrimitive = 1;

            typeChecker.IsShort(type).Should().BeTrue();
            typeChecker.IsNumeric(type).Should().BeTrue();
            typeChecker.IsShort(typePrimitive).Should().BeTrue();
            typeChecker.IsNumeric(typePrimitive).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_string()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = "foo";

            typeChecker.IsString(type).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_string_parseable()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var typeUUID = Guid.NewGuid();
            var typeEnum = Sample.READ;

            typeChecker.IsStringParseable(typeUUID).Should().BeTrue();
            typeChecker.IsStringParseable(typeEnum).Should().BeTrue();
        }

        [Fact]
        public void test_it_detects_unserializable_object_with_not_defined_metadata_object()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = new BeanWithoutMetadata();

            typeChecker.IsUnserializableObject(type).Should().BeTrue();
            typeChecker.IsSerializableObject(type).Should().BeFalse();
        }

        [Fact]
        public void test_it_detects_unserializable_object_with_wrong_defined_metadata_object()
        {
            var typeChecker = new TypeChecker(_metadataProvider);

            var type = new BeanWithWrongDefinedMetadata();

            typeChecker.IsUnserializableObject(type).Should().BeTrue();
            typeChecker.IsSerializableObject(type).Should().BeFalse();
        }
    }
}