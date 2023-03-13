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

namespace RandomFacilityEvents.Plugin
{
    internal abstract class BaseFacilityEvent
    {
        protected Config config;
        protected BaseFacilityEvent(Config config) {
            this.config = config;
        }

        public virtual bool RunEvent(Player player, ItemType itemType, FacilityRoom room, FacilityZone zone)
        {
            return true;
        }
    }
}
