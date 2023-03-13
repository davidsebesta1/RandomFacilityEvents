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

namespace RandomFacilityEvents.Plugin
{
    internal class Plugin
    {
        //Variables
        private readonly Random random = new Random();

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
            }
        }

        //Round started method
        [PluginEvent(ServerEventType.RoundStart)]
        public void RoundStart()
        {
            if (config.RandomItemSpawn)
            {

                int TotalItemsToSpawn = config.RandomItemSpawnAmount + random.Next(config.MaximumAdditionalRandomAmount);

                Timing.CallDelayed(1f, (Action)(() =>
                {
                    List<FacilityRoom> rooms = Facility.Rooms;
                    for (int i = 0; i < TotalItemsToSpawn; i++)
                    {
                        FacilityRoom room = rooms[random.Next(rooms.Count)];

                        SpawnAccidentAtRoom(ItemType.GunE11SR, room);
                        SpawnItemAtRoom(ItemType.KeycardGuard, room);
                    }

                }));

            }
        }

        //Spawn item at specified room location
        private void SpawnItemAtRoom(ItemType itemType, FacilityRoom room)
        {
            var item = Server.Instance.ReferenceHub.inventory.ServerAddItem(itemType);
            ItemPickupBase itemPickup = item.ServerDropItem();

            if (itemPickup != null)
            {
                itemPickup.transform.position = room.Position + Vector3.up * 2;
                itemPickup.transform.rotation = Quaternion.Euler(random.Next(360), 0, 0);

                Log.Info("Spawned " + itemType + " at " + room.Position + " which is "  + room.Identifier);
            }

        }

        //Spawn item with dead body at specified room location
        private void SpawnAccidentAtRoom(ItemType itemType, FacilityRoom room)
        {
            try
            {
                SpawnItemAtRoom(itemType, room);

                Log.Info("Player info: ");

                Log.Info("Obj info: ");

                Log.Info("Spawned a dead body at " + room.Position + " which is at " + room.Identifier);
            } catch(Exception ex)
            {
                Log.Info(ex.ToString());

                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                Log.Info("Exception at: " + line);
            }
        }
    }
}
