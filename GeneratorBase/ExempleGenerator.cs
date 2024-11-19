using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GeneratorBase.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeneratorBase
{
    [Generator]
    public partial class ExempleGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterSourceOutput(GetIncrementalValueProvider(context), (ctx, p) => GenerateCode(ctx, p.Left, p.Right.OfType<TypeDeclarationSyntax>()));
        }

        private static IncrementalValueProvider<(Compilation Left, ImmutableArray<TypeDeclarationSyntax> Right)>
            GetIncrementalValueProvider(IncrementalGeneratorInitializationContext context)
        {
            var providers = context.SyntaxProvider.CreateSyntaxProvider((n, _) => CreateSyntaxProviderPredicate(n), (c, _) => CreateSyntaxProviderTranform(c)).
                Where(p => p.HasAttribute).
                Select((p, _) => p.TypeDeclarationSyntax);

            return context.CompilationProvider.Combine(providers.Collect());
        }

        private static bool CreateSyntaxProviderPredicate(SyntaxNode node)
        {
            return node is ClassDeclarationSyntax || node is StructDeclarationSyntax;
        }

        private static (TypeDeclarationSyntax TypeDeclarationSyntax, bool HasAttribute) CreateSyntaxProviderTranform(GeneratorSyntaxContext context)
        {
            TypeDeclarationSyntax typeDeclarationSyntax = context.Node as TypeDeclarationSyntax;
            bool hasAttribute = typeDeclarationSyntax?.AttributeLists.SelectMany(l => l.Attributes)
                .Any(a => string.Equals(a.Name.ToString(), "DataTable", StringComparison.InvariantCultureIgnoreCase)) == true;
            return (typeDeclarationSyntax, hasAttribute);
        }

        private static void GenerateCode(SourceProductionContext context, Compilation compilation, IEnumerable<TypeDeclarationSyntax> declarations)
        {
            foreach (TypeDeclarationSyntax declaration in declarations)
            {
                TableInfo tableInfo = new TableInfo(compilation, declaration);

                GenerateMapper(context, tableInfo);
                GenerateRepository(context, tableInfo);
            }
        }
    }
}
