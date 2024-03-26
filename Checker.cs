using System.Threading;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spectre.Console;
using Cookie = System.Net.Cookie;
using Spectre.Console.Json;
using MedicoverBot.Config;
using MedicoverBot.DataModel;
using MedicoverBot.Notifiers;
namespace MedicoverBot
{
    class Checker
    {
        private int? regionId { get; set; }
        private string[] serviceIds { get; set; }
        private int doctorId { get; set; }
        private Timer _timer;
        private readonly IConfiguration config = AppSettings.Instance.Configuration;
        private readonly BotOptions botOptions;
        private readonly int _interval;
        private Notifier notifier = Notifier.Instance;
        private Authentication auth = Authentication.Instance;
        private DateTime _minDistance;
        private DateTime _maxDistance;

        public static Checker Instance { get; } = new Checker();
        static Checker() { }
        private Checker()
        {
            botOptions = config.GetSection("botOptions").Get<BotOptions>();
            _interval = botOptions.QueryInterval * 1000;
        }
        public void Initialize(int? regionId, string[] serviceIds, int doctorId = -1)
        {
            this.regionId = regionId;
            this.serviceIds = serviceIds;
            this.doctorId = doctorId;
            var autoEvent = new AutoResetEvent(false);
            _minDistance = DateTime.Now.AddHours(botOptions.MinDistance);
            _maxDistance = DateTime.Now.AddHours(botOptions.MaxDistance);
            _timer = new Timer(this.CheckStatusAsync, autoEvent, 1000, _interval);
            autoEvent.WaitOne();
        }
        private async void CheckStatusAsync(Object stateInfo)
        {
            var searchRequest = new AppointmentSearchRequest(new int?[] { regionId }, serviceIds.ToArray());
            SearchResponse searchResponse = null;

            searchResponse = await WebRequest.FetchJsonObjectPostAsync<SearchResponse>("https://mol.medicover.pl/api/MyVisits/SearchFreeSlotsToBook/",
            data: searchRequest,
            cookie: getCookie());
            var firstItem = searchResponse.Items
                .Where(x => this.doctorId == -1 || x.DoctorId == this.doctorId)
                .Where(x => !x.IsPhoneConsultation)
                .OrderBy(x => x.AppointmentDate)
                .FirstOrDefault();
            if (firstItem == null)
                return;

            if (firstItem.AppointmentDate < _maxDistance && firstItem.AppointmentDate > _minDistance)
            {
                if (!notifier.IsActive)
                    notifier.Send(firstItem);
            }
            else
            {
                notifier.Stop();
            }
        }
        private Cookie getCookie()
        {
            Cookie cookie;
            auth.Semaphore.Wait();
            cookie = auth.Cookie;
            auth.Semaphore.Release();
            return cookie;
        }
    }
}