
using Newtonsoft.Json;

public partial class QueryParameters
{
    [JsonProperty("regionIds")]
    public int? RegionId { get; set; }

    // [JsonProperty("serviceIds")]
    // public int[] ServiceIds { get; set; }
}