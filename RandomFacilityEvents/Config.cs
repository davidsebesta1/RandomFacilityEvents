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
        public bool IsEnabled { get; set; } = true; // true by default

        //Main variables
        [Description("Begin blackout delay in seconds")]
        public int BlackoutBeginDelay { get; set; } = 30; // 300 by default

        //Room related randomization
        [Description("Randomized blackouts around facility rooms")]
        public bool RandomRoomBlackouts { get; set; } = true; // true by default
        [Description("Extra dool randomization on blackout")]
        public bool DoorRandomizationOnBlackout { get; set; } = true; // true by default
        [Description("Minimum time between random blackouts in seconds")]
        public int MinBlackoutDelay { get; set; } = 180; // 180 by default
        [Description("Maximum time between random blackouts in seconds")]
        public int MaxBlackoutDelay { get; set; } = 240; // 240 by default
        [Description("Room blackout time")]
        public int RoomBlackoutTime { get; set; } = 15; // 15 by default

        //Zone related blackout randomization
        [Description("Randomized zone-wide blackouts")]
        public bool RandomZoneBlackouts { get; set; } = true; // true by default
        [Description("ZoneBlackoutDuration")]
        public int ZoneBlackoutTime { get; set; } = 30; // 30 by default

        //Item related randomization
        [Description("Spawn random items around facility")]
        public bool RandomItemSpawn { get; set; } = true; // true by default
        [Description("Amount of items to spawn around facility")]
        public int RandomItemSpawnAmount { get; set; } = 5; // 5 by default
        [Description("Do 'accidents' happen (spawn dead bodies)")]
        public bool AccidentsSpawn { get; set; } = true; // true by default
        [Description("Randomized add to items")]
        public int MaximumAdditionalRandomAmount { get; set; } = 0; // 0 by default

        //Door related malfunctionss
        [Description("Are random door malfunctions enabled")]
        public bool RandomDoorMalfunctions { get; set; } = true; // true default
        [Description("Initial door malfunction delay")]
        public int InitialDoorMalfunctionDelay { get; set; } = 30; // 30 default
        [Description("Minimum time between door malfunction in seconds")]
        public int MinMalfunctionDelay { get; set; } = 30; // 30 default
        [Description("Maximum time between door malfunction in seconds")]
        public int MaxMalfunctionDelay { get; set; } = 45; // 45 default
    }
}
