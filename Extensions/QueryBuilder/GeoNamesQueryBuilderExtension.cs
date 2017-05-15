using App.Model;
using static App.Services.DapperQueryBuilderExtension;
using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace App.Services
{
    public static class GeoNamesQueryBuilderExtension
    {
        public static IEnumerable<GeoNames> SearchGeoNames(this Database database, string query)
        {
            var predicates = new Dictionary<string, object> {
                {"(name LIKE @query) OR (asciiname LIKE @query) OR (alternatenames LIKE @query)", new {
                    query = $"%{query}%"
                }}
            };
            
            return FindAll<GeoNames>(database, predicates);
        }
    }
}