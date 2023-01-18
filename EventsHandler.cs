using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;

namespace TemporaryGruopAssignment
{
    public class EventsHandler
    {
        [PluginEvent(PluginAPI.Enums.ServerEventType.PlayerJoined)]
        public void OnPlayerVerified(Player player)
        {
            Timing.CallDelayed(0.5f, () => API.CheckForExpiration(player));
        }
    }
}
