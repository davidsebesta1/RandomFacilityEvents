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

            Cassie.Message(".g2", isHeld: true);
            if (room != null && config.DoorRandomizationOnBlackout) // room is specified
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
                                Log.Info("RoomBlackoutEventPlus: Closed all doors in " + room.Identifier);
                                HashSet<DoorVariant> doors = DoorVariant.AllDoors;
                                foreach(DoorVariant door2 in doors){
                                    foreach(RoomIdentifier identifier in door2.Rooms)
                                    {
                                        if(identifier == room.Identifier)
                                        {
                                            door2.NetworkTargetState = false;
                                            break;
                                        }
                                    }
                                }
                            } else
                            {
                                Log.Info("RoomBlackoutEventPlus: only some rooms in " + room.Identifier);
                                HashSet<DoorVariant> doors = DoorVariant.AllDoors;
                                foreach (DoorVariant door2 in doors)
                                {
                                    foreach (RoomIdentifier identifier in door2.Rooms)
                                    {
                                        if (identifier == room.Identifier)
                                        {
                                            if(Convert.ToBoolean(Random.Range(0, 1)))
                                            {
                                                door2.NetworkTargetState = false;
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        Log.Info("RoomBlackoutEventPlus: blackout in " + room.Identifier + " ,no doors closed");
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
