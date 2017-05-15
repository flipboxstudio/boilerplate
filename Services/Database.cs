using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace App.Services
{
    public class Database
    {
        private readonly string DatabaseEngine;

        private readonly string ConnectionString;

        public IDbConnection Connection => (DatabaseEngine == "mysql")
                ? new MySqlConnection(ConnectionString) as IDbConnection
                : new SqlConnection(ConnectionString) as IDbConnection;

        public Database(string databaseEngine, string connectionString)
        {
            DatabaseEngine = databaseEngine;
            ConnectionString = connectionString;
        }
    }
}
