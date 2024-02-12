using System.Net;
using System.Threading;
using MedicoverBot.Config;
using MedicoverBot.DataModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;

namespace MedicoverBot
{
    public class Authentication
    {
        public SemaphoreSlim Semaphore { get; }
        private Timer _timer;
        private Authentication()
        {
            Semaphore = new(1, 1);
            Microsoft.Playwright.Program.Main(new string[] { "install", "chromium" });
            _config = AppSettings.Instance.Configuration;
            _credentials = _config.GetSection("medicover").Get<MedicoverCredentials>();
            var autoEvent = new AutoResetEvent(false);
            _timer = new Timer(this.RefreshCookie, autoEvent, 0, 5 * 60 * 1000);
            autoEvent.WaitOne();

        }
        static Authentication() { }
        public static Authentication Instance { get; } = new Authentication();
        public System.Net.Cookie? Cookie { get; private set; }

        private BrowserContextCookiesResult? _playwrightCookie;
        private IConfiguration _config = AppSettings.Instance.Configuration;
        private MedicoverCredentials? _credentials;
        private async Task<BrowserContextCookiesResult> webLogin()
        {
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            await page.GotoAsync("https://mol.medicover.pl");
            IPage popup = await page.RunAndWaitForPopupAsync(async () => await page.ClickAsync("#oidc-submit"));
            await popup.TypeAsync("#UserName", _credentials.CardNumber.ToString());
            await popup.TypeAsync("#Password", _credentials.Password);
            await popup.ClickAsync("#loginBtn");
            await page.GotoAsync("https://mol.medicover.pl/MyVisits", new() { WaitUntil = WaitUntilState.NetworkIdle });
            var cookies = (await context.CookiesAsync()).ToList();
            await browser.CloseAsync();
            var cookie = cookies.SingleOrDefault(x => x.Name.Equals(".ASPXAUTH"));
            System.Console.WriteLine($"[{System.Environment.CurrentManagedThreadId}] retrived new auth cookie {cookie.Value.Substring(0, 20)}");
            return cookie;
        }
        private async void RefreshCookie(Object? stateInfo)
        {
            Semaphore.Wait();
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            var cookie = await GetCookieAsync();
            Cookie = cookie;
            autoEvent.Set();
            this.Semaphore.Release();
        }
        public async Task<System.Net.Cookie> GetCookieAsync()
        {
            _playwrightCookie = await webLogin();
            var cookie = new System.Net.Cookie(_playwrightCookie.Name, _playwrightCookie.Value);
            return cookie;
        }
    }
}