using System.Linq;
using HedgehogSoft.TeamFortressOutpostApi.Rest;
using AngleSharp.Parser.Html;
using HedgehogSoft.TeamFortressOutpostApi.Interfaces;
using HedgehogSoft.TeamFortressOutpostApi.Models;
using Newtonsoft.Json;
using skadisteam.login.TwoFactor;
using System.Text.RegularExpressions;

namespace HedgehogSoft.TeamFortressOutpostApi
{
    public class TeamFortressOutpostApiClient
    {
        private readonly RestClient _restClient;
        private string _uhash;

        public TeamFortressOutpostApiClient()
        {
            _restClient = new RestClient();
        }

        public ITeamFortressOutpostLoginResponse Login(string username, string password, string sharedSecret)
        {
            GetMainPage();
            var location = Login();
            var newLocation = LoginRedirect(location);
            var openIdParameters = GetOpenIdParameters(newLocation);
            SteamLogin(username, newLocation, sharedSecret, password);
            var redirectAfterLogin = OpenIdLogin(newLocation, openIdParameters);
            var loc = GetRedirectAfterLogin(redirectAfterLogin, newLocation);
            loc = SecondRedirect(loc);
            ThirdRedirect(loc);
            FinalMainPage();
            return new TeamFortressOutpostLoginResponse
            {
                Uhash = _uhash,
                CookieContainer = _restClient.CookieContainer,
            };
        }

        internal void GetMainPage()
        {
            _restClient.GetTeamFortressMainPage();
        }

        internal string Login()
        {
            var response = _restClient.GetTeamFortressLogin();
            var location = response.Headers.GetValues("Location").FirstOrDefault();
            return location;
        }

        internal string LoginRedirect(string location)
        {
            var response = _restClient.GetAuthService(location);
            var respLocation = response.Headers.GetValues("Location").FirstOrDefault();
            return respLocation;
        }

        internal IOpenIdParameters GetOpenIdParameters(string url)
        {
            var response = _restClient.GetOpenIdParameters(url);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var document = new HtmlParser().Parse(responseBody);
            var allInputs = document.QuerySelectorAll("input");
            return new OpenIdParameters()
            {
                Action = allInputs.FirstOrDefault(e => e.GetAttribute("name") == "action").Attributes.FirstOrDefault(e => e.Name == "value").Value,
                OpenIdMode = allInputs.FirstOrDefault(e => e.GetAttribute("name") == "openid.mode").Attributes.FirstOrDefault(e => e.Name == "value").Value,
                OpenIdParams = allInputs.FirstOrDefault(e => e.GetAttribute("name") == "openidparams").Attributes.FirstOrDefault(e => e.Name == "value").Value,
                Nonce = allInputs.FirstOrDefault(e => e.GetAttribute("name") == "nonce").Attributes.FirstOrDefault(e => e.Name == "value").Value
            };
        }

        internal void SteamLogin(string username, string referer, string sharedSecret, string password)
        {
            var response = _restClient.SteamRsa(username, referer);
            var responseCnt = response.Content.ReadAsStringAsync().Result;
            var rsa =  JsonConvert.DeserializeObject<RsaResponse>(responseCnt);

            var steamTwo = new SteamTwoFactorGenerator {SharedSecret = sharedSecret};
            var code = steamTwo.GenerateSteamGuardCodeForTime();

            _restClient.SteamLogin(rsa, username, password, referer, code);
        }

        internal string OpenIdLogin(string referer, IOpenIdParameters openIdParameters)
        {
            var response = _restClient.PostOpenIdLogin(referer, openIdParameters);
            var respLocation = response.Headers.GetValues("Location").FirstOrDefault();
            return respLocation;
        }

        internal string GetRedirectAfterLogin(string url, string referer)
        {
            var response = _restClient.GetAuthServiceRedirect(url, referer);
            var respLocation = response.Headers.GetValues("Location").FirstOrDefault();
            return respLocation;
        }

        internal string SecondRedirect(string url)
        {
            var response = _restClient.GetTeamFortress(url);
            var respLocation = response.Headers.GetValues("Location").FirstOrDefault();
            return respLocation;
        }

        internal string ThirdRedirect(string url)
        {
            var response = _restClient.GetTeamFortress(url);
            var respLocation = response.Headers.GetValues("Location").FirstOrDefault();
            var cookies = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
            _uhash = Regex.Split(Regex.Split(cookies, "uhash=")[1], ";")[0];
            return respLocation;
        }

        internal void FinalMainPage()
        {
            _restClient.GetTeamFortress("http://www.tf2outpost.com/");
        }
    }
}
