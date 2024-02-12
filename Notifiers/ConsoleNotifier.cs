using MedicoverBot.Config;
using MedicoverBot.DataModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spectre.Console;
using Spectre.Console.Json;

namespace MedicoverBot.Notifiers{
    class ConsoleNotifier : INotifier
    {
        public void Notify(Item appointment)
        {
            AnsiConsole.Write(new JsonText(JsonConvert.SerializeObject(appointment)));
        }
    }
}