using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PetMarketRfullApi.Infrastructure.Converters
{
    public class CustomDateConverter : JsonConverter<DateTime>
    {
        private readonly string _format;

        public CustomDateConverter(string format)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateString = reader.GetString();
            if (DateTime.TryParseExact(dateString, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }
            throw new JsonException($"Неверный формат даты. Ожидается формат: {_format}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_format));
        }
    }
}
