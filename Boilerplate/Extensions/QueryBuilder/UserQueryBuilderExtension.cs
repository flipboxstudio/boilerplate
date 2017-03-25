// ReSharper disable CheckNamespace

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Boilerplate.Model;
using Dapper;

namespace Boilerplate.Services
{
    public static class UserQueryBuilderExtension
    {
        /// <summary>
        ///     Get User by it's Username.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User FindUserByUsername(this Database database, string username)
        {
            var predicates = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> {{"Username = @username", new {username}}}
            };

            return FindAll(database, predicates).First();
        }

        /// <summary>
        ///     Get User by it's UserID.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User FindUserByID(this Database database, int userID)
        {
            var predicates = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object> {{"UserID = @userID", new {userID}}}
            };

            return FindAll(database, predicates).First();
        }

        /// <summary>
        ///     Find all Users by criteria.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        private static IEnumerable<User> FindAll(Database database,
            IEnumerable<Dictionary<string, object>> predicates = null)
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate($"SELECT /**select**/ FROM {GetTableAttribute().Name} /**where**/");

            predicates?.ToList()
                .ForEach(
                    predicate =>
                    {
                        predicate.ToList().ForEach(criteria => builder.Select("*").Where(criteria.Key, criteria.Value));
                    });

            database.Connection.Open();

            var result = database.Connection
                .Query<User>(template.RawSql, template.Parameters);

            database.Connection.Close();

            return result;
        }

        /// <summary>
        ///     Get table information from Attribute.
        /// </summary>
        /// <returns></returns>
        private static TableAttribute GetTableAttribute()
        {
            return typeof(User).GetTypeInfo().GetCustomAttribute<TableAttribute>();
        }
    }
}