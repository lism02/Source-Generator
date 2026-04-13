using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Shared;
using System;
using System.Linq;
using System.Text;

namespace BoilerPlateGeneration;

// Code mostly from tutorial https://specterops.io/blog/2024/10/01/dotnet-source-generators-in-2024-part-1-getting-started/?source=rss----f05f8696e3cc---4

[Generator]
public class CalculatorGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> calculatorClassesProvider = context.SyntaxProvider.CreateSyntaxProvider(
        predicate: (node, cancelToken)
            => node is ClassDeclarationSyntax classDeclaration && classDeclaration.Identifier.ToString() == "Calculator",
        transform: (ctx, cancelToken)
            => (ClassDeclarationSyntax)ctx.Node);

        context.RegisterSourceOutput(calculatorClassesProvider,
            (sourceProductionContext, calculatorClass) => Execute(calculatorClass, sourceProductionContext));
    }

    public void Execute(ClassDeclarationSyntax calculatorClass, SourceProductionContext context)
    {
        GeneratorLogging.SetLogFilePath(@"C:\Chipsoft\BoilerPlateGeneration\Loggies.txt");
        try
        {
            var calculatorClassMembers = calculatorClass.Members;
            GeneratorLogging.LogMessage($"[+] Found {calculatorClassMembers.Count} members in the Calculator class");
            //check if the methods we want to add exist already 

            //this string builder will hold our source code for the methods we want to add
            StringBuilder calcGeneratedClassBuilder = new StringBuilder();
            foreach (var usingStatement in calculatorClass.SyntaxTree.GetCompilationUnitRoot().Usings)
            {
                calcGeneratedClassBuilder.AppendLine(usingStatement.ToString());
            }
            GeneratorLogging.LogMessage("[+] Added using statements to generated class");

            calcGeneratedClassBuilder.AppendLine();

            //The previous Descendent Node check has been removed as it was only intended to help produce the error seen in logging
            BaseNamespaceDeclarationSyntax? calcClassNamespace = calculatorClass.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            calcClassNamespace ??= calculatorClass.Ancestors().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();

            if (calcClassNamespace is null)
            {
                GeneratorLogging.LogMessage("[-] Could not find namespace for Calculator class", LoggingLevel.Error);
            }
            GeneratorLogging.LogMessage($"[+] Found namespace for Calculator class {calcClassNamespace?.Name}");
            calcGeneratedClassBuilder.AppendLine($"namespace {calcClassNamespace?.Name};");
            calcGeneratedClassBuilder.AppendLine($"{calculatorClass.Modifiers} class {calculatorClass.Identifier}");
            calcGeneratedClassBuilder.AppendLine("{");
            calcGeneratedClassBuilder.AppendLine("//hello comment");

            AddMethodAdd(calcGeneratedClassBuilder, calculatorClassMembers);

            calcGeneratedClassBuilder.AppendLine("}");
            //while a bit crude it is a simple way to add the methods to the class

            GeneratorLogging.LogMessage("[+] Added methods to generated class");

            //to write our source file we can use the context object that was passed in
            //this will automatically use the path we provided in the target projects csproj file
            context.AddSource("Calculator.g.cs", calcGeneratedClassBuilder.ToString());
            GeneratorLogging.LogMessage("[+] Added source to context");
        }
        catch (Exception e)
        {
            GeneratorLogging.LogMessage($"[-] Exception occurred in generator: {e}", LoggingLevel.Error);
        }
    }

    private void AddMethodAdd(StringBuilder builder, SyntaxList<MemberDeclarationSyntax> calculatorClassMembers)
    {

        var addMethod = calculatorClassMembers.FirstOrDefault(member => member is MethodDeclarationSyntax method && method.Identifier.Text == "Add");
        if (addMethod is null)
        {
            //when using a raw string the first " is the far left margin in the file
            //if you want the proper indention on the methods you will want to tab the string content at least once
            builder.AppendLine(
            """
                    public int Add(int a, int b)
                    {
                        var result = a + b;
                        Console.WriteLine($"The result of adding {a} and {b} is {result}");
                        return result;
                    }
                """);
        }
    }

}