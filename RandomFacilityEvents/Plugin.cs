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

namespace RandomFacilityEvents.Plugin
{
    internal class Plugin
    {
        //Variables
        private readonly Random random = new Random();
        private Dictionary<int, BaseFacilityEvent> events = new Dictionary<int, BaseFacilityEvent>();

        //Config variable
        [PluginConfig]
        public Config config;

        //Entry point
        [PluginEntryPoint("RandomFacilityEvents", "0.0.1", "Spawns random events around facility", "davidsebesta")]
        public void Startup()
        {
            Log.Info("RandomFacilityEvents plugin is " + (config.IsEnabled ? "enabled" : "disabled"));
            if(config.IsEnabled)
            {
                EventManager.RegisterEvents((object)this);

                events.Add(0, new RandomItemEvent(config));
                events.Add(1, new RoomBlackoutEvent(config));
                events.Add(2, new ZoneBlackoutEvent(config));
            }
        }

        //Round started method
        [PluginEvent(ServerEventType.RoundStart)]
        public void RoundStart()
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

                        events[0].RunEvent(null, ItemType.Coin, room, null);
                    }

                }));

            }


            //Room related blackout coroutine
            if(config.RandomRoomBlackouts || config.RandomZoneBlackouts)
            {
                Timing.CallDelayed(1f, (Action)(() =>
                {
                    Timing.RunCoroutine(RandomBlackoutEnumerator());
                }));
            }
        }

        private IEnumerator<float> RandomBlackoutEnumerator()
        {
            Log.Info("Blackout timer has started");
            List<FacilityRoom> rooms = Facility.Rooms;
            int delay = Random.Range(config.MinBlackoutDelay, config.MaxBlackoutDelay);
            Log.Info("delay: " + delay + ", min: " + config.MinBlackoutDelay + ", max: " + config.MaxBlackoutDelay);
            yield return Timing.WaitForSeconds(delay);

            while (Round.IsRoundStarted)
            {
                FacilityRoom room = rooms[Random.Range(0, rooms.Count)];

                if (config.RandomZoneBlackouts)
                {
                    bool commenceZoneWideBlackout = Random.Range(0, 10) > 0;

                    if (commenceZoneWideBlackout)
                    {
                        events[2].RunEvent(null, ItemType.None, null, null);
                    } else if (config.RandomRoomBlackouts)
                    {
                        FacilityRoom randomRoom = rooms[Random.Range(0, rooms.Count)];
                        events[1].RunEvent(null, ItemType.None, randomRoom, null);
                    }
                } 
                else
                {
                    FacilityRoom randomRoom = rooms[Random.Range(0, rooms.Count)];
                    events[1].RunEvent(null, ItemType.None, randomRoom, null);
                }

                delay = Random.Range(config.MinBlackoutDelay, config.MaxBlackoutDelay);
                Log.Info("delay: " + delay + ", min: " + config.MinBlackoutDelay + ", max: " + config.MaxBlackoutDelay);
                yield return Timing.WaitForSeconds(Random.Range(config.MinBlackoutDelay, config.MaxBlackoutDelay));
            }
        }

    }
}
