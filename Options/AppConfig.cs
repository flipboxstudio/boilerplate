namespace App.Options
{
    public class AppConfig
    {
        // ---------------- AUTHENTICATION

        public string AuthIdentifier { get; set; } = "Id";

        // ---------------- END OF AUTHENTICATION


        // ---------------- UPLOADER

        public string RelativeUploadPath { get; set; } = "";

        public string AbsoluteUploadPath { get; set; } = "";

        public string Host { get; set; } = "";

        // ---------------- END OF UPLOADER


        // ---------------- MAILER

        public string MailerHost { get; set; } = "";

        public int MailerPort { get; set; } = 465;

        public bool MailerUseSSL { get; set; } = true;

        public string MailerUserName { get; set; } = "";

        public string MailerPassword { get; set; } = "";

        public string MailerDisplayName { get; set; } = "";

        public string MailerRelayName { get; set; } = "";

        // ---------------- END OF MAILER
    }
}