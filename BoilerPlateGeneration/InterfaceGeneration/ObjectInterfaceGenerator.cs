using System.Linq;
using BoilerPlateGeneration.ObjectImplementation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.InterfaceGeneration;

[Generator]
public class ObjectInterfaceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var toGenerateFor =
            context.SyntaxProvider.CreateSyntaxProvider(
                (node, cancelToken) => node is ClassDeclarationSyntax classDeclaration
                                       && classDeclaration.IsPartial
                                       && classDeclaration.IsSubClassOf("TimpObject"),
                (ctx, cancelToken) => (
                    (ClassDeclarationSyntax) ctx.Node,
                    GetContentInfo((INamedTypeSymbol) ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, cancelToken)),
                    GetAttributeInfo((INamedTypeSymbol) ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, cancelToken)))
            );


        // var logics = context.SyntaxProvider.ForAttributeWithMetadataName("ClassRegistrationAttribute",
        //     (node, token) => node is ClassDeclarationSyntax classDeclaration
        //                      && IsSubclassOfTimpObject(classDeclaration, "GenericObjectLogic"),
        //     (ctx, token) => new GenerationInfo((ClassDeclarationSyntax) ctx.TargetNode, ctx.TargetSymbol)) ;

        context.RegisterImplementationSourceOutput(toGenerateFor,
            (ctx, generationInfo) => Execute(context, ctx, generationInfo));
    }

    private InterfaceAttributeInfo GetAttributeInfo(INamedTypeSymbol symbol)
    {
        var logicInfo = symbol
            .GetAttributes()
            .SingleOrDefault(attribute => attribute.AttributeClass?.Name == "LogicInfoAttribute")
            ?.NamedArguments;
        return new(logicInfo.Get<string>("ClassId"));
    }

    private InterfaceContentInfo GetContentInfo(INamedTypeSymbol symbol)
        => new(symbol
            .GetMembers()
            .OfType<IPropertySymbol>()
            .Where(property => property.IsPartialDefinition)
            .Select(property => new GenerationPropertyInfoWithExternalInfo(
                property.HasAttribute("ExternalPropertyAttribute"),
                property.Type.Name,
                property.Name,
                property.GetMethod is not null,
                property.SetMethod is not null)));

    public void Execute(IncrementalGeneratorInitializationContext ctx, SourceProductionContext context,
        (ClassDeclarationSyntax classDeclaration, InterfaceContentInfo contentInfo, InterfaceAttributeInfo attributeInfo
            ) info)
    {
        if (!info.contentInfo.Properties.Any())
        {
            return;
        }

        var publicProperties = info.contentInfo.Properties
            .Where(property =>
                property is GenerationPropertyInfoWithExternalInfo externalPropertyInfo
                && externalPropertyInfo.HasExternalPropertyAttribute)
            .ToList();

        var internalProperties = info.contentInfo.Properties
            .Where(property =>
                property is GenerationPropertyInfoWithExternalInfo externalPropertyInfo
                && !externalPropertyInfo.HasExternalPropertyAttribute)
            .ToList();

        if (publicProperties.Any() && internalProperties.Any())
        {
            var (fileNameInterface, contentInterface) =
                TemplateManager.Run(new ObjectInterfaceTemplates(), info.classDeclaration,
                    new InterfaceContentInfo(publicProperties), info.attributeInfo);

            var (fileNameInternalInterface, contentInternalInterface) =
                TemplateManager.Run(new ObjectInterfaceInternalTemplates(), info.classDeclaration,
                    new InterfaceContentInfo(internalProperties), info.attributeInfo);

            context.AddSource($"{fileNameInterface}.g.cs", contentInterface);
            context.AddSource($"{fileNameInternalInterface}.g.cs", contentInternalInterface);
        }
        else
        {
            var (fileNameInterface, contentInterface) =
                TemplateManager.Run(new ObjectInterfaceTemplates(), info.classDeclaration,
                    info.contentInfo, info.attributeInfo);

            context.AddSource($"{fileNameInterface}.g.cs", contentInterface);
        }
    }
}