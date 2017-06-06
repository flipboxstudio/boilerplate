using App.Options;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using HandlebarsDotNet;
using System.IO;
using Microsoft.Extensions.Options;

namespace App.Services
{
    public class Mailer
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        private readonly AppConfig _appConfig;

        /// <summary>
        /// Initialize class.
        /// </summary>
        /// <param name="appConfig"></param>
        public Mailer(IOptions<AppConfig> appConfig)
        {
            _appConfig = appConfig.Value;
        }

        /// <summary>
        /// Sending email using HTML. Template path is in `Resources/Views/Mail`.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public void SendEmail(MailboxAddress to, string subject, string path, object data)
        {
            var message = new MimeMessage();
            var from = new MailboxAddress(_appConfig.MailerDisplayName, _appConfig.MailerRelayName);
            var content = CompileTemplate(path, data);

            // Set "from".
            message.From.Add(from);

            // Set recipient.
            message.To.Add(to);

            // Set email subject.
            message.Subject = subject;

            // Set email content.
            message.Body = new TextPart("html") { Text = content };

            SendMessage(message);
        }

        /// <summary>
        /// Compile email template.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private string CompileTemplate(string relativePath, object data)
        {
            var absolutePath = Path.Combine(_appConfig.MailterTemplatePath, relativePath);
            var source = System.IO.File.ReadAllText(absolutePath);
            var template = Handlebars.Compile(source);
            var content = template(data);

            return content;
        }

        /// <summary>
        /// Sending message via wire.
        /// </summary>
        /// <param name="message"></param>
        private void SendMessage (MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Connecting to server.
                client.Connect(_appConfig.MailerHost, _appConfig.MailerPort, _appConfig.MailerUseSSL);

                // Authenticate using configured username and password.
                client.Authenticate(_appConfig.MailerUserName, _appConfig.MailerPassword);

                // Sending message
                client.Send(message);

                // Disconnect the client.
                client.Disconnect(true);
            }
        }
    }
}