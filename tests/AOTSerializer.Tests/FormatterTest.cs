using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AOTSerializer.Tests
{
    public class FormatterTest : IClassFixture<ResolverFixture>
    {
        private T Convert<T>(T value)
        {
            return Serializer.Deserialize<T>(Serializer.Serialize(value));
        }

        private object Convert(object value, Type type)
        {
            return Serializer.Deserialize(Serializer.Serialize(value, type), type);
        }

        public static object[][] primitiveFormatterTestData = new object[][]
        {
            new object[] { Int16.MinValue, Int16.MaxValue },
            new object[] { (Int16?)100, null },
            new object[] { Int32.MinValue, Int32.MaxValue },
            new object[] { (Int32?)100, null },
            new object[] { Int64.MinValue, Int64.MaxValue },
            new object[] { (Int64?)100, null },
            new object[] { UInt16.MinValue, UInt16.MaxValue },
            new object[] { (UInt16?)100, null },
            new object[] { UInt32.MinValue, UInt32.MaxValue },
            new object[] { (UInt32?)100, null },
            new object[] { UInt64.MinValue, UInt64.MaxValue },
            new object[] { (UInt64?)100, null },
            new object[] { Single.MinValue, Single.MaxValue },
            new object[] { (Single?)100.100, null },
            new object[] { Double.MinValue, Double.MaxValue },
            new object[] { (Double?)100.100, null },
            new object[] { true, false },
            new object[] { (Boolean?)true, null },
            new object[] { Byte.MinValue, Byte.MaxValue },
            new object[] { (Byte?)100.100, null },
            new object[] { SByte.MinValue, SByte.MaxValue },
            new object[] { (SByte?)100.100, null },
            new object[] { Char.MinValue, Char.MaxValue },
            new object[] { (Char?)'a', null },
            new object[] { DateTime.MinValue.ToUniversalTime(), DateTime.MaxValue.ToUniversalTime() },
            new object[] { (DateTime?)DateTime.UtcNow, null },
        };

        [Theory]
        [MemberData(nameof(primitiveFormatterTestData))]
        public void PrimitiveFormatterTest<T>(T x, T? y)
            where T : struct
        {
            Convert(x).Is(x);
            Convert(y).Is(y);
        }

        [Fact]
        public void NullFormatterTest()
        {
            Convert<object>(null).Is(null);
            Convert(new object()).Is(null);
        }

        public static object[][] standardStructFormatterTestData = new object[][]
        {
            new object[] { decimal.MaxValue, decimal.MinValue, null },
            new object[] { TimeSpan.MaxValue, TimeSpan.MinValue, null },
            new object[] { DateTimeOffset.MaxValue, DateTimeOffset.MinValue, null },
            new object[] { Guid.NewGuid(), Guid.Empty, null },
            new object[] { System.Numerics.BigInteger.Zero, System.Numerics.BigInteger.One, null },
            new object[] { System.Numerics.Complex.Zero, System.Numerics.Complex.One, null },
        };

        [Fact]
        public void PrimitiveStringTest()
        {
            Convert("a").Is("a");
            Convert("test").Is("test");
            Convert("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest")
                .Is("testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest");
            Convert((string)null).IsNull();
        }

        [Theory]
        [MemberData(nameof(standardStructFormatterTestData))]
        public void StandardClassLibraryStructFormatterTest(object x, object y, object z)
        {
            Convert(x, x?.GetType() ?? typeof(object)).Is(x);
            Convert(y, y?.GetType() ?? typeof(object)).Is(y);
            Convert(z, z?.GetType() ?? typeof(object)).Is(z);
        }

        public static object[][] standardClassFormatterTestData = new object[][]
        {
            new object[] { new byte[] { 1, 10, 100 }, new byte[0] { }, null },
            new object[] { "aaa", "", null },
            new object[] { new Uri("Http://hogehoge.com"), new Uri("Https://hugahuga.com"), null },
            new object[] { new Version(0,0), new Version(1,2,3), new Version(255,100,30) },
            new object[] { new Version(1,2), new Version(100, 200,300,400), null },
            new object[] { new BitArray(new[] { true, false, true }), new BitArray(1), null },
        };

        [Theory]
        [MemberData(nameof(standardClassFormatterTestData))]
        public void StandardClassLibraryFormatterTest(object x, object y, object z)
        {
            Convert(x, x?.GetType() ?? typeof(object)).Is(x);
            Convert(y, y?.GetType() ?? typeof(object)).Is(y);
            Convert(z, z?.GetType() ?? typeof(object)).Is(z);
        }

        [Fact]
        public void StringBuilderTest()
        {
            var sb = new StringBuilder("aaa");
            Convert(sb).ToString().Is("aaa");

            StringBuilder nullSb = null;
            Convert(nullSb).IsNull();
        }

        [Fact]
        public void TaskTest()
        {
            Task unitTask = Task.Run(() => 100);
            Convert(unitTask).Status.Is(TaskStatus.RanToCompletion);

            Task nullUnitTask = null;
            Convert(nullUnitTask).Status.Is(TaskStatus.RanToCompletion); // write to nil
        }

        [Fact]
        public void DateTimeOffsetTest()
        {
            DateTimeOffset now = new DateTime(DateTime.UtcNow.Ticks + TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time").BaseUtcOffset.Ticks, DateTimeKind.Local);
            var binary = Serializer.Serialize(now);
            Serializer.Deserialize<DateTimeOffset>(binary).Is(now);
        }

        // https://github.com/neuecc/MessagePack-CSharp/issues/22
        [Fact]
        public void DecimalLang()
        {
            var estonian = new CultureInfo("et-EE");
            CultureInfo.CurrentCulture = estonian;

            var b = Serializer.Serialize(12345.6789M);
            var d = Serializer.Deserialize<decimal>(b);

            d.Is(12345.6789M);
        }

        [Fact]
        public void UriTest()
        {
            var absolute = new Uri("http://google.com/");
            Convert(absolute).ToString().Is("http://google.com/");

            var relative = new Uri("/me/", UriKind.Relative);
            Convert(relative).ToString().Is("/me/");
        }
    }
}
