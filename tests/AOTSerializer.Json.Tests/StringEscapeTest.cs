using AOTSerializer.Tests;
using System.Text;
using Xunit;

namespace AOTSerializer.Json.Tests
{
    public class StringEscapeTest : IClassFixture<ResolverFixture>
    {
        [Fact]
        public void Mixed()
        {
            var str = @"""\u0428\u0440\u0438-\u041b\u0430\u043d\u043a\u0430""";
            Serializer.Deserialize<string>(Encoding.UTF8.GetBytes(str)).Is("Шри-Ланка");
            str = @"""\u041d\u043e\u0432\u0430\u044f \u0437\u0435\u043b\u0430\u043d\u0434\u0438\u044f""";
            Serializer.Deserialize<string>(Encoding.UTF8.GetBytes(str)).Is("Новая зеландия");


            str = @"""\u041d\u043e\u0432\u0430\u044f___\u0437\u0435\u043b\u0430\t\u043d\u0434\u0438\u044f""";
            Serializer.Deserialize<string>(Encoding.UTF8.GetBytes(str)).Is("Новая___зела\tндия");
        }
    }
}
