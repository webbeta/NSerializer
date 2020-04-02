using System;
using System.Collections;
using System.IO;
using System.Numerics;
using Newtonsoft.Json;
using webBeta.NSerializer.Base;
using webBeta.NSerializer.Base.Types;
using webBeta.NSerializer.Configuration;
using webBeta.NSerializer.Formatter;
using webBeta.NSerializer.Metadata.Provider;

namespace webBeta.NSerializer
{
    public class NSerializer
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly IFieldFormatter _formatter;

        private JsonWriter _generator;

        private ISerializerMetadataProvider _provider;
        private TypeChecker _typeChecker;
        private TextWriter _writer;

        private ILogger _logger;
        
        public NSerializer(ConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
            _formatter = configurationManager.GetFieldFormatter();
        }
        
        public void SetLogger(ILogger logger) {
            _logger = logger;
        }

        private bool InitializeGenerator()
        {
            _writer = new StringWriter();
            _provider = _configurationManager.NewMetadataProvider();
            _typeChecker = new TypeChecker(_provider);

            try
            {
                _generator = new JsonTextWriter(_writer);
                return true;
            }
            catch 
            {
                _generator = null;
                return false;
            }
        }

        private IFieldAccessor BuildFieldAccessor(object ob, string fieldName)
        {
            var klass = ob.GetType();

            var accessType = _configurationManager.GetAccessType();
            if (_provider.HasAccessType(klass))
                accessType = _provider.GetAccessType(klass);
            if (_provider.HasPropertyAccessType(klass, fieldName))
                accessType = _provider.GetPropertyAccessType(klass, fieldName);

            var customGetter = _provider.GetPropertyCustomGetterName(klass, fieldName);

            var fieldAccessor = new FieldAccessor(ob, fieldName, accessType);
            fieldAccessor.SetCustomGetterName(customGetter);

            if (_logger != null) fieldAccessor.SetLogger(_logger);

            if (_provider.IsVirtualProperty(klass, fieldName))
                fieldAccessor.SetEnsureFieldExists(false);

            return fieldAccessor;
        }

        private string GetSerializedName(Type klass, string fieldName)
        {
            var finalFieldName = _formatter.Format(fieldName);
            if (_provider.HasPropertySerializedName(klass, fieldName))
                finalFieldName = _provider.GetPropertySerializedName(klass, fieldName);

            return finalFieldName;
        }

        private void FillRawValue(JsonWriter gen, ParentFieldData parentFieldData, object value, string[] group)
        {
            _typeChecker.Check(value, new MyStruct(gen));
        }

        private void FillWith(bool asArray, JsonWriter gen, IParentFieldData parentData, object ob, string[] group)
        {
            var klass = ob.GetType();
            var fields = _provider.GetPropertiesByGroup(klass, parentData, group);
            if (asArray)
                gen.WriteStartArray();
            else
                gen.WriteStartObject();

            if (_typeChecker.IsSerializableObject(ob))
            {
                foreach (var fieldName in fields)
                {
                    var accessor = BuildFieldAccessor(ob, fieldName);

                    if (accessor.Get<object>() == null)
                    {
                        if (accessor.Exists() && _configurationManager.GetIncludeNullValues())
                        {
                            gen.WritePropertyName(GetSerializedName(klass, fieldName));
                            gen.WriteNull();
                        }

                        continue;
                    }

                    if (!_typeChecker.IsUnserializableObject(accessor.Get<object>()))
                    {
                        var fieldData = new ParentFieldData(klass, fieldName, @group);

                        gen.WritePropertyName(GetSerializedName(klass, fieldName));
                        FillRawValue(gen, fieldData, accessor.Get<object>(), @group);
                    }
                }
            }
            else if (_typeChecker.IsIterable(ob))
            {
                foreach (var arrValue in (IEnumerable) ob)
                {
                    var fieldData = new ParentFieldData(arrValue.GetType(), null, @group);

                    FillRawValue(gen, fieldData, arrValue, @group);
                }
            }

            if (asArray)
                gen.WriteEndArray();
            else
                gen.WriteEndObject();
        }

        public string Serialize<T>(T ob)
        {
            return Serialize(ob, new string[0]);
        }

        public string Serialize<T>(T ob, params string[] group)
        {
            if (ob == null) return null;

            if (!InitializeGenerator())
                return null;

            try
            {
                FillWith(_typeChecker.IsIterable(ob), _generator, null, ob, group);

                _generator.Close();

                return _writer.ToString();
            }
            catch
            {
                return null;
            }
        }

        private struct MyStruct : ITypeCallback
        {
            private readonly JsonWriter _gen;

            public MyStruct(JsonWriter gen)
            {
                _gen = gen;
            }

            public Func<byte, bool> itsByte(byte value)
            {
                throw new NotImplementedException();
            }

            public Func<short, bool> itsShort(short value)
            {
                throw new NotImplementedException();
            }

            public Func<int, bool> itsInteger(int value)
            {
                throw new NotImplementedException();
            }

            public Func<BigInteger, bool> itsBigInteger(BigInteger value)
            {
                throw new NotImplementedException();
            }

            public Func<long, bool> itsLong(long value)
            {
                throw new NotImplementedException();
            }

            public Func<float, bool> itsFloat(float value)
            {
                throw new NotImplementedException();
            }

            public Func<double, bool> itsDouble(double value)
            {
                throw new NotImplementedException();
            }

            public Func<decimal, bool> itsBigDecimal(decimal value)
            {
                throw new NotImplementedException();
            }

            public Func<object, bool> itsNumeric(object value)
            {
                throw new NotImplementedException();
            }

            public Func<string, bool> itsString(string value)
            {
                throw new NotImplementedException();
            }

            public Func<object, bool> itsStringParseable(object value)
            {
                throw new NotImplementedException();
            }

            public Func<bool, bool> itsBoolean(bool value)
            {
                throw new NotImplementedException();
            }

            public Func<DateTime, bool> itsDate(DateTime value)
            {
                throw new NotImplementedException();
            }

            public Func<object, bool> itsSerializableObject(object value)
            {
                throw new NotImplementedException();
            }

            public Func<object, bool> itsUnserializableObject(object value)
            {
                throw new NotImplementedException();
            }

            public Func<IEnumerable, bool> itsIterable(IEnumerable value)
            {
                throw new NotImplementedException();
            }

            public Func<IDictionary, bool> itsMap(IDictionary value)
            {
                throw new NotImplementedException();
            }
        }
    }
}