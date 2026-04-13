using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.Namespaces;

public class FileScoped<T> : INamespaceStrategry
    where T : BaseNamespaceDeclarationSyntax
{
    public Priority Priority => Priority.High;

    public bool CanExecute(SyntaxNode node)
        => node.Ancestors().OfType<T>().SingleOrDefault() is not null;

    public string Execute(SyntaxNode node)
        => node.Ancestors().OfType<T>().SingleOrDefault()!.Name.ToString();
}