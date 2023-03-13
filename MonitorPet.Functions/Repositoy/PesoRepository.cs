using System.Threading.Tasks;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;

namespace MonitorPet.Functions.Repository;

internal class PesoRepository : IPesoRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public PesoRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Create(WeightDosador model)
    {
        await Task.CompletedTask;
    }
}