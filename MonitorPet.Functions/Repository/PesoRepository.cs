using Dapper;
using System.Threading.Tasks;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;
using System;
using System.Threading;

namespace MonitorPet.Functions.Repository;

#nullable enable

internal interface IPesoRepository
{
    Task Create(Model.WeightDosador model);
    Task<QueryWeightDosadorModel?> GetLastByIdDosadorOrDefault(string idDosador, CancellationToken cancellationToken = default);
}

internal class PesoRepository : IPesoRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public PesoRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Create(WeightDosador model)
    {
        var connection = await _connectionFactory.CreateOpenConnection();

        await connection
            .ExecuteAsync(
                @"INSERT INTO monitorpet.historicopeso (IdDosador, PesoGr, DateAt) VALUES (@IdDosador, @PesoGr, @DateAt);",
                new { IdDosador = model.IdDosador, PesoGr = model.Weight, DateAt = model.CreateAt }
            );
    }

    public async Task<QueryWeightDosadorModel?> GetLastByIdDosadorOrDefault(Guid idDosador, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var connection = await _connectionFactory.CreateOpenConnection();

        return await connection.QueryFirstOrDefaultAsync<QueryWeightDosadorModel>(
            @"
SELECT PesoGr Weight, DateAt DateAtUtc
FROM monitorpet.historicopeso
WHERE IdDosador = @IdDosador
ORDER BY DateAtUtc DESC
LIMIT 1;
            ",
            new { IdDosador = idDosador.ToString() }
        );
    }
}