using HedgehogSoft.TeamFortressOutpostApi.Models;
using HedgehogSoft.TeamFortressOutpostApi.Rest;
using Newtonsoft.Json;

namespace HedgehogSoft.TeamFortressOutpostApi.SteamLogin
{
    internal static class OffsetGenerator
    {
        internal static OffsetResponse GetOffset()
        {
            var response = new RestClient().GetSteamOffset();
            return JsonConvert.DeserializeObject<OffsetResponse>(response.Content.ReadAsStringAsync().Result); 
        }
    }
}
