using HedgehogSoft.TeamFortressOutpostApi.Interfaces;

namespace HedgehogSoft.TeamFortressOutpostApi.Models
{
    internal class OpenIdParameters : IOpenIdParameters
    {
        public string Action { get; set; }
        public string OpenIdMode { get; set; }
        public string OpenIdParams { get; set; }
        public string Nonce { get; set; }
    }
}
