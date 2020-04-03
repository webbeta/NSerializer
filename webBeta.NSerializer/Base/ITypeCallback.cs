using System;
using System.Collections;
using System.Numerics;

namespace webBeta.NSerializer.Base
{
    public interface ITypeCallback
    {
        public delegate void itsByte(byte value);

        public delegate void itsShort(short value);

        public delegate void itsInteger(int value);

        public delegate void itsBigInteger(BigInteger value);

        public delegate void itsLong(long value);

        public delegate void itsFloat(float value);

        public delegate void itsDouble(double value);

        public delegate void itsBigDecimal(decimal value);

        public delegate void itsNumeric(object value);

        public delegate void itsString(string value);

        public delegate void itsStringParseable(object value);

        public delegate void itsBoolean(bool value);

        public delegate void itsDate(DateTime value);

        public delegate void itsSerializableObject(object value);

        public delegate void itsUnserializableObject(object value);

        public delegate void itsIterable(IEnumerable value);

        public delegate void itsMap(IDictionary value);
    }
}