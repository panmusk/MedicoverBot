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
            var groups = await WebRequest.FetchJsonObjectPostAsync<Group>("https://mol.medicover.pl/api/Selector/GetSelectedGroup", new { KeyWordId = selectedKeyWord.Id, RegionId = queryParameters.RegionId }, getCookie());
            var selectedSection = groups.Sections.Length == 1 ? groups.Sections.SingleOrDefault() : Prompt.SelectSection(groups.Sections);
            var serviceIds = selectedSection.SectionSettings.SelectMany(x => x.ButtonSettings).SelectMany(x => x.Specialties).Select(x => x.CisSpecId);
            var checker = Checker.Instance;
            checker.Initialize(queryParameters.RegionId, serviceIds.ToArray());
        }

        private static async Task ExtractAppointmentTypes(QueryParameters? queryParameters, KeyWordsObject keyWords)
        {
            var list = new List<AppointmentType>();
            var isChild = await WebRequest.FetchBoolGet("https://mol.medicover.pl/api/PatientProfiles/IsPatientAChild", getCookie());
            var filenameSuffix = isChild ? "-child" : "-adult";
            foreach (KeyWord keyWord in keyWords.KeyWords)
            {
                var groups = await WebRequest.FetchJsonObjectPostAsync<Group>("https://mol.medicover.pl/api/Selector/GetSelectedGroup", new { KeyWordId = keyWord.Id, RegionId = queryParameters.RegionId }, getCookie());
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
                        int[] ids = section.SectionSettings
                            .SelectMany(ss => ss.ButtonSettings)
                            .Where(bs => bs.Specialties != null)
                            .SelectMany(bs => bs.Specialties)
                            .Select(s => s.CisSpecId)
                            .ToArray();
                        list.Add(new AppointmentType { Name = name, Ids = ids });

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
        public int[] Ids { get; set; }
    }

}