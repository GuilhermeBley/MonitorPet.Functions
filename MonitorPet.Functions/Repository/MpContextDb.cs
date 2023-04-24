using MonitorPet.Functions.MySqlConnection;
using MonitorPet.Functions.Settings;
using System;
using System.Threading.Tasks;

namespace MonitorPet.Functions.Repository;

internal class MpContextDb : IDisposable
{
    private readonly IConnectionFactory _connectionFactory;

    public readonly IDosadorRepository DosadorRepository;
    public readonly IPesoRepository PesoRepository;
    public readonly IScheduleRepository ScheduleRepository;

    public MpContextDb(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;

        DosadorRepository = new DosadorRepository(connectionFactory);
        PesoRepository = new PesoRepository(connectionFactory);
        ScheduleRepository = new ScheduleRepository(connectionFactory);
    }

    public async Task OpenConnectionAsync()
    {
        await _connectionFactory.CreateOpenConnection();
    }

    public void Dispose()
    {
        _connectionFactory.Dispose();
    }

    public static MpContextDb Create()
        => new MpContextDb(
            new ConnectionFactory(AppSettings.TryGetSettings(AppSettings.DEFAULT_MYSQL_CONFIG))
        );
}
