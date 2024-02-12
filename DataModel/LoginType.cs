namespace MedicoverBot.DataModel
{
    using Newtonsoft.Json;
    public partial class LoginType
    {
        [JsonProperty("loginType")]
        public long LoginTypeLoginType { get; set; }

        [JsonProperty("debugData")]
        public object DebugData { get; set; }

        [JsonProperty("errorText")]
        public object ErrorText { get; set; }

        [JsonProperty("errorCode")]
        public long ErrorCode { get; set; }
    }
}