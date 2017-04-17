using System.Collections.Generic;

namespace HedgehogSoft.TeamFortressOutpostApi.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static Dictionary<string, string> AddCacheControl(this Dictionary<string, string> dict)
        {
            dict.Add("Cache-Control", "max-age=0");
            return dict;
        }

        internal static Dictionary<string, string> AddUpgradeInsecureRequests(this Dictionary<string, string> dict)
        {
            dict.Add("Upgrade-Insecure-Requests", "1");
            return dict;
        }

        internal static Dictionary<string, string> AddAccept(this Dictionary<string, string> dict, string value)
        {
            dict.Add("Accept", value);
            return dict;
        }

        internal static Dictionary<string, string> AddReferer(this Dictionary<string, string> dict, string referer)
        {
            dict.Add("Referer", referer);
            return dict;
        }

        internal static Dictionary<string, string> AddXrequestedWith(this Dictionary<string, string> dict)
        {
            dict.Add("X-Requested-With", "XMLHttpRequest");
            return dict;
        }

        internal static Dictionary<string, string> AddOrigin(this Dictionary<string, string> dict, string origin)
        {
            dict.Add("Origin", origin);
            return dict;
        }
    }
}
