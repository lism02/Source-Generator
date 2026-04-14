using BoilerPlateGeneration.LogicFields;
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
                using System.Collections.Generic;
                using System.ComponentModel.Design;
                using System.Text;
                
                namespace BoilerPlate.Request;
                
                
                public class ObjectVeld
                {
                    public T ValueAs<T>()
                    {
                        return default;
                    }
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
