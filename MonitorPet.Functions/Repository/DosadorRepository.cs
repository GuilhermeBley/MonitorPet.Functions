using System;
using System.Threading.Tasks;
using Dapper;
using MonitorPet.Functions.MySqlConnection;

namespace MonitorPet.Functions.Repository;

internal interface IDosadorRepository
{
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
}