using HedgehogSoft.TeamFortressOutpostApi.Interfaces;
using HedgehogSoft.TeamFortressOutpostApi.Models;
using System.Net;
using System.Net.Http;
using HedgehogSoft.TeamFortressOutpostApi.Extensions;

namespace HedgehogSoft.TeamFortressOutpostApi.Rest
{
    internal class RestClient
    {
        internal CookieContainer CookieContainer;
        internal RestClient()
        {
            CookieContainer = new CookieContainer();
        }

        private HttpClientHandler GetDefaultHttpClientHandler(bool includeCookieContainer)
        {
            var httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            if (includeCookieContainer)
            {
                httpClientHandler.CookieContainer = CookieContainer;
            }
            return httpClientHandler;
        }

        internal HttpResponseMessage GetSteamOffset()
        {
            var httpClientHandler = GetDefaultHttpClientHandler(false);
            var httpClient = new HttpClient(httpClientHandler);
            return httpClient.PostAsync("http://api.steampowered.com/ITwoFactorService/QueryTime/v1/", null).Result;
        }

        internal HttpResponseMessage GetTeamFortressMainPage()
        {
            var httpHeaders = RequestHeaders.GetTeamFortressDefaultHeaders();
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.GetAsync("http://www.tf2outpost.com/").Result;
        }

        internal HttpResponseMessage GetTeamFortressLogin()
        {
            var httpHeaders = RequestHeaders.GetTeamFortressDefaultHeaders();
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.GetAsync("http://www.tf2outpost.com/login").Result;
        }

        internal HttpResponseMessage GetAuthService(string location)
        {
            var httpHeaders = RequestHeaders.GetAuthOpenIdHeaders();
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.GetAsync(location).Result;
        }

        internal HttpResponseMessage GetOpenIdParameters(string url)
        {
            var httpHeaders = RequestHeaders.GetAuthOpenIdHeaders();
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.GetAsync(url).Result;
        }

        internal HttpResponseMessage SteamRsa(string username, string referer)
        {
            var httpHeaders = RequestHeaders.SteamLoginHeaders(referer);
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.PostAsync("https://steamcommunity.com/login/getrsakey/", FormDataGenerator.SteamRsa(username)).Result;
        }

        internal HttpResponseMessage SteamLogin(RsaResponse rsaResponse, string username, string password, string referer, string code)
        {
            var httpHeaders = RequestHeaders.SteamLoginHeaders(referer);
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            var content = FormDataGenerator.SteamDoLogin(rsaResponse, username, password, code);
            return httpClient.PostAsync("https://steamcommunity.com/login/dologin/", content).Result;
        }

        internal HttpResponseMessage PostOpenIdLogin(string referer, IOpenIdParameters openIdParameters)
        {
            var httpHeaders = RequestHeaders.PostOpenIdLoginHeaders(referer);
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.PostAsync("https://steamcommunity.com/openid/login", FormDataGenerator.OpenIdData(openIdParameters)).Result;
        }

        internal HttpResponseMessage GetAuthServiceRedirect(string url, string referer)
        {
            var httpHeaders = RequestHeaders.GetAuthServiceRedirectHeaders(referer);
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.GetAsync(url).Result;
        }

        internal HttpResponseMessage GetTeamFortress(string url)
        {
            var httpHeaders = RequestHeaders.GetTeamFortressHeaders();
            var httpClientHandler = GetDefaultHttpClientHandler(true);
            var httpClient = new HttpClient(httpClientHandler).AddRequestHeaders(httpHeaders);
            return httpClient.GetAsync(url).Result;
        }
    }
}
