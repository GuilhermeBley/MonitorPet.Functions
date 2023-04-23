using System;
using System.Data;
using System.Threading.Tasks;

namespace MonitorPet.Functions.MySqlConnection;

internal interface IConnectionFactory : IDisposable
{
    Task<IDbConnection> CreateOpenConnection();
}