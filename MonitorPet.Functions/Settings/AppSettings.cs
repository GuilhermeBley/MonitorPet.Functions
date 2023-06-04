
namespace MonitorPet.Functions.Settings;

internal static class AppSettings
{
    public const string DEFAULT_QUERY_ACCESS_TOKEN = "KeyAccessApi";
    public const string DEFAULT_MYSQL_CONFIG = "MySqlConnection";
    public const string DEFAULT_STORAGE_CONFIG = "Storage";

    public static string TryGetSettings(string key)
    {
        try
        {
            return System.Environment.GetEnvironmentVariable(key) ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
}

