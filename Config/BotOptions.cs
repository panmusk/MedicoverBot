namespace MedicoverBot.Config
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TopLevel
    {
        [JsonProperty("botOptions")]
        public BotOptions BotOptions { get; set; }
    }

    public partial class BotOptions
    {
        [JsonProperty("queryInterval")]
        public int QueryInterval { get; set; }

        [JsonProperty("minDistance")]
        public int MinDistance { get; set; }
        [JsonProperty("retriesInterval")]
        public int RetriesInterval { get; set; }
        [JsonProperty("notificationRetries")]
        public int NotificationRetries { get; set; }
    }
}