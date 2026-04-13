using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shared;
using System;
using System.Linq;
using System.Text;

namespace BoilerPlateGeneration;

[Generator]
public class LogicFieldsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        GeneratorLogging.SetLogFilePath(@"C:\Chipsoft\BoilerPlateGeneration\LoggiesLogicFields.txt");

        GeneratorLogging.LogMessage($"[+] Do you even run buddy?");
        GeneratorLogging.LogMessage($"[+] Yes the answer is yes?");

        IncrementalValuesProvider<InterfaceDeclarationSyntax> calculatorClassesProvider =
            context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (node, cancelToken)
                    => node is InterfaceDeclarationSyntax classDeclaration &&
                       classDeclaration.Identifier.ToString() == "IRequest",
                transform: (ctx, cancelToken)
                    => (InterfaceDeclarationSyntax) ctx.Node);

        //var wtf = typeof(GenerateLogicFieldsAttribute);


        var toGenerateFor = context.SyntaxProvider.ForAttributeWithMetadataName(
                "BoilerPlate.GenerateLogicFieldsAttribute",
                (node, cancelToken) => node is InterfaceDeclarationSyntax

                //inter
                //&& inter.AttributeLists.Any(attribute => attribute.Attributes.Any(at =>
                //{
                //    var hi = at.Name;
                //    return true;
                //}))
                ,
                (ctx, cancelToken) => (InterfaceDeclarationSyntax) ctx.TargetNode)
            //.Collect()
            //.Select((ini,cancelToken)=>ini)
            ;


        var who = calculatorClassesProvider.Collect();


        // context.RegisterSourceOutput(toGenerateFor,
        //     (sourceProductionContext, calculatorClass) => Execute(calculatorClass, sourceProductionContext));

        context.RegisterSourceOutput(calculatorClassesProvider,
            (sourceProductionContext, calculatorClass) => Execute(calculatorClass, sourceProductionContext));
    }


    public void Execute(InterfaceDeclarationSyntax logicFieldInterface, SourceProductionContext context)
    {
        try
        {
            
            var members = logicFieldInterface.Members;


            //var calculatorClassMembers = calculatorClass.Members;
            GeneratorLogging.LogMessage(
                $"[+] Found {logicFieldInterface.Identifier} with attribute GenerateLogicFieldsAttribute");
            //check if the methods we want to add exist already 

            //this string builder will hold our source code for the methods we want to add
            StringBuilder calcGeneratedClassBuilder = new StringBuilder();
            foreach (var usingStatement in logicFieldInterface.SyntaxTree.GetCompilationUnitRoot().Usings)
            {
                calcGeneratedClassBuilder.AppendLine(usingStatement.ToString());
            }
            //GeneratorLogging.LogMessage("[+] Added using statements to generated class");

            calcGeneratedClassBuilder.AppendLine();

            //The previous Descendent Node check has been removed as it was only intended to help produce the error seen in logging
            var namespaceName = logicFieldInterface.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().SingleOrDefault()?.Name;


            calcGeneratedClassBuilder.AppendLine($"namespace {namespaceName};");
            calcGeneratedClassBuilder.AppendLine($"public class {logicFieldInterface.Identifier.Text[1..]}LogicFields");
            calcGeneratedClassBuilder.AppendLine("{");


            foreach (var member in members)
            {
                if (member is PropertyDeclarationSyntax property)
                {
                    AddLogicField(calcGeneratedClassBuilder, property.Identifier.Text);
                }
            }
            

            calcGeneratedClassBuilder.AppendLine("}");
            
            context.AddSource($"MyLogicFields.g.cs", calcGeneratedClassBuilder.ToString());
            GeneratorLogging.LogMessage("[+] Added source to context");
        }
        catch (Exception e)
        {
            GeneratorLogging.LogMessage($"[-] Exception occurred in generator: {e}", LoggingLevel.Error);
        }
    }
    
    private void AddLogicField(StringBuilder builder,string name)
    {
        builder.AppendLine($"\tpublic const string {name} = \"{name}\";");
    }
}