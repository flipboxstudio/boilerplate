using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using App.Model;
using App.Services;
using Dapper;

namespace App.Attributes
{
    /// <summary>
    /// Check if a value exists on the database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class ExistsAttribute : ValidationAttribute
    {
        /// <summary>
        /// Table name.
        /// </summary>
        protected readonly string _table;

        /// <summary>
        /// Column name.
        /// </summary>
        protected readonly string _column;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        public ExistsAttribute(string table, string column)
        {
            _table = table;
            _column = column;
        }

        /// <summary>
        /// Check if value is valid.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var database = (Database) validationContext
                         .GetService(typeof(Database));

            var count = database.Connection.ExecuteScalar<int>(
                $"SELECT COUNT(*) FROM {_table} WHERE {_column} = @value",
                new { value }
            );

            return (count > 0)
                ? ValidationResult.Success
                : new ValidationResult(FormatErrorMessage(_column));
        }
    }
}