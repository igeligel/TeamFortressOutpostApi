using System.Net;
using HedgehogSoft.TeamFortressOutpostApi.Interfaces;

namespace HedgehogSoft.TeamFortressOutpostApi.Models
{
    /// <summary>
    /// Class which will implement <see cref="ITeamFortressOutpostLoginResponse"/> for login response.
    /// </summary>
    public class TeamFortressOutpostLoginResponse : ITeamFortressOutpostLoginResponse
    {
        /// <summary>
        /// Uhash which is needed for authenticating the requests which go to the API.
        /// </summary>
        public string Uhash { get; set; }

        /// <summary>
        /// Cookies which will be saved during the login process.
        /// </summary>
        public CookieContainer CookieContainer { get; set; }
    }
}
