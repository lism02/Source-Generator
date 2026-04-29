namespace BoilerPlate;

public class ObjectVeld
{
    public T ValueAs<T>()
    {
        return (T) Value;
    }

    public object Value { get; set; }
}