

using Newtonsoft.Json;
namespace MedicoverBot.DataModel
{
    public partial class AppointmentSearchRequest
    {
        public AppointmentSearchRequest(int?[] regionIds, int[] serviceIds)
        {
            RegionIds = regionIds;
            ServiceIds = serviceIds;
        }

        [JsonProperty("regionIds")]
        public int?[] RegionIds { get; set; }

        [JsonProperty("serviceTypeId")]
        public int ServiceTypeId => 2;

        [JsonProperty("serviceIds")]
        public int[] ServiceIds { get; set; }

        // [JsonProperty("searchSince")]
        // public double SearchSince => ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

        // [JsonProperty("startTime")]
        // public string StartTime => "0:00";

        // [JsonProperty("endTime")]
        // public string EndTime => "23:59";

        // [JsonProperty("selectedSpecialties")]
        // public long[] SelectedSpecialties { get; set; }

        // [JsonProperty("visitType")]
        // public string VisitType => "0";

        // [JsonProperty("disablePhoneSearch")]
        // public bool DisablePhoneSearch { get; set; }

        // [JsonProperty("pageSize")]
        // public long PageSize => 1;

        // [JsonProperty("pageNo")]
        // public long PageNo { get; set; }

        // [JsonProperty("isLastMinute")]
        // public bool IsLastMinute { get; set; }

        // [JsonProperty("doctorLanguagesIds")]
        // public object[] DoctorLanguagesIds { get; set; }
    }

}
