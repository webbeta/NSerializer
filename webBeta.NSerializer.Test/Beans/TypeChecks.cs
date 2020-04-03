using System;
using System.Numerics;

namespace webBeta.NSerializer.Test.Beans
{
    public class TypeChecks
    {
        private BigInteger bigInt = new BigInteger(500);

        private readonly DateTime fooDate;
        private FooEnum fooEnum = FooEnum.A;

        private string fooString = "Foo";
        private Guid fooUUID = Guid.Parse("8c748766-df38-421b-9eb2-0f0d2ffa2299");
        private bool objectBoolean = true;
        private byte objectByte = 1;
        private double objectDouble = 500.1;
        private float objectFloat = 500.1F;
        private int objectInt = 500;
        private long objectLong = 500L;
        private short objectShort = 50;

        private bool primitiveBoolean = false;

        private byte primitiveByte = 0;
        private double primitiveDouble = 500.1;
        private float primitiveFloat = 500.1F;
        private int primitiveInt = 500;
        private long primitiveLong = 500L;
        private short primitiveShort = 50;

        public TypeChecks()
        {
            var cal = new DateTime(2017, 8, 22, 10, 11, 12, 112, DateTimeKind.Utc);
            fooDate = cal;
        }

        public DateTime getFooDate()
        {
            return fooDate;
        }

        private enum FooEnum
        {
            A,
            B,
            C
        }
    }
}