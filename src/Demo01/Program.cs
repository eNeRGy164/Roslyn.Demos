using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Demo01
{
    class Program
    {
        static void Main(string[] args)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(@"using System;

namespace Demo01
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}");

            var root = (CompilationUnitSyntax)tree.GetRoot();

            var @using = root.Usings.First();
            var @namespace = root.Members.First() as NamespaceDeclarationSyntax;
            var @class = @namespace.Members.First() as ClassDeclarationSyntax;
            var method = @class.Members.First() as MethodDeclarationSyntax;
            var expressionStatement = method.Body.Statements.First() as ExpressionStatementSyntax;
            var invocationExpression = expressionStatement.Expression as InvocationExpressionSyntax;
            var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax;

            var sourceText = tree.GetText();
            Console.WriteLine(sourceText.GetSubText(invocationExpression.Span));

            Console.WriteLine(sourceText.GetSubText(memberAccessExpression.Expression.Span));
            Console.WriteLine(sourceText.GetSubText(memberAccessExpression.Name.Span));

            var argument = invocationExpression.ArgumentList.Arguments.First();
            Console.WriteLine(sourceText.GetSubText(argument.Span));
        }
    }
}
