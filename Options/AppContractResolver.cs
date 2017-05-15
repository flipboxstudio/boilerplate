using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace App.Options
{
    public class AppContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType == typeof(string))
                property.ValueProvider = new NullToEmptyStringValueProvider(property.ValueProvider);
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(double) || property.PropertyType == typeof(long))
                property.ValueProvider = new NullToZeroValueProvider(property.ValueProvider);
            else if (property.PropertyType == typeof(DateTime))
                property.ValueProvider = new NullToMinDateValueProvider(property.ValueProvider);

            return property;
        }

        public class NullToEmptyStringValueProvider : IValueProvider
        {
            private readonly IValueProvider _provider;

            public NullToEmptyStringValueProvider(IValueProvider provider)
            {
                if (provider == null) throw new ArgumentNullException(nameof(provider));

                _provider = provider;
            }

            public object GetValue(object target) => _provider.GetValue(target) ?? "";

            public void SetValue(object target, object value)
            {
                _provider.SetValue(target, value);
            }
        }

        public class NullToZeroValueProvider : IValueProvider
        {
            private readonly IValueProvider _provider;

            public NullToZeroValueProvider(IValueProvider provider)
            {
                if (provider == null) throw new ArgumentNullException(nameof(provider));

                _provider = provider;
            }

            public object GetValue(object target) => _provider.GetValue(target) ?? 0;

            public void SetValue(object target, object value)
            {
                _provider.SetValue(target, value);
            }
        }

        public class NullToMinDateValueProvider : IValueProvider
        {
            private readonly IValueProvider _provider;

            public NullToMinDateValueProvider(IValueProvider provider)
            {
                if (provider == null) throw new ArgumentNullException(nameof(provider));

                _provider = provider;
            }

            public object GetValue(object target) => _provider.GetValue(target) ?? DateTime.MinValue;

            public void SetValue(object target, object value)
            {
                _provider.SetValue(target, value);
            }
        }
    }
}