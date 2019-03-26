using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOTSerializer.Generator.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var name = "MSGraph";
            var graphSourceDir = @"C:\Users\Ashik\Desktop\msgraph-sdk-dotnet\";

            var responseModels = Directory.GetFiles(
                $@"{graphSourceDir}src\Microsoft.Graph\Requests\Generated",
                "*Response*");

            var compilation = RoslynExtensions.GetCompilation(
                $@"{graphSourceDir}Microsoft.Graph.sln",
               new string[] {
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Exceptions\Error.cs",
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Exceptions\ErrorResponse.cs",
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Models\AsyncOperationStatus.cs",
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Models\Date.cs",
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Models\Duration.cs",
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Models\ReferenceRequestBody.cs",
                   $@"{graphSourceDir}src\Microsoft.Graph.Core\Models\TimeOfDay.cs",
               }
                   .Concat(responseModels),
               new string[] {
                   $@"{graphSourceDir}src\Microsoft.Graph\Models\Generated",
                   //$@"{graphSourceDir}src\Microsoft.Graph\Requests\Generated",
                   //$@"{graphSourceDir}src\Microsoft.Graph.Core\Models",
               },
               null,
               (project, index, count) => { System.Console.WriteLine($"Compiled {project} - {index} of {count}"); }
               );

            var additionalTypesToInclude = new Type[0];


            // var name = "HoloBeam";
            // var hbSourceDir = @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\"

            // var compilation = RoslynExtensions.GetCompilation(
            //     $@"{hbSourceDir}HoloBeam.sln",
            //     new string[] {
            //         $@"{hbSourceDir}Assets\_HoloBeam\DataTypes\AuthenticatedKey.cs",
            //         $@"{hbSourceDir}Assets\_HoloBeam\DataTypes\IdentityDescription.cs",
            //         $@"{hbSourceDir}Assets\_HoloBeam\DataTypes\ViewerStatus.cs"
            //     },
            //     new string[] {
            //         $@"{hbSourceDir}Assets\_HoloBeam\DataModels\_Contracts",
            //     },
            //     new string[] {
            //         @"C:\Program Files\Unity\Hub\Editor\2018.3.7f1\Editor\Data\Managed"
            //     },
            //     (project, index, count) => { System.Console.WriteLine($"Compiled {project} - {index} of {count}"); }
            //     );

            // var additionalTypesToInclude = new[] { typeof(HoloBeam.DataModels.IceServer[]) };

            var targetCompilation = compilation.Result.TargetCompilation;
            var referenceCompilations = compilation.Result.ReferenceCompilations;
            var additionalNamedTypes = new List<ITypeSymbol>();

            foreach (var t in additionalTypesToInclude)
            {
                var namedType = RoslynExtensions.GetTypeSymbolForType(t, targetCompilation);
                if (namedType != null)
                {
                    additionalNamedTypes.Add(namedType);
                }
                else
                {
                    System.Console.WriteLine($"Couldn't find symbol for type {t.Name}");
                }
            }

            var codeGenerator = new Json.CodeGeneratorImpl(targetCompilation, additionalNamedTypes);
            var generated = codeGenerator.Generate($"{name}Resolver");

            System.IO.File.WriteAllText($@"{name}.Generated.Json.cs", generated);

            System.Console.WriteLine("Done!!");
        }
    }
}
