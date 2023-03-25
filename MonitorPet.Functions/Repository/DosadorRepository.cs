using System;
using System.Threading.Tasks;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;

namespace MonitorPet.Functions.Repository;

internal interface IDosadorRepository
{
    Task UpdateLastRefresh(DateTime timeUtc);
}

internal class DosadorRepository : IDosadorRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public DosadorRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task UpdateLastRefresh(DateTime titimeUtcme)
    {
        await Task.CompletedTask;
    }
}