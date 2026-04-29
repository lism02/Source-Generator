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
                using System;
                
                public class TimpObject
                {
                    public ObjectVeld this[string path] => new ObjectVeld();
                }
                
                public partial class Request : TimpObject
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

            var generator = new ObjectImplementationGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);

            var result = driver.RunGenerators(compilation);

        }
    }
}
