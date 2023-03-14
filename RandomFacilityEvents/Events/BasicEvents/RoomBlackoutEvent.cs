using PluginAPI.Core.Zones;
using PluginAPI.Core;
using RandomFacilityEvents.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = UnityEngine.Random;
using CommandSystem.Commands.RemoteAdmin;
using PluginAPI.Core.Doors;

namespace RandomFacilityEvents.Plugin
{

    internal class RoomBlackoutEvent : BaseFacilityEvent
    {
        //Constructor
        public RoomBlackoutEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, FacilityZone zone = null, FacilityDoor door = null)
        {
            Log.Info("RoomBlackoutEvent has started");
            var flickerControllerInstances = FlickerableLightController.Instances;

            Cassie.Message(".g2", isHeld: false, isNoisy: false);
            if (room != null) // room is specified
            {
                foreach (FlickerableLightController fl in flickerControllerInstances)
                {
                    if (fl.Room == room.Identifier)
                    {
                        fl.ServerFlickerLights(config.RoomBlackoutTime);
                        Log.Info("Specified blackout in " + room.Identifier);
                        return true; // turned lights off
                    }
                }
            } else // room is not specified
            {
                var lightController = flickerControllerInstances[Random.Range(0, flickerControllerInstances.Count())];
                lightController.ServerFlickerLights(config.RoomBlackoutTime);

                Log.Info("Blackout in " + lightController.Room);
                return true; // turned lights off
            }

            return false; // couldnt turn off the lights
        }
    }

}
