using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace BoilerPlate.Request;

[GenerateLogicFields]
[LogicInfo(
    ClassId = "MyLogicClassId",
    PrimaryDisplayField = nameof(Id),
    Guid = "guid",
    TabelType = TabelTypes.Production, RightsId = "RequestRight")]
public partial class Request : TimpObject
{
    [ExternalProperty] [IdField] public partial string Id { get; set; }

    [DefaultLookupDisplayField] public partial double Test { get; }
    public partial bool Reqint { get; set; }
}