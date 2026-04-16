namespace BoilerPlate;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public class LogicGroupAttribute : Attribute
{
    public required string Group { get; set; }
}