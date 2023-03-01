using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;

namespace E_CommercialAPI.API.Configurations.ColumnWriters
{
    public class UserNameColumnWriter : ColumnWriterBase
    {
        public UserNameColumnWriter() : base(NpgsqlDbType.Varchar)
        {
        }

        public override object GetValue(LogEvent logEvent, IFormatProvider formatProvider = null)
        {
            var (username, value) = logEvent.Properties.FirstOrDefault(p => p.Key == "user_name"); // burdaki user_name propertysini Program.cs de middleware icerisinde elave etmisdik.

            return value?.ToString() ?? null;
        }
    }
}
