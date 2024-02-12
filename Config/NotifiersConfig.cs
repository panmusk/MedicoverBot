namespace MedicoverBot.Config
{
    using Newtonsoft.Json;

    public partial class NotifiersConfig
    {
        [JsonProperty("notifiers")]
        public string[] Notifiers { get; set; }
    }
}