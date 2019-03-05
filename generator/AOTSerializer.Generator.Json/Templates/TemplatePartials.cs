namespace AOTSerializer.Generator.Json.Templates
{
    public partial class FormatterTemplate
    {
        public string Namespace;
        public ObjectSerializationInfo[] ObjectSerializationInfos;
    }

    public partial class ResolverTemplate
    {
        public string Namespace;
        public string ResolverName = "GeneratedResolver";
        public IResolverRegisterInfo[] RegisterInfos;
    }
    public partial class EnumTemplate
    {
        public string Namespace;
        public EnumSerializationInfo[] EnumSerializationInfos;
    }
}
