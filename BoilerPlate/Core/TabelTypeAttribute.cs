namespace BoilerPlate;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class TabelTypeAttribute : Attribute
{
    private TabelTypes _tabelType;

    public TabelTypeAttribute(TabelTypes tabelType)
    {
        _tabelType = tabelType;
    }
}