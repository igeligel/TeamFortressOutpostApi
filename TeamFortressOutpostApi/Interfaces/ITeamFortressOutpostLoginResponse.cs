using System.Net;

namespace HedgehogSoft.TeamFortressOutpostApi.Interfaces
{
    public interface ITeamFortressOutpostLoginResponse
    {
        string Uhash { get; set; }
        CookieContainer CookieContainer { get; set; }
    }
}
