using System;

namespace webBeta.NSerializer.Base
{
    public class ParentFieldData : IParentFieldData
    {
        private readonly string _fieldName;
        private readonly string[] _groups;
        private readonly Type _klass;

        public ParentFieldData(Type klass, string fieldName, string[] groups)
        {
            _klass = klass;
            _fieldName = fieldName;
            _groups = groups;
        }

        public Type GetKlass()
        {
            return _klass;
        }

        public string GetFieldName()
        {
            return _fieldName;
        }

        public string[] GetGroups()
        {
            return _groups;
        }

        public bool IsRecursive(Type klass, string fieldName)
        {
            return _klass == klass && Equals(_fieldName, fieldName);
        }
    }
}