using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using StLogger = Microsoft.Build.Logging.StructuredLogger;

namespace AOTSerializer.Generator
{
    // Utility and Extension methods for Roslyn
    public static class RoslynExtensions
    {
        private static (string fname, string args) GetBuildCommandLine(string csprojPath, string tempPath, bool useDotNet)
        {
            string fname = "dotnet";
            const string tasks = "Restore;ResolveReferences";
            // from Buildalyzer implementation
            // https://github.com/daveaglick/Buildalyzer/blob/b42d2e3ba1b3673a8133fb41e72b507b01bce1d6/src/Buildalyzer/Environment/BuildEnvironment.cs#L86-L96
            Dictionary<string, string> properties = new Dictionary<string, string>()
                {
                    {"ProviderCommandLineArgs", "true"},
                    {"GenerateResourceMSBuildArchitecture", "CurrentArchitecture"},
                    {"DesignTimeBuild", "true"},
                    {"BuildProjectReferences","false"},
                    // {"SkipCompilerExecution","true"},
                    {"DisableRarCache", "true"},
                    {"AutoGenerateBindingRedirects", "false"},
                    {"CopyBuildOutputToOutputDirectory", "false"},
                    {"CopyOutputSymbolsToOutputDirectory", "false"},
                    {"SkipCopyBuildProduct", "true"},
                    {"AddModules", "false"},
                    {"UseCommonOutputDirectory", "true"},
                    {"GeneratePackageOnBuild", "false"},
                    {"RunPostBuildEvent", "false"},
                    {"SolutionDir", (new FileInfo(csprojPath).Directory.FullName) + "/"}
                };
            var propargs = string.Join(" ", properties.Select(kv => $"/p:{kv.Key}=\"{kv.Value}\""));
            // how to determine whether command should be executed('dotnet msbuild' or 'msbuild')?
            if (useDotNet)
            {
                fname = "dotnet";
                return (fname, $"msbuild \"{csprojPath}\" /t:{tasks} {propargs} /bl:\"{Path.Combine(tempPath, "build.binlog")}\" /v:n");
            }
            else
            {
                fname = "msbuild";
                return (fname, $"\"{csprojPath}\" /t:{tasks} {propargs} /bl:\"{Path.Combine(tempPath, "build.binlog")}\" /v:n");
            }
        }

