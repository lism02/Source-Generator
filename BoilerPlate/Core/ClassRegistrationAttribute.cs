namespace BoilerPlate;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ClassRegistrationAttribute:Attribute
{
    public string Id { get; set; }
    public string Guid { get; set; }
    public string Group { get; set; }
}