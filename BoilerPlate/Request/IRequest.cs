namespace BoilerPlate.Request;

[GenerateLogicFields]
public interface IRequest
{
    string Id { get; }
    string MyTest { get; set; }
}