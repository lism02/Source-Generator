using System.Collections.Generic;
using System.Linq;

namespace BoilerPlateGeneration.LogicFields;

public static class LogicFieldTemplates
{
    public static string ClassName(string name)
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