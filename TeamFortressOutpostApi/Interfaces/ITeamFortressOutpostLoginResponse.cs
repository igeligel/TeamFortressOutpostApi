using System.Net;

namespace HedgehogSoft.TeamFortressOutpostApi.Interfaces
{
    /// <summary>
    /// Interface to describe properties of the response by TF2 Outpost.
    /// </summary>
    public interface ITeamFortressOutpostLoginResponse
    {
        /// <summary>
        /// Uhash which is needed for authenticating the requests which go to the API.
        /// </summary>
        string Uhash { get; set; }

        /// <summary>
        /// Cookies which will be saved during the login process.
        /// </summary>
        CookieContainer CookieContainer { get; set; }
    }
}
