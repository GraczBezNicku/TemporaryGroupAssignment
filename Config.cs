using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporaryGruopAssignment
{
    public class Config
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("How often should the plugin check if player's group expired (in seconds)")]
        public float CheckInterval { get; set; } = 30;

        [Description("Which groups should be granted reserved slots on join (if a user was granted the role using this plugin)")]
        public List<string> ReservedGroups { get; set; } = new List<string>()
        {
            "owner"
        };
    }
}
