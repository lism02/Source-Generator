using System.Collections.Generic;
using System.Linq;
using BoilerPlateGeneration.LogicFields;
using BoilerPlateGeneration.ObjectImplementation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.InterfaceGeneration;

public record InterfaceContentInfo(IEnumerable<GenerationPropertyInfo> Properties);

public record InterfaceAttributeInfo(string BelongsToLogic);

public class ObjectInterfaceTemplates:ITemplate<InterfaceContentInfo, InterfaceAttributeInfo>
{
    public bool NeedsLogicFieldsUsing => false;

    public IEnumerable<string> GetUsings()
        => ["using System;"];

    public virtual string GetName(ClassDeclarationSyntax classDeclaration)
        => $"I{classDeclaration.Identifier.Text}";

    public virtual string GetSignature(ClassDeclarationSyntax classDeclaration)
        => $"public partial interface {GetName(classDeclaration)}";

    public string GetContent(InterfaceContentInfo contentContentInfo)
        => string.Join("\n", contentContentInfo.Properties.Select(Property));
    
    private static string Property(GenerationPropertyInfo property)
        => $"public {property.Type} {property.Name} {{ {Getter(property.HasGetter)} {Setter(property.HasSetter)} }} ";

    private static string Getter(bool hasGetter) => hasGetter ? "get;" : string.Empty;
    private static string Setter(bool hasSetter) => hasSetter ? "set;" : string.Empty;

    public IEnumerable<string> GetAttributes(InterfaceAttributeInfo attributeInfo)
        => [$"[BelongsToLogic(\"{attributeInfo.BelongsToLogic}\")]"];
}