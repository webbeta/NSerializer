using System;
using System.Collections;
using System.Numerics;
using webBeta.NSerializer.Metadata.Provider;

namespace webBeta.NSerializer.Base
{
    public class TypeChecker
    {
        private readonly ISerializerMetadataProvider _provider;

        public TypeChecker(ISerializerMetadataProvider provider)
        {
            _provider = provider;
        }

        public bool IsByte<T>(T value)
        {
            return value is byte;
        }

        public bool IsShort<T>(T value)
        {
            return value is short;
        }

        public bool IsInteger<T>(T value)
        {
            return value is int;
        }

        public bool IsBigInteger<T>(T value)
        {
            return value is BigInteger;
        }

        public bool IsLong<T>(T value)
        {
            return value is long;
        }

        public bool IsFloat<T>(T value)
        {
            return value is float;
        }

        public bool IsDouble<T>(T value)
        {
            return value is double;
        }

        public bool IsBigDecimal<T>(T value)
        {
            return value is decimal;
        }

        public bool IsNumeric<T>(T value)
        {
            return IsByte(value) ||
                   IsShort(value) ||
                   IsInteger(value) ||
                   IsBigInteger(value) ||
                   IsLong(value) ||
                   IsFloat(value) ||
                   IsDouble(value) ||
                   IsBigDecimal(value);
        }

        public bool IsString<T>(T value)
        {
            return value is string;
        }

        public bool IsStringParseable<T>(T value)
        {
            return value is Enum || value is Guid;
        }

        public bool IsBool<T>(T value)
        {
            return value is bool;
        }

        public bool IsDate<T>(T value)
        {
            return value is DateTime;
        }

        public bool IsSerializableObject<T>(T value)
        {
            return !(value is IEnumerable) && _provider.CanProvide(value.GetType());
        }

        public bool IsUnserializableObject<T>(T value)
        {
            return !IsNumeric(value) &&
                   !IsString(value) &&
                   !IsStringParseable(value) &&
                   !IsBool(value) &&
                   !IsDate(value) &&
                   !IsIterable(value) &&
                   !IsMap(value) &&
                   !_provider.CanProvide(value.GetType());
        }

        public bool IsIterable<T>(T value)
        {
            return value is IEnumerable;
        }

        public bool IsMap<T>(T value)
        {
            return value is IDictionary;
        }

        public void Check<T>(T value, ITypeCallback callback)
        {
            if (IsByte(value))
            {
                callback.itsByte(Convert.ToByte(value));
                callback.itsNumeric(value);
            }
            else if (IsShort(value))
            {
                callback.itsShort(Convert.ToInt16(value));
                callback.itsNumeric(value);
            }
            else if (IsInteger(value))
            {
                callback.itsInteger(Convert.ToInt32(value));
                callback.itsNumeric(value);
            }
            else if (IsBigInteger(value))
            {
                callback.itsBigInteger(Convert.ToInt64(value));
                callback.itsNumeric(value);
            }
            else if (IsLong(value))
            {
                callback.itsLong(Convert.ToInt64(value));
                callback.itsNumeric(value);
            }
            else if (IsFloat(value))
            {
                callback.itsFloat(Convert.ToSingle(value));
                callback.itsNumeric(value);
            }
            else if (IsDouble(value))
            {
                callback.itsDouble(Convert.ToDouble(value));
                callback.itsNumeric(value);
            }
            else if (IsBigDecimal(value))
            {
                callback.itsBigDecimal(Convert.ToDecimal(value));
                callback.itsNumeric(value);
            }
            else if (IsString(value))
            {
                callback.itsString(Convert.ToString(value));
            }
            else if (IsStringParseable(value))
            {
                callback.itsStringParseable(value);
            }
            else if (IsBool(value))
            {
                callback.itsBoolean(Convert.ToBoolean(value));
            }
            else if (IsDate(value))
            {
                callback.itsDate(Convert.ToDateTime(value));
            }
            else if (IsUnserializableObject(value))
            {
                callback.itsUnserializableObject(value);
            }
            else if (IsSerializableObject(value))
            {
                callback.itsSerializableObject(value);
            }
            else if (IsIterable(value))
            {
                callback.itsIterable((IEnumerable) value);
            }
            else if (IsMap(value))
            {
                callback.itsMap((IDictionary) value);
            }
        }
    }
}