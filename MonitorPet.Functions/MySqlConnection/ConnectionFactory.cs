using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorPet.Functions.MySqlConnection;

internal class ConnectionFactory : IConnectionFactory
{
    private DbConnection _poolConnection;
    private static SemaphoreSlim _semaphore = new(1,1);
    public string ConnectionString { get; }

    public ConnectionFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public static DbConnection Create(string connectionString)
        => new MySql.Data.MySqlClient.MySqlConnection(connectionString);

    public async Task<IDbConnection> CreateOpenConnection()
    {
        if (_poolConnection is not null)
            return _poolConnection;

        try
        {
            await _semaphore.WaitAsync();

            if (_poolConnection is not null)
                return _poolConnection;

            var dbConnection = Create(ConnectionString);
            await dbConnection.OpenAsync();
            _poolConnection = dbConnection;
            return dbConnection;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _poolConnection?.Dispose();
    }
}