using System;
using System.Collections.Generic;

namespace BlazorCausality
{
    public class PropertyFinder<EntityBase> where EntityBase : class
    {
        public static string GetValueAsString(string propertyName, IEnumerable<PropertyBase> list)
        {
            string ret = "";
            try
            {
                foreach (PropertyBase item in list)
                {
                    if (item.Key.ToLower().Equals(propertyName.ToLower()))
                    {
                        return item.Value;
                    }
                }
                return ret;
            }
            catch
            {
                return ret;
            }
        }
        public static Int32 GetValueAsInt32(string propertyName, IEnumerable<PropertyBase> list)
        {
            int ret = 0;
            try
            {
                foreach (PropertyBase item in list)
                {
                    if (item.Key.ToLower().Equals(propertyName.ToLower()))
                    {
                        return Int32.Parse(item.Value);
                    }
                }
                return ret;
            }
            catch
            {
                return ret;
            }
        }
        public static DateTime GetValueAsDateTime(string propertyName, IEnumerable<PropertyBase> list)
        {
            DateTime ret = new DateTime();
            try
            {
                foreach (PropertyBase item in list)
                {
                    if (item.Key.ToLower().Equals(propertyName.ToLower()))
                    {
                        _ = DateTime.TryParse(item.Value, out DateTime dt);
                        return dt;
                    }
                }
                return ret;
            }
            catch
            {
                return ret;
            }
        }
    }
}
