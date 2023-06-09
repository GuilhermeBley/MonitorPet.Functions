using MonitorPet.Functions.Settings;

namespace MonitorPet.Functions.Email;

internal class EmailConfig
{
    public string AddressFrom { get; private set; }
    public string AddressNameFrom  { get; private set; }
    public string Host { get; private set; }
    public string UserName { get; private set; }
    public string Password { get; private set; }

    private EmailConfig()
    {

    }

    public static EmailConfig GetConfig()
        => new()
        {
            AddressFrom = AppSettings.TryGetSettings("EmailAddressFrom"), 
            AddressNameFrom = AppSettings.TryGetSettings("EmailAddressNameFrom"), 
            Host = AppSettings.TryGetSettings("EmailHost"),
            Password = AppSettings.TryGetSettings("EmailPassword"), 
            UserName = AppSettings.TryGetSettings("EmailUserName"),
        };
}
