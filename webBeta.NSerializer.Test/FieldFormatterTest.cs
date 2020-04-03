using System;
using FluentAssertions;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Formatter;
using Xunit;

namespace webBeta.NSerializer.Test
{
    public class FieldFormatterTest
    {
        [Fact]
        public void test_formatter_format_correctly_to_inherited_from_mixed_string()
        {
            var formatter = new FieldFormatter(FieldFormatterType.INHERITED);

            formatter.Format("Hola_donPepito").Should().BeEquivalentTo("Hola_donPepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_inherited_from_upper_camel()
        {
            var formatter = new FieldFormatter(FieldFormatterType.INHERITED);

            formatter.Format("HolaDonPepito").Should().BeEquivalentTo("HolaDonPepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_lower_camel_from_all_upper()
        {
            var formatter = new FieldFormatter(FieldFormatterType.LOWER_CAMEL);

            formatter.Format("HOLADONPEPITO").Should().BeEquivalentTo("hOLADONPEPITO");
        }

        [Fact]
        public void test_formatter_format_correctly_to_lower_camel_from_upper_camel()
        {
            var formatter = new FieldFormatter(FieldFormatterType.LOWER_CAMEL);

            formatter.Format("HolaDonPepito").Should().BeEquivalentTo("holaDonPepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_lower_hyphen_from_mixed_string()
        {
            var formatter = new FieldFormatter(FieldFormatterType.LOWER_HYPHEN);

            formatter.Format("Hola_don-Pepito").Should().BeEquivalentTo("hola-don-pepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_lower_hyphen_from_upper_camel()
        {
            var formatter = new FieldFormatter("LOWER_HYPHEN");

            formatter.Format("HolaDonPepito").Should().BeEquivalentTo("hola-don-pepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_lower_underscore_from_mixed_string()
        {
            var formatter = new FieldFormatter(FieldFormatterType.LOWER_UNDERSCORE);

            formatter.Format("Hola_don-Pepito").Should().BeEquivalentTo("hola_don_pepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_lower_underscore_from_upper_camel()
        {
            var formatter = new FieldFormatter("lower_underscore");

            formatter.Format("HolaDonPepito").Should().BeEquivalentTo("hola_don_pepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_upper_camel_from_all_upper()
        {
            var formatter = new FieldFormatter(FieldFormatterType.UPPER_CAMEL);

            formatter.Format("HOLADONPEPITO").Should().BeEquivalentTo("HOLADONPEPITO");
        }

        [Fact]
        public void test_formatter_format_correctly_to_upper_camel_from_lower_camel()
        {
            var formatter = new FieldFormatter(FieldFormatterType.UPPER_CAMEL);

            formatter.Format("holaDonPepito").Should().BeEquivalentTo("HolaDonPepito");
        }

        [Fact]
        public void test_formatter_format_correctly_to_upper_underscore_from_upper_camel()
        {
            var formatter = new FieldFormatter("UppER_UNDERSCORE");

            formatter.Format("HolaDonPepito").Should().BeEquivalentTo("HOLA_DON_PEPITO");
        }

        [Fact]
        public void test_formatter_throws_exception_with_fields_with_whitespace()
        {
            var formatter = new FieldFormatter(FieldFormatterType.INHERITED);
            formatter.Invoking(y => y.Format("Hola don pepito")).Should().Throw<ArgumentException>();
        }
    }
}