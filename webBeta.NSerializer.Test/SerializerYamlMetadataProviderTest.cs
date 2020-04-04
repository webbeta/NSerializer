using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using webBeta.NSerializer.Configuration;
using webBeta.NSerializer.Metadata;
using webBeta.NSerializer.Test.Base;
using webBeta.NSerializer.Test.Beans;
using Xunit;

namespace webBeta.NSerializer.Test
{
    public class SerializerYamlMetadataProviderTest
    {
        public static ConfigurationManager BuildAs(bool isProd)
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            var config = new Dictionary<string, object>
            {
                {
                    ConfigurationManager.METADATA_DIR_KEY,
                    Path.Combine(currentDirectory.Parent?.Parent?.ToString(), "Resources", "provider_metadatas")
                }
            };

            var configurationProvider = new MockConfigurationProvider(config);

            var environment = new MockEnvironment(isProd);

            var fileMetadataAccessor = new FileMetadataAccessor();
            fileMetadataAccessor.SetMetadataPath((string) config[ConfigurationManager.METADATA_DIR_KEY]);

            var cache = new MockProdCache(fileMetadataAccessor);

            return new ConfigurationManager(configurationProvider, environment, cache);
        }

        [Fact]
        public void test_can_provide_metadata_for_bean_defined_by_yaml()
        {
            var configurationManager = BuildAs(false);
            var provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is FileMetadataAccessor).Should().BeTrue();
            provider.CanProvide(typeof(Foo)).Should().BeTrue();

            configurationManager = BuildAs(true);
            provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is CacheMetadataAccessor).Should().BeTrue();
            provider.CanProvide(typeof(Foo)).Should().BeTrue();
        }

        [Fact]
        public void test_can_provide_metadata_for_bean_defined_by_yml()
        {
            var configurationManager = BuildAs(false);
            var provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is FileMetadataAccessor).Should().BeTrue();
            provider.CanProvide(typeof(Bar)).Should().BeTrue();

            configurationManager = BuildAs(true);
            provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is CacheMetadataAccessor).Should().BeTrue();
            provider.CanProvide(typeof(Bar)).Should().BeTrue();
        }

        [Fact]
        public void test_can_provide_metadata_for_bean_defined_with_correct_filename_but_bad_declaration()
        {
            var configurationManager = BuildAs(false);
            var provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is FileMetadataAccessor).Should().BeTrue();
            provider.CanProvide(typeof(BeanWithWrongDefinedMetadata)).Should().BeFalse();

            configurationManager = BuildAs(true);
            provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is CacheMetadataAccessor).Should().BeTrue();
            provider.CanProvide(typeof(BeanWithWrongDefinedMetadata)).Should().BeFalse();
        }

        [Fact]
        public void test_provide_correct_properties_by_groups_for_metadata()
        {
            var configurationManager = BuildAs(false);
            var provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is FileMetadataAccessor).Should().BeTrue();
            provider.GetPropertiesByGroup(typeof(Foo), null, "grupo").Should().BeEquivalentTo("id", "fooField");

            configurationManager = BuildAs(true);
            provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is CacheMetadataAccessor).Should().BeTrue();
            provider.GetPropertiesByGroup(typeof(Foo), null, "grupo").Should().BeEquivalentTo("id", "fooField");
        }

        [Fact]
        public void test_provide_correct_properties_by_groups_for_wrong_metadata()
        {
            var configurationManager = BuildAs(false);
            var provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is FileMetadataAccessor).Should().BeTrue();
            provider.GetPropertiesByGroup(typeof(BeanWithWrongDefinedMetadata), null, "grupo").Should()
                .BeEquivalentTo();

            configurationManager = BuildAs(true);
            provider = configurationManager.NewMetadataProvider();

            (configurationManager.GetMetadataAccessor() is CacheMetadataAccessor).Should().BeTrue();
            provider.GetPropertiesByGroup(typeof(BeanWithWrongDefinedMetadata), null, "grupo").Should()
                .BeEquivalentTo();
        }
    }
}