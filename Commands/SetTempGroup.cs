using CommandSystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporaryGruopAssignment.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SetTempGroup : ICommand
    {
        public string Command { get; } = "settempgroup";

        public string[] Aliases { get; } = { "stg" };

        public string Description { get; } = "Can assign a group to someone for a specified amount of time.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if(!((CommandSender)sender).CheckPermission(PlayerPermissions.PermissionsManagement))
            {
                response = "You lack permission to set a group!";
                return false;
            }
            if(arguments.Count < 4)
            {
                response = "Wrong usage: settempgroup <UserID> <groupToSet> <groupToGoBackTo> <Time>";
                return false;
            }

            string[] argumentsArray = arguments.ToArray();

            if(API.GetGroupFromString(argumentsArray[1]) == null && argumentsArray[1] != "default")
            {
                response = "groupToSet does not exist.";
                return false;
            }
            if(API.GetGroupFromString(argumentsArray[2]) == null && argumentsArray[2] != "default")
            {
                response = "groupToGoBackTo does not exist.";
                return false;
            }

            string timeArg = argumentsArray[3];
            char lastLetter = timeArg[timeArg.Length - 1];
            string timeArgWithoutLast = timeArg.Remove(timeArg.Length - 1, 1);

            API.DebugLog($"Command SetTempGroup was run with following arguments:\n" +
                $"UserId: {argumentsArray[0]}\n" +
                $"GroupToSet: {argumentsArray[1]}\n" +
                $"GroupToGoBackTo: {argumentsArray[2]}\n" +
                $"FullTimeArg: {argumentsArray[3]}\n" +
                $"TimeScaleIdentifier: {lastLetter}\n" +
                $"RawTime: {timeArgWithoutLast}");

            long time;

            switch(lastLetter)
            {
                case 'm': time = long.Parse(timeArgWithoutLast) * 60; break;
                case 'h': time = long.Parse(timeArgWithoutLast) * 3600; break;
                case 'd': time = long.Parse(timeArgWithoutLast) * 86400; break;
                case 'y': time = long.Parse(timeArgWithoutLast) * 31536000; break;
                default: time = long.Parse(timeArg); break;
            }

            if (File.Exists(Path.Combine(Plugin.GroupDir, $"{argumentsArray[0]}")))
                File.Delete(Path.Combine(Plugin.GroupDir, $"{argumentsArray[0]}"));

            using (StreamWriter sw = File.CreateText(Path.Combine(Plugin.GroupDir, $"{argumentsArray[0]}.txt")))
            {
                sw.WriteLine($"{argumentsArray[1]}");
                sw.WriteLine($"{argumentsArray[2]}");
                sw.WriteLine($"{DateTimeOffset.Now.ToUnixTimeSeconds() + time}");
            }

            Player playerToChange = null;

            if(Player.GetPlayers().Any(x => x.UserId == argumentsArray[0]))
                playerToChange = Player.GetPlayers().Where(x => x.UserId == argumentsArray[0]).First();

            if(playerToChange == null)
            {
                response = "Group set successfully!";
                return true;
            }

            if (argumentsArray[1] == "default")
                playerToChange.ReferenceHub.serverRoles.SetGroup(null, false);
            else
                playerToChange.ReferenceHub.serverRoles.SetGroup(API.GetGroupFromString(argumentsArray[1]), false);

            response = "Group set successfully!";
            return true;
        }
    }
}
