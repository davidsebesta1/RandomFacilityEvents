using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Core.Zones;
using InventorySystem.Items.Pickups;
using InventorySystem;
using UnityEngine;
using PluginAPI.Core.Doors;

namespace RandomFacilityEvents.Plugin
{
    internal abstract class BaseFacilityEvent
    {
        protected Config config;
        protected BaseFacilityEvent(Config config) {
            this.config = config;
        }

        public virtual bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, FacilityZone zone = null, FacilityDoor door = null)
        {
            return true;
        }
    }
}
