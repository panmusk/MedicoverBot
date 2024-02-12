
using Newtonsoft.Json;
namespace MedicoverBot.DataModel
{
    public partial class SearchResponse
    {
        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("appointmentDate")]
        public DateTimeOffset AppointmentDate { get; set; }

        [JsonProperty("serviceId")]
        public long ServiceId { get; set; }

        [JsonProperty("specializationName")]
        public string SpecializationName { get; set; }

        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }

        [JsonProperty("doctorId")]
        public long DoctorId { get; set; }

        [JsonProperty("doctorScheduleId")]
        public long DoctorScheduleId { get; set; }

        [JsonProperty("specialtyId")]
        public long SpecialtyId { get; set; }

        [JsonProperty("sysVisitTypeId")]
        public long SysVisitTypeId { get; set; }

        [JsonProperty("vendorTypeId")]
        public string VendorTypeId { get; set; }

        [JsonProperty("doctorLanguages")]
        public object[] DoctorLanguages { get; set; }

        [JsonProperty("clinicId")]
        public long ClinicId { get; set; }

        [JsonProperty("clinicName")]
        public string ClinicName { get; set; }

        [JsonProperty("isPhoneConsultation")]
        public bool IsPhoneConsultation { get; set; }

        [JsonProperty("isBloodCollectionPointConsultation")]
        public bool IsBloodCollectionPointConsultation { get; set; }

        [JsonProperty("adHocYN")]
        public bool AdHocYn { get; set; }
    }
}