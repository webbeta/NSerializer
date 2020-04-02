using System;
using System.Collections.Generic;
using System.Reflection;
using webBeta.NSerializer.Base.Types;

namespace webBeta.NSerializer.Base
{
    public class FieldAccessor : IFieldAccessor
    {
        private readonly FieldAccessType accessType;
        private readonly string fieldName;

        private readonly Type klass;
        private readonly List<Type> klassTree;
        private readonly object ob;
        private ILogger _logger;
        private string customGetterName;

        private bool ensureFieldExists = true;

        private bool exists;
        private bool initialized;
        private object value;

        public FieldAccessor(
            object ob,
            string fieldName,
            FieldAccessType accessType
        )
        {
            this.ob = ob;
            this.fieldName = fieldName;
            this.accessType = accessType;

            klass = ob.GetType();
            klassTree = new List<Type> {klass};
        }

        public bool Exists()
        {
            Init();
            return exists;
        }

        public T Get<T>()
        {
            Init();
            return (T) value;
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
            names.Add("Is" + firstLetter.ToUpper() + fieldName.Substring(1));

            return names.ToArray();
        }

        private void Init()
        {
            if (initialized) return;

            var superKlass = klass;
            while (superKlass != null)
            {
                superKlass = superKlass.BaseType;
                if (superKlass.FullName.Equals(typeof(object).FullName))
                    break;

                klassTree.Add(superKlass);
            }

            klassTree.Reverse();

            if (accessType == FieldAccessType.PROPERTY)
                BuildAsProperty();
            else if (accessType == FieldAccessType.PUBLIC_METHOD)
                buildAsMethod();

            initialized = true;
        }

        private void BuildAsProperty()
        {
            foreach (var klass in klassTree)
                try
                {
                    const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                                   | BindingFlags.Static;
                    var field = klass.GetTypeInfo().GetField(fieldName, bindFlags);
                    value = field.GetValue(ob);
                    exists = true;
                }
                catch
                {
                    _logger?.Error(
                        $"Serializer cannot serialize field '{klass.FullName}.{fieldName}', because it is not public.");
                }
        }

        private void buildAsMethod()
        {
            if (!klass.IsPublic)
                throw new UnauthorizedAccessException(
                    $"Serializer cannot access \"{klass.FullName}\" class. It can be caused because it has non public access.");

            string[] names;
            if (customGetterName == null)
                names = BuildGetterNames(fieldName);
            else
                names = new[] {customGetterName};

            foreach (var name in names)
                try
                {
                    var method = klass.GetMethod(name);
                    value = method.Invoke(ob, new object[] { });
                    exists = true;
                    break;
                }
                catch
                {
                    exists = false;

                    if (_logger == null) continue;
                    var method = klass.GetMethod(name);
                    _logger.Error(
                        $"Serializer cannot serialize method '{klass.FullName}.{name}', because it {(method.IsPublic ? "is not public." : "throw an exception when was called.")}");
                }

            if (ensureFieldExists)
            {
                var accessorAsField =
                    new FieldAccessor(ob, fieldName, FieldAccessType.PROPERTY);

                exists = exists && accessorAsField.Exists();

                if (!exists)
                    value = null;
            }
        }

        public void SetEnsureFieldExists(bool ensureFieldExists)
        {
            this.ensureFieldExists = ensureFieldExists;
        }

        public void SetCustomGetterName(string customGetterName)
        {
            this.customGetterName = customGetterName;
        }
    }
}