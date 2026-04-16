using Microsoft.CodeAnalysis;

namespace BoilerPlateGeneration.LogicGeneration.FieldsAppenders;

public interface IFieldAppender
{
    string Append(IPropertySymbol property);

}