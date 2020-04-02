using System;
using System.Collections;
using System.Numerics;

namespace webBeta.NSerializer.Base
{
    public interface ITypeCallback
    {
        Func<byte, bool> itsByte(byte value);

        Func<short, bool> itsShort(short value);

        Func<int, bool> itsInteger(int value);

        Func<BigInteger, bool> itsBigInteger(BigInteger value);

        Func<long, bool> itsLong(long value);

        Func<float, bool> itsFloat(float value);

        Func<double, bool> itsDouble(double value);

        Func<decimal, bool> itsBigDecimal(decimal value);

        Func<object, bool> itsNumeric(object value);

        Func<string, bool> itsString(string value);

        Func<object, bool> itsStringParseable(object value);

        Func<bool, bool> itsBoolean(bool value);

        Func<DateTime, bool> itsDate(DateTime value);

        Func<object, bool> itsSerializableObject(object value);

        Func<object, bool> itsUnserializableObject(object value);

        Func<IEnumerable, bool> itsIterable(IEnumerable value);

        Func<IDictionary, bool> itsMap(IDictionary value);
    }
}