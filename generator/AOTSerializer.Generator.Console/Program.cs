using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace AOTSerializer.Generator.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var additionalTypesToInclude = new Type[0];

            //var compilation = RoslynExtensions.GetCompilation(
            //    @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\Microsoft.Graph.sln",
            //    null,
            //    new string[] {
            //        @"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph\Models\Generated",
            //        //@"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph\Requests\Generated",
            //        //@"C:\Users\asalim\Desktop\msgraph-sdk-dotnet\src\Microsoft.Graph.Core\Models",
            //    },
            //    (project, index, count) => { System.Console.WriteLine($"Compiled {project} - {index} of {count}"); }
            //    );


            var compilation2 = RoslynExtensions.GetCompilation(
                @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\HoloBeam.sln",
                new string[] {
                    @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataTypes\AuthenticatedKey.cs",
                    @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataTypes\IdentityDescription.cs",
                    @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataTypes\ViewerStatus.cs"
                },
                new string[] {
                    @"C:\GitRepo\HoloBeam_Rearch\HoloBeam\Assets\_HoloBeam\DataModels\_Contracts",
                },
                new string[] {
                    @"C:\Program Files\Unity\Hub\Editor\2018.3.7f1\Editor\Data\Managed"
                },
                (project, index, count) => { System.Console.WriteLine($"Compiled {project} - {index} of {count}"); }
                );

            var additionalTypesToInclude2 = new[] { typeof(HoloBeam.DataModels.IceServer[]) };

            var targetCompilation = compilation2.Result.TargetCompilation;
            var referenceCompilations = compilation2.Result.ReferenceCompilations;
            var additionalNamedTypes = new List<ITypeSymbol>();

            foreach (var t in additionalTypesToInclude2)
            {
                var namedType = RoslynExtensions.GetTypeSymbolForType(t, targetCompilation);
                if (namedType != null)
                {
                    additionalNamedTypes.Add(namedType);
                }
            }

            var codeGenerator = new Json.CodeGeneratorImpl(targetCompilation, additionalNamedTypes);
            var generated = codeGenerator.Generate();

            System.IO.File.WriteAllText(@"C:\GitRepo\AOTSerializer\Generated.Json.cs", generated);

            System.Console.WriteLine("Done!!");
        }
    }
}
