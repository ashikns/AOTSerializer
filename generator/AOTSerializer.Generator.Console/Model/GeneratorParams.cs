namespace AOTSerializer.Generator.Console.Model
{
    public static class GeneratorIPCConstants
    {
        public const string SharedMemoryName = "JsonGeneratorSharedMemory";
    }

    public struct GeneratorParams
    {
        public string ResolverName;
        public string GeneratedFileFullPath;

        public string[] SourceFiles;
        public string[] SourceDirs;
        public (string AssemblyQualifiedName, string AssemblyPath)[] AdditionalTypes;

        public string ReferenceSolution;
        public string[] ReferenceDirs;
    }
}
