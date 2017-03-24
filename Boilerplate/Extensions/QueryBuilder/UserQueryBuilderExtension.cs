// ReSharper disable CheckNamespace

using System.Linq;
using Boilerplate.Model;
using Boilerplate.Services;
using Dapper;

namespace Boilerplate
{
    public static class UserQueryBuilderExtension
    {
        /// <summary>
        /// Get User by it's Username.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public static User FindUserByUsername(this Database db, string username)
        {
            return db.Connection
                .Query<User>("SELECT * FROM [dbo].[Users] WHERE [Username] = @username", new {username})
                .FirstOrDefault();
        }

        /// <summary>
        /// Get User by it's UserID.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User FindUserByID(this Database db, int userID)
        {
            return db.Connection
                .Query<User>("SELECT * FROM [dbo].[Users] WHERE [UserID] = @userID", new {userID})
                .FirstOrDefault();
        }
    }
}