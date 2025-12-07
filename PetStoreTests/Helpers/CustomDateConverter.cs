using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace PetStoreTests // Твой namespace
{
    public class CustomDateConverter : JsonConverter<DateTime>
    {
        // Формат, который шлет сервер (например, +0000)
        private const string Format = "yyyy-MM-ddTHH:mm:ss.fff+0000";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Читаем строку из JSON
            string dateString = reader.GetString();

            // Пытаемся превратить её в дату, используя спец. формат
            if (DateTime.TryParseExact(dateString, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }

            // Если не вышло по нашему формату - пробуем стандартный (на всякий случай)
            return DateTime.Parse(dateString);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Когда отправляем на сервер - пишем в том же формате
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}