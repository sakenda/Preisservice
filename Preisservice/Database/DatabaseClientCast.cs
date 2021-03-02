using System;

namespace PreisService
{
    public static class DatabaseClientCast
    {
        public static T? DBToValue<T>(object value) where T : struct
        {
            if (value != null && value != DBNull.Value)
                return (T)value;
            else
                return null;
        }

        public static object ValueToDb<T>(this object value) where T : struct
        {
            if (value == null)
                return DBNull.Value;

            if (Nullable.GetUnderlyingType(value.GetType()) != null)
            {
                if (!((T?)value).HasValue)
                    return DBNull.Value;
            }

            return (T)value;
        }

        public static object StringToDb(this object value)
        {
            if (value == null || value == DBNull.Value || (Convert.ToString(value)).Length == 0)
                return DBNull.Value;

            return Convert.ToString(value);
        }
    }
}