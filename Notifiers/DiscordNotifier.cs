using System.Text;
using Flurl.Http;
using MedicoverBot.Config;
using MedicoverBot.DataModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MedicoverBot.Notifiers
{
    class DiscordNotifier : INotifier
    {
        DiscordConfig discordConfig = AppSettings.Instance.Configuration.GetSection("discord").Get<DiscordConfig>();
        private readonly IConfiguration config = AppSettings.Instance.Configuration;
        public void Notify(Item appointment)
        {
            var daysDisnance = (int)(appointment.AppointmentDate - DateTime.Now).TotalDays;
            var hrsDisnance = (int)(appointment.AppointmentDate - DateTime.Now).Hours;

            var humanReadableDistance = daysDisnance == 0 ? $"DZIŚ za {hrsDisnance} godzin" : $"za {daysDisnance} dni";
            var sb = new StringBuilder();
            sb.AppendLine("@everyone");
            sb.Append($"Wolny termin na **{appointment.SpecializationName}** ");
            sb.Append(humanReadableDistance);
            sb.AppendLine();
            sb.Append($"{appointment.ClinicName} {appointment.AppointmentDate.LocalDateTime}");
            discordConfig.DiscordWebhookUrl
                .PostJsonAsync(new { content = sb.ToString() });
        }
    }
}