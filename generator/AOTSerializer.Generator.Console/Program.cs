using System;
using System.IO;
using UnityEngine;

namespace AOTSerializer.Generator.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var compilation = RoslynExtensions.GetCompilationFromFiles(
                null,
                null,
                new[] {
                    @"C:\Program Files\Unity\Hub\Editor\2018.3.7f1\Editor\Data\Managed" }
                );

            var additionalTypesToInclude = new Type[]
            {
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Quaternion),
                typeof(Color),
                typeof(Bounds),
                typeof(Rect),

                typeof(AnimationCurve),
                typeof(RectOffset),
                typeof(Gradient),
                typeof(WrapMode),
                typeof(GradientMode),
                typeof(Keyframe),
                typeof(Matrix4x4),
                typeof(GradientColorKey),
                typeof(GradientAlphaKey),
                typeof(Color32),
                typeof(LayerMask),
            };

            var codeGenerator = new Json.CodeGeneratorImpl(compilation, additionalTypesToInclude);
            var generated = codeGenerator.Generate();

            File.WriteAllText(@"C:\GitRepo\AOTSerializer\Generated.Json.cs", generated);

            System.Console.WriteLine("Done!!");
        }
    }
}
