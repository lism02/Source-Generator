using System;
using System.Linq;
using BoilerPlateGeneration.Namespaces;
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
            (node, cancelToken) => node is TypeDeclarationSyntax,
            (ctx, cancelToken) => (TypeDeclarationSyntax) ctx.TargetNode);

        context.RegisterImplementationSourceOutput(toGenerateForList,
            (sourceProductionContext, toGenerateFor) => Execute(toGenerateFor, sourceProductionContext));
    }


    private void Execute(TypeDeclarationSyntax logicFieldType, SourceProductionContext context)
    {
        try
        {
            var className = LogicFieldTemplates.ClassName(logicFieldType);

            context.AddSource($"{className}.g.cs",
                LogicFieldTemplates.Class(
                    NamespaceManager.Run(logicFieldType), 
                    className, 
                    logicFieldType.Members
                        .OfType<PropertyDeclarationSyntax>()
                        .Select(member => member.Identifier.Text)
                        .Distinct()));
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}