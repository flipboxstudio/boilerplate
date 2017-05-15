using Dapper;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Services {
    public static class DapperQueryBuilderExtension
    {
        /// <summary>
        ///     Find all records by criteria.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="predicates"></param>
        /// <returns></returns>
        public static IEnumerable<T> FindAll<T>(Database database,
            Dictionary<string, object> predicates)
        {
            var builder = new SqlBuilder();
            var template = builder.AddTemplate($"SELECT * FROM {GetTableAttribute<T>().Name} /**where**/");

            foreach (var predicate in predicates)
                builder.Where(predicate.Key, predicate.Value);

            return database.Connection
                .Query<T>(template.RawSql, template.Parameters);
        }

        /// <summary>
        ///     Get table information from Attribute.
        /// </summary>
        /// <returns></returns>
        public static TableAttribute GetTableAttribute<T>() => typeof(T).GetTypeInfo().GetCustomAttribute<TableAttribute>();
    }
}