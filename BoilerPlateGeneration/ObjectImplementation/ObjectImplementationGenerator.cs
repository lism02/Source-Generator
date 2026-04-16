using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.ObjectImplementation;

public record GenerationPropertyInfo(string Type, string Name, bool HasGetter, bool HasSetter);

[Generator]
public class ObjectImplementationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var toGenerateFor =
            context.SyntaxProvider.CreateSyntaxProvider(
                (node, cancelToken) => node is ClassDeclarationSyntax classDeclaration
                                       && classDeclaration.IsPartial
                                       && classDeclaration.IsSubClassOf("TimpObject"),
                (ctx, cancelToken) => ((ClassDeclarationSyntax) ctx.Node,
                    GetPropertyImplementationInfo(
                        (INamedTypeSymbol) ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, cancelToken))));

        context.RegisterImplementationSourceOutput(toGenerateFor, Execute);
    }

    private IEnumerable<GenerationPropertyInfo> GetPropertyImplementationInfo(INamedTypeSymbol? symbol)
        => symbol is null
            ? []
            : symbol.GetMembers()
                .OfType<IPropertySymbol>()
                .Where(property => property.IsPartialDefinition && property.PartialImplementationPart is null)
                .Select(property =>
                    new GenerationPropertyInfo(
                        property.Type.Name,
                        property.Name,
                        property.GetMethod is not null,
                        property.SetMethod is not null));

    public void Execute(SourceProductionContext context,
        (ClassDeclarationSyntax classDeclarationSyntax, IEnumerable<GenerationPropertyInfo> properties) info)
    {
        var (fileName, content) =
            TemplateManager.Run(new ObjectImplementationTemplates(), info.classDeclarationSyntax, info.properties);
        context.AddSource($"{fileName}.g.cs", content);
    }
}