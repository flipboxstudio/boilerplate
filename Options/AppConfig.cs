namespace App.Options
{
    // TODO: Separate section to each class.

    /// <summary>
    /// Application config.
    /// </summary>
    public class AppConfig
    {
        // ---------------- AUTHENTICATION

        /// <summary>
        /// Unique indentifier for user.
        /// </summary>
        /// <returns></returns>
        public string AuthIdentifier { get; set; } = "Id";

        // ---------------- END OF AUTHENTICATION


        // ---------------- UPLOADER

        /// <summary>
        /// Relative upload path to root app directory.
        /// </summary>
        /// <returns></returns>
        public string RelativeUploadPath { get; set; } = "";

        /// <summary>
        /// Absolute upload path (filesystem full path).
        /// </summary>
        /// <returns></returns>
        public string AbsoluteUploadPath { get; set; } = "";

        /// <summary>
        /// Host of our application.
        /// </summary>
        /// <returns></returns>
        public string Host { get; set; } = "";

        // ---------------- END OF UPLOADER


        // ---------------- MAILER

        /// <summary>
        /// Host address of your email server.
        /// </summary>
        /// <returns></returns>
        public string MailerHost { get; set; } = "";

        /// <summary>
        /// SMTP port.
        /// </summary>
        /// <returns></returns>
        public int MailerPort { get; set; } = 465;

        /// <summary>
        /// Determine if your SMTP required encrypted connection.
        /// </summary>
        /// <returns></returns>
        public bool MailerUseSSL { get; set; } = true;

        /// <summary>
        /// Username of your email (ussually it's an email address).
        /// </summary>
        /// <returns></returns>
        public string MailerUserName { get; set; } = "";

        /// <summary>
        /// Password of your email.
        /// </summary>
        /// <returns></returns>
        public string MailerPassword { get; set; } = "";

        /// <summary>
        /// Display name for your email.
        /// </summary>
        /// <returns></returns>
        public string MailerDisplayName { get; set; } = "";

        /// <summary>
        /// Relay email name for your email.
        /// </summary>
        /// <returns></returns>
        public string MailerRelayName { get; set; } = "";

        /// <summary>
        /// Email template path.
        /// </summary>
        /// <returns></returns>
        public string MailterTemplatePath { get; set; } = "";

        // ---------------- END OF MAILER
    }
}