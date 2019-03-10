using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AOTSerializer.Generator.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var name = "MSGraph";

            var compilation = RoslynExtensions.GetCompilation(
               @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\Microsoft.Graph.sln",
               new string[] {
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Exceptions\Error.cs",
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Exceptions\ErrorResponse.cs",
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models\AsyncOperationStatus.cs",
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models\Date.cs",
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models\Duration.cs",
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models\ReferenceRequestBody.cs",
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models\TimeOfDay.cs",
               },
               new string[] {
                   @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph\Models\Generated",
                   //@"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph\Requests\Generated",
                   //@"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models",
               },
               null,
               (project, index, count) => { System.Console.WriteLine($"Compiled {project} - {index} of {count}"); }
               );

            var additionalTypesToInclude = new Type[0];


            // var name = "HoloBeam";

            // var compilation = RoslynExtensions.GetCompilation(
            //     @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\HoloBeam.sln",
            //     new string[] {
            //         @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataTypes\AuthenticatedKey.cs",
            //         @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataTypes\IdentityDescription.cs",
            //         @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataTypes\ViewerStatus.cs"
            //     },
            //     new string[] {
            //         @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataModels\_Contracts",
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
