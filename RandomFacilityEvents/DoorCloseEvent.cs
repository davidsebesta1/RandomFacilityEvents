using PluginAPI.Core.Zones;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Doors;
using Interactables.Interobjects.DoorUtils;
using Interactables.Interobjects;

namespace RandomFacilityEvents.Plugin
{
    internal class DoorCloseEvent : BaseFacilityEvent
    {
        public DoorCloseEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, FacilityZone zone = null, FacilityDoor door = null)
        {
            foreach(var door2 in DoorVariant.AllDoors)
            {
                if(door.OriginalObject == door2)
                {
                    door2.NetworkTargetState = true;
                    break;
                }
            }
            return true;
        }
    }
}
