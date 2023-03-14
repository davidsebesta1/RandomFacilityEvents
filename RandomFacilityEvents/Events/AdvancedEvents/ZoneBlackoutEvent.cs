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

namespace RandomFacilityEvents.Plugin
{
    internal class ZoneBlackoutEvent : BaseFacilityEvent
    {
        //Constructor
        public ZoneBlackoutEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player = null, ItemType itemType = ItemType.None, FacilityRoom room = null, FacilityZone zone = null, FacilityDoor door = null)
        {
            Log.Info("ZoneBlackoutEvent has started");
            float CallDelayedBy = 0;
            if(zone != null) // zone has been set in method parameters
            {
                Cassie.Message("Attention to all jam_010_2 personnel . . " + zone.ZoneType +" power repair .g1 protocol will activate in . 3 . 2 . 1 .");

                Timing.CallDelayed(13.5f, (Action)(() =>
                {
                    KillLightsInZone(zone.ZoneType);
                }));

                Timing.CallDelayed(config.ZoneBlackoutTime, (Action)(() =>
                {
                    Cassie.Message("Zone jam_026_5 repair protocol successfully .g6 .g4 .g4 .g5 pitch_0.9 unstable");
                }));
            }
            else // zone has not been set in method params, random one will be choosen
            {

                int zoneID = Random.Range(0,3);
                switch (zoneID)
                {
                    case 0:
                        Cassie.Message("Attention to all jam_009_3 personnel . . light containment zone power repair .g1 protocol will activate in . 5 . 4 . 3 . 2 . 1 .");
                        CallDelayedBy = 16.5f;
                        Timing.CallDelayed(CallDelayedBy, (Action)(() =>
                        {
                            KillLightsInZone(MapGeneration.FacilityZone.LightContainment);
                        }));
                        break;
                    case 1:
                        Cassie.Message("jam_009_1 Attention to all personnel . . heavy containment zone .g1 power repair protocol will activate in . 3 . 2 . 1 .");
                        CallDelayedBy = 14f;
                        Timing.CallDelayed(CallDelayedBy, (Action)(() =>
                        {

                            KillLightsInZone(MapGeneration.FacilityZone.HeavyContainment);
                        }));
                        break;
                    case 2:
                        Cassie.Message("Attention to all personnel . . entrance zone power repair .g4 protocol will activate in . 3 . 2 . 1 .");
                        CallDelayedBy = 13f;
                        Timing.CallDelayed(CallDelayedBy, (Action)(() =>
                        {
                        KillLightsInZone(MapGeneration.FacilityZone.Entrance);
                        }));
                        break;
                    case 3:
                        Cassie.Message("Attention to all personnel . . surface zone power repair protocol jam_001_2 will activate in . 3 . 2 . 1 .");
                        CallDelayedBy = 13f;
                        Timing.CallDelayed(CallDelayedBy, (Action)(() =>
                        {
                            KillLightsInZone(MapGeneration.FacilityZone.Surface);
                        }));
                        break;
                }


                //Lights back on
                Timing.CallDelayed(config.ZoneBlackoutTime + CallDelayedBy, (Action)(() =>
                {
                    Cassie.Message("Zone jam_020_6 repair protocol successfully .g5 .g4 .g5 .g5 pitch_0.85 unstable");
                }));
            }

            return true;
        }

        //Kill lights in specified zone
        private void KillLightsInZone(MapGeneration.FacilityZone zone)
        {
            var flickerControllerInstances = FlickerableLightController.Instances;

            foreach (FlickerableLightController fl in flickerControllerInstances)
            {
                if(fl.Room.Zone == zone)
                {
                    fl.ServerFlickerLights(config.ZoneBlackoutTime);
                    break;
                }
            }
        }
    }
}
