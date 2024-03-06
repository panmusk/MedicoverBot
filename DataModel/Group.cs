namespace MedicoverBot.DataModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Group
    {
        [JsonProperty("groupName")]
        public string GroupName { get; set; }

        [JsonProperty("sections")]
        public Section[] Sections { get; set; }

        [JsonProperty("telemedicineSpecialityId")]
        public object TelemedicineSpecialityId { get; set; }
    }

    public partial class Section
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("sectionSettings")]
        public SectionSetting[] SectionSettings { get; set; }

        [JsonProperty("sectionTypeId")]
        public long SectionTypeId { get; set; }

        [JsonProperty("subSections")]
        public object[] SubSections { get; set; }
    }

    public partial class SectionSetting
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("optionName")]
        public object OptionName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("telephoneNumber")]
        public object TelephoneNumber { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("tooltip")]
        public object Tooltip { get; set; }

        [JsonProperty("isTooltipEnabled")]
        public bool IsTooltipEnabled { get; set; }

        [JsonProperty("badge")]
        public object Badge { get; set; }

        [JsonProperty("badgeColor")]
        public object BadgeColor { get; set; }

        [JsonProperty("isBadgeEnabled")]
        public bool IsBadgeEnabled { get; set; }

        [JsonProperty("telemedicineSpecialityId")]
        public object TelemedicineSpecialityId { get; set; }

        [JsonProperty("medicalProblemTypeId")]
        public object MedicalProblemTypeId { get; set; }

        [JsonProperty("buttonSettings")]
        public ButtonSetting[] ButtonSettings { get; set; }

        [JsonProperty("isAdHocDisabled")]
        public bool IsAdHocDisabled { get; set; }
    }

    public partial class ButtonSetting
    {
        [JsonProperty("buttonName")]
        public string ButtonName { get; set; }

        [JsonProperty("isChat")]
        public bool IsChat { get; set; }

        [JsonProperty("isCovidSurvey")]
        public bool IsCovidSurvey { get; set; }

        [JsonProperty("specialties")]
        public Specialty[] Specialties { get; set; }

        [JsonProperty("alternativeSpecialities")]
        public object[] AlternativeSpecialities { get; set; }

        [JsonProperty("survey")]
        public long Survey { get; set; }

        [JsonProperty("buttonType")]
        public string ButtonType { get; set; }

        [JsonProperty("isInfectiousSurvey")]
        public bool IsInfectiousSurvey { get; set; }
    }

    public partial class Specialty
    {
        [JsonProperty("cisSpecId")]
        public string CisSpecId { get; set; }
    }
}