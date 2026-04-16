using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.LogicGeneration;

[Generator]
public class LogicGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var to = context.SyntaxProvider.ForAttributeWithMetadataName("BoilerPlate.LogicInfoAttribute",
            (node, cancelToken) => node is ClassDeclarationSyntax,
            (ctx, cancelToken) => (
                (ClassDeclarationSyntax) ctx.TargetNode,
                GetContentInfo((INamedTypeSymbol) ctx.TargetSymbol, ctx.Attributes),
                GetAttributeInfo(ctx.Attributes))
        );

        context.RegisterSourceOutput(to, Execute);
    }

    private LogicContentInfo GetContentInfo(INamedTypeSymbol symbol, ImmutableArray<AttributeData> attributes)
    {
        var logicInfoArguments = attributes
            .SingleOrDefault(attribute => attribute.AttributeClass?.Name == "LogicInfoAttribute")
            ?.NamedArguments;

        var properties = symbol.GetMembers().OfType<IPropertySymbol>();

        return new LogicContentInfo(symbol.Name,
            logicInfoArguments.Get<string>("ClassId"),
            properties.GetPropertyNamesWithAttribute("IdFieldAttribute"),
            properties.Select(property => property.Name),
            logicInfoArguments.Get<string>("PrimaryDisplayField"),
            properties.GetPropertyNamesWithAttribute("DefaultLookupDisplayFieldAttribute"));
    }

    private LogicAttributeInfo GetAttributeInfo(ImmutableArray<AttributeData> attributes)
    {
        var arguments = attributes
            .SingleOrDefault(attribute => attribute.AttributeClass?.Name == "LogicInfoAttribute")
            ?.NamedArguments;
        return new LogicAttributeInfo(
            arguments.Get<string>("Group"),
            arguments.Get<string>("Guid"),
            arguments.Get<int>("TabelType"));
    }


    public void Execute(SourceProductionContext context,
        (ClassDeclarationSyntax classDeclaration, LogicContentInfo contentInfo, LogicAttributeInfo attributeInfo) info)
    {
        var (fileName, content) = TemplateManager.Run(new LogicTemplates(),
            info.classDeclaration, info.contentInfo, info.attributeInfo);
        context.AddSource($"{fileName}.g.cs", content);
    }
}