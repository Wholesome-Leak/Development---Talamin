using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wManager.Wow.Class;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;

namespace WholesomeDungeons.Helper
{
    internal static class BundledOffmesh
    {
            public static List<PathFinder.OffMeshConnection> offMeshes = new List<PathFinder.OffMeshConnection>
        {
            //Gnomeregan Elevator Down
            new PathFinder.OffMeshConnection(new List<Vector3>
                {
                    new Vector3(-5163.13, 659.6456, 348.2787),
                    new Vector3(-5164.263, 649.1226, 348.2698) {Action = "c#: Logging.WriteNavigator(\"[OffMeshConnection] Gnomeregan Elevator > Wait Elevator\"); while (Conditions.InGameAndConnectedAndProductStartedNotInPause) { var elevator = ObjectManager.GetWoWGameObjectByEntry(80023).OrderBy(o => o.GetDistance).FirstOrDefault(); if (elevator != null && elevator.IsValid && elevator.Position.DistanceTo(new Vector3(-5164.24, 650.354, 349.52)) < 0.5) break; Thread.Sleep(10); }"},
                    new Vector3(-5163.218, 660.1608, 247.7662) {Action = "c#: Logging.WriteNavigator(\"[OffMeshConnection] Gnomeregan Elevator > Wait to leave Elevator\"); while (Conditions.InGameAndConnectedAndProductStartedNotInPause) { var elevator = ObjectManager.GetWoWGameObjectByEntry(80023).OrderBy(o => o.GetDistance).FirstOrDefault(); if (elevator != null && elevator.IsValid && elevator.Position.DistanceTo(new Vector3(-5164.24, 650.354, 249.4379)) < 0.5) break; Thread.Sleep(10); }"},
                }, (int)ContinentId.Azeroth, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Gnomeregan Elevator Up
            new PathFinder.OffMeshConnection(new List<Vector3>
                {
                    new Vector3(-5163.218, 660.1608, 247.7662),
                    new Vector3(-5164.148, 649.5967, 248.1887) {Action = "c#: Logging.WriteNavigator(\"[OffMeshConnection] Gnomeregan Elevator > Wait Elevator\"); while (Conditions.InGameAndConnectedAndProductStartedNotInPause) { var elevator = ObjectManager.GetWoWGameObjectByEntry(80023).OrderBy(o => o.GetDistance).FirstOrDefault(); if (elevator != null && elevator.IsValid && elevator.Position.DistanceTo(new Vector3(-5164.24, 650.354, 249.4379)) < 0.5) break; Thread.Sleep(10); }"},
                    new Vector3(-5163.13, 659.6456, 348.2787) {Action = "c#: Logging.WriteNavigator(\"[OffMeshConnection] Gnomeregan Elevator > Wait to leave Elevator\"); while (Conditions.InGameAndConnectedAndProductStartedNotInPause) { var elevator = ObjectManager.GetWoWGameObjectByEntry(80023).OrderBy(o => o.GetDistance).FirstOrDefault(); if (elevator != null && elevator.IsValid && elevator.Position.DistanceTo(new Vector3(-5164.24, 650.354, 349.52)) < 0.5) break; Thread.Sleep(10); }"},
                }, (int)ContinentId.Azeroth, PathFinder.OffMeshConnectionType.Unidirectional, true),

//------------------------- Dungeon  Offmeshes --------------------------------------
            //---Offmeshes Wailing Caverns
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(-40.20011, 316.338318, -89.97383, "None"),
                new Vector3(-49.8126221, 312.061218, -106.416878, "None")
            }, (int)ContinentId.WailingCaverns, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Jump1
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(-105.768417, 423.203644, -74.12253, "None"),
                new Vector3(-91.35857, 421.744049, -106.479027, "None")
            }, (int)ContinentId.WailingCaverns, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Jump2
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(-289.711853, -3.12540936, -58.2189865, "None"),
                new Vector3(-286.5002, 1.82894528, -64.10084, "None")
            }, (int)ContinentId.WailingCaverns, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Jump3
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(-53.3504066, 43.08806, -29.9475212, "None"),
                new Vector3(-32.2219238, 49.9484367, -107.619881, "Swimming")
            }, (int)ContinentId.WailingCaverns, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Jump4
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(7.58147335, 222.207169, -83.81526, "None"),
                new Vector3(-0.8281952, 232.389435, -106.248062, "None")
            }, (int)ContinentId.WailingCaverns, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- Wailing  Caverns End
            //---  Earth Song Falls
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(474.774963, -4.930656, -96.31281, "None"),
                new Vector3(461.249969, -1.06105137, -132.284027, "Swimming")
            }, (int)ContinentId.Mauradon, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- Earth Song Falls End
            //---  The  Foulspore
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(815.0435, -168.794586, -77.24495, "None"),
                new Vector3(824.082153, -165.813141, -88.30523, "None")
            }, (int)ContinentId.Mauradon, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- The  Foulspore End
            //---  The  Wicked  Grotto
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(815.975159, -169.217209, -77.24175, "None"),
                new Vector3(824.1289, -166.65036, -88.30741, "None")
            }, (int)ContinentId.Mauradon, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- The  Wicked  Grotto End
            //---  The  Sunken  Temple
            //jump to Boss
                new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3( -448.234741, 51.55307, -148.741669, "None"),
                new Vector3( -455.337463, 65.9520645, -189.72963, "None")
            }, (int)ContinentId.SunkenTemple, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Offmesh to the  Stairs
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3( -337.611176, 111.100029, -131.849655, "None"),
                new Vector3( -341.709473, 105.135048, -131.849655, "None"),
                new Vector3( -344.550568, 101.281067, -131.849655, "None"),
                new Vector3( -348.7211, 96.26017, -131.849655, "None"),
                new Vector3( -355.041046, 91.72966, -131.849655, "None"),
                new Vector3( -359.651855, 88.56651, -131.849655, "None"),
                new Vector3( -364.0866, 84.26096, -131.849655, "None"),
                new Vector3( -368.0995, 80.3650055, -131.849655, "None"),
                new Vector3( -373.370178, 75.7278061, -131.849655, "None"),
                new Vector3( -371.93457, 67.88632, -130.431046,"None")
            }, (int)ContinentId.SunkenTemple, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Lair of the Chosen
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3( -511.1341, -70.35768, -90.82813, "None"),
                new Vector3( -521.854431, -70.2520142, -90.828125, "None"),
                new Vector3( -534.803345, -70.29035, -90.828125, "None"),
                new Vector3( -538.82, -58.2245674, -90.84271, "None")
            }, (int)ContinentId.SunkenTemple, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- The  Sunken  Temple End
            //--- The Prison
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new  Vector3(7513.299, -1071.6792, 264.310516, "None"),
                new  Vector3(7515.20947, -1069.36987, 263.879761, "None"),
                new  Vector3(7525.29443, -1076.75684, 180.348557,  "None"),
            }, (int)ContinentId.BlackrockDepths, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- The  Prison  End
            //---BlackrockSpire Stadium
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(161.27121, -398.742584, 123.033508, "None"),
                new Vector3(161.008957, -407.06546, 110.862968, "None")
            }, (int)ContinentId.BlackRockSpire, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //Blackrockspire Hordemar City
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(-85.69106, -320.178223, 70.95243, "None"),
                new Vector3(-79.54012, -320.063751, 70.95755, "None"),
                new Vector3(-73.43043, -321.0072, 70.9679642, "None"),
                new Vector3(-67.48064, -320.959259, 71.3093643, "None"),
                new Vector3(-61.65857, -320.828, 71.3093643, "None"),
                new Vector3(-54.5414, -320.685883, 71.26088, "None")
            }, (int)ContinentId.BlackRockSpire, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- BlackrockSpire End
            //---  DireMaul East
            //Jump 1
            new PathFinder.OffMeshConnection(new List<Vector3>
            {
                new Vector3(-34.6804848, -419.862732, -36.39973, "None"),
                new Vector3(-46.0699539, -416.602173, -52.8169632, "None"),
                new Vector3(-50.8581619, -414.018555, -58.61438, "None")
            }, (int)ContinentId.DireMaul, PathFinder.OffMeshConnectionType.Unidirectional, true),
            //--- DireMaul  East End
        };
    }
}