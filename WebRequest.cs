using System.Net;
using System.Net.Http.Headers;
using Flurl.Http;
using Newtonsoft.Json;

namespace MedicoverBot
{
    public static class WebRequest
    {
        public static async Task<T> FetchJsonObjectPostAsync<T>(string url, object data = null, Cookie cookie = null)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), mediaType: new MediaTypeHeaderValue("application/json"));
            IFlurlResponse resp;

            resp = await url
                .WithHeader("Accept", "application/json")
                .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                .WithCookie(cookie.Name, cookie.Value)
                .PostAsync(content: content);

            var result = await resp.GetJsonAsync<T>();
            var S = await resp.GetStringAsync();
            return result;
        }
        public static async Task<T> FetchJsonObjectGetAsync<T>(string url, Cookie cookie = null)
        {
            var resp = await url
                .WithHeader("Accept", "application/json")
                .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                .WithCookie(cookie.Name, cookie.Value)
                .GetAsync();
            var result = await resp.GetJsonAsync<T>();
            var S = await resp.GetStringAsync();
            return result;
        }

        internal static async Task<bool> FetchBoolGet(string url, Cookie cookie = null)
        {
            var resp = await url
                .WithHeader("Accept", "application/json, text/plain, */*")
                .WithHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36")
                .WithCookie(cookie.Name, cookie.Value)
                .GetAsync();
            var result = await resp.GetStringAsync();
            var S = await resp.GetStringAsync();
            return bool.Parse(result);
        }
    }
}