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
            if (!TokenServer.IsValidAccessToken(req.Query[AppSettings.DEFAULT_QUERY_ACCESS_TOKEN]))
                return new UnauthorizedResult();

            var modelWeightDosador = await CreateByBody(req.Body);

            if (modelWeightDosador is null)
                return new BadRequestObjectResult("Invalid model");

            var repositoryWeight = CreateRepositoryWithConnection();

            await repositoryWeight.Create(modelWeightDosador);

            return new CreatedResult("Peso", modelWeightDosador);
        }

        private static Repository.PesoRepository CreateRepositoryWithConnection()
            => new Repository.PesoRepository(
                new MySqlConnection.ConnectionFactory(
                    AppSettings.TryGetSettings(AppSettings.DEFAULT_MYSQL_CONFIG))
            );
    
        private static async Task<Model.WeightDosador> CreateByBody(Stream body)
        {
            string requestBody = await new StreamReader(body).ReadToEndAsync();
            var modelWeightDosador = JsonConvert.DeserializeObject<Model.WeightDosador>(requestBody);

            if (modelWeightDosador is null)
                return null;

            modelWeightDosador.CreateAt = System.DateTime.Now;

            return modelWeightDosador;
        }
    }
}
