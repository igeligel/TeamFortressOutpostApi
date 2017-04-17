using System.Collections.Generic;
using HedgehogSoft.TeamFortressOutpostApi.Extensions;

namespace HedgehogSoft.TeamFortressOutpostApi.Rest
{
    internal static class RequestHeaders
    {
        internal static Dictionary<string, string> GetBaseRequestHeaders()
        {
            return new Dictionary<string, string>
            {
                {"Accept-Encoding", "gzip, deflate"},
                {"Accept-Language", "en-US,en;q=0.8,de-DE;q=0.6,de;q=0.4,it;q=0.2"},
                {"User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36"},
            };
        }

        internal static Dictionary<string, string> GetTeamFortressDefaultHeaders()
        {
            return GetBaseRequestHeaders()
                .AddAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .AddCacheControl()
                .AddReferer("http://www.tf2outpost.com/")
                .AddUpgradeInsecureRequests();
        }

        internal static Dictionary<string, string> GetAuthOpenIdHeaders()
        {
            return GetBaseRequestHeaders()
                .AddAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .AddReferer("http://www.tf2outpost.com/")
                .AddUpgradeInsecureRequests();
        }

        internal static Dictionary<string, string> SteamLoginHeaders(string referer)
        {
            return GetBaseRequestHeaders()
                .AddAccept("*/*")
                .AddReferer(referer)
                .AddXrequestedWith()
                .AddOrigin("https://steamcommunity.com");
        }

        internal static Dictionary<string, string> PostOpenIdLoginHeaders(string referer)
        {
            return GetBaseRequestHeaders()
                .AddCacheControl()
                .AddAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .AddOrigin("https://steamcommunity.com")
                .AddReferer(referer)
                .AddUpgradeInsecureRequests();
        }

        internal static Dictionary<string, string> GetAuthServiceRedirectHeaders(string referer)
        {
            return GetBaseRequestHeaders()
                .AddCacheControl()
                .AddAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .AddReferer(referer)
                .AddUpgradeInsecureRequests();
        }

        internal static Dictionary<string, string> GetTeamFortressHeaders()
        {
            return GetBaseRequestHeaders()
                .AddCacheControl()
                .AddAccept("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8")
                .AddUpgradeInsecureRequests();
        }
    }
}
