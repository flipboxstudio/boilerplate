using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using App.Model;
using App.Services;
using Dapper;

namespace App.Attributes
{
    /// <summary>
    /// Validate to ensure that value is unique across database.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueAttribute : ValidationAttribute
    {
        /// <summary>
        /// Table name.
        /// </summary>
        private readonly string _table;

        /// <summary>
        /// Column name.
        /// </summary>
        private readonly string _column;

        /// <summary>
        /// Current operation is creating or updating.
        /// </summary>
        private readonly bool _creating;

        /// <summary>
        /// A constructor.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="creating"></param>
        public UniqueAttribute(string table, string column, bool creating = true)
        {
            _table = table;
            _column = column;
            _creating = creating;
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

            return (count > (_creating ? 0 : 1))
                ? new ValidationResult(FormatErrorMessage(_column))
                : ValidationResult.Success;
        }
    }
}