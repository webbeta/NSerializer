using System;
using FluentAssertions;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Test.Beans;
using Xunit;

namespace webBeta.NSerializer.Test
{
    public class FieldAccessorTest
    {
        [Fact]
        public void test_field_accessor_access_property_with_value_correctly()
        {
            var cls = new ClassWithGetters();

            var accessor = new FieldAccessor(cls, "id", FieldAccessType.PROPERTY);

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            (obj is Int32).Should().BeTrue();
            ((Int32) obj).Should().Be(500);
        }

        [Fact]
        public void test_field_accessor_access_property_with_NULL_value_correctly()
        {
            var cls = new ClassWithGetters(null);

            var accessor = new FieldAccessor(cls, "id", FieldAccessType.PROPERTY);

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            obj.Should().BeNull();
        }

        [Fact]
        public void test_field_accessor_access_getter_with_value_correctly()
        {
            var cls = new ClassWithGetters();

            var accessor = new FieldAccessor(cls, "foo", FieldAccessType.PUBLIC_METHOD);

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            (obj is String).Should().BeTrue();
            ((string) obj).Should().BeEquivalentTo("Hello world!");

            var accessor2 = new FieldAccessor(cls, "bar", FieldAccessType.PUBLIC_METHOD);

            accessor2.Exists().Should().BeTrue();
            var obj2 = accessor2.Get<object>();
            (obj2 is Boolean).Should().BeTrue();
            ((bool) obj2).Should().BeTrue();
        }

        [Fact]
        public void test_field_accessor_access_getter_with_value_correctly_ensuring_that_field_exists()
        {
            var cls = new ClassWithGetters();

            var accessor = new FieldAccessor(cls, "getterWithoutProperty", FieldAccessType.PUBLIC_METHOD);

            accessor.Exists().Should().BeFalse();
            var obj = accessor.Get<object>();
            ((string) obj).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void test_field_accessor_access_property_with_value_correctly_using_a_custom_getter()
        {
            var cls = new ClassWithGetters();

            var accessor = new FieldAccessor(cls, "barMethod2", FieldAccessType.PUBLIC_METHOD);
            accessor.SetCustomGetterName("getBarMethodBis");

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            (obj is String).Should().BeTrue();
            ((string) obj).Should().BeEquivalentTo("Foo Bar Foo Bar");
        }

        [Fact]
        public void
            test_field_accessor_access_property_with_value_correctly_using_a_custom_getter_as_field_has_no_effect()
        {
            var cls = new ClassWithGetters();

            var accessor = new FieldAccessor(cls, "barMethod3", FieldAccessType.PROPERTY);
            accessor.SetCustomGetterName("getBarMethod3Bis");

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            (obj is String).Should().BeTrue();
            ((string) obj).Should().BeEquivalentTo("Foo");
        }

        [Fact]
        public void test_field_accessor_can_access_property_over_class_parents()
        {
            var cls = new Child();

            var accessor = new FieldAccessor(cls, "childField", FieldAccessType.PROPERTY);

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            (obj is String).Should().BeTrue();
            ((string) obj).Should().BeEquivalentTo("Hello child!");

            accessor = new FieldAccessor(cls, "parentField", FieldAccessType.PROPERTY);

            accessor.Exists().Should().BeTrue();
            var obj2 = accessor.Get<object>();
            (obj2 is String).Should().BeTrue();
            ((string) obj2).Should().BeEquivalentTo("Hello parent!");

            accessor = new FieldAccessor(cls, "grandparentField", FieldAccessType.PROPERTY);

            accessor.Exists().Should().BeTrue();
            var obj3 = accessor.Get<object>();
            (obj3 is String).Should().BeTrue();
            ((string) obj3).Should().BeEquivalentTo("Hello grandparent!");
        }

        [Fact]
        public void test_field_accessor_can_access_getter_over_class_parents()
        {
            var cls = new Child();

            var accessor = new FieldAccessor(cls, "childField", FieldAccessType.PUBLIC_METHOD);

            accessor.Exists().Should().BeTrue();
            var obj = accessor.Get<object>();
            (obj is String).Should().BeTrue();
            ((string) obj).Should().BeEquivalentTo("Hello child!");

            accessor = new FieldAccessor(cls, "parentField", FieldAccessType.PUBLIC_METHOD);

            accessor.Exists().Should().BeTrue();
            var obj2 = accessor.Get<object>();
            (obj2 is String).Should().BeTrue();
            ((string) obj2).Should().BeEquivalentTo("Hello parent!");

            accessor = new FieldAccessor(cls, "grandparentField", FieldAccessType.PUBLIC_METHOD);

            accessor.Exists().Should().BeTrue();
            var obj3 = accessor.Get<object>();
            (obj3 is String).Should().BeTrue();
            ((string) obj3).Should().BeEquivalentTo("Hello grandparent!");
        }
    }
}