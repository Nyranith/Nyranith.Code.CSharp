using System;
using Xunit;
using Xunit.Abstractions;

namespace Nyranith.Code.CSharp.Test
{
    public class CSharpClassGeneratorTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CSharpClassGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestGenerateClass()
        {
            _testOutputHelper.WriteLine(new CSharpClassGenerator("testClass").ToString());
        }

        [Fact]
        public void TestGenerateClassWithNoModifier()
        {
            _testOutputHelper.WriteLine(new CSharpClassGenerator("testClass", accessModifer: null).ToString());
        }

        [Fact]
        public void TestGenerateConstructorInClass()
        {
            _testOutputHelper.WriteLine(new CSharpClassGenerator("testClass", _ => {
                _.AddConstructor();
            }).ToString());
        }

        [Fact]
        public void TestGenerateConstructorAndMethodClass()
        {
            _testOutputHelper.WriteLine(new CSharpClassGenerator("testClass", _ => {
                _.AddVariable<string>("testVariable")
                .AddConstructor()
                .AddMethod<string>("testMethod", body: _ =>
                {
                    _.AddEmptyLine();
                });
            }).ToString());
        }
    }
}
