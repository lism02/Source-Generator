using System;
using System.Collections.Generic;
using System.Linq;
using BoilerPlateGeneration.InterfaceGeneration;
using BoilerPlateGeneration.LogicFields;
using BoilerPlateGeneration.ObjectImplementation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BoilerPlateGeneration.LogicGeneration;

public record LogicAttributeInfo(string Group, string Guid, int TabelType);

public record LogicContentInfo(
    string Name,
    string ClassId,
    IEnumerable<string> IdFields,
    IEnumerable<GenerationLogicFieldInfo> Fields,
    string PrimaryDisplayField,
    IEnumerable<string> DefaultLookupDisplayFields);

public class LogicTemplates : ITemplate<LogicContentInfo, LogicAttributeInfo>
{
    public bool NeedsLogicFieldsUsing => true;

    public IEnumerable<string> GetUsings()
        => ["using System;"];

    public IEnumerable<string> GetAttributes(LogicAttributeInfo logicAttributeInfo)
        =>
        [
            $"[ClassRegistration(Id = ClassId, Guid = \"{logicAttributeInfo.Guid}\", Group = \"{logicAttributeInfo.Group}\")]",
            $"[TabelType((TabelTypes){logicAttributeInfo.TabelType})]"
        ];

    public string GetName(ClassDeclarationSyntax classDeclaration)
        => GetName(classDeclaration.Identifier.Text);

    private string GetName(string name)
        => $"{name}Logic";

    public string GetSignature(ClassDeclarationSyntax classDeclaration)
        =>
            $"public partial class {GetName(classDeclaration)} : GenericObjectLogic<{new ObjectInterfaceTemplates().GetName(classDeclaration)}, {new ObjectImplementationTemplates().GetName(classDeclaration)}>";

    public string GetContent(LogicContentInfo contentInfo)
    {
        var logicFieldTemplator = new LogicFieldTemplates();

        var primaryDisplayLogicField = logicFieldTemplator.GetFieldName(contentInfo.PrimaryDisplayField);
        var idLogicFields = contentInfo.IdFields.Select(logicFieldTemplator.GetFieldName);
        var logicFields = contentInfo.Fields.Select(logicFieldTemplator.GetFieldName);
        var defaultLookupDisplayLogicFields =
            contentInfo.DefaultLookupDisplayFields.Select(logicFieldTemplator.GetFieldName);
        return $"""
                    public const string ClassId = "{contentInfo.ClassId}";
                    //public static {GetName(contentInfo.Name)} Instance => BaseUtil.CreateObjectLogicByClassId(ClassId);
                    
                    public override void CSInitialize()
                    {"{"}
                        {string.Join("\n", idLogicFields.Select(AddIdField))}
                        
                        SetGenerateUniqueIdInfo(ClassId);
                        
                        InitializeFields();
                        SetSearchInfo();
                    {"}"}
                    
                    private void InitializeFields()
                    {"{"}
                        {string.Join("\n\t\t", logicFields.Select(AddField))}
                    {"}"}
                    
                    private void SetSearchInfo()
                    {"{"}
                        PrimaryDisplayField = LogicFields.{primaryDisplayLogicField};
                        {string.Join("\n\t", defaultLookupDisplayLogicFields.Select(AddDefaultLookupField))}
                    {"}"}
                """;
    }

    private static string AddIdField(string path)
        => $"ID_Velden.Add(LogicFields.{path});";

    private static string AddDefaultLookupField(string path)
        => $"DefaultLookupFields.Add(LogicFields.{path});";

    private static string AddField(string path)
        => $"AddStringField(LogicFields.{path}, LogicFields.{path}, string.Empty, 100);";

    private static string FieldMethod(Type type)
    {
        return type switch
        {
            _ when type == typeof(bool) => "AddBooleanVeld",
            _ when type == typeof(decimal) || type == typeof(float) => "AddFloatVeld",
            _ when type == typeof(int) => "AddIntegerVeld",
            _ when type == typeof(string) => "AddStringVeld",
            _ => string.Empty
        };
    }
}