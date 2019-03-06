using Buildalyzer;
using Buildalyzer.Workspaces;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AOTSerializer.Generator
{
    // Utility and Extension methods for Roslyn
    public static class RoslynExtensions
    {
        public static async Task<(Compilation TargetCompilation, Compilation[] ReferenceCompilations)> GetCompilation(
            string solutionPath,
            IEnumerable<string> filesToIncludeInGeneration = null,
            IEnumerable<string> dirsToIncludeInGeneration = null,
            IEnumerable<string> additionalDllReferenceDirectories = null,
            Action<string, int, int> progressCallback = null)
        {
            var analyzer = new AnalyzerManager(solutionPath);
            var compilations = new List<Compilation>();
            var references = new List<MetadataReference>();

            var projectCount = analyzer.Projects.Count;
            int projectIndex = 0;
            var taskList = new List<Task>();

            foreach (var project in analyzer.Projects.Values)
            {
                using (var workspace = project.GetWorkspace(true))
                {
                    var i = ++projectIndex;
                    workspace.WorkspaceFailed += (sender, args) => Console.WriteLine(args.Diagnostic.Message);
                    taskList.Add(workspace.CurrentSolution.Projects.First().GetCompilationAsync()
                        .ContinueWith(t =>
                        {
                            compilations.Add(t.Result);
                            references.Add(t.Result.ToMetadataReference());
                            progressCallback?.Invoke(project.ProjectFile.Path, i, projectCount);
                        }));
                }
            }

            await Task.WhenAll(taskList);

            var parseOptions = new CSharpParseOptions(LanguageVersion.Default, DocumentationMode.Parse, SourceCodeKind.Regular);
            var syntaxTrees = new List<SyntaxTree>();

            var referenceDlls = Directory.GetFiles(Path.GetDirectoryName(typeof(object).Assembly.Location), "*.dll");
            foreach (var dllFile in referenceDlls)
            {
                references.Add(MetadataReference.CreateFromFile(dllFile));
            }

            foreach (var dir in dirsToIncludeInGeneration ?? new string[0])
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

            foreach (var path in filesToIncludeInGeneration ?? new string[0])
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

            foreach (var dir in additionalDllReferenceDirectories ?? new string[0])
            {
                var dlls = Directory.GetFiles(dir, "*.dll", SearchOption.AllDirectories);
                foreach (var dll in dlls)
                {
                    references.Add(MetadataReference.CreateFromFile(dll));
                }
            }

            if (syntaxTrees.Count == 0)
            {
                var files = Directory.GetFiles(Path.GetDirectoryName(solutionPath), "*.cs", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var text = File.ReadAllText(file);
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(text, parseOptions));
                }
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                "In-Memory",
                syntaxTrees: syntaxTrees,
                references: references
            );
            return (compilation, compilations.ToArray());
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

        public static ITypeSymbol GetTypeSymbolForType(Type type, Compilation compilation)
        {
            if (type.IsArray
                && compilation.GetTypeByMetadataName(type.FullName) == null
                && compilation.GetTypeByMetadataName(type.GetElementType().FullName) is var arrayElementType
                && arrayElementType != null)
            {
                var allSymbols = compilation.GetSymbolsWithName((_) => true);
                foreach (var symbol in allSymbols)
                {
                    switch (symbol.Kind)
                    {
                        case SymbolKind.ArrayType:
                            var currentArrayType = (IArrayTypeSymbol)symbol;
                            if (currentArrayType.ElementType == arrayElementType
                                && currentArrayType.Rank == type.GetArrayRank())
                            {
                                return currentArrayType;
                            }
                            break;
                        case SymbolKind.Field:
                            var fieldType = ((IFieldSymbol)symbol).Type;
                            if (fieldType.Kind == SymbolKind.ArrayType
                                && ((IArrayTypeSymbol)fieldType).ElementType == arrayElementType
                                && ((IArrayTypeSymbol)fieldType).Rank == type.GetArrayRank())
                            {
                                return fieldType;
                            }
                            break;
                        case SymbolKind.Property:
                            var propertyType = ((IPropertySymbol)symbol).Type;
                            if (propertyType.Kind == SymbolKind.ArrayType
                                && ((IArrayTypeSymbol)propertyType).ElementType == arrayElementType
                                && ((IArrayTypeSymbol)propertyType).Rank == type.GetArrayRank())
                            {
                                return propertyType;
                            }
                            break;
                        case SymbolKind.Method:
                            var methodSymbol = (IMethodSymbol)symbol;
                            if (methodSymbol.ReturnType.Kind == SymbolKind.ArrayType
                                && ((IArrayTypeSymbol)methodSymbol.ReturnType).ElementType == arrayElementType
                                && ((IArrayTypeSymbol)methodSymbol.ReturnType).Rank == type.GetArrayRank())
                            {
                                return methodSymbol.ReturnType;
                            }
                            else if (methodSymbol.Parameters.Where(p => p.Type.Kind == SymbolKind.ArrayType
                                          && ((IArrayTypeSymbol)p.Type).ElementType == arrayElementType
                                          && ((IArrayTypeSymbol)p.Type).Rank == type.GetArrayRank()).Any())
                            {
                                return methodSymbol.Parameters.First(p => p.Type.Kind == SymbolKind.ArrayType
                                          && ((IArrayTypeSymbol)p.Type).ElementType == arrayElementType
                                          && ((IArrayTypeSymbol)p.Type).Rank == type.GetArrayRank()).Type;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!type.IsConstructedGenericType)
            {
                return compilation.GetTypeByMetadataName(type.FullName);
            }

            // get all typeInfo's for the Type arguments 
            var typeArgumentsTypeInfos = type.GenericTypeArguments.Select(a => GetTypeSymbolForType(a, compilation)).ToArray();

            var openType = type.GetGenericTypeDefinition();
            var typeSymbol = compilation.GetTypeByMetadataName(openType.FullName);

            return typeSymbol != null && typeArgumentsTypeInfos.All(t => t != null)
                ? typeSymbol.Construct(typeArgumentsTypeInfos)
                : null;
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