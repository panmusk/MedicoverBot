using System.Net;
using System.Net.Http.Headers;
using Flurl.Http;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Spectre.Console;

namespace MedicoverBot
{
    public static class WebRequest
    {
        private static int _triesNumber = 4;
        private static AsyncRetryPolicy _retryPolicy => Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(_triesNumber,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    AnsiConsole.WriteLine($"[{System.Environment.CurrentManagedThreadId}] Try {retryCount}/{_triesNumber}. Waiting {timeSpan} before next try.");
                    AnsiConsole.WriteException(exception);
                });
        public static async Task<T> FetchJsonObjectPostAsync<T>(string url, object data = null, Cookie cookie = null)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                using var content = new StringContent(JsonConvert.SerializeObject(data), mediaType: new MediaTypeHeaderValue("application/json"));

                using var resp = await url
                    .WithHeader("Accept", "application/json")
                    .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                    .WithCookie(cookie.Name, cookie.Value)
                    .PostAsync(content: content);

                var result = await resp.GetJsonAsync<T>();
                return result;
            });
        }
        public static async Task<T> FetchJsonObjectGetAsync<T>(string url, Cookie cookie = null, IEnumerable<KeyValuePair<string,string>> param = null)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var request = url
                    .WithHeader("Accept", "application/json")
                    .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                    .WithCookie(cookie.Name, cookie.Value);
                if (param != null)
                    request.SetQueryParams(param);
                using var resp = await request
                    .GetAsync();
                var resultString = await resp.GetStringAsync();
                var result = JsonConvert.DeserializeObject<T>(resultString);
                return result;
            });
        }

        internal static async Task<bool> FetchBoolGet(string url, Cookie cookie = null)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                using var resp = await url
                    .WithHeader("Accept", "application/json, text/plain, */*")
                    .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                    .WithCookie(cookie.Name, cookie.Value)
                    .GetAsync();
                var result = await resp.GetStringAsync();
                return bool.Parse(result);
            });
        }
    }
}