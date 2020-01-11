using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nyranith.Code.CSharp
{
    public class CSharpBlockGenerator : CSharpCodeGeneratorBase<BlockSyntax>
    {
        public CSharpBlockGenerator()
        {
            Code = SyntaxFactory.Block(); 
        }


        public override CompilationUnitSyntax GetCompilationUnitSyntax()
        {
            return CompilationUnit;
        }

        public CSharpBlockGenerator AddEmptyLine()
        {
            Code = Code.AddStatements(SyntaxFactory.EmptyStatement(SyntaxFactory.MissingToken(SyntaxKind.SemicolonToken)));
            return this; 
        }
    }
}
