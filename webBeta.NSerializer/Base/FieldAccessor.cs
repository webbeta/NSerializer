using System;
using System.Collections.Generic;
using System.Reflection;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Base
{
    public class FieldAccessor : IFieldAccessor
    {
        private readonly FieldAccessType _accessType;
        private readonly string _fieldName;

        private readonly Type _klass;
        private readonly List<Type> _klassTree;
        private readonly object _ob;
        private ILogger _logger;
        private string _customGetterName;

        private bool _ensureFieldExists = true;

        private bool _exists;
        private bool _initialized;
        private object _value;

        public FieldAccessor(
            object ob,
            string fieldName,
            FieldAccessType accessType
        )
        {
            _ob = ob;
            _fieldName = fieldName;
            _accessType = accessType;

            _klass = ob.GetType();
            _klassTree = new List<Type> {_klass};
        }

        public bool Exists()
        {
            Init();
            return _exists;
        }

        public T Get<T>()
        {
            Init();
            return (T) _value;
        }

        public void SetLogger(ILogger logger)
        {
            _logger = logger;
        }

        private string[] BuildGetterNames(string fieldName)
        {
            var names = new List<string>();

            var firstLetter = fieldName.Substring(0, 1);

            names.Add("Get" + firstLetter.ToUpper() + fieldName.Substring(1));
            names.Add("get" + firstLetter.ToUpper() + fieldName.Substring(1));
            names.Add("Is" + firstLetter.ToUpper() + fieldName.Substring(1));
            names.Add("is" + firstLetter.ToUpper() + fieldName.Substring(1));

            return names.ToArray();
        }

        private void Init()
        {
            if (_initialized) return;

            var superKlass = _klass;
            while (superKlass != null)
            {
                superKlass = superKlass.BaseType;
                if (superKlass.FullName.Equals(typeof(object).FullName))
                    break;

                _klassTree.Add(superKlass);
            }

            _klassTree.Reverse();

            if (_accessType == FieldAccessType.PROPERTY)
                BuildAsProperty();
            else if (_accessType == FieldAccessType.PUBLIC_METHOD)
                BuildAsMethod();

            _initialized = true;
        }

        private void BuildAsProperty()
        {
            foreach (var klass in _klassTree)
            {
                try
                {
                    const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                                   | BindingFlags.Static;
                    var field = klass.GetTypeInfo().GetField(_fieldName, bindFlags);
                    _value = field.GetValue(_ob);
                    _exists = true;
                }
                catch
                {
                    _logger?.Error(
                        $"Serializer cannot serialize field '{klass.FullName}.{_fieldName}', because it is not public.");
                }
            }
        }

        private void BuildAsMethod()
        {
            if (!_klass.IsPublic)
                throw new UnauthorizedAccessException(
                    $"Serializer cannot access \"{_klass.FullName}\" class. It can be caused because it has non public access.");

            var names = _customGetterName == null ?
                BuildGetterNames(_fieldName) : 
                new[] {_customGetterName};

            foreach (var name in names)
                try
                {
                    var method = _klass.GetMethod(name);
                    _value = method.Invoke(_ob, new object[] { });
                    _exists = true;
                    break;
                }
                catch
                {
                    _exists = false;

                    if (_logger == null) continue;
                    var method = _klass.GetMethod(name);
                    _logger.Error(
                        $"Serializer cannot serialize method '{_klass.FullName}.{name}', because it {(method.IsPublic ? "is not public." : "throw an exception when was called.")}");
                }

            if (_ensureFieldExists)
            {
                var accessorAsField =
                    new FieldAccessor(_ob, _fieldName, FieldAccessType.PROPERTY);

                _exists = _exists && accessorAsField.Exists();

                if (!_exists)
                    _value = null;
            }
        }

        public void SetEnsureFieldExists(bool ensureFieldExists)
        {
            _ensureFieldExists = ensureFieldExists;
        }

        public void SetCustomGetterName(string customGetterName)
        {
            _customGetterName = customGetterName;
        }
    }
}