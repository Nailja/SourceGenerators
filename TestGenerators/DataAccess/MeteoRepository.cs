using Microsoft.Data.Sqlite;
using TestGenerators.Contract;
using TestGenerators.Entities;

namespace TestGenerators.DataAccess
{
    public class MeteoRepository : IMeteoRepository
    {
        public MeteoRepository() { }

        public IEnumerable<MeteoData> GetMeteoDatas()
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();
            SqliteCommand command = new("select * from MeteoData2020", connection);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return Mapper.MeteoData(reader);
            }
            connection.Close();
        }

        private static SqliteConnection GetConnection()
        {
            return new SqliteConnection("Data Source = D:\\SourceGenerators\\TestGenerators\\DataBase\\meteoData.sqlite");
        }
    }
}
