using Newtonsoft.Json;
using System;

namespace AltradyNotifier.Logic
{
    public static class Converters
    {
        public class TrimmingConverter : JsonConverter
        {
            public override bool CanRead => true;

            public override bool CanWrite => true;

            public override bool CanConvert(Type objectType) => objectType == typeof(string);

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return (reader.Value as string ?? string.Empty).Trim();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue((value as string ?? string.Empty).Trim());
            }
        }
    }
}
