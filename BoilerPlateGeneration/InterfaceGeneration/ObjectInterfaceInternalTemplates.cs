using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.InterfaceGeneration;

public class ObjectInterfaceInternalTemplates : ObjectInterfaceTemplates
{
    public override string GetName(ClassDeclarationSyntax classDeclaration)
        => $"I{classDeclaration.Identifier.Text}Internal";

    public override string GetSignature(ClassDeclarationSyntax classDeclaration)
        => $"public partial interface {GetName(classDeclaration)} : {base.GetName(classDeclaration)}";
}