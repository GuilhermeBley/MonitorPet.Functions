using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MonitorPet.Functions.Settings;
using MonitorPet.Functions.Security;
using System;
using MonitorPet.Functions.Repository;

namespace MonitorPet.Functions
{
    public static partial class AddWeightFunction
    {
        private static TokenAccess TokenServer { get; } = 
            new TokenAccess(AppSettings.TryGetSettings(AppSettings.DEFAULT_QUERY_ACCESS_TOKEN));

        [FunctionName("AddWeightFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "AddWeightFunction")] HttpRequest req,
            ILogger log)
        {
            try
            {
                if (!TokenServer.IsValidAccessToken(req.Query[AppSettings.DEFAULT_QUERY_ACCESS_TOKEN]))
                    return new UnauthorizedResult();

                var modelWeightDosador = await CreateByBody(req.Body);

                if (modelWeightDosador is null)
                    return new BadRequestObjectResult("Invalid model");

                using var context = MpContextDb.Create();

                await context.OpenConnectionAsync();

                await context.PesoRepository.Create(modelWeightDosador);

                return new CreatedResult("Peso", modelWeightDosador);
            }
            catch (Exception e)
            {
                log.LogError(e, "Failed");

                throw;
            }
        }
    
        private static async Task<Model.WeightDosador> CreateByBody(Stream body)
        {
            string requestBody = await new StreamReader(body).ReadToEndAsync();
            var modelWeightDosador = JsonConvert.DeserializeObject<Model.WeightDosador>(requestBody);

            if (modelWeightDosador is null)
                return null;

            modelWeightDosador.CreateAt = System.DateTime.UtcNow;

            return modelWeightDosador;
        }
    }
}
