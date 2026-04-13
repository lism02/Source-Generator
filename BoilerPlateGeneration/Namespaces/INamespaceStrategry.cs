using Microsoft.CodeAnalysis;

namespace BoilerPlateGeneration.Namespaces;

public interface INamespaceStrategry
{
    Priority Priority { get; }
    bool CanExecute(SyntaxNode node);
    string Execute(SyntaxNode node);
}