using MedicoverBot.Config;
using MedicoverBot.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Spectre.Console;
using Microsoft.Extensions.Logging;
using Cookie = System.Net.Cookie;
using Newtonsoft.Json;

namespace MedicoverBot
{
    internal class Program
    {
        static IConfiguration config = AppSettings.Instance.Configuration;
        static Authentication auth;
        private static async Task Main(string[] args)
        {
            auth = Authentication.Instance;
            var queryParameters = config.GetSection("queryParameters").Get<QueryParameters>();
            var keyWords = await WebRequest.FetchJsonObjectPostAsync<KeyWordsObject>("https://mol.medicover.pl/api/Selector/GetAvailableKeywords", new { RegionId = queryParameters.RegionId }, getCookie());

            if (args.Length == 1 && args[0] == "-e")
            {
                await ExtractAppointmentTypes(queryParameters, keyWords);
                return;
            }
            var selectedKeyWord = Prompt.SelectKeyWord(keyWords);
            var groups = await WebRequest.FetchJsonObjectPostAsync<Group>(Static.GetSelectedGroupUrl, new { KeyWordId = selectedKeyWord.Id, RegionId = queryParameters.RegionId }, getCookie());
            var selectedSection = groups.Sections.Length == 1 ? groups.Sections.SingleOrDefault() : Prompt.SelectSection(groups.Sections);
            var specialtiesIds = selectedSection.SectionSettings.SelectMany(x => x.ButtonSettings).Where(x=>x.Specialties!=null).SelectMany(x => x.Specialties).Select(x => x.CisSpecId).Distinct();
            var parameters = specialtiesIds.Select(x => new KeyValuePair<string, string>("selectedSpecialties", x)).ToList();
            parameters.AddRange(specialtiesIds.Select(x => new KeyValuePair<string, string>("serviceIds", x)));
            parameters.Add(new("regionIds", queryParameters.RegionId.ToString()));
            parameters.Add(new("serviceTypeId", "2"));
            var filterData = await WebRequest.FetchJsonObjectGetAsync<FilterDatas>("https://mol.medicover.pl/api/MyVisits/SearchFreeSlotsToBook/GetFiltersData", getCookie(), parameters);
            var doctors = filterData.Doctors.Prepend(new Entry(){Id = Static.AnyDoctor, Text = "(dowolny)"});
            var selectedDoctor = Prompt.SelectEntry(doctors);
            var checker = Checker.Instance;
            checker.Initialize(queryParameters.RegionId, specialtiesIds.ToArray(), selectedDoctor.Id);
        }

        private static async Task ExtractAppointmentTypes(QueryParameters? queryParameters, KeyWordsObject keyWords)
        {
            var list = new List<AppointmentType>();
            var isChild = await WebRequest.FetchBoolGet("https://mol.medicover.pl/api/PatientProfiles/IsPatientAChild", getCookie());
            var filenameSuffix = isChild ? "-child" : "-adult";
            foreach (KeyWord keyWord in keyWords.KeyWords)
            {
                var groups = await WebRequest.FetchJsonObjectPostAsync<Group>(Static.GetSelectedGroupUrl, new { KeyWordId = keyWord.Id, RegionId = queryParameters.RegionId }, getCookie());
                if (groups.Sections.Length == 1)
                {
                    var section = groups.Sections.FirstOrDefault();
                    list.Add(new AppointmentType { Name = keyWord.Value, Ids = section.SectionSettings.SelectMany(x => x.ButtonSettings).SelectMany(x => x.Specialties).Select(x => x.CisSpecId).ToArray() });
                }
                else
                {
                    foreach (Section section in groups.Sections)
                    {
                        string name = $"{keyWord.Value} - {section.Name}";
                        string[] specialtieIds = section.SectionSettings
                            .SelectMany(ss => ss.ButtonSettings)
                            .Where(bs => bs.Specialties != null)
                            .SelectMany(bs => bs.Specialties)
                            .Select(s => s.CisSpecId)
                            .ToArray();
                        list.Add(new AppointmentType { Name = name, Ids = specialtieIds });

                    }
                }
            }
            File.WriteAllText($"types{filenameSuffix}.json", JsonConvert.SerializeObject(list));
        }
        private static Cookie getCookie()
        {
            Cookie cookie;
            auth.Semaphore.Wait();
            cookie = auth.Cookie;
            auth.Semaphore.Release();
            return cookie;
        }
    }
    class AppointmentType
    {
        public string Name { get; set; }
        public string[] Ids { get; set; }
    }

}