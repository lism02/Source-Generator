using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace BoilerPlate.Request;

public class ObjectVeld
{
    public T ValueAs<T>()
    {
        return (T) Value;
    }

    public object Value { get; set; }
}

public class TimpObject
{
    public ObjectVeld this[string path] => new ObjectVeld();
}

[GenerateLogicFields]
public partial class Request : TimpObject
{
    public partial string Id { get; set; }
    public partial double Test { get; }
    public partial bool Reqint { get; set; }
}