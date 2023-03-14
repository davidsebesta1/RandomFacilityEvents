using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using Log = PluginAPI.Core.Log;
using PluginAPI.Enums;
using System;
using PluginAPI.Events;
using System.Collections.Generic;
using PluginAPI.Core.Zones;
using MEC;
using UnityEngine;
using Random = UnityEngine.Random;
using InventorySystem;
using InventorySystem.Items.Pickups;
using PlayerRoles;
using PlayerStatsSystem;
using Mirror;
using MapGeneration;
using Footprinting;
using System.Diagnostics;
using RandomFacilityEvents.Plugins;
using Interactables.Interobjects.DoorUtils;
using PluginAPI.Core.Doors;
using System.Linq;

namespace RandomFacilityEvents.Plugin
{
    internal class Plugin
    {
        //Variables
        private Dictionary<int, BaseFacilityEvent> events = new Dictionary<int, BaseFacilityEvent>();

        //Config variable
        [PluginConfig]
        public Config config;

        //Entry point
        [PluginEntryPoint("RandomFacilityEvents", "0.0.2", "Spawns random events around facility", "davidsebesta")]
        public void Startup()
        {
            Log.Info("RandomFacilityEvents plugin is " + (config.IsEnabled ? "enabled" : "disabled"));
            if(config.IsEnabled)
            {
                EventManager.RegisterEvents((object)this);

                events.Add(0, new RandomItemEvent(config));
                events.Add(1, new RoomBlackoutEvent(config));
                events.Add(2, new ZoneBlackoutEvent(config));
                events.Add(3, new RoomBlackoutEventPlus(config));
                events.Add(4, new DoorCloseEvent(config));
                events.Add(5, new DoorLockdownEvent(config));
            }
        }

        //Round started method
        [PluginEvent(ServerEventType.RoundStart)]
        public void RoundStart()
        {
            InitializeStartingSpawnItems();

            InitializeBlackoutEvents();

            InitializeDoorEvents();
            
        }

        private void InitializeStartingSpawnItems()
        {
            //Random items spawn
            if (config.RandomItemSpawn)
            {
                int TotalItemsToSpawn = config.RandomItemSpawnAmount + Random.Range(0, config.MaximumAdditionalRandomAmount);

                Timing.CallDelayed(1f, (Action)(() =>
                {
                    List<FacilityRoom> rooms = Facility.Rooms;
                    for (int i = 0; i < TotalItemsToSpawn; i++)
                    {
                        FacilityRoom room = rooms[Random.Range(0, rooms.Count)];

                        events[0].RunEvent(itemType: ItemType.Coin, room: room);
                    }

                }));

            }
        }

        private void InitializeBlackoutEvents()
        {
            //Room related blackout coroutine
            if (config.RandomRoomBlackouts || config.RandomZoneBlackouts)
            {
                Timing.CallDelayed(2f, (Action)(() =>
                {
                    Timing.RunCoroutine(RandomBlackoutEnumerator());
                }));
            }
        }

        private void InitializeDoorEvents()
        {
            //Door related events coroutine
            if (config.RandomDoorMalfunctions)
            {
                Timing.CallDelayed(3f, (Action)(() =>
                {
                    Timing.RunCoroutine(RandomDoorMalfunctionEvent());
                }));
            }
        }

        private IEnumerator<float> RandomBlackoutEnumerator()
        {
            Log.Info("Blackout timer has started");
            List<FacilityRoom> rooms = Facility.Rooms;

            int delay = Random.Range(config.MinBlackoutDelay, config.MaxBlackoutDelay) + config.BlackoutBeginDelay;
            Log.Info("delay: " + delay + ", min: " + config.MinBlackoutDelay + ", max: " + config.MaxBlackoutDelay);

            yield return Timing.WaitForSeconds(delay); // initial blackout event delay

            //Blackout events loop
            while (Round.IsRoundStarted)
            {
                FacilityRoom room = rooms[Random.Range(0, rooms.Count)];

                if (config.RandomZoneBlackouts) // zone blackouts enabled
                {
                    bool commenceZoneWideBlackout = Random.Range(0, 10) > 7;

                    if (commenceZoneWideBlackout) // zone wide blackout
                    {
                        events[2].RunEvent();
                    } else if (config.RandomRoomBlackouts) // room blackout
                    {
                        FacilityRoom randomRoom = rooms[Random.Range(0, rooms.Count)];

                        if(config.DoorRandomizationOnBlackout) events[1].RunEvent(room: randomRoom); // classic room blackout

                        else events[3].RunEvent(room: randomRoom); // room blackout with door variation
                    }
                } 
                else // zone blackouts disabled
                {
                    FacilityRoom randomRoom = rooms[Random.Range(0, rooms.Count)];
                    events[1].RunEvent(room: randomRoom);
                }

                delay = Random.Range(config.MinBlackoutDelay, config.MaxBlackoutDelay);
                Log.Info("delay: " + delay + ", min: " + config.MinBlackoutDelay + ", max: " + config.MaxBlackoutDelay);
                yield return Timing.WaitForSeconds(Random.Range(config.MinBlackoutDelay, config.MaxBlackoutDelay));
            }
        }

        private IEnumerator<float> RandomDoorMalfunctionEvent()
        {
            Log.Info("Malfunction timer has started");
            yield return Timing.WaitForSeconds(config.InitialDoorMalfunctionDelay);

            while (config.RandomDoorMalfunctions)
            {
                bool commenceDoorLockMalfunction = Random.Range(0, 10) > 6;

                if(commenceDoorLockMalfunction) events[5].RunEvent(door: FacilityDoor.List[Random.Range(0, FacilityDoor.List.Count)]);  // door close event
                else events[4].RunEvent(door: FacilityDoor.List[Random.Range(0, FacilityDoor.List.Count)]); // door lockdown event


                yield return Timing.WaitForSeconds(Random.Range(config.MinMalfunctionDelay, config.MaxMalfunctionDelay));
            }
        }

    }
}