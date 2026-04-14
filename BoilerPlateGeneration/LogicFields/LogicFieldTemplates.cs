using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.LogicFields;

public static class LogicFieldTemplates
{
    public static string ClassName(TypeDeclarationSyntax typeDeclaration)
        => typeDeclaration switch
        {
            InterfaceDeclarationSyntax interfaceDeclaration => ClassName(interfaceDeclaration),
            ClassDeclarationSyntax classDeclaration => ClassName(classDeclaration),
            _ => string.Empty
        };
    
    public static string ClassName(InterfaceDeclarationSyntax interfaceDeclaration)
        =>ClassName(interfaceDeclaration.Identifier.Text[1..]);
    
    public static string ClassName(ClassDeclarationSyntax classDeclaration)
        =>ClassName(classDeclaration.Identifier.Text);
    
    private static string ClassName(string name)
        => $"{name}LogicFields";
    
    public static string Class(string namespaceName, string className, params IEnumerable<string> fields)
        => $"""
            using System;
            
            namespace {namespaceName};
            
            public static class {className}
            {"{"}
                {string.Join("\n\t", fields.Select(Field))}
            {"}"}
            """;

    private static string Field(string name)
        => $"public const string {name} = \"{name}\";";
}