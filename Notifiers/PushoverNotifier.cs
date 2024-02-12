using MedicoverBot.DataModel;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using MedicoverBot.Config;
using System.Web;

namespace MedicoverBot.Notifiers
{
    class PushoverNotifier : INotifier
    {
        private readonly Uri apiUrl = new Uri("https://api.pushover.net/1/messages.json");
        private readonly IConfiguration config = AppSettings.Instance.Configuration;

        public async void Notify(Item appointment)
        {
            PushoverConfig pushoverConfig = config.GetSection("pushover").Get<PushoverConfig>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("token", pushoverConfig.Token);
            data.Add("user", pushoverConfig.User);
            data.Add("title", "MedicoverBot");
            data.Add("message", $"{appointment.SpecializationName};{appointment.AppointmentDate.LocalDateTime};{appointment.ClinicName}");
            data.Add("sound", pushoverConfig.Sound);
            data.Add("priority", pushoverConfig.Priority.ToString());
            if (pushoverConfig.Priority == 2)
            {
                data.Add("retry", pushoverConfig.Retry.ToString());
                data.Add("expire", pushoverConfig.Expire.ToString());
            }
            var response = await apiUrl
                .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                .PostUrlEncodedAsync(data);
        }
    }
}