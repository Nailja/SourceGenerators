using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneratorBase.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GeneratorBase
{
    public partial class ExempleGenerator
    {
        private static void GenerateMapper(SourceProductionContext context, TableInfo tableInfo)
        {
            IEnumerable<string> properties = tableInfo.Properties.Select(GetMapperProperty);
            string text = string.Join($", {Environment.NewLine}", properties);

            string source = $@"using Microsoft.Data.Sqlite;
using TestGenerators.Entities;

namespace TestGenerators.Generated
{{
    public static partial class Mapper    
    {{
        public static {tableInfo.EntityName} {tableInfo.EntityName}(SqliteDataReader reader) =>

            new {tableInfo.EntityName}
            {{
{text}
            }};
    }}
}}";

            context.AddSource($"Mapper/{tableInfo.EntityName}.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        private static string GetMapperProperty(Entities.PropertyInfo property)
        {
            if (property.Type.SpecialType == SpecialType.System_String)
            {
                return $"                {property.Name} = reader.IsDBNull(reader.GetOrdinal(\"{property.RealName}\")) ? null : reader.GetString(reader.GetOrdinal(\"{property.RealName}\"))";
            }
            else if (property.Type.SpecialType == SpecialType.System_Int32)
            {
                return $"                {property.Name} = reader.GetInt32(reader.GetOrdinal(\"{property.RealName}\"))";
            }
            else if (property.Type.SpecialType == SpecialType.System_DateTime)
            {
                return $"                {property.Name} = DateTime.Parse(reader.GetString(reader.GetOrdinal(\"{property.RealName}\")))";
            }
            else if (property.Type.SpecialType == SpecialType.System_Double)
            {
                return $"                {property.Name} = reader.GetDouble(reader.GetOrdinal(\"{property.RealName}\"))";
            }
            else
            {
                return $"                // Custom mapping for {property.Name} of type {property.Type}";
            }
        }
    }
}
