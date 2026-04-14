using System.Linq;
using BoilerPlateGeneration.Namespaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.InterfaceGeneration;

[Generator]
public class ObjectInterfaceGenerator : IIncrementalGenerator
{
    public record GenerationInfo(ClassDeclarationSyntax ClassDeclaration, ISymbol? Symbol);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var toGenerateFor =
            context.SyntaxProvider.CreateSyntaxProvider(
                (node, cancelToken) => node is ClassDeclarationSyntax classDeclaration
                                       && IsPartialClass(classDeclaration)
                                       && IsSubclassOfTimpObject(classDeclaration),
                (ctx, cancelToken) => new GenerationInfo((ClassDeclarationSyntax) ctx.Node,
                    ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, cancelToken))
            );

        context.RegisterImplementationSourceOutput(toGenerateFor, Execute);
    }

    private bool IsPartialClass(ClassDeclarationSyntax classDeclaration)
        => classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);

    private bool IsSubclassOfTimpObject(ClassDeclarationSyntax classDeclaration)
        => classDeclaration.BaseList is not null
           && classDeclaration.BaseList.Types.Any(baseType => baseType.Type is IdentifierNameSyntax
           {
               Identifier.Text: "TimpObject"
           });


    public void Execute(SourceProductionContext context, GenerationInfo generationInfo)
    {
        if (generationInfo.Symbol is not INamedTypeSymbol symbol)
        {
            return;
        }

        var properties = symbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(property => property.IsPartialDefinition)
            .Select(property =>
                ObjectInterfaceTemplates.Property(
                    property.Type.Name,
                    property.Name,
                    property.GetMethod is not null,
                    property.SetMethod is not null));


        context.AddSource($"I{generationInfo.ClassDeclaration.Identifier.Text}.g.cs",
            ObjectInterfaceTemplates.Class(
                NamespaceManager.Run(generationInfo.ClassDeclaration),
                generationInfo.ClassDeclaration,
                properties));
    }
}