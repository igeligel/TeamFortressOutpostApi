using System.Net;
using HedgehogSoft.TeamFortressOutpostApi.Interfaces;

namespace HedgehogSoft.TeamFortressOutpostApi.Models
{
    public class TeamFortressOutpostLoginResponse : ITeamFortressOutpostLoginResponse
    {
        public string Uhash { get; set; }
        public CookieContainer CookieContainer { get; set; }
    }
}
