using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using MonitorPet.Functions.Settings;
using MonitorPet.Functions.Security;
using MonitorPet.Functions.MySqlConnection;
using MonitorPet.Functions.Repository;
using MonitorPet.Functions.Model;

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
            try
            {
                if (!TokenServer.IsValidAccessToken(req.Query[AppSettings.DEFAULT_QUERY_ACCESS_TOKEN]))
                    return new UnauthorizedResult();

                var paramIdDosador = req.Query["IdDosador"];

                if (string.IsNullOrEmpty(paramIdDosador) ||
                    !Guid.TryParse(paramIdDosador, out Guid generatedGuid))
                    return new BadRequestObjectResult("Param 'IdDosador' is invalid.");

                using var context = MpContextDb.Create();

                await context.OpenConnectionAsync();
                
                var lastRelease = await context.DosadorRepository.GetLastRelease(generatedGuid);

                if (lastRelease is not null)
                    await context.DosadorRepository.UpdateLastRelease(generatedGuid, null);

                await context.DosadorRepository.UpdateLastRefresh(generatedGuid, DateTime.UtcNow);
                
                var schedules = await context.ScheduleRepository.GetSchedulesFromDosador(generatedGuid.ToString());

                var model = new ReturnScheduleModel { LastRelease = lastRelease, Schedules = schedules.Where(s => s.Activated).ToArray() };

                return new OkObjectResult(
                    model
                );
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed");

                throw;
            }
        }
    }
}
