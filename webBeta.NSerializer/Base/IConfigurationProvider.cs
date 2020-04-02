namespace webBeta.NSerializer.Base
{
    public interface IConfigurationProvider
    {
        bool GetBoolean(string key, bool defaultValue);
        string GetString(string key, string defaultValue);
    }
}