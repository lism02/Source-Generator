using Microsoft.CodeAnalysis;

namespace BoilerPlateGeneration.Namespaces;

public class Fallback : INamespaceStrategry
{
    public Priority Priority => Priority.Low;

    public bool CanExecute(SyntaxNode node)
        => true;

    public string Execute(SyntaxNode node)
        => "Unknown.Fallback";
}