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
            if (!IsValidAccessToken(req.Query[DEFAULT_QUERY_ACCESS_TOKEN]))
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var modelWeightDosador = JsonConvert.DeserializeObject<Model.WeightDosador>(requestBody);

            var repositoryWeight = CreateRepositoryWithConnection();

            modelWeightDosador.CreateAt = System.DateTime.Now;

            await repositoryWeight.Create(modelWeightDosador);

            return new CreatedResult("Peso", modelWeightDosador);
        }

        private static Repository.PesoRepository CreateRepositoryWithConnection()
            => new Repository.PesoRepository(
                new MySqlConnection.ConnectionFactory(
                    System.Environment.GetEnvironmentVariable(DEFAULT_MYSQL_CONFIG))
            );

        private static bool IsValidAccessToken(string token)
        {
            var tokenSideByServer = System.Environment.GetEnvironmentVariable(DEFAULT_QUERY_ACCESS_TOKEN);

            if (string.IsNullOrEmpty(tokenSideByServer )||
                string.IsNullOrEmpty(token) ||
                tokenSideByServer != token)
                return false;
            
            return true;
        }
    }
}
