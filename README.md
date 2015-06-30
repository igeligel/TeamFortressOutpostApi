# TF2Outpost-API
Some classes to get access to tf2outpost.

## Login
CookieContainer _cookies = steamWeb._cookies; 
// You need to grab the cookies after you did the steam login.
// I suggest u to use Jessecar96's Steambot: https://github.com/Jessecar96/SteamBot.
var tf2outpostLogin = new Tf2OutpostLogin();
tf2outpostLogin.Login(d);

## Get Cookies after the login
// After you did the login you can get the hash as a string directly (only authentification) or as CookieContainer.
String uHash = tf2outpostLogin.GetUhash();
CookieContainer newCookies = tf2outpostLogin.GetCookieContainer();

## Get Data from api/core/ of tf2outpost after you login successfully.
t.GetId();
tf2outpostLogin.GetIsInGroup();
tf2outpostLogin.GetIsPrivate();
tf2outpostLogin.GetNickname();
tf2outpostLogin.GetStatus();
