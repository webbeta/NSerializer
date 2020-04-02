namespace webBeta.NSerializer.Base
{
    public interface IFieldAccessor
    {
        bool Exists();
        T Get<T>();
    }
}