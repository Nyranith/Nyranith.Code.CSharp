using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nyranith.Code.CSharp
{

    public partial class CSharpClassGenerator : CSharpCodeGeneratorBase<ClassDeclarationSyntax>
    {
        private NamespaceDeclarationSyntax _namespace = null;

        public CSharpClassGenerator(string className) : this(className, null, null, null)
        {

        }

        public CSharpClassGenerator(string className, Modifier? accessModifer = Modifier.Public) : this(className, null, null, accessModifer)
        {

        }

        public CSharpClassGenerator(string className, Action<CSharpClassGenerator> action = null, Modifier? accessModifer = Modifier.Public) : this(className, null, action, accessModifer) 
        {
            
        }

        public CSharpClassGenerator(string className, string @namespace = null, Action<CSharpClassGenerator> action = null, Modifier? accessModifer = Modifier.Public)
        {
            if (string.IsNullOrEmpty(className))
            {
                throw new ArgumentNullException("The classname cannot be null"); 
            }

            if(!string.IsNullOrEmpty(@namespace))
            {
                _namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(@namespace));
            }

            Code = SyntaxFactory.ClassDeclaration(className)
                .WithModifiers(CreateModifier(accessModifer));

            action?.Invoke(this); 
        }

        public override CompilationUnitSyntax GetCompilationUnitSyntax()
        {
            if (_namespace != null)
            {
                _namespace = _namespace.AddMembers(Code);
                CompilationUnit = CompilationUnit.AddMembers(_namespace);
            }
            else
            {
                CompilationUnit = CompilationUnit.AddMembers(Code);
            }

            return CompilationUnit;
        }


        public CSharpClassGenerator AddUsing(SyntaxList<UsingDirectiveSyntax> usings)
        {
            if(usings == null || usings.Count == 0)
            {
                throw new ArgumentNullException("No usings found."); 
            }

            CompilationUnit = CompilationUnit.WithUsings(usings);

            return this; 
        }
        
        public CSharpClassGenerator AddConstructor((Type type, string variableName)[] constructorVariables = null, Action<CSharpBlockGenerator> body = null, Modifier? accessModifer = Modifier.Public, ConstructorCallingType? constructorCallingType = null, string[] baseArguments = null)
        {
            var constructor = SyntaxFactory.ConstructorDeclaration(Code.Identifier.ToString());

            constructor = constructor.WithModifiers(CreateModifier(accessModifer));

            if (constructorCallingType != null)
            {
                constructor = constructor.WithInitializer(constructorCallingType != null ? SyntaxFactory.ConstructorInitializer(constructorCallingType == ConstructorCallingType.Base ?
                    SyntaxKind.BaseConstructorInitializer : SyntaxKind.ThisConstructorInitializer) : null);

                if(baseArguments != null && baseArguments.Count() > 0)
                {
                    constructor = constructor.WithParameterList(constructor
                        .ParameterList
                        .WithParameters(SyntaxFactory
                        .SeparatedList(baseArguments
                            .Select(argument => CreateParamter(argument)))));
                }
            }
            var cSharpBlockGenerator = new CSharpBlockGenerator();

            if (body != null)
            {
                body(cSharpBlockGenerator);
            }
            else
            {
                cSharpBlockGenerator.AddEmptyLine();
            }
            constructor = constructor.WithBody(cSharpBlockGenerator.Code);

            if (constructorVariables != null && constructorVariables.Count() > 0)
            {
                constructor = constructor.WithParameterList(CreateParamterListSyntax(constructorVariables.Select(argument => CreateParamter(argument.type, argument.variableName))));
            }

            Code = Code.AddMembers(constructor);

            return this; 
        }

        public CSharpClassGenerator AddMethod<T>(string methodName, (Type type, string variableName)[] variables = null, Action<CSharpBlockGenerator> body = null, Modifier? accessModifer = Modifier.Public)
        {
            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(typeof(T).Name), methodName)
                .WithModifiers(CreateModifier(accessModifer));

            if (variables != null && variables.Count() > 0)
            {

                method = method
                    .WithParameterList(CreateParamterListSyntax(variables
                        .Select(argument => CreateParamter(argument.type, argument.variableName))));
            }
            var cSharpBlockGenerator = new CSharpBlockGenerator();

            if (body != null)
            {
                body(cSharpBlockGenerator);

            }
            else
            {
                cSharpBlockGenerator.AddEmptyLine();
            }
            method = method.WithBody(cSharpBlockGenerator.Code); 

            Code = Code.AddMembers(method);

            return this; 
        }

        public CSharpClassGenerator AddVariable<TType>(string name, Modifier? accessModifer = Modifier.Public)
        {
            var variable = CreateVariable<TType>(name, accessModifer);

            Code = Code.AddMembers(variable);

            return this;
        }

        public CSharpClassGenerator AddProperty(string propertyName, Type type, Modifier accessModifer = Modifier.Public)
        {
            return AddProperty(propertyName, SyntaxFactory.ParseTypeName(type.Name), accessModifer);
        }
        public CSharpClassGenerator AddProperty(string propertyName, Modifier accessModifer = Modifier.Public)
        {
            return AddProperty(propertyName, SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.ObjectKeyword)), accessModifer);
        }
        public CSharpClassGenerator AddProperty(string propertyName, TypeSyntax dataType, Modifier accessModifer = Modifier.Public)
        {
            var propertyDeclarationSyntax = CreateProperty(propertyName, dataType)
                .WithModifiers(CreateModifier(accessModifer));

            Code = Code.AddMembers(propertyDeclarationSyntax);

            return this; 
        }
    }
}
