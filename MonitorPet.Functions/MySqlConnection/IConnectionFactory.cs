using System.Data;
using System.Threading.Tasks;

namespace MonitorPet.Functions.MySqlConnection;

internal interface IConnectionFactory
{
    Task<IDbConnection> CreateOpenConnection();
}