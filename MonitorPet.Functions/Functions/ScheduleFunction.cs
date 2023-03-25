using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using MonitorPet.Functions.Settings;
using MonitorPet.Functions.Security;

namespace MonitorPet.Functions
{
    public static class ScheduleFunction
    {
        private static TokenAccess TokenServer { get; } =
            new TokenAccess(AppSettings.TryGetSettings(AppSettings.DEFAULT_QUERY_ACCESS_TOKEN));

        [FunctionName("ScheduleFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ScheduleFunction")] HttpRequest req,
            ILogger log)
        {
            if (!TokenServer.IsValidAccessToken(req.Query[AppSettings.DEFAULT_QUERY_ACCESS_TOKEN]))
                return new UnauthorizedResult();

            var paramIdDosador = req.Query["IdDosador"];

            if (string.IsNullOrEmpty(paramIdDosador) ||
                !Guid.TryParse(paramIdDosador, out Guid generatedGuid))
                return new BadRequestObjectResult("Param 'IdDosador' is invalid.");

            var tupleRep = CreateRepositoryWithConnection();

            await tupleRep.DosadorRep.UpdateLastRefresh(DateTime.UtcNow);
            var schedules = await tupleRep.ScheduleRep.GetSchedulesFromDosador(generatedGuid.ToString());

            return new OkObjectResult(
                schedules.ToArray()
            );
        }

        private static (Repository.IScheduleRepository ScheduleRep, Repository.IDosadorRepository DosadorRep) CreateRepositoryWithConnection()
        {
            var connection =
                new MySqlConnection.ConnectionFactory(
                    AppSettings.TryGetSettings(AppSettings.DEFAULT_MYSQL_CONFIG));
            var scheduleRep = new Repository.ScheduleRepository(connection);
            var dosadorRep = new Repository.DosadorRepository(connection);
            return (scheduleRep, dosadorRep);
        }
    }
}
