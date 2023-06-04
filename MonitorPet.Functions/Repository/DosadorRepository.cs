using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;

namespace MonitorPet.Functions.Repository;

internal interface IDosadorRepository
{
    Task<DateTime?> GetLastRelease(Guid idDosador);
    Task UpdateLastRefresh(Guid idDosador, DateTime timeUtc);
    Task UpdateLastRelease(Guid idDosador, DateTime? lastReleaseUtc);
    IAsyncEnumerable<DosadorModel> GetAllAsync();
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

    public async IAsyncEnumerable<DosadorModel> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        const int TAKE = 100;

        const string QUERY = @"
SELECT 
    IdDosador IdDosador, 
    Nome Nome, 
    ImgUrl, 
    UltimaAtualizacao LastRefresh, 
    UltimaLiberacao LastRelease 
FROM monitorpet.dosador
LIMIT @Skip, @Take;
";

        var connection = await _connectionFactory.CreateOpenConnection();

        bool hasMore = true;
        for (var skip = 0; hasMore; skip += TAKE)
        {
            var dosadores = await connection.QueryAsync<DosadorModel>(
                QUERY,
                new { Skip = skip, Take = TAKE }
            );
            
            if (!dosadores.Any())
            {
                hasMore = false;
                yield break;
            }

            foreach (var dosador in dosadores)
                yield return dosador;
        }
    }
}
