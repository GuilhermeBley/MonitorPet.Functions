using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.Repository;
using MonitorPet.Functions.Storage;

namespace MonitorPet.Functions.Functions
{
    public class SendEmailFunction
    {
        [FunctionName("SendEmailFunction")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
        {
            using var context = MpContextDb.Create();

            await context.OpenConnectionAsync();

            await foreach (var dosador in context.DosadorRepository.GetAllAsync())
            {
                var currentWeight 
                    = (await context.PesoRepository.GetLastByIdDosadorOrDefault(dosador.IdDosador))
                    ?.Weight ?? 0;

                // Check Offline
                if (dosador.LastRefresh is null ||
                    dosador.LastRefresh.Value.AddMinutes(2) < DateTime.UtcNow)
                    await TrySendEmailOffline(dosador, context, log);

                // Check without food in last 12 hours
                if (currentWeight == 0 &&
                    (dosador.LastRefresh is null ||
                    dosador.LastRefresh.Value.AddHours(12) < DateTime.UtcNow))
                    await TrySendEmailWithoutFood(dosador, context, log);
            }

            log.LogInformation($"Email timer trigger function executed at: {DateTime.UtcNow}");
        }

        private static async Task TrySendEmailWithoutFood(DosadorModel dosador, MpContextDb contextDb, ILogger log)
        {
            const string EMAIL_TYPE = "sem_alimento";

            var emailStorage = new EmailsSentStorage();

            foreach (var user in await contextDb.UserRepository.GetByDosador(dosador.IdDosador))
            {
                var sents = await emailStorage.GetBySentDateAsync(user.Email, DateTime.UtcNow.AddDays(-1));

                if (sents.Any(s => s.EmailType == EMAIL_TYPE))
                    continue;

                await emailStorage.CreateAsync(
                    new EmailSentModel(user.Email, EMAIL_TYPE)
                );

                log.LogInformation($"Pet '{dosador.IdDosador}' - E-mail 'sem_alimento' send to '{user.Email}'.");
            }
        }

        private static async Task TrySendEmailOffline(DosadorModel dosador, MpContextDb contextDb, ILogger log)
        {
            const string EMAIL_TYPE = "offline_pet";

            var emailStorage = new EmailsSentStorage();

            foreach (var user in await contextDb.UserRepository.GetByDosador(dosador.IdDosador))
            {
                var sents = await emailStorage.GetBySentDateAsync(user.Email, DateTime.UtcNow.AddDays(-1));

                if (sents.Any(s => s.EmailType == EMAIL_TYPE))
                    continue;

                await emailStorage.CreateAsync(
                    new EmailSentModel(user.Email, EMAIL_TYPE)
                );

                log.LogInformation($"Pet '{dosador.IdDosador}' - E-mail 'offline_pet' send to '{user.Email}'.");
            }
        }
    }
}
