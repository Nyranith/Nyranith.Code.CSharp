using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nyranith.Code.CSharp.Visitor
{
    /// <summary>
    /// https://stackoverflow.com/questions/31143056/roslyn-how-to-get-all-classes
    /// </summary>
    public class VirtualizationVisitor : CSharpSyntaxRewriter
    {
        public IList<ClassDeclarationSyntax> Classes { get; set; } = new List<ClassDeclarationSyntax>();

        public IList<EnumDeclarationSyntax> Enums { get; set; } = new List<EnumDeclarationSyntax>();

        public IList<StructDeclarationSyntax> Structs { get; set; } = new List<StructDeclarationSyntax>();

        public VirtualizationVisitor()
        {
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            node = (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
            Classes.Add(node);
            return node;
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            node = (EnumDeclarationSyntax)base.VisitEnumDeclaration(node);
            Enums.Add(node);
            return node;
        }

        public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node)
        {
            node = (StructDeclarationSyntax)base.VisitStructDeclaration(node);
            Structs.Add(node);
            return node;
        }
    }
}
