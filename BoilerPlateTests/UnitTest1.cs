using BoilerPlateGeneration.LogicFields;
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
                using System.Text;

                namespace BoilerPlate.Request;

                [BoilerPlate.GenerateLogicFields]
                public interface IRequest
                {
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
