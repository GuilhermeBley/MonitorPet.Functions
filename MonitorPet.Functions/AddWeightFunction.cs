using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MonitorPet.Functions
{
    public static partial class AddWeightFunction
    {
        private const string DEFAULT_QUERY_ACCESS_TOKEN = "KeyAccessApi";
        private const string DEFAULT_MYSQL_CONFIG = "MySqlConnection";

        [FunctionName("AddWeightFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "AddWeightFunction")] HttpRequest req,
            ILogger log)
        {
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
                    TryGetSettings(DEFAULT_MYSQL_CONFIG))
            );

        private static bool IsValidAccessToken(string token)
        {
            var tokenSideByServer = TryGetSettings(DEFAULT_QUERY_ACCESS_TOKEN);

            if (string.IsNullOrEmpty(tokenSideByServer )||
                string.IsNullOrEmpty(token) ||
                tokenSideByServer != token)
                return false;
            
            return true;
        }
    
        private static async Task<Model.WeightDosador> CreateByBody(Stream body)
        {
            string requestBody = await new StreamReader(body).ReadToEndAsync();
            var modelWeightDosador = JsonConvert.DeserializeObject<Model.WeightDosador>(requestBody);

            if (modelWeightDosador is null)
                return null;

            modelWeightDosador.CreateAt = System.DateTime.Now;

            return modelWeightDosador;
        }
    
        private static string TryGetSettings(string key)
        {
            try
            {
                return System.Environment.GetEnvironmentVariable(key) ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
