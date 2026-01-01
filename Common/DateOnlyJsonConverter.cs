using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudentCourseEnrollmentApi.Common;

public sealed class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))
            throw new JsonException("DateOnly value is required.");

        if (!DateOnly.TryParseExact(str, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            throw new JsonException($"Invalid date format. Use '{Format}'.");

        return date;
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
}
