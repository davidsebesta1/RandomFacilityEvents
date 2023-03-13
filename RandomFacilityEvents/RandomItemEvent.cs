using InventorySystem.Items.Pickups;
using InventorySystem;
using PluginAPI.Core;
using PluginAPI.Core.Zones;
using RandomFacilityEvents.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RandomFacilityEvents.Plugins 
{

    internal class RandomItemEvent : BaseFacilityEvent
    {
    public RandomItemEvent(Config config) : base(config)
    {
                
    }

    public override bool RunEvent(Player player, ItemType itemType, FacilityRoom room, FacilityZone zone)
        {
            Log.Info("Random Item spawn event has started");
            System.Random random = new System.Random();
            var item = Server.Instance.ReferenceHub.inventory.ServerAddItem(itemType);
            ItemPickupBase itemPickup = item.ServerDropItem();

            if (itemPickup != null)
            {
                itemPickup.transform.position = room.Position + Vector3.up * 3; // prevent clipping through floor
                itemPickup.transform.rotation = Quaternion.Euler(random.Next(360), 0, 0);

                Log.Info("Spawned " + itemType + " at " + room.Position + " which is " + room.Identifier);
            }
            return true;
        }
    }
}
