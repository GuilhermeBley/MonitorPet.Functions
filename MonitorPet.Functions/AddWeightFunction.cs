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
        [FunctionName("AddWeightFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "AddWeightFunction")] HttpRequest req,
            ILogger log)
        {
            var value = System.Environment.GetEnvironmentVariable("MySqlConnection");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var modelWeightDosador = JsonConvert.DeserializeObject<Model.WeightDosador>(requestBody);

            var repositoryWeight = CreateRepositoryWithConnection(value);

            modelWeightDosador.CreateAt = System.DateTime.Now;

            await repositoryWeight.Create(modelWeightDosador);

            return new CreatedResult("Peso", modelWeightDosador);
        }

        private static Repository.PesoRepository CreateRepositoryWithConnection(string conn)
            => new Repository.PesoRepository(
                new MySqlConnection.ConnectionFactory(conn)
            );
    }
}
