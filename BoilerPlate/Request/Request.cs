using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace BoilerPlate.Request;


public class ObjectVeld
{
    public T ValueAs<T>()
    {
        return (T)Value;
    }

    public object Value { get; set; }
}

public class TimpObject
{
    public int HiObj { get; set; }
    public ObjectVeld this[string path] => new ObjectVeld();
}

public interface IRequest
{
    public bool Reqint { get; set; }
}

public interface IRequest2
{
    public int Reqint { get; set; }
}

[GenerateLogicFields]
public partial class Request : TimpObject, IRequest, IRequest2
{
    public partial string Id { get; set; }
    public partial double Test { get; }
    public partial bool Reqint { get; set; }
    int IRequest2.Reqint { get; set; }
}