using PluginAPI.Core.Zones;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;
using PluginAPI.Core.Doors;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;

namespace RandomFacilityEvents.Plugin
{
    internal class RoomBlackoutEventPlus : BaseFacilityEvent
    {
        public RoomBlackoutEventPlus(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, PluginAPI.Core.Zones.FacilityZone zone = null, FacilityDoor door = null)
        {
            Log.Info("RoomBlackoutEvent has started");
            var flickerControllerInstances = FlickerableLightController.Instances;

            Cassie.Message(".g2", isHeld: false);
            if (room != null) // room is specified
            {
                foreach (FlickerableLightController fl in flickerControllerInstances)
                {
                    if (fl.Room == room.Identifier)
                    {
                        fl.ServerFlickerLights(config.RoomBlackoutTime);

                        bool additionalDoorEvent = Convert.ToBoolean(Random.Range(0, 1));

                        if (additionalDoorEvent)
                        {
                            bool closeAllDoors = Convert.ToBoolean(Random.Range(0, 1));

                            if (closeAllDoors)
                            {
                                //TODO: close whole room doors
                            } else
                            {
                                //TODO: close only some doors
                             }
                        }

                        Log.Info("Specified blackout in " + room.Identifier);
                        return true; // turned lights off
                    }
                }
            }
            else // room is not specified
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
