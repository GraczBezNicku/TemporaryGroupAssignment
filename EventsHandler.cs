using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.Events.EventArgs;
using MEC;

namespace TemporaryGruopAssignment
{
    public class EventsHandler
    {
        public void OnPlayerVerified(VerifiedEventArgs ev)
        {
            Timing.CallDelayed(0.5f, () => API.CheckForExpiration(ev.Player));
        }
    }
}
