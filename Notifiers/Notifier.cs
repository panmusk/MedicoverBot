using System.Timers;
using MedicoverBot.Config;
using MedicoverBot.DataModel;
using Microsoft.Extensions.Configuration;

namespace MedicoverBot.Notifiers
{
    public class Notifier
    {
        private static readonly IConfiguration _config = AppSettings.Instance.Configuration;
        private System.Timers.Timer _timer;
        private Item _appointment;
        private List<INotifier> _avaliableNotifiers;
        public static Notifier Instance { get; } = new Notifier();
        private static EventWaitHandle autoEvent = new AutoResetEvent(false);
        private BotOptions _botOptions = AppSettings.Instance.Configuration.GetSection("botOptions").Get<BotOptions>();
        private int _invokeCount;
        private int _maxCount;
        private string[] _notifiersConfig;
        public bool IsActive => this._timer.Enabled;
        static Notifier() { }
        private Notifier()
        {
            _botOptions = AppSettings.Instance.Configuration.GetSection("botOptions").Get<BotOptions>();
            _timer = new System.Timers.Timer(_botOptions.RetriesInterval * 1000);
            _notifiersConfig = _config.GetSection("notifiers").Get<string[]>();
            _avaliableNotifiers = new List<INotifier>();
            if (_notifiersConfig.Contains("console"))
                _avaliableNotifiers.Add(new ConsoleNotifier());
            if (_notifiersConfig.Contains("pushover"))
                _avaliableNotifiers.Add(new PushoverNotifier());
            if (_notifiersConfig.Contains("discord"))
                _avaliableNotifiers.Add(new DiscordNotifier());
        }
        public void Send(Item appointment)
        {
            _appointment = appointment;
            if (!IsActive)
            {
                _timer.Elapsed += OnTimedEvent;
                _timer.Enabled = true;
            }
        }
        private void OnTimedEvent(object? sender, ElapsedEventArgs e)
        {
            _invokeCount++;
            Notify(_appointment);
            if (_invokeCount >= _maxCount)
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimedEvent;
                _invokeCount = 0;
            }
        }

        private void Notify(Item appointment)
        {
            _appointment = appointment;
            _maxCount = _botOptions.NotificationRetries;
            Parallel.ForEach(_avaliableNotifiers, notifier =>
            {
                notifier.Notify(_appointment);
            });
        }
        public void Stop()
        {
            _timer.Stop();
        }
    }
}