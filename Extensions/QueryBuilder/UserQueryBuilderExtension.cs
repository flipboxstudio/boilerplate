using App.Model;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System;

namespace App.Services
{
    public static class UserQueryBuilderExtension
    {
        /// <summary>
        ///     Get User by it's email.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static User FindUserByEmail(this Database database, string email)
        {
            using(var connection = database.Connection)
            {
                var result = connection.GetRange<User>("WHERE Email = @email", new { email });

                return result.Count() == 0 ? null : result.First();
            }
        }

        /// <summary>
        ///     Get User by it's UserID.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static User FindUserByID(this Database database, int userID)
        {
            return database.Connection.Find<User>(userID);
        }

        /// <summary>
        ///     Insert new User.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static User AddUser(this Database database, User user)
        {
            using(var connection = database.Connection)
            {
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;

                connection.Insert(user);

                return user;
            }
        }

        /// <summary>
        ///     Update a User.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static User UpdateUser(this Database database, User user)
        {
            using(var connection = database.Connection)
            {
                user.UpdatedAt = DateTime.UtcNow;

                connection.Update(user);

                return user;
            }
        }
    }
}