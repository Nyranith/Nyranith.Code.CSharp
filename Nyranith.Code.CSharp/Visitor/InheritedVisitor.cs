using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq; 
using System.Collections.Generic;
using System.Text;

namespace Nyranith.Code.CSharp.Visitor
{
    /// <summary>
    /// https://stackoverflow.com/questions/28234052/finding-all-class-declarations-than-inherit-from-another-with-roslyn
    /// https://johnkoerner.com/csharp/creating-a-stand-alone-code-analyzer/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InheritedVisitor : CSharpSyntaxRewriter
    {
        public IList<ClassDeclarationSyntax> Classes { get; set; } = new List<ClassDeclarationSyntax>();

        private readonly SemanticModel _model;

        private readonly string[] _fullNames; 



        public InheritedVisitor(SemanticModel model, string fullName)
        {
            _model = model;
            _fullNames = new string[] { fullName }; 
        }

        public InheritedVisitor(SemanticModel model, params string[] fulleNames)
        {
            _model = model;
            _fullNames = fulleNames;
        }


        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);

            if (InheritsFrom(_model.GetDeclaredSymbol(node), _fullNames))
            {
                Classes.Add(node); 
            }

            return node;  
        }

        private bool InheritsFrom(INamedTypeSymbol symbol, string[] fullNames)
        {
            while (true)
            {
                if (fullNames.Where(fullName => symbol.ToString() == fullName).FirstOrDefault() != null)
                {
                    return true;
                }
                if (symbol.BaseType != null)
                {
                    symbol = symbol.BaseType;
                    continue;
                }
                break;
            }
            return false;
        }
    }


    public class InheritedVisitor<T> : InheritedVisitor
    {
        public InheritedVisitor(SemanticModel model) : base(model, typeof(T).FullName)
        {
        }
    }
}
