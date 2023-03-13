using System.Data;

namespace MonitorPet.Functions.MySqlConnection;

internal class ConnectionFactory
{
    public static IDbConnection Create(string connectionString)
        => new MySql.Data.MySqlClient.MySqlConnection(connectionString);
}