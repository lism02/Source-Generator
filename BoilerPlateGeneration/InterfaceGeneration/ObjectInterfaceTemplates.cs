using System.Collections.Generic;
using BoilerPlateGeneration.LogicFields;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.InterfaceGeneration;

public class ObjectInterfaceTemplates
{
    public static string Class(string namespaceName, ClassDeclarationSyntax classDeclaration, 
        IEnumerable<string> properties)
        => $"""
            using System;

            namespace {namespaceName};

            public partial interface I{classDeclaration.Identifier.Text}
            {"{"}
                {string.Join("\n", properties)}
            {"}"}
            """;

    public static string Property(string type, string name, bool hasGetter, bool hasSetter)
        => $"public {type} {name} {{ {Getter(hasGetter)} {Setter(hasSetter)} }} ";

    private static string Getter(bool hasGetter) => hasGetter ? "get;" : string.Empty;
    private static string Setter(bool hasSetter) => hasSetter ? "set;" : string.Empty;
}