using System;
using System.Linq;
using System.Text;
using GeneratorBase.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace GeneratorBase
{
    public partial class ExempleGenerator
    {
        private const string ADD_QUERY_PARAMETER_FORMAT = "\t\t\tQueryParameters.Add(\"{0}\", Get{0}Value);";

        private const string GET_PARAMETERS_VALUE_METHOD_FORMAT = "\t\tinternal object Get{0}Value({1} entity) => entity.{0};";

        private static void GenerateRepository(SourceProductionContext context, TableInfo tableInfo)
        {
            var template = $@"using Microsoft.Data.Sqlite;
using TestGenerators.Entities;
using TestGenerators.DataAccess;

namespace TestGenerators.Generated
{{
    public class {tableInfo.EntityName}Repository : RepositoryBase<{tableInfo.EntityName}>
    {{
        internal override string QueryGetAll => ""{tableInfo.QueryGetAll}"";

        internal override string QueryGetOne => ""{tableInfo.QueryGetOne}"";

        internal override string QueryUpdate => ""{tableInfo.QueryUpdate}"";

        internal override string QueryInsert => ""{tableInfo.QueryInsert}"";

        internal override string QueryDelete => ""{tableInfo.QueryDelete}"";

        internal override {tableInfo.EntityName} GetElement(SqliteDataReader reader) => Mapper.{tableInfo.EntityName}(reader);

        public {tableInfo.EntityName}Repository(string connectionString) : base(connectionString) {{
{GetConstructor(tableInfo)}
        }}

{GetPropertiesValueMethods(tableInfo)}
    }}
}}
";

            context.AddSource($"Repository/{tableInfo.EntityName}Repository.g.cs", SourceText.From(template, Encoding.UTF8));
        }

        private static string GetConstructor(TableInfo tableInfo)
        {
            return string.Join(Environment.NewLine, tableInfo.Properties.Select(c => string.Format(ADD_QUERY_PARAMETER_FORMAT, c.Name)));
        }

        private static string GetPropertiesValueMethods(TableInfo tableInfo)
        {
            return string.Join(Environment.NewLine, tableInfo.Properties.Select(c => string.Format(GET_PARAMETERS_VALUE_METHOD_FORMAT, c.Name, tableInfo.EntityName)));
        }
    }
}
