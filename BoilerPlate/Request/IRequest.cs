using System;
using System.Collections.Generic;
using System.Text;

namespace BoilerPlate.Request;

[GenerateLogicFields]
public interface IRequest
{
    string Id { get; }
}