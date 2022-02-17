using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Exiled.API.Extensions;

namespace TemporaryGruopAssignment
{
    public static class API
    {
        public static IEnumerator<float> checkForGroupExpire(float delay)
        {
            for(;;)
            {
                yield return Timing.WaitForSeconds(delay);
                foreach(Player p in Player.List)
                    CheckForExpiration(p);
            }
        }

        public static void CheckForExpiration(Player playerToCheck)
        {
            foreach (string fileName in Directory.GetFiles(Plugin.GroupDir))
            {
                string[] lines = File.ReadAllLines(Path.Combine(Plugin.GroupDir, fileName));
                Player playerToChange = null;

                if (fileName == Path.Combine(Plugin.GroupDir, $"{playerToCheck.RawUserId}.txt"))
                    playerToChange = playerToCheck;

                if (playerToChange != null)
                {
                    if (DateTimeOffset.Now.ToUnixTimeSeconds() >= long.Parse(lines[2]))
                    {
                        playerToChange.Group = UserGroupExtensions.GetValue(lines[1]);
                        File.Delete(Path.Combine(Plugin.GroupDir, fileName));
                    }
                    else
                        playerToChange.Group = UserGroupExtensions.GetValue(lines[0]);
                }
            }
        }
    }
}
