using System;
using HedgehogSoft.TeamFortressOutpostApi;

namespace TeamFortressOutpostApiConsole
{
    internal class Program
    {
        // ReSharper disable once UnusedMember.Local
        private static void Main()
        {
            Loader.LoadSettings();
            var teamFortressOutpostApiClient = new TeamFortressOutpostApiClient();
            Console.ReadKey();
            teamFortressOutpostApiClient.Login(
                ConsoleSettings.Instance["username"], 
                ConsoleSettings.Instance["password"], 
                ConsoleSettings.Instance["sharedSecret"]);
            Console.ReadKey();
        }
    }
}