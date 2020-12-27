using Newtonsoft.Json;
using System;

namespace AltradyNotifier.Entities.Pushover
{
    public class Reponse
    {
        public class RateLimit
        {
            [JsonProperty("limit")]
            public int Limit;

            [JsonProperty("remaining")]
            public int Remaining;

            [JsonConverter(typeof(Newtonsoft.Json.Converters.UnixDateTimeConverter))]
            [JsonProperty("reset")]
            public DateTime Reset;

            [JsonProperty("status")]
            public string Status;

            [JsonProperty("request")]
            public string Request;
        }
    }
}
