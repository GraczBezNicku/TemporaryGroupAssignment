using Exiled.API.Features;
using MEC;
using System;
using System.IO;

namespace TemporaryGruopAssignment
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin instance;
        public EventsHandler eventsHandler;

        public override string Name => "TemporaryGroupAssignment";
        public override string Author => "GBN";
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion => new Version(5, 3, 0);
        
        public static string GroupDir = Path.Combine(Paths.Configs, "TemporaryGruopAssignment");

        public override void OnEnabled()
        {
            instance = this;
            eventsHandler = new EventsHandler();

            Exiled.Events.Handlers.Player.Verified += eventsHandler.OnPlayerVerified;

            if (!Directory.Exists(GroupDir))
            {
                Log.Warn("Directory where times to expire are stored does not exist. Don't worry though, we're creating one right now ;)");
                Directory.CreateDirectory(GroupDir);
            }

            Timing.RunCoroutine(API.checkForGroupExpire(Plugin.instance.Config.checkInterval));

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Verified -= eventsHandler.OnPlayerVerified;

            eventsHandler = null;
            instance = null;

            base.OnDisabled();
        }
    }
}
