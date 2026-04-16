namespace BoilerPlate;

public class GenericObjectLogic<TInterface, TImplementation>
{
    protected IList<string> ID_Velden=[]; 
    protected string PrimaryDisplayField { get; set; }
    protected IList<string> DefaultLookupFields = [];
    
    public virtual void CSInitialize()
    {
    }

    protected void SetGenerateUniqueIdInfo(string classId)
    {
    }

    protected void AddStringField(string name, string databaseName, string defaultValue, int length)
    {
    }
}