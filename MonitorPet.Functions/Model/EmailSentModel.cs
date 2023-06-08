using Azure;
using Azure.Data.Tables;
using System;

namespace MonitorPet.Functions.Model;

internal class EmailSentModel : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string EmailType { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }

    public EmailSentModel()
    {
    }

    public EmailSentModel(string email, string emailType)
    {
        EmailAddress = email;
        EmailType = emailType;
        SentAt = DateTime.UtcNow;
        PartitionKey = $"{EmailAddress}_{EmailType}";
        RowKey = Guid.NewGuid().ToString();
    }
}
