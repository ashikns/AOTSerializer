using System;
using System.Globalization;
using System.Text;
using Xunit;

namespace AOTSerializer.Tests
{
    public class DateAndTime : IClassFixture<ResolverFixture>
    {
        [Fact]
        public void DateTimeOffsetTest()
        {
            DateTimeOffset now = new DateTime(DateTime.UtcNow.Ticks + TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time").BaseUtcOffset.Ticks, DateTimeKind.Local);
            var binary = Serializer.Serialize(now);
            Serializer.Deserialize<DateTimeOffset>(binary).Is(now);

            foreach (var item in new[] { TimeSpan.MaxValue, TimeSpan.MinValue, TimeSpan.MaxValue.Add(TimeSpan.FromTicks(-1)), TimeSpan.MinValue.Add(TimeSpan.FromTicks(1)), TimeSpan.Zero })
            {
                var ts = Serializer.Deserialize<TimeSpan>(Serializer.Serialize(item));
                ts.Is(item);
            }

            foreach (var item in new[] { DateTime.MaxValue, DateTime.MaxValue.AddTicks(-1), DateTime.MinValue.AddTicks(1) })
            {
                var ts = Serializer.Deserialize<DateTime>(Serializer.Serialize(item));
                ts.Is(item);
            }

            foreach (var item in new[] { DateTimeOffset.MinValue.ToUniversalTime(), DateTimeOffset.MaxValue.ToUniversalTime() })
            {
                var ts = Serializer.Deserialize<DateTimeOffset>(Serializer.Serialize(item));
                ts.Is(item);
            }

            foreach (var item in new[] { new DateTimeOffset(DateTime.MinValue.Ticks, new TimeSpan(-1, 0, 0)) })
            {
                var ts = Serializer.Deserialize<DateTimeOffset>(Serializer.Serialize(item));
                ts.Is(item);
            }
        }

        [Fact]
        public void Nullable()
        {
            DateTimeOffset? now = new DateTime(DateTime.UtcNow.Ticks + TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time").BaseUtcOffset.Ticks, DateTimeKind.Local);
            var binary = Serializer.Serialize(now);
            Serializer.Deserialize<DateTimeOffset?>(binary).ToString().Is(now.ToString());
        }

        [Fact]
        public void Misc()
        {
            {
                var dto = DateTime.UtcNow;
                var serialized = Serializer.Serialize(dto);
                var deSerialized = Serializer.Deserialize<DateTime>(serialized);
                var serialized2 = Serializer.Serialize(deSerialized);

                serialized2.Is(serialized);
            }

            {
                Console.WriteLine("DateTimeOffset.UtcNow");
                var dto = DateTimeOffset.UtcNow;
                var serialized = Serializer.Serialize(dto);
                var deSerialized = Serializer.Deserialize<DateTimeOffset>(serialized);
                var serialized2 = Serializer.Serialize(deSerialized);
                serialized2.Is(serialized);
            }

            {
                Console.WriteLine("DateTime.Now");
                var dto = DateTime.Now;
                var serialized = Serializer.Serialize(dto);
                var deSerialized = Serializer.Deserialize<DateTime>(serialized);
                var serialized2 = Serializer.Serialize(deSerialized);
                serialized2.Is(serialized);
            }


            {
                Console.WriteLine("DateTimeOffset.Now");
                var dto = DateTimeOffset.Now;
                var serialized = Serializer.Serialize(dto);
                var deSerialized = Serializer.Deserialize<DateTimeOffset>(serialized);
                var serialized2 = Serializer.Serialize(deSerialized);
                serialized2.Is(serialized);
            }
        }

        [Fact]
        public void Offset()
        {
            DateTimeOffset now = new DateTime(DateTime.UtcNow.Ticks + TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time").BaseUtcOffset.Ticks, DateTimeKind.Local);
            var binary = "    " + Encoding.UTF8.GetString(Serializer.Serialize(now));
            Serializer.Deserialize<DateTimeOffset>(Encoding.UTF8.GetBytes(binary)).Is(now);

            foreach (var item in new[] { TimeSpan.MaxValue, TimeSpan.MinValue, TimeSpan.MaxValue.Add(TimeSpan.FromTicks(-1)), TimeSpan.MinValue.Add(TimeSpan.FromTicks(1)), TimeSpan.Zero })
            {
                var ts = Serializer.Deserialize<TimeSpan>(Encoding.UTF8.GetBytes("    " + Encoding.UTF8.GetString(Serializer.Serialize(item))));
                ts.Is(item);
            }

            foreach (var item in new[] { DateTime.MaxValue, DateTime.MaxValue.AddTicks(-1), DateTime.MinValue.AddTicks(1) })
            {
                var ts = Serializer.Deserialize<DateTime>(Encoding.UTF8.GetBytes("    " + Encoding.UTF8.GetString(Serializer.Serialize(item))));
                ts.Is(item);
            }

            foreach (var item in new[] { DateTimeOffset.MinValue.ToUniversalTime(), DateTimeOffset.MaxValue.ToUniversalTime() })
            {
                var ts = Serializer.Deserialize<DateTimeOffset>(Encoding.UTF8.GetBytes("    " + Encoding.UTF8.GetString(Serializer.Serialize(item))));
                ts.Is(item);
            }

            foreach (var item in new[] { new DateTimeOffset(DateTime.MinValue.Ticks, new TimeSpan(-1, 0, 0)) })
            {
                var ts = Serializer.Deserialize<DateTimeOffset>(Encoding.UTF8.GetBytes("    " + Encoding.UTF8.GetString(Serializer.Serialize(item))));
                ts.Is(item);
            }
        }
    }

}
