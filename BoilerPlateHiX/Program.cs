using BoilerPlate;
using BoilerPlate.Request;

Console.WriteLine("MyLogicfield? " + RequestLogicFields.Id);

var req = new Request();
req.Id = "hi";

Console.WriteLine("request id: " + req.Id);