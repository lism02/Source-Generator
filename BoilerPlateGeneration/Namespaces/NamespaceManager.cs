using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.Namespaces;

//Trying some easy things out for MedAdmin/MDA/VSB/MediPrima
public class NamespaceManager
{
    public static string Run(SyntaxNode node)
    {
        IEnumerable<INamespaceStrategry> strategies =
        [
            new Fallback(),
            new FileScoped<FileScopedNamespaceDeclarationSyntax>(),
            new FileScoped<NamespaceDeclarationSyntax>()
        ];

        return strategies
            .OrderBy(strategy => strategy.Priority)
            .First(strategy => strategy.CanExecute(node))
            .Execute(node);
    }
}