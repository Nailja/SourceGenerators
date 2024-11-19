using Attributes;

namespace TestGenerators.Entities
{
    [DataTable("MeteoData2020")]
    public class MeteoData
    {
        [DataField(true)]
        public int Id { get; set; }
        public string? Region { get; set; }
        public DateTime Date { get; set; }
        [DataField("TemperatureMaxDeg")]
        public double TemperatureMaximum { get; set; }
        [DataField("TemperatureMinDeg")]
        public double TemperatureMinimum { get; set; }
        public double WindKmh { get; set; }
        public double WetPercent { get; set; }
        public double VisibilityKm { get; set; }
        public double CloudCoveragePercent { get; set; }
        public double DayDurationMin { get; set; }
    }
}
