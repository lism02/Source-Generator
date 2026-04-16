namespace BoilerPlate;

[AttributeUsage(AttributeTargets.Class| AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class BelongsToLogicAttribute:Attribute
{
    private readonly string _classId;

    public BelongsToLogicAttribute(string classId)
    {
        _classId = classId;
    }
}