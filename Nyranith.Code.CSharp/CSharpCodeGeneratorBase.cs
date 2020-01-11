using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nyranith.Code.CSharp
{
    public abstract class CSharpCodeGeneratorBase<T>  
        where T : CSharpSyntaxNode
    {
        public T Code { get; protected set; }

        public CompilationUnitSyntax CompilationUnit { get; protected set; } = SyntaxFactory.CompilationUnit();

        public CSharpCodeGeneratorBase()
        {

        }

        public CSharpCodeGeneratorBase(T type)
        {
            Code = type; 
        }

        /// <summary>
        /// Gets the stupid compilation unit.
        /// </summary>
        /// <returns></returns>
        public abstract CompilationUnitSyntax GetCompilationUnitSyntax();


        /// <summary>
        /// Create an argument from type and nam
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ArgumentSyntax CreateArgument(Type type, string name)
        {
            return SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(name)));
        }

        /// <summary>
        /// Create an argument list syntax from the nodes. 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected ArgumentListSyntax CreateArgumentListSyntax(IEnumerable<ArgumentSyntax> nodes)
        {
            return SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(nodes));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ParameterSyntax CreateParamter(string name)
        {
            return SyntaxFactory.Parameter(SyntaxFactory.Identifier(name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterSyntaxes"></param>
        /// <returns></returns>
        protected ParameterListSyntax CreateParamterListSyntax(IEnumerable<ParameterSyntax> parameterSyntaxes)
        {
            return SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameterSyntaxes));
        }

        /// <summary>
        /// Create a parameter from type and string
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected ParameterSyntax CreateParamter(Type type, string name)
        {
            return SyntaxFactory.Parameter(
                SyntaxFactory.List<AttributeListSyntax>(),
                SyntaxFactory.TokenList(),
                SyntaxFactory.ParseTypeName(type.Name),
                SyntaxFactory.Identifier(name),
                null);
        }

        /// <summary>
        /// Create a modifier. 
        /// </summary>
        /// <param name="accessModifer"></param>
        /// <returns></returns>
        protected SyntaxTokenList CreateModifier(params Modifier?[] accessModifer)
        {
            return SyntaxFactory.TokenList(accessModifer?.Select(modifier => CreateSyntaxToken(modifier)));
        }

        protected SyntaxToken CreateSyntaxToken(Modifier? modifier)
        {
            switch (modifier)
            {
                case Modifier.Public:
                    return SyntaxFactory.Token(SyntaxKind.PublicKeyword);
                case Modifier.Protected:
                    return SyntaxFactory.Token(SyntaxKind.ProtectedKeyword);
                case Modifier.Private:
                    return SyntaxFactory.Token(SyntaxKind.PrivateKeyword);
                case Modifier.Internal:
                    return SyntaxFactory.Token(SyntaxKind.InternalKeyword);
            }
            return SyntaxFactory.Token(SyntaxKind.None);
        }

        protected TypeSyntax CreateTypeSyntax(Type type)
        {
            return SyntaxFactory.ParseTypeName(type.Name);
        }

        protected TypeSyntax CreateTypeSyntax<TType>(TType obt)
        {
            return CreateTypeSyntax(typeof(TType).Name);
        }

        /// <summary>
        /// Create a property from string and string type name. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected PropertyDeclarationSyntax CreateProperty(string name, string type)
        {
            return CreateProperty(name, SyntaxFactory.ParseTypeName(type));
        }

        protected PropertyDeclarationSyntax CreateProperty(string name, TypeSyntax type, Modifier? accessModifer = Modifier.Public, Func<ArrowExpressionClauseSyntax> arrowDeclaration = null)
        {
            return SyntaxFactory.PropertyDeclaration(
                        type,
                        name
                    )
                    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                    .AddAccessorListAccessors(
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                        SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                    );
        }

        protected FieldDeclarationSyntax CreateVariable<TType>(string name, Modifier? accessModifer = Modifier.Public)
        {
            VariableDeclarationSyntax variable = SyntaxFactory.VariableDeclaration(CreateTypeSyntax(typeof(TType)), variables:
                SyntaxFactory.SeparatedList(new List<VariableDeclaratorSyntax>()
                { SyntaxFactory.VariableDeclarator(
                    identifier: SyntaxFactory.Identifier(name))
                }));

            return SyntaxFactory.FieldDeclaration(variable)
               .AddModifiers(CreateSyntaxToken(accessModifer));
        }



        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                GetCompilationUnitSyntax().NormalizeWhitespace().WriteTo(writer);
                return writer.ToString();
            }
        }

    }
}
