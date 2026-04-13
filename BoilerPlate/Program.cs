using BoilerPlate;
using BoilerPlate.Request;

Console.WriteLine("Hello, World!");

var c = new Calculator();
Console.WriteLine("4+5 = " + c.Add(4, 5));

Console.WriteLine("MyLogicfield? "+RequestLogicFields.Id);
var hi = typeof(GenerateLogicFieldsAttribute);
