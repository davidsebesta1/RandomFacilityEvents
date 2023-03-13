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

namespace RandomFacilityEvents.Plugin
{
    internal class ZoneBlackoutEvent : BaseFacilityEvent
    {
        public ZoneBlackoutEvent(Config config) : base(config)
        {
        }

        public override bool RunEvent(Player player, ItemType itemType, FacilityRoom room, FacilityZone zone)
        {
            Log.Info("ZoneBlackoutEvent has started");
            if(zone != null)
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
            else
            {

                int zoneID = Random.Range(0,3);

                switch (zoneID)
                {
                    case 0:
                        Cassie.Message("Attention to all jam_009_3 personnel . . light containment zone power repair .g1 protocol will activate in . 5 . 4 . 3 . 2 . 1 .");
                        Timing.CallDelayed(16.5f, (Action)(() =>
                        {
                            KillLightsInZone(MapGeneration.FacilityZone.LightContainment);
                        }));
                        break;
                    case 1:
                        Cassie.Message("jam_009_1 Attention to all personnel . . heavy containment zone .g1 power repair protocol will activate in . 3 . 2 . 1 .");
                        Timing.CallDelayed(14f, (Action)(() =>
                        {

                            KillLightsInZone(MapGeneration.FacilityZone.HeavyContainment);
                        }));
                        break;
                    case 2:
                        Cassie.Message("Attention to all personnel . . entrance zone power repair .g4 protocol will activate in . 3 . 2 . 1 .");
                        Timing.CallDelayed(13f, (Action)(() =>
                        {
                        KillLightsInZone(MapGeneration.FacilityZone.Entrance);
                        }));
                        break;
                    case 3:
                        Cassie.Message("Attention to all personnel . . surface zone power repair protocol jam_001_2 will activate in . 3 . 2 . 1 .");
                        Timing.CallDelayed(13f, (Action)(() =>
                        {
                            KillLightsInZone(MapGeneration.FacilityZone.Surface);
                        }));
                        break;
                }

                Timing.CallDelayed(config.ZoneBlackoutTime, (Action)(() =>
                {
                    Cassie.Message("Zone jam_020_6 repair protocol successfully .g5 .g4 .g5 .g5 pitch_0.85 unstable");
                }));
            }

            return true;
        }

        private void KillLightsInZone(MapGeneration.FacilityZone zone)
        {
            var flickerControllerInstances = FlickerableLightController.Instances;

            foreach (FlickerableLightController fl in flickerControllerInstances)
            {
                if(fl.Room.Zone == zone)
                {
                    fl.ServerFlickerLights(config.ZoneBlackoutTime);
                }
            }
        }
    }
}
