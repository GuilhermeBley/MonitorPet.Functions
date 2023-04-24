using Dapper;
using System.Threading.Tasks;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;
using System;

namespace MonitorPet.Functions.Repository;

internal interface IPesoRepository
{
    Task Create(Model.WeightDosador model);
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
}