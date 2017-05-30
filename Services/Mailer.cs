using App.Options;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using HandlebarsDotNet;

namespace App.Services
{
    public class Mailer
    {
        /// <summary>
        /// Email host.
        /// </summary>
        private readonly string _host;

        /// <summary>
        /// Email port.
        /// </summary>
        private readonly int _port;

        /// <summary>
        /// Choose whether to use SSL or not.
        /// </summary>
        private readonly bool _ssl;

        /// <summary>
        /// Email authentication: Username
        /// </summary>
        private readonly string _username;

        /// <summary>
        /// Email authentication: Password
        /// </summary>
        private readonly string _password;

        /// <summary>
        /// Initialize basic configuration.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="ssl"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public Mailer(string host, int port, bool ssl, string username, string password)
        {
            _host = host;
            _port = port;
            _ssl = ssl;
            _username = username;
            _password = password;
        }

        /// <summary>
        /// Send email to user, with given subject and plain text content.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        public void SendEmail(MailboxAddress to, string subject, TextPart content)
        {
            var message = new MimeMessage();
            var from = new MailboxAddress(
                AppConfig.MailerName,
                AppConfig.MailerUser
            );

            // Set "from".
            message.From.Add(from);

            // Set recipient.
            message.To.Add(to);

            // Set email subject.
            message.Subject = subject;

            // Set email content.
            message.Body = content;

            SendMessage(message);
        }

        public void SendEmail(MailboxAddress to, string subject, string path, object data)
        {
            var message = new MimeMessage();
            var from = new MailboxAddress(
                AppConfig.MailerName,
                AppConfig.MailerUser
            );

            // Set "from".
            message.From.Add(from);

            // Set recipient.
            message.To.Add(to);

            // Set email subject.
            message.Subject = subject;

            var source = System.IO.File.ReadAllText(path);
            var template = Handlebars.Compile(source);
            var content = template(data);

            // Set email content.
            message.Body = new TextPart("plain") { Text = content };

            SendMessage(message);
        }

        private void SendMessage (MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Connecting to server.
                client.Connect(_host, _port, _ssl);

                // Authenticate using configured username and password.
                client.Authenticate(_username, _password);

                // Sending message
                client.Send(message);

                // Disconnect the client.
                client.Disconnect(true);
            }
        }
    }
}