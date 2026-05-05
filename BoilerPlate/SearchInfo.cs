namespace BoilerPlate;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
public class SearchInfoAttribute : Attribute
{
    public required FilterOptions FilterOption { get; set; }
}