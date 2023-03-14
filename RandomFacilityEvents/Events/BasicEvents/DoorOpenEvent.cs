using Interactables.Interobjects.DoorUtils;
using PluginAPI.Core.Doors;
using PluginAPI.Core.Zones;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFacilityEvents.Plugin
{
    internal class DoorOpenEvent : BaseFacilityEvent
    {
        public DoorOpenEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, FacilityZone zone = null, FacilityDoor door = null)
        {
            foreach (var door2 in DoorVariant.AllDoors)
            {
                if (door.OriginalObject == door2)
                {
                    Log.Info("Closed door " + door2.name);
                    door2.NetworkTargetState = true;
                    break;
                }
            }
            return true;
        }
    }
}
