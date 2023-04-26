using System;
using System.Threading.Tasks;
using Dapper;
using MonitorPet.Functions.MySqlConnection;

namespace MonitorPet.Functions.Repository;

internal interface IDosadorRepository
{
    Task GetLastRefresh(Guid idDosador);
    Task UpdateLastRefresh(Guid idDosador, DateTime timeUtc);
}

internal class DosadorRepository : IDosadorRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public DosadorRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task UpdateLastRefresh(Guid idDosador, DateTime timeUtc)
    {
        var connection = await _connectionFactory.CreateOpenConnection();

        await connection.ExecuteAsync(@"UPDATE monitorpet.dosador SET UltimaAtualizacao = @UltimaAtualizacao WHERE (IdDosador = @IdDosador);",
            new { UltimaAtualizacao = timeUtc, IdDosador = idDosador });
    }
    
    public async Task GetLastRefresh(Guid idDosador)
    {
        var connection = await _connectionFactory.CreateOpenConnection();

        await connection.QueryFirstOrDefaultAsync<DateTime?>(@"SELECT  UltimaAtualizacao FROM monitorpet.dosador WHERE IdDosador = @IdDosador;",
            new { IdDosador = idDosador });
    }
}
