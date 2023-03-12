using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFacilityEvents.Plugin
{
    internal class Config
    {
        //Plugin enable
        [Description("Is plugin enabled")]
        public bool IsEnabled { get; set; } = true;

        //Room related randomization
        [Description("Randomized blackouts around facility rooms")]
        public bool RandomRoomBlackouts { get; set; } = true;
        [Description("Randomized zone-wide blackouts")]
        public bool RandomZoneBlackouts { get; set; } = true;

        //Item related randomization
        [Description("Spawn random items around facility")]
        public bool RandomItemSpawn { get; set; } = true;
        [Description("Amount of items to spawn around facility")]
        public int RandomItemSpawnAmount { get; set; } = 5;
        [Description("Randomized add to items")]
        public int MaximumAdditionalRandomAmount { get; set; } = 0;
    }
}
