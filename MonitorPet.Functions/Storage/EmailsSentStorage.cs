using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage.Table;
using MonitorPet.Functions.Model;
using MonitorPet.Functions.Settings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MonitorPet.Functions.Storage
{
    internal interface IEmailsSentStorage
    {
        Task<EmailSentModel> CreateAsync(EmailSentModel entity, CancellationToken cancellationToken = default);
        Task<IEnumerable<EmailSentModel>> GetBySentDateAsync(string email, string emailType, DateTime date, CancellationToken cancellationToken = default);
    }

    internal class EmailsSentStorage : IEmailsSentStorage
    {
        public static readonly TableClient _tableClient 
            = new(connectionString: AppSettings.TryGetSettings(AppSettings.DEFAULT_STORAGE_CONFIG), tableName: "emailsSent");

        public async Task<EmailSentModel> CreateAsync(EmailSentModel entity, CancellationToken cancellationToken = default)
        {
            await _tableClient.AddEntityAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<EmailSentModel>> GetBySentDateAsync(string email, string emailType, DateTime date, CancellationToken cancellationToken = default)
        {
            List<EmailSentModel> entities = new();

            await foreach(var entity in _tableClient.QueryAsync<EmailSentModel>(
                (e) => e.SentAt > date && e.EmailAddress == email && e.EmailType == emailType,
                cancellationToken: cancellationToken))
            {
                entities.Add(entity);
            }

            return entities;
        }
    }
}
