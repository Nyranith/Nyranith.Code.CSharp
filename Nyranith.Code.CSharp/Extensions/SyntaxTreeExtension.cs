using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nyranith.Code.CSharp.Extensions
{
    public static class SyntaxTreeExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static SemanticModel GetSemanticModel(this SyntaxTree tree, string name)
        {
            return GetCSharpCompilation(tree, name).GetSemanticModel(tree);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static CSharpCompilation GetCSharpCompilation(this SyntaxTree tree, string name)
        {
            return CSharpCompilation.Create(name, syntaxTrees: new[] { tree });
        }

    }
}
