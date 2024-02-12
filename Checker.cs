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
        private int[] serviceIds { get; set; }
        private Timer _timer;
        private readonly IConfiguration config = AppSettings.Instance.Configuration;
        private readonly int _interval;
        private readonly int _minDistance;
        private Notifier notifier = Notifier.Instance;
        private Authentication auth = Authentication.Instance;
        public static Checker Instance { get; } = new Checker();
        static Checker() { }
        private Checker()
        {
            var botOptions = config.GetSection("botOptions").Get<BotOptions>();
            _minDistance = botOptions.MinDistance;
            _interval = botOptions.QueryInterval * 1000;
        }
        public void Initialize(int? regionId, int[] serviceIds)
        {
            this.regionId = regionId;
            this.serviceIds = serviceIds;
            var autoEvent = new AutoResetEvent(false);
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
            var firstItem = searchResponse.Items.OrderBy(x => x.AppointmentDate).First();
            if (firstItem.AppointmentDate < DateTime.Now.AddHours(_minDistance))
            {
                if (!notifier.IsActive)
                    notifier.Send(firstItem);
            }
            else
            {
                notifier.Stop();
            }
        }
        private Cookie getCookie(){
            Cookie cookie;
            auth.Semaphore.Wait();
            cookie = auth.Cookie;
            auth.Semaphore.Release();
            return cookie;
        }
    }
}