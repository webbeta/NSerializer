using System.IO;
using FluentAssertions;
using webBeta.NSerializer.Metadata;
using webBeta.NSerializer.Test.Beans;
using Xunit;

namespace webBeta.NSerializer.Test
{
    public class FileMetadataAccessorTest
    {
        public FileMetadataAccessorTest()
        {
            var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory());
            _metadataPath = Path.Combine(currentDirectory.Parent?.Parent?.ToString(), "Resources",
                "accessor_metadatas");

            _accessor = new FileMetadataAccessor();
            _accessor.SetMetadataPath(_metadataPath);
        }

        private readonly IMetadataAccessor _accessor;
        private readonly string _metadataPath;

        [Fact]
        public void test_get_correct_contents_for_yaml()
        {
            var content = File.ReadAllText(Path.Combine(_metadataPath, typeof(Foo).FullName + ".yaml"));
            _accessor.GetMetadataContent(typeof(Foo)).Should().BeEquivalentTo(content);
        }

        [Fact]
        public void test_get_correct_contents_for_yml()
        {
            var content = File.ReadAllText(Path.Combine(_metadataPath, typeof(Bar).FullName + ".yml"));
            _accessor.GetMetadataContent(typeof(Bar)).Should().BeEquivalentTo(content);
        }

        [Fact]
        public void test_has_metadata_for_bean_defined_by_yaml()
        {
            _accessor.HasMetadata(typeof(Foo)).Should().BeTrue();
        }

        [Fact]
        public void test_has_metadata_for_bean_defined_by_yml()
        {
            _accessor.HasMetadata(typeof(Bar)).Should().BeTrue();
        }

        [Fact]
        public void test_has_not_metadata_for_undefined_bean()
        {
            _accessor.HasMetadata(typeof(BeanWithoutMetadata)).Should().BeFalse();
        }
    }
}