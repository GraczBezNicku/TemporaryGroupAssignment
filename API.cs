using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;
using Mirror.LiteNetLib4Mirror;

namespace TemporaryGruopAssignment
{
    public static class API
    {
        public static IEnumerator<float> CheckForGroupExpire(float delay)
        {
            while(true)
            {
                yield return Timing.WaitForSeconds(delay);
                foreach(Player p in Player.GetPlayers())
                    CheckForExpiration(p);
            }
        }

        public static void CheckForExpiration(Player playerToCheck)
        {
            if (!File.Exists(Path.Combine(Plugin.GroupDir, $"{playerToCheck.UserId}.txt")))
                return;

            string[] content = File.ReadAllLines(Path.Combine(Plugin.GroupDir, $"{playerToCheck.UserId}.txt"));

            if (DateTimeOffset.Now.ToUnixTimeSeconds() >= long.Parse(content[2]))
            {
                if (content[1] == "default")
                    playerToCheck.ReferenceHub.serverRoles.SetGroup(null, false);
                else
                    playerToCheck.ReferenceHub.serverRoles.SetGroup(GetGroupFromString(content[1]), false);

                DebugLog($"Player {playerToCheck} had an expired group ({content[1]}). Taking group away and deleting the file.");

                ReservedSlots.Remove(playerToCheck.UserId);
                File.Delete(Path.Combine(Plugin.GroupDir, $"{playerToCheck.UserId}.txt"));
            }
            else
            {
                if (Plugin.Instance.Config.ReservedGroups.Contains(content[0]))
                    ReservedSlots.Add(playerToCheck.UserId);

                if (content[0] == "default")
                    playerToCheck.ReferenceHub.serverRoles.SetGroup(null, false);
                else
                    playerToCheck.ReferenceHub.serverRoles.SetGroup(GetGroupFromString(content[0]), false);
            }
        }

        public static UserGroup GetGroupFromString(string groupName)
        {
            ServerStatic.GetPermissionsHandler().GetAllGroups().TryGetValue(groupName, out UserGroup userGroup);
            return userGroup;
        }

        public static void DebugLog(string message)
        {
            if (Plugin.Instance.Config.Debug)
                Log.Debug($"{message}");
        }
    }
}
