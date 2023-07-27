using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using PluginAPI.Helpers;
using System;
using System.IO;

namespace TemporaryGruopAssignment
{
    public class Plugin
    {
        public static Plugin Instance;

        public static string GroupDir;

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("TemporaryGroupAssignment", "1.2.1", "Lets you assign someone a group for a specified peroid of time.", "GBN")]
        public void EntryPoint()
        {
            Instance = this;
            GroupDir = Path.Combine(PluginHandler.Get(this).PluginDirectoryPath, "TemporaryGruopAssignment");

            EventManager.RegisterAllEvents(this);

            if (!Directory.Exists(GroupDir))
            {
                Log.Warning("Directory where times to expire are stored does not exist. Don't worry though, we're creating one right now ;)");
                Directory.CreateDirectory(GroupDir);
            }

            Timing.RunCoroutine(API.CheckForGroupExpire(Plugin.Instance.Config.CheckInterval));
        }
    }
}
