using System;
using System.Text;
using System.Threading.Tasks;

using TF2OutpostAPI.TF2Outpost;

namespace TF2OutpostAPI
{
    class Program
    {
        static void Main(string[] args)
        {
			var tf2outpostLogin = new Tf2OutpostLogin();
			tf2outpostLogin.Login(d);
			String uHash = tf2outpostLogin.GetUhash();
			CookieContainer newCookies = tf2outpostLogin.GetCookieContainer();
			t.GetId();
			tf2outpostLogin.GetIsInGroup();
			tf2outpostLogin.GetIsPrivate();
			tf2outpostLogin.GetNickname();
			tf2outpostLogin.GetStatus();
			
			Console.ReadKey();
        }
    }
}