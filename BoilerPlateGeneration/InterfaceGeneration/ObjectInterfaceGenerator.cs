using System.Linq;
using BoilerPlateGeneration.ObjectImplementation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.InterfaceGeneration;

[Generator]
public class ObjectInterfaceGenerator : IIncrementalGenerator
{
    public record GenerationPropertyInfoWithExternalInfo(
        bool HasExternalPropertyAttribute,
        string Type,
        string Name,
        bool HasGetter,
        bool HasSetter)
        : GenerationPropertyInfo(Type, Name, HasGetter, HasSetter);

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var toGenerateFor =
            context.SyntaxProvider.CreateSyntaxProvider(
                (node, cancelToken) => node is ClassDeclarationSyntax classDeclaration
                                       && classDeclaration.IsPartial
                                       && classDeclaration.IsSubClassOf("TimpObject"),
                (ctx, cancelToken) => (
                    (ClassDeclarationSyntax) ctx.Node,
                    GetInterfaceInfo((INamedTypeSymbol) ctx.SemanticModel.GetDeclaredSymbol(ctx.Node, cancelToken)))
            );


        // var logics = context.SyntaxProvider.ForAttributeWithMetadataName("ClassRegistrationAttribute",
        //     (node, token) => node is ClassDeclarationSyntax classDeclaration
        //                      && IsSubclassOfTimpObject(classDeclaration, "GenericObjectLogic"),
        //     (ctx, token) => new GenerationInfo((ClassDeclarationSyntax) ctx.TargetNode, ctx.TargetSymbol)) ;

        context.RegisterImplementationSourceOutput(toGenerateFor,
            (ctx, generationInfo) => Execute(context, ctx, generationInfo));
    }

    private InterfaceInfo GetInterfaceInfo(INamedTypeSymbol symbol)
        => new InterfaceInfo(symbol
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
        (ClassDeclarationSyntax classDeclaration, InterfaceInfo interfaceInfo) info)
    {
        if (!info.interfaceInfo.Properties.Any())
        {
            return;
        }
        
        var publicProperties = info.interfaceInfo.Properties
            .Where(property =>
                property is GenerationPropertyInfoWithExternalInfo externalPropertyInfo
                && externalPropertyInfo.HasExternalPropertyAttribute)
            .ToList();

        var internalProperties = info.interfaceInfo.Properties
            .Where(property =>
                property is GenerationPropertyInfoWithExternalInfo externalPropertyInfo
                && !externalPropertyInfo.HasExternalPropertyAttribute)
            .ToList();
        
        if (publicProperties.Any() && internalProperties.Any())
        {
            var (fileNameInterface, contentInterface) =
                TemplateManager.Run(new ObjectInterfaceTemplates(), info.classDeclaration, new InterfaceInfo(publicProperties));

            var (fileNameInternalInterface, contentInternalInterface) =
                TemplateManager.Run(new ObjectInterfaceInternalTemplates(), info.classDeclaration, new InterfaceInfo(internalProperties));

            context.AddSource($"{fileNameInterface}.g.cs", contentInterface);
            context.AddSource($"{fileNameInternalInterface}.g.cs", contentInternalInterface);
        }
        else
        {
            var (fileNameInterface, contentInterface) =
                TemplateManager.Run(new ObjectInterfaceTemplates(), info.classDeclaration, info.interfaceInfo);

            context.AddSource($"{fileNameInterface}.g.cs", contentInterface);
        }
    }
}