        private static async Task<bool> TryExecute(string csprojPath, string tempPath, bool useDotNet)
        {
            // executing build command with output binary log
            var (fname, args) = GetBuildCommandLine(csprojPath, tempPath, useDotNet);
            try
            {
                var buildlogpath = Path.Combine(tempPath, "build.binlog");
                if (File.Exists(buildlogpath))
                {
                    try
                    {
                        File.Delete(buildlogpath);
                    }
                    catch { }
                }
                using (var stdout = new MemoryStream())
                using (var stderr = new MemoryStream())
                {
                    var exitCode = await ProcessUtil.ExecuteProcessAsync(fname, args, stdout, stderr, null).ConfigureAwait(false);
                    if (exitCode != 0)
                    {
                        // write process output to stdout and stderr when error.
                        using (var stdout2 = new MemoryStream(stdout.ToArray()))
                        using (var stderr2 = new MemoryStream(stderr.ToArray()))
                        using (var consoleStdout = Console.OpenStandardOutput())
                        using (var consoleStderr = Console.OpenStandardError())
                        {
                            await stdout2.CopyToAsync(consoleStdout).ConfigureAwait(false);
                            await stderr2.CopyToAsync(consoleStderr).ConfigureAwait(false);
                        }
                    }
                    return File.Exists(buildlogpath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"exception occured(fname={fname}, args={args}):{e}");
                return false;
            }
        }

        private static IEnumerable<StLogger.Error> FindAllErrors(StLogger.Build build)
        {
            var lst = new List<StLogger.Error>();
            build.VisitAllChildren<StLogger.Error>(er => lst.Add(er));
            return lst;
        }

        private static (StLogger.Build, IEnumerable<StLogger.Error>) ProcessBuildLog(string tempPath)
        {
            var reader = new StLogger.BinLogReader();
            var stlogger = new StLogger.StructuredLogger();
            // prevent output temporary file
            StLogger.StructuredLogger.SaveLogToDisk = false;
            // never output, but if not set, throw exception when initializing
            stlogger.Parameters = "tmp.buildlog";
            stlogger.Initialize(reader);
            reader.Replay(Path.Combine(tempPath, "build.binlog"));
            stlogger.Shutdown();
            var buildlog = stlogger.Construction.Build;
            if (buildlog.Succeeded)
            {
                return (buildlog, null);
            }
            else
            {
                var errors = FindAllErrors(buildlog);
                return (null, errors);
            }
        }

        private static async Task<(StLogger.Build, IEnumerable<StLogger.Error>)> TryGetBuildResultAsync(string csprojPath, string tempPath, bool useDotNet, params string[] preprocessorSymbols)
        {
            try
            {
                if (!await TryExecute(csprojPath, tempPath, useDotNet).ConfigureAwait(false))
                {
                    return (null, Array.Empty<StLogger.Error>());
                }
                else
                {
                    return ProcessBuildLog(tempPath);
                }
            }
            finally
            {
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
        }

        private static async Task<StLogger.Build> GetBuildResult(string csprojPath, params string[] preprocessorSymbols)
        {
            var tempPath = Path.Combine(new FileInfo(csprojPath).Directory.FullName, "__buildtemp");
            try
            {
                (StLogger.Build build, IEnumerable<StLogger.Error> errors) = await TryGetBuildResultAsync(csprojPath, tempPath, true, preprocessorSymbols).ConfigureAwait(false);
                if (build == null)
                {
                    Console.WriteLine("execute `dotnet msbuild` failed, retry with `msbuild`");
                    var dotnetException = new InvalidOperationException($"failed to build project with dotnet:{string.Join("\n", errors)}");
                    (build, errors) = await TryGetBuildResultAsync(csprojPath, tempPath, false, preprocessorSymbols).ConfigureAwait(false);
                    if (build == null)
                    {
                        throw new InvalidOperationException($"failed to build project: {string.Join("\n", errors)}");
                    }
                }
                return build;
            }
            finally
            {
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
            }
        }

        private static Workspace GetWorkspaceFromBuild(this StLogger.Build build, params string[] preprocessorSymbols)
        {
            var csproj = build.Children.OfType<StLogger.Project>().FirstOrDefault();
            if (csproj == null)
            {
                throw new InvalidOperationException("cannot find cs project build");
            }
            StLogger.Item[] compileItems = Array.Empty<StLogger.Item>();
            var properties = new Dictionary<string, StLogger.Property>();
            foreach (var folder in csproj.Children.OfType<StLogger.Folder>())
            {
                if (folder.Name == "Items")
                {
                    var compileFolder = folder.Children.OfType<StLogger.Folder>().FirstOrDefault(x => x.Name == "Compile");
                    if (compileFolder == null)
                    {
                        throw new InvalidOperationException("failed to get compililation documents");
                    }
                    compileItems = compileFolder.Children.OfType<StLogger.Item>().ToArray();
                }
                else if (folder.Name == "Properties")
                {
                    properties = folder.Children.OfType<StLogger.Property>().ToDictionary(x => x.Name);
                }
            }
            var assemblies = Array.Empty<StLogger.Item>();
            foreach (var target in csproj.Children.OfType<StLogger.Target>())
            {
                if (target.Name == "ResolveReferences")
                {
                    var folder = target.Children.OfType<StLogger.Folder>().Where(x => x.Name == "TargetOutputs").FirstOrDefault();
                    if (folder == null)
                    {
                        throw new InvalidOperationException("cannot find result of resolving assembly");
                    }
                    assemblies = folder.Children.OfType<StLogger.Item>().ToArray();
                }
            }
            var ws = new AdhocWorkspace();
            var roslynProject = ws.AddProject(Path.GetFileNameWithoutExtension(csproj.ProjectFile), Microsoft.CodeAnalysis.LanguageNames.CSharp);
            var projectDir = properties["ProjectDir"].Value;
            var pguid = properties.ContainsKey("ProjectGuid") ? Guid.Parse(properties["ProjectGuid"].Value) : Guid.NewGuid();
            var projectGuid = ProjectId.CreateFromSerialized(pguid);
            foreach (var compile in compileItems)
            {
                var filePath = compile.Text;
                var absFilePath = Path.Combine(projectDir, filePath);
                roslynProject = roslynProject.AddDocument(filePath, File.ReadAllText(absFilePath)).Project;
            }
            foreach (var asm in assemblies)
            {
                roslynProject = roslynProject.AddMetadataReference(MetadataReference.CreateFromFile(asm.Text));
            }
            var compopt = roslynProject.CompilationOptions as CSharpCompilationOptions;
            compopt = roslynProject.CompilationOptions as CSharpCompilationOptions;
            compopt = compopt ?? new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            OutputKind kind;
            switch (properties["OutputType"].Value)
            {
                case "Exe":
                    kind = OutputKind.ConsoleApplication;
                    break;
                case "Library":
                    kind = OutputKind.DynamicallyLinkedLibrary;
                    break;
                default:
                    kind = OutputKind.DynamicallyLinkedLibrary;
                    break;
            }
            roslynProject = roslynProject.WithCompilationOptions(compopt.WithOutputKind(kind).WithAllowUnsafe(true));
            var parseopt = roslynProject.ParseOptions as CSharpParseOptions;
            roslynProject = roslynProject.WithParseOptions(parseopt.WithPreprocessorSymbols(preprocessorSymbols));
            if (!ws.TryApplyChanges(roslynProject.Solution))
            {
                throw new InvalidOperationException("failed to apply solution changes to workspace");
            }
            return ws;
        }

        private static void WorkSpaceFailed(object sender, WorkspaceDiagnosticEventArgs e)
        {
            Console.WriteLine(e);
        }

        public static Compilation GetCompilationFromFiles(
            IEnumerable<string> inputFiles,
            IEnumerable<string> inputDirectories,
            IEnumerable<string> additionalReferenceDirectories = null,
            params string[] preprocessorSymbols)
        {
            var parseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Parse, SourceCodeKind.Regular, preprocessorSymbols ?? new string[0]);
            var references = new List<MetadataReference>();
            var syntaxTrees = new List<SyntaxTree>();

            var referenceDlls = Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location), "*.dll");
            foreach (var dllFile in referenceDlls)
            {
                references.Add(MetadataReference.CreateFromFile(dllFile));
            }

