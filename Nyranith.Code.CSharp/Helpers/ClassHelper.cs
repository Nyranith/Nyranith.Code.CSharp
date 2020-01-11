using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nyranith.Code.CSharp.Helpers
{
    public static class ClassHelper
    {

        /// <summary>
        /// Based off : https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="syntaxNode"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetParentSyntax<T>(SyntaxNode syntaxNode, out T result)
           where T : SyntaxNode
        {
            // set defaults
            result = null;

            if (syntaxNode == null)
            {
                return false;
            }

            try
            {
                syntaxNode = syntaxNode.Parent;

                if (syntaxNode == null)
                {
                    return false;
                }

                if (syntaxNode.GetType() == typeof(T))
                {
                    result = syntaxNode as T;
                    return true;
                }

                return TryGetParentSyntax<T>(syntaxNode, out result);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/20458457/getting-class-fullname-including-namespace-from-roslyn-classdeclarationsyntax
        /// </summary>
        /// <param name="classDeclarationSyntax"></param>
        /// <returns></returns>
        public static NamespaceDeclarationSyntax GetNamespace(this ClassDeclarationSyntax classDeclarationSyntax)
        {
            NamespaceDeclarationSyntax namespaceDeclarationSyntax = null;

            if (TryGetParentSyntax(classDeclarationSyntax, out namespaceDeclarationSyntax))
            {
                return namespaceDeclarationSyntax;
            }

            return null; 
        }

    }
}
