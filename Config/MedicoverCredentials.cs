using Newtonsoft.Json;

namespace MedicoverBot.Config
{
    internal class MedicoverCredentials
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cardNumber")]
        public required long CardNumber { get; set; }
        [JsonProperty("password")]
        public required string Password { get; set; }
    }
}
