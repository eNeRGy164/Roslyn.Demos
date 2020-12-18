using Buildalyzer;
using Buildalyzer.Workspaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo03
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var analyzerManager = new AnalyzerManager();
            var projectAnalyzer = analyzerManager.GetProject(@"C:\GitHub\LD\Demo01\Demo01.csproj");

            var analyzerResults = projectAnalyzer.Build();

            var sourceFiles = analyzerResults.First().SourceFiles;

            var workspace = projectAnalyzer.GetWorkspace();

            var project = workspace.CurrentSolution.Projects.First();
            var compilation = await project.GetCompilationAsync();

            var diagnostics = compilation.GetDiagnostics();

            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var semanticModel = compilation.GetSemanticModel(syntaxTree, true);

                
            }
        }
    }
}
