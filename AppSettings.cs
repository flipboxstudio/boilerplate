using System;
using Microsoft.AspNetCore.Identity;

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

        public double ExpiryDays { get; set; } = 30;
    }
}