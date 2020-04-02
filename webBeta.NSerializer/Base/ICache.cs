namespace webBeta.NSerializer.Base
{
    public interface ICache
    {
        string Get(string key);
        void Set(string key, string content);
        void Remove(string key);
    }
}