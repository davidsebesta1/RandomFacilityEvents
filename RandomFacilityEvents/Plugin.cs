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

namespace RandomFacilityEvents.Plugin
{
    internal class Plugin
    {
        [PluginConfig]
        public Config config;

        [PluginEntryPoint("RandomFacilityEvents", "0.0.1", "Spawns random events around facility", "davidsebesta")]
        public void Startup()
        {
            Log.Info("RandomFacilityEvents plugin is " + (config.IsEnabled ? "enabled" : "disabled"));
            if(config.IsEnabled)
            {
                EventManager.RegisterEvents((object)this);
            }
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void RoundStart()
        {
            if (config.RandomItemSpawn)
            {
                Random random = new Random();

                int TotalItemsToSpawn = config.RandomItemSpawnAmount + random.Next(config.MaximumAdditionalRandomAmount);

                Timing.CallDelayed(1f, (Action)(() =>
                {
                    List<FacilityRoom> rooms = Facility.Rooms;
                    for (int i = 0; i < TotalItemsToSpawn; i++)
                    {
                        FacilityRoom room = rooms[random.Next(rooms.Count)];


                        SpawnItemAtRoom(ItemType.KeycardGuard, room);
                    }

                }));

            }
        }

        private void SpawnItemAtRoom(ItemType itemType, FacilityRoom room)
        {
            var item = Server.Instance.ReferenceHub.inventory.ServerAddItem(itemType);
            ItemPickupBase itemPickup = item.ServerDropItem();

            if (itemPickup != null)
            {
                itemPickup.transform.position = room.Position;
                itemPickup.transform.rotation = Quaternion.identity;
            }

            Log.Info("Spawned " + itemType + " at " + room.Position + " which is "  + room.Identifier);
        }
    }
}
