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
            SyntaxTree tree = CSharpSyntaxTree.ParseText(@"
using System;

namespace Demo02
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

            var @namespace = root.Members.First() as NamespaceDeclarationSyntax;
            var @class = @namespace.Members.First() as ClassDeclarationSyntax;
            var method = @class.Members.First() as MethodDeclarationSyntax;
            var expressionStatement = method.Body.Statements.First() as ExpressionStatementSyntax;
            var invocationExpression = expressionStatement.Expression as InvocationExpressionSyntax;
            var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax;

            var compilation = CSharpCompilation
                .Create("Demo02")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location))
                .AddSyntaxTrees(tree);

            var diagnostics = compilation.GetDiagnostics();

            var model = compilation.GetSemanticModel(tree);

            var methodSymbol = model.GetSymbolInfo(invocationExpression).Symbol;

            var containingType = methodSymbol.ContainingType;
            Console.WriteLine(containingType.ContainingAssembly);
            Console.WriteLine();

            var displayParts = methodSymbol.ToDisplayParts();
            foreach (var part in displayParts)
            {
                Console.WriteLine(string.Format("{0,-13} = {1}", part.Kind, part));
            }
        }
    }
}
