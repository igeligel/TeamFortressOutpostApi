using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;

namespace SteamBotV2
{
    /// <summary>
    ///     This is a class which will do the login for you into tf2outpost.com.
    ///     Here is a screenshot of the requests.
    ///     Link: http://i.gyazo.com/3f6c33a7c0661975eddf0d8f7d66efdf.png
    ///     Request 1 = Step1();
    ///     Request 2 = Step2();
    ///     ...
    ///     If you need help message me @steam: 76561198028630048 (this is my steamId64). Make sure your profile is not private (scammers...).
    /// 
    ///     We set auto-redirect always to false because we need some data at some requests.
    /// </summary>
    public class Tf2OutpostLogin
    {
        // CookieContainer to save the cookies from the requests.
        CookieContainer _cookieContainer = new CookieContainer();

        // These are the openId parameters you need to login via steam.
        private string _action;
        private string _openidMode;
        private string _openidparams;
        private string _nonce;

        /// <summary>
        ///     This is the authentication key for your account on tf2outpost.
        /// </summary>
        private string _uhash;

        // These are the parameters which will be set after you login into tf2outpost successfully.
        private long _id;
        private string _nickname;
        private string _status;
        private bool _isInGroup;
        private bool _isPrivate;

        // Formal Methods you need to login.

        /// <summary>
        ///     Only method you will use.
        /// </summary>
        /// <param name="inpCookieContainer"> 
        ///     This is a CookieContainer from your Steam Login. If you use the steambot by Jessecar96 (props to you if you see this), you will need to pass steamWeb._cookies.
        /// </param>
        /// <returns>
        ///     A boolean if the login was successful.
        /// </returns>
        public bool Login(CookieContainer inpCookieContainer)
        {
            _cookieContainer = inpCookieContainer;
            Step1();
            var location = Step2();
            Step3(location);
            var location2 = Step4(location);
            Step5(location2);
            Step6();
            return Step7();
        }

