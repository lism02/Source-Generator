using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.LogicFields;

[Generator]
public class LogicFieldsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var toGenerateForList = context.SyntaxProvider.ForAttributeWithMetadataName(
            "BoilerPlate.GenerateLogicFieldsAttribute",
            (node, cancelToken) => node is ClassDeclarationSyntax,
            (ctx, cancelToken) =>
                ((ClassDeclarationSyntax) ctx.TargetNode,
                    new LogicFieldInfo(GetPropertyNames((INamedTypeSymbol) ctx.TargetSymbol))));

        context.RegisterImplementationSourceOutput(toGenerateForList, Execute);
    }

    private IEnumerable<string> GetPropertyNames(INamedTypeSymbol symbol)
        => symbol.GetMembers()
            .OfType<IPropertySymbol>()
            .Select(property => property.Name)
            .ToList();

    private void Execute(SourceProductionContext context,
        (ClassDeclarationSyntax classDeclaration, LogicFieldInfo info) parameters)
    {
        var (fileName, content) = TemplateManager.Run(new LogicFieldTemplates(), 
            parameters.classDeclaration, parameters.info);

        context.AddSource($"{fileName}.g.cs", content);
    }
}