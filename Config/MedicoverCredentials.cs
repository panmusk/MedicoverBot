using Newtonsoft.Json;

namespace MedicoverBot.Config
{
    internal class MedicoverCredentials
    {
        [JsonProperty("cardNumber")]
        public required long CardNumber { get; set; }
        [JsonProperty("password")]
        public required string Password { get; set; }
    }
}
