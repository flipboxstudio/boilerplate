using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using System.Data.SqlTypes;

namespace App.Services
{
    public class AppContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Initialize Naming Strategy.
        /// </summary>
        public AppContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }

        /// <summary>
        /// Create property from MemberInfo and MemberSerialization.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override JsonProperty CreateProperty(
            MemberInfo memberInfo,
            MemberSerialization memberSerialization
        ) {
            var property = base.CreateProperty(memberInfo, memberSerialization);
            var propertyType = property.PropertyType;
            var valueProvider = property.ValueProvider;

            if (IsString(propertyType))
                property.ValueProvider = new NullToEmptyStringValueProvider(valueProvider);
            else if (IsNumber(propertyType))
                property.ValueProvider = new NullToZeroValueProvider(valueProvider);
            else if (IsDateTime(propertyType))
                property.ValueProvider = new NullToMinDateValueProvider(valueProvider);

            return property;
        }

        /// <summary>
        /// Determine if given type is a string.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsString(Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Determine if given type is a number.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsNumber(Type type)
        {
            return type == typeof(int)
                || type == typeof(long)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(decimal);
        }

        /// <summary>
        /// Determine if given type is a dateTime.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool IsDateTime(Type type)
        {
            return type == typeof(DateTime) || type == typeof(SqlDateTime);
        }
    }

    public abstract class NullToAnyValueProvider : IValueProvider
    {
        /// <summary>
        /// Get default value of value specific provider.
        /// </summary>
        /// <returns></returns>
        protected abstract object DefaultValue();

        /// <summary>
        /// Base value provider.
        /// </summary>
        protected readonly IValueProvider _provider;

        /// <summary>
        /// A constructor. Here we inject base value provider for get current value.
        /// </summary>
        /// <param name="provider"></param>
        public NullToAnyValueProvider(IValueProvider provider)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));

            _provider = provider;
        }

        /// <summary>
        /// Get value of a property.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public object GetValue(object target) => _provider.GetValue(target) ?? DefaultValue();

        /// <summary>
        /// Set value of a property.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void SetValue(object target, object value)
        {
            _provider.SetValue(target, value);
        }
    }

    public class NullToEmptyStringValueProvider : NullToAnyValueProvider
    {
        /// <summary>
        /// Hack.
        /// </summary>
        /// <param name="provider"></param>
        public NullToEmptyStringValueProvider(IValueProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Return empty string ("").
        /// </summary>
        /// <returns></returns>
        protected override object DefaultValue()
        {
            return "";
        }
    }

    public class NullToZeroValueProvider : NullToAnyValueProvider
    {
        /// <summary>
        /// Hack.
        /// </summary>
        /// <param name="provider"></param>
        public NullToZeroValueProvider(IValueProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Return zero (0).
        /// </summary>
        /// <returns></returns>
        protected override object DefaultValue()
        {
            return 0;
        }
    }

    public class NullToMinDateValueProvider : NullToAnyValueProvider
    {
        /// <summary>
        /// Hack.
        /// </summary>
        /// <param name="provider"></param>
        public NullToMinDateValueProvider(IValueProvider provider) : base(provider)
        {
        }

        /// <summary>
        /// Return the lowest possible datetime value.
        /// </summary>
        /// <returns></returns>
        protected override object DefaultValue()
        {
            return DateTime.MinValue;
        }
    }
}