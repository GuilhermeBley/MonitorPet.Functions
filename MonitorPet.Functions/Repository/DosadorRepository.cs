using System;
using System.Threading.Tasks;
using Dapper;
using MonitorPet.Functions.MySqlConnection;

namespace MonitorPet.Functions.Repository;

internal interface IDosadorRepository
{
    Task<DateTime?> GetLastRelease(Guid idDosador);
    Task UpdateLastRefresh(Guid idDosador, DateTime timeUtc);
    Task UpdateLastRelease(Guid idDosador, DateTime? lastReleaseUtc);
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
    
    public async Task<DateTime?> GetLastRelease(Guid idDosador)
    {
        var connection = await _connectionFactory.CreateOpenConnection();

        return
            await connection.QueryFirstOrDefaultAsync<DateTime?>(@"SELECT UltimaLiberacao FROM monitorpet.dosador WHERE IdDosador = @IdDosador;",
                new { IdDosador = idDosador });
    }

    public async Task UpdateLastRelease(Guid idDosador, DateTime? lastReleaseUtc)
    {
        var connection = await _connectionFactory.CreateOpenConnection();

        await connection.ExecuteAsync(@"UPDATE monitorpet.dosador SET UltimaLiberacao = @UltimaLiberacao WHERE (IdDosador = @IdDosador);",
            new { UltimaLiberacao = lastReleaseUtc, IdDosador = idDosador });
    }
}
