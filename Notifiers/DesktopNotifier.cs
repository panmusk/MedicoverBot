using MedicoverBot.DataModel;
using Microsoft.Toolkit.Uwp.Notifications;

namespace MedicoverBot.Notifiers
{
    internal class DesktopNotifier : INotifier
    {
        public void Notify(Item appointment)
        {
            var daysDisnance = (int)(appointment.AppointmentDate - DateTime.Now).TotalDays;
            var hrsDisnance = (int)(appointment.AppointmentDate - DateTime.Now).Hours;
            var humanReadableDistance = daysDisnance == 0 ? $"DZIŚ za {hrsDisnance} godzin" : $"za {daysDisnance} dni";
            var logoPath = Path.GetFullPath("./Resources/logo.png");
            new ToastContentBuilder()
                .AddAppLogoOverride(new Uri($"file://{logoPath}", UriKind.Absolute))
                .AddText($"Znaleziono nowy termin na {appointment.SpecializationName}")
                .AddText(humanReadableDistance)
                .AddText($"{appointment.ClinicName} {appointment.AppointmentDate}")
                .Show();
        }
    }
}