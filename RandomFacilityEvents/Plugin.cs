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
using Random = System.Random;
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

                int TotalItemsToSpawn = config.RandomItemSpawnAmount + random.Next(config.MaximumAdditionalRandomAmount);

                Timing.CallDelayed(1f, (Action)(() =>
                {
                    List<FacilityRoom> rooms = Facility.Rooms;
                    for (int i = 0; i < TotalItemsToSpawn; i++)
                    {
                        FacilityRoom room = rooms[random.Next(rooms.Count)];

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
            int delay = random.Next(config.MinBlackoutDelay, config.MaxBlackoutDelay);
            Log.Info("delay: " + delay + ", min: " + config.MinBlackoutDelay + ", max: " + config.MaxBlackoutDelay);
            yield return Timing.WaitForSeconds(delay);

            while (Round.IsRoundStarted)
            {
                FacilityRoom room = rooms[random.Next(rooms.Count)];

                if (config.RandomZoneBlackouts)
                {
                    bool commenceZoneWideBlackout = random.Next(10) > 0;

                    if (commenceZoneWideBlackout)
                    {
                        events[2].RunEvent(null, ItemType.None, null, null);
                    } else if (config.RandomRoomBlackouts)
                    {
                        FacilityRoom randomRoom = rooms[random.Next(rooms.Count)];
                        events[1].RunEvent(null, ItemType.None, randomRoom, null);
                    }
                } 
                else
                {
                    FacilityRoom randomRoom = rooms[random.Next(rooms.Count)];
                    events[1].RunEvent(null, ItemType.None, randomRoom, null);
                }

                delay = random.Next(config.MinBlackoutDelay, config.MaxBlackoutDelay);
                Log.Info("delay: " + delay + ", min: " + config.MinBlackoutDelay + ", max: " + config.MaxBlackoutDelay);
                yield return Timing.WaitForSeconds(random.Next(config.MinBlackoutDelay, config.MaxBlackoutDelay));
            }
        }

    }
}
