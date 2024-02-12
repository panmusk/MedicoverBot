using Microsoft.Extensions.Configuration;

namespace MedicoverBot.Config
{
    public class AppSettings
    {
        public static AppSettings Instance {get;} = new AppSettings();
        static AppSettings(){}
        private AppSettings(){
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public readonly IConfiguration Configuration;
    }
}