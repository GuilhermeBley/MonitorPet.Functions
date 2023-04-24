using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitorPet.Functions.Repository;

internal interface IScheduleRepository
{
    Task<IEnumerable<ScheduleModel>> GetSchedulesFromDosador(string idDosador);
}

internal class ScheduleRepository : IScheduleRepository
{
    private readonly ConnectionFactory _connectionFactory;

    public ScheduleRepository(ConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ScheduleModel>> GetSchedulesFromDosador(string idDosador)
    {
        var connection = await _connectionFactory.CreateOpenConnection();

        return await connection
            .QueryAsync<ScheduleModel>(
                @"SELECT 
                    Id IdSchedule,
                    IdDosador IdDosador,
                    DiaSemana DayOfWeek,
                    HoraAgendada ScheduledDate,
                    QtdeLiberadaGr Quantity,
                    Ativado Activeted
                FROM monitorpet.agendamento
                WHERE IdDosador = @IdDosador; ",
                new { IdDosador = idDosador }
            );
    }
}

