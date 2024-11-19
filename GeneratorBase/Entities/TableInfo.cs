using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeneratorBase.Entities
{
    internal class TableInfo
    {
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
        public List<PropertyInfo> Properties { get; set; }
        public string EntityName { get; set; }

        public string QueryGetAll => $"select * from {Name}";
        public string QueryGetOne => $"SELECT* FROM {Name} WHERE {PrimaryKey} = @{PrimaryKey}";
        public string QueryUpdate => $"UPDATE {Name} SET {UpdateAssignements} WHERE {PrimaryKey} = @{PrimaryKey}";
        public string QueryInsert => $"INSERT INTO {Name} ({InsertColumns}) VALUES({InsertParameters})";
        public string QueryDelete => $"DELETE FROM {Name} WHERE {PrimaryKey} = @{PrimaryKey}";

        private string UpdateAssignements => string.Join(", ", Properties.Where(prop => !prop.IsAutogenerate)
                .Select(p => !string.IsNullOrEmpty(p.ColumnName) ?
                $"{p.ColumnName} = @{p.Name}" :
                $"{p.Name} = @{p.Name}"));

        private string InsertColumns => string.Join(", ", Properties.Where(prop => !prop.IsAutogenerate).Select(p => p.RealName));
        private string InsertParameters => string.Join(", ", Properties.Where(prop => !prop.IsAutogenerate).Select(p => "@" + p.Name));

        public TableInfo(Compilation compilation, TypeDeclarationSyntax declaration)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(declaration.SyntaxTree);
            ISymbol symbol = semanticModel.GetDeclaredSymbol(declaration);
            AttributeData tableAttribute = symbol.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass.Name == "DataTableAttribute");
            string tableName = tableAttribute.ConstructorArguments
                .FirstOrDefault().Value?.ToString();
            string primaryKey = tableAttribute.ConstructorArguments.Length > 1
                    ? tableAttribute.ConstructorArguments[1].Value as string
                    : "ID";

            Name = tableName;
            EntityName = declaration.Identifier.Text;
            PrimaryKey = primaryKey;
            Properties = declaration.Members.OfType<PropertyDeclarationSyntax>()
           .Select(p => GetPropertyInfo(p, semanticModel))
           .ToList();
        }

        private static Entities.PropertyInfo GetPropertyInfo(PropertyDeclarationSyntax property, SemanticModel semanticModel)
        {
            IPropertySymbol propertySymbol = semanticModel.GetDeclaredSymbol(property) as IPropertySymbol;
            var dataFieldAttribute = propertySymbol?.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass.Name == "DataField");

            bool isAutogenerate = GetPropertyIsAutogenerate(dataFieldAttribute);
            string tableName = GetPropertyTableName(dataFieldAttribute);

            return new Entities.PropertyInfo()
            {
                Name = property.Identifier.Text,
                Type = propertySymbol?.Type,
                IsAutogenerate = isAutogenerate,
                ColumnName = tableName
            };
        }

        private static bool GetPropertyIsAutogenerate(AttributeData dataFieldAttribute)
        {
            if (dataFieldAttribute != null && dataFieldAttribute.ConstructorArguments.Length > 0)
            {
                int isAutogenerateIndex = dataFieldAttribute.ConstructorArguments.Length == 2 ? 1 : 0;
                return dataFieldAttribute.ConstructorArguments[isAutogenerateIndex].Value is bool isAutogenerate && isAutogenerate;
            }
            return false;
        }

        private static string GetPropertyTableName(AttributeData dataFieldAttribute)
        {
            return dataFieldAttribute != null &&
                dataFieldAttribute.ConstructorArguments.Length > 0 &&
                dataFieldAttribute.ConstructorArguments[0].Value is string tableName ?
                tableName :
                string.Empty;
        }
    }
}
