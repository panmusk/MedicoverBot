namespace MedicoverBot.Config
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PushoverConfig
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("retry")]
        public int Retry { get; set; }

        [JsonProperty("expire")]
        public int Expire { get; set; }

        [JsonProperty("sound")]
        public string Sound { get; set; }
    }
}