            foreach (var dir in inputDirectories ?? new string[0])
            {
                var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);
                foreach (var dll in dlls)
                {
                    references.Add(MetadataReference.CreateFromFile(dll));
                }

                var files = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var text = File.ReadAllText(file);
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                }
            }

            foreach (var path in inputFiles ?? new string[0])
            {
                if (path.EndsWith(".dll", StringComparison.Ordinal))
                {
                    references.Add(MetadataReference.CreateFromFile(path));
                }

                if (path.EndsWith(".cs", StringComparison.Ordinal))
                {
                    var text = File.ReadAllText(path);
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                }
            }

            if (additionalReferenceDirectories != null)
            {
                var interimSyntaxTrees = new List<SyntaxTree>(syntaxTrees);

                foreach (var dir in additionalReferenceDirectories)
                {
                    var files = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        var text = File.ReadAllText(file);
                        interimSyntaxTrees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                    }

                    var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);
                    foreach (var dll in dlls)
                    {
                        references.Add(MetadataReference.CreateFromFile(dll));
                    }
                }

                CSharpCompilation intermediateCompilation = CSharpCompilation.Create(
                    "Assembly-CSharp",
                    syntaxTrees: interimSyntaxTrees,
                    references: references
                );

                references.Add(intermediateCompilation.ToMetadataReference());
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                "Assembly-CSharp",
                syntaxTrees: syntaxTrees,
                references: references
            );
            return compilation;
        }

        public static async Task<Compilation> GetCompilationFromProject(
            string csprojPath,
            IEnumerable<string> filesToInclude = null,
            IEnumerable<string> dirsToInclude = null,
            params string[] preprocessorSymbols)
        {
            var build = await GetBuildResult(csprojPath, preprocessorSymbols).ConfigureAwait(false);

            Compilation projectCompilation;

            using (var workspace = GetWorkspaceFromBuild(build, preprocessorSymbols))
            {
                workspace.WorkspaceFailed += WorkSpaceFailed;
                var project = workspace.CurrentSolution.Projects.First();
                project = project
                    .WithParseOptions((project.ParseOptions as CSharpParseOptions).WithPreprocessorSymbols(preprocessorSymbols))
                    .WithCompilationOptions((project.CompilationOptions as CSharpCompilationOptions).WithAllowUnsafe(true));

                projectCompilation = await project.GetCompilationAsync().ConfigureAwait(false);
            }

            if (filesToInclude == null && dirsToInclude == null)
            {
                return projectCompilation;
            }

            var references = new List<MetadataReference> { projectCompilation.ToMetadataReference() };
            var parseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Parse, SourceCodeKind.Regular, preprocessorSymbols);
            var syntaxTrees = new List<SyntaxTree>();

            var referenceDlls = Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location), "*.dll");
            foreach (var dllFile in referenceDlls)
            {
                references.Add(MetadataReference.CreateFromFile(dllFile));
            }

            foreach (var dir in dirsToInclude ?? new string[0])
            {
                var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);
                foreach (var dll in dlls)
                {
                    references.Add(MetadataReference.CreateFromFile(dll));
                }

                var files = Directory.GetFiles(dir, "*.cs", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var text = File.ReadAllText(file);
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                }
            }

            foreach (var path in filesToInclude ?? new string[0])
            {
                if (path.EndsWith(".dll", StringComparison.Ordinal))
                {
                    references.Add(MetadataReference.CreateFromFile(path));
                }

                if (path.EndsWith(".cs", StringComparison.Ordinal))
                {
                    var text = File.ReadAllText(path);
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                }
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                "In-Memory",
                syntaxTrees: syntaxTrees,
                references: references
            );
            return compilation;
        }

        public static IEnumerable<INamedTypeSymbol> GetNamedTypeSymbols(this Compilation compilation)
        {
            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                var semModel = compilation.GetSemanticModel(syntaxTree);

                foreach (var item in syntaxTree.GetRoot()
                    .DescendantNodes()
                    .Select(x => semModel.GetDeclaredSymbol(x))
                    .Where(x => x != null))
                {
                    var namedType = item as INamedTypeSymbol;
                    if (namedType != null)
                    {
                        yield return namedType;
                    }
                }
            }
        }

        public static IEnumerable<INamedTypeSymbol> EnumerateBaseType(this ITypeSymbol symbol)
        {
            var t = symbol.BaseType;
            while (t != null)
            {
                yield return t;
                t = t.BaseType;
            }
        }

        public static AttributeData FindAttribute(this IEnumerable<AttributeData> attributeDataList, string typeName)
        {
            return attributeDataList
                .Where(x => x.AttributeClass.ToDisplayString() == typeName)
                .FirstOrDefault();
        }

        public static AttributeData FindAttributeShortName(this IEnumerable<AttributeData> attributeDataList,
            string typeName)
        {
            return attributeDataList
                .Where(x => x.AttributeClass.Name == typeName)
                .FirstOrDefault();
        }

        public static AttributeData FindAttributeIncludeBasePropertyShortName(this IPropertySymbol property,
            string typeName)
        {
            do
            {
                var data = FindAttributeShortName(property.GetAttributes(), typeName);
                if (data != null) return data;
                property = property.OverriddenProperty;
            } while (property != null);

            return null;
        }

        public static AttributeSyntax FindAttribute(this BaseTypeDeclarationSyntax typeDeclaration, SemanticModel model,
            string typeName)
        {
            return typeDeclaration.AttributeLists
                .SelectMany(x => x.Attributes)
                .Where(x => model.GetTypeInfo(x).Type?.ToDisplayString() == typeName)
                .FirstOrDefault();
        }

        public static INamedTypeSymbol FindBaseTargetType(this ITypeSymbol symbol, string typeName)
        {
            return symbol.EnumerateBaseType()
                .Where(x => x.OriginalDefinition?.ToDisplayString() == typeName)
                .FirstOrDefault();
        }

        public static object GetSingleNamedArgumentValue(this AttributeData attribute, string key)
        {
            foreach (var item in attribute.NamedArguments)
            {
                if (item.Key == key)
                {
                    return item.Value.Value;
                }
            }

            return null;
        }

        public static string GetSingleNamedArgumentValueFromSyntaxTree(this AttributeData attribute, string key)
        {
            if (attribute == null) return null;

            var ctxxx = attribute.ApplicationSyntaxReference.GetSyntax();
            foreach (var p in ctxxx.DescendantNodes().OfType<AttributeArgumentSyntax>())
            {
                if (p.NameEquals.Name.Identifier.ValueText == key)
                {
                    if (p.Expression is LiteralExpressionSyntax) return (p.Expression as LiteralExpressionSyntax).Token.ValueText;
                    if (p.Expression is MemberAccessExpressionSyntax) return (p.Expression as MemberAccessExpressionSyntax).Name.ToString();
                }
            }

            return null;
        }

        public static bool IsNullable(this INamedTypeSymbol symbol)
        {
            if (symbol.IsGenericType)
            {
                if (symbol.ConstructUnboundGenericType().ToDisplayString() == "T?")
                {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol symbol)
        {
            var t = symbol;
            while (t != null)
            {
                foreach (var item in t.GetMembers())
                {
                    yield return item;
                }
                t = t.BaseType;
            }
        }

        public static IEnumerable<ISymbol> GetAllInterfaceMembers(this ITypeSymbol symbol)
        {
            return symbol.GetMembers()
                .Concat(symbol.AllInterfaces.SelectMany(x => x.GetMembers()));
        }
    }
}