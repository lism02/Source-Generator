using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration;

public static class Extensions
{
    extension(ClassDeclarationSyntax classDeclaration)
    {
        public bool IsPartial => classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword);

        public bool IsSubClassOf(string type)
            => classDeclaration.BaseList is not null
               && classDeclaration.BaseList.Types
                   .Any(baseType => baseType.Type is IdentifierNameSyntax name && name.Identifier.Text == type);
    }

    extension(IPropertySymbol property)
    {
        public bool HasAttribute(string attribute)
            => property.GetAttributes().Any(attr => attr.AttributeClass?.Name == attribute);
    }

    public static T Get<T>(this ImmutableArray<KeyValuePair<string, TypedConstant>>? list, string key)
        => (T) list?.SingleOrDefault(item => item.Key == key).Value.Value;

    public static IEnumerable<string> GetPropertyNamesWithAttribute(this IEnumerable<IPropertySymbol>? properties,
        string attributeName)
        => properties is null
            ? [string.Empty]
            : properties
                .Where(property => property.GetAttributes()
                    .Any(attribute => attribute.AttributeClass?.Name == attributeName))
                .Select(property => property.Name);
}