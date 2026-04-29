namespace BoilerPlate;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class LogicInfoAttribute : Attribute
{
    public required string ClassId { get; set; }
    public required string Guid { get; set; }
    public required string PrimaryDisplayField { get; set; }
    public required TabelTypes TabelType { get; set; }
    public string RightsId { get; set; }
}