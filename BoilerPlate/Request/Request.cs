using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace BoilerPlate.Request;

public class Veld
{
}

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
[LogicInfo(
    ClassId = "MyLogicClassId",
    PrimaryDisplayField = nameof(Id),
    Guid = "guid",
    TabelType = TabelTypes.Production)]
public partial class Request : TimpObject
{
    [ExternalProperty] [IdField] public partial string Id { get; set; }

    [DefaultLookupDisplayField] public partial double Test { get; }
    public partial bool Reqint { get; set; }
}