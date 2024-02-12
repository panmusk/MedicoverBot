using Newtonsoft.Json;

namespace MedicoverBot.Config
{
    public partial class DiscordConfig
    {
        [JsonProperty("discordWebhookURL")]
        public string DiscordWebhookUrl { get; set; }
    }
}