        /// <summary>
        ///     The formal method to use HTTPWebRequests.
        ///     I got the headers out of Fiddler. I highly recommend this tool to follow HTTPWebRequests.
        ///     Some Headers are unnecessary but i will use them for a natural-looking HTTPWebRequests.
        /// </summary>
        /// <param name="inpUrl">URL of the HTTPWebRequest.</param>
        /// <param name="inpMethod">Method of the HTTPWebRequest.
        ///     Example: "POST", "GET", "PUT",...</param>
        /// <param name="inpReferer">The Referer of the URL. Sometimes its needed.</param>
        /// <param name="inpHost">The Host of the request.</param>
        /// <param name="inpAccept">Accept format of the request.</param>
        /// <param name="inpNvc">Data which will be wrote if you will do a "Post".</param>
        /// <param name="xRequestedWith">If Ajax, then this is required.</param>
        /// <param name="xPrototypeVersion">If Ajax, then this is required.</param>
        /// <param name="cacheControl">Says, if the request controlls the Cache.</param>
        /// <param name="pragma">Pragma argument.</param>
        /// <param name="allowAutoRedirect">This will desribe, if the request allows to auto-redirect.</param>
        /// <returns>A HttpWebResponse of the request.</returns>
        private HttpWebResponse Request(string inpUrl, string inpMethod, string inpReferer, string inpHost, string inpAccept, NameValueCollection inpNvc, string xRequestedWith, string xPrototypeVersion,
            string cacheControl, string pragma, bool allowAutoRedirect)
        {
            var request = (HttpWebRequest)WebRequest.Create(inpUrl);
            request.Accept = inpAccept;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            request.Timeout = 10000;
            request.Headers.Add("Accept-Language", "de,en-US;q=0.7,en;q=0.3");

            request.AllowAutoRedirect = allowAutoRedirect;

            request.CookieContainer = _cookieContainer;

            request.Method = inpMethod;

            //Volatile variables

            if (inpHost != "")
            {
                request.Host = inpHost;
            }

            if (inpReferer != "")
            {
                request.Referer = inpReferer;
            }

            if (xRequestedWith != "")
            {
                request.Headers.Add("X-Requested-With", xRequestedWith);
            }

            if (xPrototypeVersion != "")
            {
                request.Headers.Add("X-Prototype-Version", xPrototypeVersion);
            }

            if (cacheControl != "")
            {
                request.Headers.Add("Cache-Control", cacheControl);
            }

            if (pragma != "")
            {
                request.Headers.Add("Pragma", pragma);
            }

            if (inpMethod != "POST") return request.GetResponse() as HttpWebResponse;
            var dataString = (inpNvc == null ? null : string.Join("&", Array.ConvertAll(inpNvc.AllKeys, key =>
                string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(inpNvc[key]))
                )));
            if (dataString == null) return request.GetResponse() as HttpWebResponse;
            var dataBytes = Encoding.UTF8.GetBytes(dataString);
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = dataBytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(dataBytes, 0, dataBytes.Length);
            }
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        ///     This method will parse a String out of an HTTPWebResponse.
        /// </summary>
        /// <param name="inpResponse">The HTTPWebResponse to be parsed.</param>
        /// <returns>The Body of the HTTPWebRequest.</returns>
        private static string GetResponseMessage(WebResponse inpResponse)
        {
            var result = "";
            using (inpResponse)
            {
                using (var responseStream = inpResponse.GetResponseStream())
                {
                    if (responseStream == null) return result;
                    using (var reader = new StreamReader(responseStream))
                    {
                        result += reader.ReadToEnd();
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///     This method will update the CookieContainer in the Credentials.cs Object.
        ///     This requires a HTTPWebResponse because in there, there are the Cookies which are
        ///     required for further requests.
        /// </summary>
        /// <param name="inpResponse"></param>
        private void UpdateCookieContainer(HttpWebResponse inpResponse)
        {
            _cookieContainer.Add(inpResponse.Cookies);
        }

        // Requests:

        /// <summary>
        ///     First Request. Just need to make sure you are on tf2outpost.
        /// </summary>
        private void Step1()
        {
            const string url = "http://www.tf2outpost.com/";
            const string host = "www.tf2outpost.com";
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Request(url, "GET", "", host, accept, null, "", "", "", "", false);
        }

        /// <summary>
        ///     First login process on tf2. This is the request sent when you click on the login button.
        /// </summary>
        /// <returns>
        ///     The url you need to connect to. This is given because you set auto-redirect to false.
        /// </returns>
        private string Step2()
        {
            const string url = "http://www.tf2outpost.com/login";
            const string referer = "http://www.tf2outpost.com/";
            const string host = "www.tf2outpost.com";
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            var response = Request(url, "GET", referer, host, accept, null, "", "", "", "", false);
            return response.Headers["Location"]; // Here is the redirect, we redirect manually.
        }

        /// <summary>
        ///     Here we load the via steam login site. You need to grab the openId parameters from this site to continue with the login.
        /// </summary>
        /// <param name="url">
        ///     It is the redirect url.
        /// </param>
        private void Step3(string url)
        {
            const string referer = "http://www.tf2outpost.com/";
            const string host = "steamcommunity.com";
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            var openIdParamBody = GetResponseMessage(Request(url, "GET", referer, host, accept, null, "", "", "", "", false));
            SetOpenIdParams(openIdParamBody);
        }

        /// <summary>
        ///     OpenId login. Simple Post to Steam with certain parameters.
        /// </summary>
        /// <param name="referer">
        ///     Its defined before so you dont need to add it manually.
        /// </param>
        /// <returns>
        ///     Another Location (another redirect), because we set auto-redirect to false.
        /// </returns>
        private string Step4(string referer)
        {
            const string url = "https://steamcommunity.com/openid/login";
            const string host = "steamcommunity.com";
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            var data = new NameValueCollection
            {
                {"action", _action},
                {"openid.mode", _openidMode},
                {"openidparams", _openidparams},
                {"nonce", _nonce}
            };
            return Request(url, "POST", referer, host, accept, data, "", "", "", "", false).Headers["Location"];
        }

        /// <summary>
        ///     OpenId login should be completed and with this request we will get the authentication data of tf2outpost.
        /// </summary>
        /// <param name="url">
        ///     Redirect url.
        /// </param>
        private void Step5(string url)
        {
            const string host = "www.tf2outpost.com";
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            var response = Request(url, "GET", "", host, accept, null, "", "", "", "", false); //Now you get the TF2Outpost Cookie
            var setCookieHeader = response.Headers["Set-Cookie"];
            // Set-Cookie looks like this:
            // string d = "uhash=56c5b70755XXXXXXXXXXXXXXXXXXXXXX; expires=Wed, 29-Jun-2016 20:44:45 GMT; path=/; domain=.tf2outpost.com";
            var d1 = Regex.Split(setCookieHeader, "uhash=");
            var d2 = Regex.Split(d1[1], ";");
            _uhash = d2[0];
            var target = new Uri("http://www.tf2outpost.com/login?openid.ns=http%3A%2F%2Fspecs.openid.net%2Fauth%2F2.0&openid.mode=id_res&openid.op_endpoint=https%3A%2F%2Fsteamcommunity.com%2Fopenid%2Flogin&openid.claimed_id=http%3A%2F%2Fsteamcommunity.com%2Fopenid%2Fid%2F76561198138947175&openid.identity=http%3A%2F%2Fsteamcommunity.com%2Fopenid%2Fid%2F76561198138947175&openid.return_to=http%3A%2F%2Fwww.tf2outpost.com%2Flogin&openid.response_nonce=2015-06-30T20%3A45%3A24Zj4ZtPw3lZkFc4bkTdHliZaaY5Fo%3D&openid.assoc_handle=1234567890&openid.signed=signed%2Cop_endpoint%2Cclaimed_id%2Cidentity%2Creturn_to%2Cresponse_nonce%2Cassoc_handle&openid.sig=7%2BCa74LbOUTrYhTXmzBYryC0QEg%3D");
            _cookieContainer.Add(new Cookie("uhash", _uhash) { Domain = target.Host });
            //Set Manually the cookie, was buggy @ d2lounge, idk why...
            UpdateCookieContainer(response);
        }

        /// <summary>
        ///     Simple authentication finish on tf2outpost.
        /// </summary>
        private void Step6()
        {
            const string url = "http://www.tf2outpost.com/";
            const string host = "www.tf2outpost.com";
            const string accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            Request(url, "GET", "", host, accept, null, "", "", "", "", false);
        }

        /// <summary>
        ///     Get the Api Core data.
        /// </summary>
        /// <returns>
        ///     A boolean. It is true if the Code is 200. So the login was alright.
        /// </returns>
        private bool Step7()
        {
            const string url = "http://www.tf2outpost.com/api/core";
            const string host = "www.tf2outpost.com";
            const string referer = "http://www.tf2outpost.com/";
            const string xRequestedWith = "XMLHttpRequest";
            const string accept = "application/json, text/javascript, */*; q=0.01";
            const string pragma = "no-cache";
            const string cacheControl = "no-cache";
            var data = new NameValueCollection
            {
                {"action", "user.update"},
                {"hash", _uhash}
            };
            var responseMessage = GetResponseMessage(Request(url, "POST", referer, host, accept, data, xRequestedWith, "", cacheControl, pragma, false));
            var accountApiData = JsonConvert.DeserializeObject<Tf2OutpostData>(responseMessage);
            if (accountApiData.Meta.Code != 200) return false;
            _id = accountApiData.Data.Id;
            _nickname = accountApiData.Data.Nickname;
            _status = accountApiData.Data.Status;
            _isInGroup = accountApiData.Data.in_group;
            _isPrivate = accountApiData.Data.is_private;
            return true;
        }

        /// <summary>
        ///     Sets the OpenId parameters on the login via steam site.
        /// </summary>
        /// <param name="htmlBody">
        ///     This is the html Body of the login via steam site.
        /// </param>
        private void SetOpenIdParams(string htmlBody)
        {
            var temp = Regex.Split(htmlBody, "<input type=\"hidden\" id=\"actionInput\" name=\"action\" value=\"");
            var temp1 = Regex.Split(temp[1], "\" />\r\n\t\t\t\t\t\t\t\t\t\t\t<input type=\"hidden\" name=\"openid.mode\" value=\"");
            var temp2 = Regex.Split(temp1[1], "\" />\r\n\t\t\t\t\t\t<input type=\"hidden\" name=\"openidparams\" value=\"");
            var temp3 = Regex.Split(temp2[1], "\" />\r\n\t\t\t\t\t\t\t\t\t\t<input type=\"hidden\" name=\"nonce\" value=\"");
            var temp4 = Regex.Split(temp3[1], "\"");
            _action = temp1[0];
            _openidMode = temp2[0];
            _openidparams = temp3[0];
            _nonce = temp4[0];
        }

        /// <summary>
        ///     Get CookieContainer if you want to do further requests on tf2outpost.
        /// </summary>
        /// <returns></returns>
        public CookieContainer GetCookieContainer()
        {
            return _cookieContainer;
        }

        /// <summary>
        ///     Returns the Id of the api/core/ Element.
        ///     api/core/ looks like this:
        ///     {&quot;meta&quot;:{&quot;code&quot;:200},&quot;data&quot;:{&quot;id&quot;:101XXXX,&quot;nickname&quot;:&quot;&lt;span class=\&quot;nickname regular\&quot;&gt;nickName&lt;\/span&gt;&quot;,&quot;avatar&quot;:&quot;https:\/\/steamcdn-a.akamaihd.net\/steamcommunity\/public\/images\/avatars\/21\/2126950e8ebea4d0e992ae69dXXXXXXXXXXXXXXX_medium.jpg&quot;,&quot;status&quot;:&quot;offline&quot;,&quot;in_group&quot;:false,&quot;is_private&quot;:false}}
        /// </summary>
        /// <returns>
        ///     Id of the api/core/ Element.
        /// </returns>
        public long GetId()
        {
            return _id;
        }

        /// <summary>
        ///     Returns the Nickname of the api/core/ Element.
        ///     api/core/ looks like this:
        ///     {&quot;meta&quot;:{&quot;code&quot;:200},&quot;data&quot;:{&quot;id&quot;:101XXXX,&quot;nickname&quot;:&quot;&lt;span class=\&quot;nickname regular\&quot;&gt;nickName&lt;\/span&gt;&quot;,&quot;avatar&quot;:&quot;https:\/\/steamcdn-a.akamaihd.net\/steamcommunity\/public\/images\/avatars\/21\/2126950e8ebea4d0e992ae69dXXXXXXXXXXXXXXX_medium.jpg&quot;,&quot;status&quot;:&quot;offline&quot;,&quot;in_group&quot;:false,&quot;is_private&quot;:false}}
        /// </summary>
        /// <returns>
        ///     Nickname of the api/core/ Element.
        /// </returns>
        public string GetNickname()
        {
            return _nickname;
        }

        /// <summary>
        ///     Returns the Status of the api/core/ Element.
        ///     api/core/ looks like this:
        ///     {&quot;meta&quot;:{&quot;code&quot;:200},&quot;data&quot;:{&quot;id&quot;:101XXXX,&quot;nickname&quot;:&quot;&lt;span class=\&quot;nickname regular\&quot;&gt;nickName&lt;\/span&gt;&quot;,&quot;avatar&quot;:&quot;https:\/\/steamcdn-a.akamaihd.net\/steamcommunity\/public\/images\/avatars\/21\/2126950e8ebea4d0e992ae69dXXXXXXXXXXXXXXX_medium.jpg&quot;,&quot;status&quot;:&quot;offline&quot;,&quot;in_group&quot;:false,&quot;is_private&quot;:false}}
        /// </summary>
        /// <returns>
        ///     Status of the api/core/ Element.
        /// </returns>
        public string GetStatus()
        {
            return _status;
        }

        /// <summary>
        ///     Returns the IsInGroup status of the api/core/ Element.
        ///     api/core/ looks like this:
        ///     {&quot;meta&quot;:{&quot;code&quot;:200},&quot;data&quot;:{&quot;id&quot;:101XXXX,&quot;nickname&quot;:&quot;&lt;span class=\&quot;nickname regular\&quot;&gt;nickName&lt;\/span&gt;&quot;,&quot;avatar&quot;:&quot;https:\/\/steamcdn-a.akamaihd.net\/steamcommunity\/public\/images\/avatars\/21\/2126950e8ebea4d0e992ae69dXXXXXXXXXXXXXXX_medium.jpg&quot;,&quot;status&quot;:&quot;offline&quot;,&quot;in_group&quot;:false,&quot;is_private&quot;:false}}
        /// </summary>
        /// <returns>
        ///     IsInGroup status of the api/core/ Element.
        /// </returns>
        public bool GetIsInGroup()
        {
            return _isInGroup;
        }

        /// <summary>
        ///     Returns the IsPrivate status of the api/core/ Element.
        ///     api/core/ looks like this:
        ///     {&quot;meta&quot;:{&quot;code&quot;:200},&quot;data&quot;:{&quot;id&quot;:101XXXX,&quot;nickname&quot;:&quot;&lt;span class=\&quot;nickname regular\&quot;&gt;nickName&lt;\/span&gt;&quot;,&quot;avatar&quot;:&quot;https:\/\/steamcdn-a.akamaihd.net\/steamcommunity\/public\/images\/avatars\/21\/2126950e8ebea4d0e992ae69dXXXXXXXXXXXXXXX_medium.jpg&quot;,&quot;status&quot;:&quot;offline&quot;,&quot;in_group&quot;:false,&quot;is_private&quot;:false}}
        /// </summary>
        /// <returns>
        ///     IsPrivate status of the api/core/ Element.
        /// </returns>
        public bool GetIsPrivate()
        {
            return _isPrivate;
        }

        /// <summary>
        ///     Returns the value of the uHash after the openId login and you visit the tf2outpost site.
        /// </summary>
        /// <returns>
        ///     A string of the uhash key.
        /// </returns>
        public string GetUhash()
        {
            return _uhash;
        }
    }

    /// <summary>
    ///     Class to deserialize api/core/ response.
    /// </summary>
    public class Meta
    {
        public int Code { get; set; }
    }

    /// <summary>
    ///     Class to deserialize api/core/ response.
    /// </summary>
    public class Data
    {
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Status { get; set; }
        public bool in_group { get; set; }
        public bool is_private { get; set; }
    }

    /// <summary>
    ///     Class to deserialize api/core/ response.
    /// </summary>
    public class Tf2OutpostData
    {
        public Meta Meta { get; set; }
        public Data Data { get; set; }
    }

}
