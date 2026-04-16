using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.ObjectImplementation;

public class ObjectImplementationTemplates : ITemplate<IEnumerable<GenerationPropertyInfo>>
{
    public bool NeedsLogicFieldsUsing => true;

    public IEnumerable<string> GetUsings()
        => ["using System;",];

    public string GetName(ClassDeclarationSyntax classDeclaration)
        => classDeclaration.Identifier.Text;

    public string GetSignature(ClassDeclarationSyntax classDeclaration)
        => $"public partial class {GetName(classDeclaration)}";

    public string GetContent(IEnumerable<GenerationPropertyInfo> contentInfo)
        => string.Join("\n\t", contentInfo.Select(GetImplementationProperty));
    
    private static string GetImplementationProperty(GenerationPropertyInfo info)
        => (info.HasGetter, info.HasSetter) switch
        {
            (true, true) => $"""
                             public partial {info.Type} {info.Name} 
                                 {"{"}
                                     get => this[LogicFields.{info.Name}].ValueAs<{info.Type}>();
                                     set => this[LogicFields.{info.Name}].Value = value;
                                 {"}"}
                             """,
            (true, false) => $"public partial {info.Type} {info.Name} => this[LogicFields.{info.Name}].ValueAs<{info.Type}>();",
            _ => string.Empty
        };
}