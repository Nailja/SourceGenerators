using Microsoft.CodeAnalysis;

namespace GeneratorBase.Entities
{
    internal struct PropertyInfo
    {
        public string Name { get; set; }

        public ITypeSymbol Type { get; set; }

        public bool IsAutogenerate { get; set; }

        public string ColumnName { get; set; }

        public string RealName => !string.IsNullOrEmpty(ColumnName) ? ColumnName : Name;
    }
}
