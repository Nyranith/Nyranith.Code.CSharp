
### Simplified code generation using roslyn

[![Build status](https://kaalsaas.visualstudio.com/kaalsaas/_apis/build/status/Nyranith.Code.CSharp-CI)](https://kaalsaas.visualstudio.com/kaalsaas/_build/latest?definitionId=18)

This package is a simplification of creating classes in roslyn. 

### 
Create a class 

```
var class = new CSharpClassGenerator("testClass").ToString(); 
```

Will just give you: 
```
public class testClass
{
}
```

Doing the same in roslyn you have to : 

```
var compilationUnit = SyntaxFactory.CompilationUnit();

ClassDeclarationSyntax code = SyntaxFactory.ClassDeclaration("testClass")
    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));

compilationUnit = compilationUnit.AddMembers(code);

string value = "";
using (var writer = new StringWriter())
{
    compilationUnit.NormalizeWhitespace().WriteTo(writer);
    value = writer.ToString(); 
}
```
This is a little unfair to roslyn since i wrapped it in method. To simplify this i could do something like this: 

```
public static CompilationUnitSyntax compilationUnit = SyntaxFactory.CompilationUnit();

public static string GetAsString<T>(T classSyntaxNode)
    where T : CSharpSyntaxNode
{
    using (var writer = new StringWriter())
    {
        compilationUnit.NormalizeWhitespace().WriteTo(writer);
        return writer.ToString();
    }
}
```
And then just the class generation: 
``` 
var compilationUnit = SyntaxFactory.CompilationUnit();

ClassDeclarationSyntax code = SyntaxFactory.ClassDeclaration("testClass")
    .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)));

compilationUnit = compilationUnit.AddMembers(code);

var value = GetAsString(compilationUnit); 
```
