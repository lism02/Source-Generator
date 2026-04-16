using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration;

public interface ITemplate<TContent>
{
    bool NeedsLogicFieldsUsing { get; }
    IEnumerable<string> GetUsings();
    string GetName(ClassDeclarationSyntax classDeclaration);
    string GetSignature(ClassDeclarationSyntax classDeclaration);

    string GetContent(TContent contentInfo);
}

public interface ITemplate<TContent, TAttribute> : ITemplate<TContent>
{
    IEnumerable<string> GetAttributes(TAttribute attributeInfo);
}