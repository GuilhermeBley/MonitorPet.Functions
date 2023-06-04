using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.Repository;

namespace MonitorPet.Functions.Functions
{
    public class SendEmailFunction
    {
        [FunctionName("SendEmailFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            using var context = MpContextDb.Create();

            await foreach (var dosador in context.DosadorRepository.GetAllAsync())
            {
                var currentWeight = 0;

                // Check Offline
                if (dosador.LastRefresh is null ||
                    dosador.LastRefresh.Value.AddMinutes(2) < DateTime.UtcNow)
                    await TrySendEmailOffline(dosador, context);

                // Check without food in last 12 hours
                if (currentWeight == 0 &&
                    dosador.LastRefresh.Value.AddHours(12) < DateTime.UtcNow)
                    await TrySendEmailWithoutFood(dosador, context);
            }

            log.LogInformation($"Email timer trigger function executed at: {DateTime.UtcNow}");
        }

        private static async Task TrySendEmailWithoutFood(DosadorModel dosador, MpContextDb contextDb)
        {

        }

        private static async Task TrySendEmailOffline(DosadorModel dosador, MpContextDb contextDb)
        {

        }
    }
}
