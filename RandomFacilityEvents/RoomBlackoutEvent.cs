using PluginAPI.Core.Zones;
using PluginAPI.Core;
using RandomFacilityEvents.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomFacilityEvents.Plugin
{

    internal class RoomBlackoutEvent : BaseFacilityEvent
    {
        public RoomBlackoutEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player, ItemType itemType, FacilityRoom room, FacilityZone zone)
        {
            Log.Info("RoomBlackoutEvent has started");
            var flickerControllerInstances = FlickerableLightController.Instances;
            foreach(FlickerableLightController fl in flickerControllerInstances)
            {
                if(fl.Room == room.Identifier)
                {
                    fl.ServerFlickerLights(config.RoomBlackoutTime);
                    return true;
                }
            }


            return false; // couldnt find specified room
        }
    }

}
