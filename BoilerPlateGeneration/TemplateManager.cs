using BoilerPlateGeneration.LogicFields;
using BoilerPlateGeneration.Namespaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration;

public class TemplateManager
{
    public static (string FileName, string Content) Run<T>(ITemplate<T> template,
        ClassDeclarationSyntax classDeclaration, T info)
        => (template.GetName(classDeclaration),
            $"""
             {string.Join("\n", template.GetUsings())}
             {GetLogicFieldsUsing(template, classDeclaration)}

             namespace {NamespaceManager.Run(classDeclaration)};

             {template.GetSignature(classDeclaration)}
             {"{"}
                 {template.GetContent(info)}
             {"}"}
             """);

    public static (string FileName, string Content) Run<TContent, TAttribute>(ITemplate<TContent, TAttribute> template,
        ClassDeclarationSyntax classDeclaration, TContent contentInfo, TAttribute attributeInfo)
        => (template.GetName(classDeclaration),
            $"""
             {string.Join("\n", template.GetUsings())}
             {GetLogicFieldsUsing(template, classDeclaration)}

             namespace {NamespaceManager.Run(classDeclaration)};

             {string.Join("\n", template.GetAttributes(attributeInfo))}
             {template.GetSignature(classDeclaration)}
             {"{"}
                 {template.GetContent(contentInfo)}
             {"}"}
             """);

    private static string GetLogicFieldsUsing<T>(ITemplate<T> template, ClassDeclarationSyntax classDeclaration)
        => template.NeedsLogicFieldsUsing
            ? $"using LogicFields = {NamespaceManager.Run(classDeclaration)}.{new LogicFieldTemplates().GetName(classDeclaration)};"
            : string.Empty;
}