using App.Model;
using static App.Services.DapperQueryBuilderExtension;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace App.Services
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
            var predicates = new Dictionary<string, object> {{"username = @username", new {username}}};
            var result = FindAll<User>(database, predicates);

            return result.Count() == 0 ? null : result.First();
        }

        /// <summary>
        ///     Get User by it's UserID.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User FindUserByID(this Database database, int userID)
        {
            var predicates = new Dictionary<string, object> {{"id = @userID", new {userID}}};
            var result = FindAll<User>(database, predicates);

            return result.Count() == 0 ? null : result.First();
        }
    }
}