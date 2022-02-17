using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporaryGruopAssignment
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        [Description("How often should the plugin check if player's group expired (in seconds)")]
        public float checkInterval { get; set; } = 30;
    }
}
