using System.Data;
using System.Data.SqlClient;

namespace Boilerplate.Services
{
    public class Database
    {
        public readonly IDbConnection Connection;

        public Database(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }
    }
}