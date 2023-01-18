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
        public static Plugin instance;
        public EventsHandler eventsHandler;

        public static string GroupDir;

        [PluginConfig]
        public Config Config;

        [PluginEntryPoint("TemporaryGroupAssignment", "1.2.0", "Lets you assign someone a group for a short peroid of time.", "GBN#1862")]
        public void EntryPoint()
        {
            instance = this;
            GroupDir = Path.Combine(PluginHandler.Get(this).PluginDirectoryPath, "TemporaryGruopAssignment");
            eventsHandler = new EventsHandler();

            EventManager.RegisterAllEvents(this);

            if (!Directory.Exists(GroupDir))
            {
                Log.Warning("Directory where times to expire are stored does not exist. Don't worry though, we're creating one right now ;)");
                Directory.CreateDirectory(GroupDir);
            }

            Timing.RunCoroutine(API.checkForGroupExpire(Plugin.instance.Config.checkInterval));
        }
    }
}
