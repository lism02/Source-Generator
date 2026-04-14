using System.Collections.Generic;
using BoilerPlateGeneration.LogicFields;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.ObjectImplementation;

public class ObjectImplementationTemplates
{
    public static string Class(string namespaceName, ClassDeclarationSyntax classDeclaration, 
        IEnumerable<string> properties)
        => $"""
            using System;
            using LogicFields = {namespaceName}.{LogicFieldTemplates.ClassName(classDeclaration)};

            namespace {namespaceName};

            public partial class {classDeclaration.Identifier.Text} : TimpObject
            {"{"}
                {string.Join("\n", properties)}
            {"}"}
            """;

    public static string Property(string type, string name, bool hasGetter, bool hasSetter)
        => (hasGetter, hasSetter) switch
        {
            (true, true)=> $"""
                                    public partial {type} {name} 
                                        {"{"}
                                            get => this[LogicFields.{name}].ValueAs<{type}>();
                                            set => this[LogicFields.{name}].Value = value;
                                        {"}"}
                                    """,
            (true, false) => $"public partial {type} {name} => this[LogicFields.{name}].ValueAs<{type}>();",
            _ => string.Empty
        };
}