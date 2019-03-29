using AOTSerializer.Generator.Console.Model;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Reflection;
using System.Text;

namespace AOTSerializer.Generator.Console
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string data;

            using (var sharedMemory = MemoryMappedFile.OpenExisting("JsonGeneratorSharedMemory"))
            {
                using (var stream = sharedMemory.CreateViewStream())
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8))
                    {
                        data = reader.ReadString();
                    }
                }
            }

            var generatorParams = JsonConvert.DeserializeObject<GeneratorParams>(data);

            var compilation = RoslynExtensions.GetCompilation(
                generatorParams.ReferenceSolution,
               generatorParams.SourceFiles,
               generatorParams.SourceDirs,
               generatorParams.ReferenceDirs,
               (project, index, count) => { System.Console.WriteLine($"{project}:{index}:{count}"); }
               );

            var targetCompilation = compilation.Result.TargetCompilation;
            var referenceCompilations = compilation.Result.ReferenceCompilations;
            var additionalNamedTypes = new List<ITypeSymbol>();

            foreach (var t in generatorParams.AdditionalTypes)
            {
                var type = Type.GetType(
                    t.AssemblyQualifiedName,
                    name => Assembly.LoadFrom(t.AssemblyPath),
                    (assembly, typeName, caseSensitive) => assembly.GetType(typeName));

                var namedType = RoslynExtensions.GetTypeSymbolForType(type, targetCompilation);
                if (namedType != null)
                {
                    additionalNamedTypes.Add(namedType);
                }
                else
                {
                    System.Console.Error.WriteLine($"Couldn't find symbol for type {type.Name}");
                }
            }

            var codeGenerator = new Json.CodeGeneratorImpl(targetCompilation, additionalNamedTypes);
            var generated = codeGenerator.Generate($"{generatorParams.ResolverName}Resolver");

            File.WriteAllText(generatorParams.GeneratedFileFullPath, generated);
        }
    }
}
