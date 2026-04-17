using BoilerPlateGeneration.InterfaceGeneration;
using BoilerPlateGeneration.LogicFields;
using BoilerPlateGeneration.LogicGeneration;
using BoilerPlateGeneration.ObjectImplementation;
using Microsoft.CodeAnalysis.CSharp;

namespace BoilerPlateGeneration.Tests
{
    public class Tests
    {


        [Test]
        public void Test1()
        {
            var source = """
                [GenerateLogicFields]
                public partial class Request
                {
                    public partial string Id { get; set; }
                
                    public partial double Test { get; }
                    public partial bool Reqint { get; set; }
                }
                
                """;

            Verify(source);
        }


        private void Verify(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var compilation = CSharpCompilation.Create("TestAssembly", [syntaxTree]);

            var generator = new LogicFieldsGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);

            var result = driver.RunGenerators(compilation);

        }
    }
}
