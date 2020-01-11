using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Nyranith.Code.CSharp.Test
{
    public class CSharpCodeGeneratorTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public CSharpCodeGeneratorTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestGenerateClass()
        {
            CSharpClassGenerator cSharpCodeGenerator = new CSharpClassGenerator("testClass");

            Assert.NotNull(cSharpCodeGenerator.GetCompilationUnitSyntax());
        }

        [Fact]
        public void TestGenerateConstructor()
        {
            CSharpClassGenerator cSharpCodeGenerator = new CSharpClassGenerator("testClass");

            cSharpCodeGenerator.AddConstructor(new[] { (typeof(string), "Test") });

            _testOutputHelper.WriteLine(cSharpCodeGenerator.ToString());

            Assert.NotNull(cSharpCodeGenerator.GetCompilationUnitSyntax());
        }
    }
}
