
namespace MonitorPet.Functions.Settings;

internal static class AppSettings
{
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

