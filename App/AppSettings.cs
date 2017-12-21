namespace App
{
    public class AppSettings
    {
        public Database Database { get; set; } = new Database();

        public Jwt Jwt { get; set; } = new Jwt();
    }

    public class Database
    {
        public string Driver { get; set; } = "Sqlite";
    }

    public class Jwt
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Authority { get; set; }

        public double Expiry { get; set; } = 60;
    }
}