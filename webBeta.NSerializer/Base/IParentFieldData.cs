using System;

namespace webBeta.NSerializer.Base
{
    public interface IParentFieldData
    {
        Type GetKlass();
        string GetFieldName();
        string[] GetGroups();
        bool IsRecursive(Type klass, string fieldName);
    }
}