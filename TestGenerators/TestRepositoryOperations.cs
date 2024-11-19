using TestGenerators.Contract;
using TestGenerators.Entities;
using TestGenerators.Generated;

namespace TestGenerators
{
    internal class TestRepositoryOperations(MeteoDataRepository repo) : ITestRepositoryOperations
    {
        private readonly MeteoDataRepository _repo = repo;

        private static void LogMessage(MeteoData item)
        {
            const string meteoDataLogFormat = "Id = {0}, Region = {1}, Date = {2}, TemperatureMaxDeg = {3}, " +
            "TemperatureMinDeg = {4}, WindKmh = {5}, WetPercent = {6}, VisibilityKm = {7}," +
            " CloudCoveragePercent = {8}, DayDurationMin = {9}";

            string logMessage = string.Format(meteoDataLogFormat, item.Id, item.Region, item.Date, item.TemperatureMaximum,
                item.TemperatureMinimum, item.WindKmh, item.WetPercent, item.VisibilityKm,
                item.CloudCoveragePercent, item.DayDurationMin);
            Console.WriteLine(logMessage);
        }

        public void GetAllOpertation()
        {
            var datas = _repo.GetAll().Take(5).ToList();

            foreach (MeteoData item in datas)
            {
                LogMessage(item);
            }
        }

        public void TestGetElementOperation(int id)
        {
            var fifthElement = _repo.GetElement(id);
            if (fifthElement != null)
            {
                LogMessage(fifthElement);
            }
        }

        public void InsertOperation()
        {
            MeteoData newData = new()
            {
                Region = "test",
                Date = DateTime.Now,
                TemperatureMaximum = 100,
                TemperatureMinimum = 101,
                WindKmh = 102,
                WetPercent = 103,
                VisibilityKm = 104,
                CloudCoveragePercent = 105,
                DayDurationMin = 106
            };

            _repo.Insert(newData);
        }

        public void UpdateOperation(int id)
        {
            MeteoData newData = new()
            {
                Region = "test",
                Date = DateTime.Now,
                TemperatureMaximum = 200,
                TemperatureMinimum = 201,
                WindKmh = 202,
                WetPercent = 203,
                VisibilityKm = 204,
                CloudCoveragePercent = 205,
                DayDurationMin = 206
            };

            _repo.Update(newData, id);
        }

        public void DeleteOperation(int id)
        {
            _repo.Delete(id);
        }
    }
}


