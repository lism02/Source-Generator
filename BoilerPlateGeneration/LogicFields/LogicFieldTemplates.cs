using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.LogicFields;

public class LogicFieldTemplates : ITemplate<LogicFieldInfo>
{
    public bool NeedsLogicFieldsUsing => false;

    public IEnumerable<string> GetUsings()
        => ["using System;"];

    public string GetName(ClassDeclarationSyntax classDeclaration)
        => $"{classDeclaration.Identifier.Text}LogicFields";

    public string GetSignature(ClassDeclarationSyntax classDeclaration)
        => $"public static class {GetName(classDeclaration)}";

    public IEnumerable<string> GetModifiers()
        => ["static"];

    public string GetContent(LogicFieldInfo contentInfo)
        => string.Join("\n\t", contentInfo.Fields.Select(field=>$"public const string {GetFieldName(field)} = \"{GetFieldName(field)}\";"));

    public string GetFieldName(string field)
        => field;
}