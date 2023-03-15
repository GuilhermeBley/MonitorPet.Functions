using MonitorPet.Functions.Model;
using MonitorPet.Functions.MySqlConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        const string EXAMPLE_SCHEDULE = "6bfb570b-ff74-417e-aef9-a46ac66c0184";

        await Task.CompletedTask;

        if (idDosador != EXAMPLE_SCHEDULE)
            return Enumerable.Empty<ScheduleModel>();

        return new ScheduleModel[]
        {
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Sunday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Sunday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Sunday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },

            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Monday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Monday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Monday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },

            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Tuesday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Tuesday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Tuesday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },

            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Wednesday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Wednesday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Wednesday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },

            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Thursday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Thursday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Thursday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },

            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Friday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Friday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Friday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },

            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Saturday,
                IdSchedule = 1,
                Quantity = 50.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 09, 00, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Saturday,
                IdSchedule = 2,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 13, 10, 00)
            },
            new ScheduleModel
            {
                DayOfWeek = (int)DayOfWeek.Saturday,
                IdSchedule = 3,
                Quantity = 20.00,
                IdDosador = EXAMPLE_SCHEDULE,
                ScheduledDate = new DateTime(1900, 1, 1, 16, 10, 00)
            },
        };
    }
}

