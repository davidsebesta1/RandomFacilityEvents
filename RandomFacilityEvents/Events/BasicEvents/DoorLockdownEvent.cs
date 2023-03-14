using Interactables.Interobjects.DoorUtils;
using PluginAPI.Core.Doors;
using PluginAPI.Core.Zones;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;

namespace RandomFacilityEvents.Plugin
{
    internal class DoorLockdownEvent : BaseFacilityEvent
    {
        public DoorLockdownEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, FacilityZone zone = null, FacilityDoor door = null)
        {
            door.Lock(DoorLockReason.NoPower, true);
            Log.Info("Locked door " + door.OriginalObject.name);

            Timing.CallDelayed(10f, (Action)(() =>
            {
                door.Lock(DoorLockReason.NoPower, false);
            }));
            return true;
        }
    }
}
