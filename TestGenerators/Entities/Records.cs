using Attributes;

namespace TestGenerators.Entities
{
    [DataTable("Records")]
    public class Records
    {
        [DataField(true)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [DataField("TempMax")]
        public double TempMaximum { get; set; }
        [DataField("TempMid")]
        public double TemperatureMiddle { get; set; }
        [DataField("TempMin")]
        public double TemperatureMinimum { get; set; }
        [DataField("DegUni")]
        public double DegresDaysUnited { get; set; }
        [DataField("RayonGlob")]
        public double RayonnementGlobal { get; set; }
        [DataField("DurSun")]
        public double DurationSunny { get; set; }
        [DataField("Etp")]
        public double EvapotranspirationPotential { get; set; }

        public double SpeedWind { get; set; }

        public double Precipitations { get; set; }
    }
}
