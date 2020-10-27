using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using Venjix.Infrastructure.DAL;

namespace Venjix.Infrastructure.Helpers
{
    public static class StoreConfigurationExtension
    {
        public static T GetStoreConfiguration<T>(this DbContext db, string key)
        {
            var sc = db.Set<Setting>().Find(key);
            if (sc == null) return default(T);

            var value = sc.Value;
            var tc = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                var convertedValue = (T)tc.ConvertFromString(value);
                return convertedValue;
            }
            catch (NotSupportedException)
            {
                return default(T);
            }
        }

        public static void SetStoreConfiguration(this DbContext db, string key, object value)
        {
            var sc = db.Set<Setting>().Find(key);
            if (sc == null)
            {
                sc = new Setting { Key = key };
                db.Set<Setting>().Add(sc);
            }

            sc.Value = value == null ? null : value.ToString();
        }
    }
}
