using TestGenerators.Entities;

namespace TestGenerators.Contract
{
    public interface IMeteoRepository
    {
        IEnumerable<MeteoData> GetMeteoDatas();
    }
}
