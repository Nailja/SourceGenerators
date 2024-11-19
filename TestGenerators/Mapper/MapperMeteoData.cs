using Microsoft.Data.Sqlite;
using TestGenerators.Entities;

namespace TestGenerators
{
    public static class Mapper
    {
        public static MeteoData MeteoData(SqliteDataReader reader) => new()
        {
            Id = reader.GetInt32(0),
            Region = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
            Date = reader.GetDateTime(2),
            TemperatureMaximum = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
            TemperatureMinimum = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4),
            WindKmh = reader.IsDBNull(5) ? 0.0 : reader.GetDouble(5),
            WetPercent = reader.IsDBNull(6) ? 0.0 : reader.GetDouble(6),
            VisibilityKm = reader.IsDBNull(7) ? 0.0 : reader.GetDouble(7),
            CloudCoveragePercent = reader.IsDBNull(8) ? 0.0 : reader.GetDouble(8),
            DayDurationMin = reader.IsDBNull(9) ? 0.0 : reader.GetDouble(9)
        };
    }
}
