using Microsoft.Extensions.Hosting;
using MimeKit;
using MonitorPet.Functions.Model;
using System.Net;
using MailKit.Net.Smtp;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MonitorPet.Functions.Email;

internal class EmailTemplate
{
    private const bool HAS_SSL = true;
    private const int SMTP_PORT = 587;
    private readonly static EmailConfig _config = EmailConfig.GetConfig();


    public static async Task SendEmailSemRacaoAsync(DosadorModel dosador, QueryUserJoinEmailModel userEmail, CancellationToken cancellationToken = default)
    {
        await SendEmailAsync(
            to: new string[] { userEmail.Email },
            subject: "Aviso ❗ Pet sem ração.",
            bodyHtml: $"<h1>Pet sem alimentação</h1><p>Atenção {userEmail.UserName}! Pet '{dosador.Nome}' não foi alimentado.</p>",
            cancellationToken: cancellationToken
        );
    }

    public static async Task SendEmailOffline(DosadorModel dosador, QueryUserJoinEmailModel userEmail, CancellationToken cancellationToken = default)
    {
        await SendEmailAsync(
            to: new string[] { userEmail.Email },
            subject: "Aviso ❗ Pet offline.",
            bodyHtml: $"<h1>Pet offline</h1><p>Atenção {userEmail.UserName}! Pet '{dosador.Nome}' está offline, verifique a conexão do seu Dosador.</p>",
            cancellationToken: cancellationToken
        );
    }

    private static async Task SendEmailAsync(string[] to, string subject, string bodyHtml, CancellationToken cancellationToken = default)
    {
        var mimeMessage = new MimeMessage();

        mimeMessage.From.AddRange(new[] { new MailboxAddress(_config.AddressNameFrom, _config.AddressFrom) });

        mimeMessage.To.AddRange(to.Select(t => new MailboxAddress(string.Empty, t)));

        mimeMessage.Subject = subject;

        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = bodyHtml
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(_config.Host, SMTP_PORT, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);

        client.AuthenticationMechanisms.Remove("XOAUTH2");

        await client.AuthenticateAsync(
            new NetworkCredential(_config.UserName, _config.Password), 
            cancellationToken: cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();
        await client.SendAsync(mimeMessage, cancellationToken);

        await client.DisconnectAsync(true);
    }
}
