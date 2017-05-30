using MySql.Data.MySqlClient;
using System.Data;
using Dapper.MicroCRUD;

namespace App.Services
{
    public class Database
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string ConnectionString;

        /// <summary>
        /// Always create new instance to prevent locking.
        /// </summary>
        public IDbConnection Connection => new MySqlConnection(ConnectionString);

        /// <summary>
        /// Initialize class.
        /// </summary>
        /// <param name="connectionString"></param>
        public Database(string connectionString)
        {
            ConnectionString = connectionString;
            MicroCRUDConfig.DefaultDialect = Dialect.PostgreSql;
        }
    }
}
