﻿using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;

namespace TemporaryGruopAssignment
{
    public static class API
    {
        public static IEnumerator<float> checkForGroupExpire(float delay)
        {
            for(;;)
            {
                yield return Timing.WaitForSeconds(delay);
                foreach(Player p in Player.GetPlayers())
                    CheckForExpiration(p);
            }
        }

        public static void CheckForExpiration(Player playerToCheck)
        {
            foreach (string fileName in Directory.GetFiles(Plugin.GroupDir))
            {
                string[] lines = File.ReadAllLines(Path.Combine(Plugin.GroupDir, fileName));
                Player playerToChange = null;

                if (fileName == Path.Combine(Plugin.GroupDir, $"{playerToCheck.UserId}.txt"))
                    playerToChange = playerToCheck;

                if (playerToChange != null)
                {
                    if (DateTimeOffset.Now.ToUnixTimeSeconds() >= long.Parse(lines[2]))
                    {
                        if (lines[1] == "default")
                            playerToChange.ReferenceHub.serverRoles.Group = null;
                        else
                            playerToChange.ReferenceHub.serverRoles.Group = GetGroupFromString(lines[1]);

                        File.Delete(Path.Combine(Plugin.GroupDir, fileName));
                    }
                    else
                    {
                        if (lines[0] == "default")
                            playerToChange.ReferenceHub.serverRoles.Group = null;
                        else
                            playerToChange.ReferenceHub.serverRoles.Group = GetGroupFromString(lines[0]);
                    }
                }
            }
        }

        public static UserGroup GetGroupFromString(string groupName)
        {
            ServerStatic.GetPermissionsHandler().GetAllGroups().TryGetValue(groupName, out UserGroup userGroup);
            return userGroup;
        }
    }
}
