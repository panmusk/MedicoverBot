namespace MedicoverBot.DataModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class KeyWordsObject
    {
        [JsonProperty("keyWords")]
        public KeyWord[] KeyWords { get; set; }
    }

    public partial class KeyWord
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
