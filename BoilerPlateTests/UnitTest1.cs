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
                using System.Collections.Generic;
                using System.ComponentModel.Design;
                using System.Text;
                
                namespace BoilerPlate.Request;
                
                public class ObjectVeld
                {
                    public T ValueAs<T>()
                    {
                        return (T) Value;
                    }
                
                    public object Value { get; set; }
                }
                
                public class TimpObject
                {
                    public ObjectVeld this[string path] => new ObjectVeld();
                }
                
                [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
                public class BelongsToLogicAttribute : Attribute
                {
                    public required string ClassId { get; set; }
                }
                
                [GenerateLogicFields]
                [LogicInfo(ClassId = "MyLogicClassId", Group = "MCN", PrimaryDisplayField = RequestLogicFields.Id, Guid = "guid")]
                public partial class Request : TimpObject
                {
                    [ExternalProperty] 
                    public partial string Id { get; set; }
                    
                    [DefaultLookupDisplayField]
                    public partial double Test { get; }
                    public partial bool Reqint { get; set; }
                }
                
                public class TimpObjectLogic
                {
                    public virtual void CSInitialize()
                    {
                    }
                
                    protected void SetGenerateUniqueIdInfo(string classId)
                    {
                    }
                
                    protected void AddStringField(string name, string databaseName, string defaultValue, int length)
                    {
                    }
                }
                
                [assembly:LogicGroup(Group = "MyGroupieAssembly")]
                
                """;

            Verify(source);
        }


        private void Verify(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);
            var compilation = CSharpCompilation.Create("TestAssembly", [syntaxTree]);

            var generator = new LogicGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);

            var result = driver.RunGenerators(compilation);

        }
    }
}
