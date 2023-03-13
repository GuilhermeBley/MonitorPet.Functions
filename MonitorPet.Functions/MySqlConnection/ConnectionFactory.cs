using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MonitorPet.Functions.MySqlConnection;

internal class ConnectionFactory : IConnectionFactory
{
    public string ConnectionString { get; }

    public ConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public static DbConnection Create(string connectionString)
        => new MySql.Data.MySqlClient.MySqlConnection(connectionString);

    public async Task<IDbConnection> CreateOpenConnection()
    {
        var dbConnection = Create(ConnectionString);
        await dbConnection.OpenAsync();
        return dbConnection;
    }
}