using System;
using System.Collections.Generic;
using System.Linq;
using RDManaged;
using robotManager.Helpful;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Helper {
    public static class AoEAvoider {
        private static readonly object Locker = new object();
        private static readonly List<Tuple<Vector3, float>> Areas = new List<Tuple<Vector3, float>>();
        private static readonly List<Tuple<Vector3, float>> Blacklist = new List<Tuple<Vector3, float>>();

        public static List<Tuple<Vector3, float>> GetAreas() {
            lock (Locker) {
                return Areas;
            }
        }

        public static void Update(bool automaticallyDetectRange = false) {
            // Find areas
            lock (Locker) {
                Areas.Clear();
                foreach (WoWObject wObject in ObjectManager.ObjectList)
                    if (Helper.Blacklist.KnownAoE.TryGetValue(wObject.Entry, out float radius)) {
                        Vector3 position;
                        switch (wObject.Type) {
                            case WoWObjectType.Unit:
                                position = new WoWUnit(wObject.GetBaseAddress).PositionWithoutType;
                                break;
                            case WoWObjectType.DynamicObject:
                                var dObject = new DynamicObject(wObject.GetBaseAddress);
                                position = dObject.Position;
                                if (automaticallyDetectRange) radius = dObject.Radius;
                                break;
                            case WoWObjectType.GameObject:
                                position = new WoWGameObject(wObject.GetBaseAddress).Position;
                                break;
                            default:
                                Logger.LogError($"Invalid object type ({wObject.Type}) in AoE avoider.");
                                return;
                        }

                        Areas.Add(new Tuple<Vector3, float>(position, radius));
                    }

                // Add to blacklist
                foreach ((Vector3 position, float radius) in Blacklist)
                    PathFinder.ResetArea(position, radius, RD.PolyArea.POLYAREA_BIGDANGER);

                Blacklist.Clear();
                foreach ((Vector3 position, float radius) in Areas) {
                    Blacklist.Add(new Tuple<Vector3, float>(position, radius));
                    PathFinder.ReportArea(position, radius, RD.PolyArea.POLYAREA_BIGDANGER);
                }
            }
        }

        public static bool Avoid(byte minimumSearchRange = 15, byte maximumSearchRange = 50) {
            Vector3 myPosition = ObjectManager.Me.PositionWithoutType;
            lock (Locker) {
                if (!Areas.Any(tuple => myPosition.DistanceTo2D(tuple.Item1) < tuple.Item2)) return false;

                // Finding safe spot to go to.
                var safeSpots = new List<Vector3>();
                for (byte range = minimumSearchRange; range <= maximumSearchRange; range++)
                for (int y = -range; y <= range; y++)
                for (int x = -range; x <= range; x++) {
                    Vector3 position = myPosition + new Vector3(x, y, 0);
                    if (!Areas.Any(tuple => position.DistanceTo2D(tuple.Item1) < tuple.Item2 + 1f))
                        safeSpots.Add(position);
                }

                if (safeSpots.Count <= 0) {
                    Logger.LogError("Failed to find any safe spots!");
                    return false;
                }

                var escapePath = new List<Vector3>();
                var foundPath = false;
                foreach (Vector3 spot in safeSpots.OrderBy(spot => myPosition.DistanceTo2D(spot))) {
                    var targetPosition = new Vector3(spot.X, spot.Y, PathFinder.GetZPosition(spot));
                    if (!TraceLine.TraceLineGo(targetPosition)) {
                        escapePath = PathFinder.FindPath(targetPosition, out foundPath);
                        if (foundPath) break;
                    }
                }

                if (!foundPath) {
                    Logger.LogError("Failed to find a path to any safe spot!");
                    return false;
                }

                if (MovementManager.InMovement) {
                    Vector3 currentTarget = MovementManager.CurrentPath.Last();
                    if (!MovementManager.IsUnStuck &&
                        Areas.Any(tuple => currentTarget.DistanceTo2D(tuple.Item1) < tuple.Item2)) {
                        Logger.LogDebug("Aborting current MoveTo as it leads into AoE!");
                        MovementManager.Go(escapePath);
                    } else {
                        // We might do intense checking on each point of the path to check whether it leads through AoE
                        // but that will just cost a lot of performance
                        return false;
                    }
                } else {
                    Logger.LogDebug("Avoiding AoE.");
                    MovementManager.Go(escapePath);
                }
            }

            return true;
        }

        public static bool Pulse() {
            Update();
            return Avoid();
        }
    }
}