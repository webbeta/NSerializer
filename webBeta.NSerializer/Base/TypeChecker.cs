using System;
using System.Collections;
using System.Collections.Generic;
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
            return !IsIterable(value) && _provider.CanProvide(value.GetType());
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
            return value is IList &&
                   value.GetType().IsGenericType &&
                   value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public bool IsMap<T>(T value)
        {
            return value is IDictionary &&
                   value.GetType().IsGenericType &&
                   value.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        public void Check<T>(T value,
            Action<T> itsScalar,
            Action<IDictionary> itsDictionary,
            Action<IEnumerable> itsEnumerable,
            Action<DateTime> itsDate,
            Action<T> itsSerializableObject)
        {
            if (IsIterable(value))
                itsEnumerable((IEnumerable) value);
            else if (IsMap(value))
                itsDictionary((IDictionary) value);
            else if (IsSerializableObject(value))
                itsSerializableObject(value);
            else if (IsDate(value))
                itsDate(Convert.ToDateTime(value));
            else if (IsStringParseable(value))
                itsScalar((T) Convert.ChangeType(value.ToString(), typeof(T)));
            else
                itsScalar(value);
        }
    }
}