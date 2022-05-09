using System.Linq;
using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.Profiles.Base {
    internal class MoveToUnit : Step {
        private readonly Vector3 _expectedPosition;
        private readonly bool _findClosest;
        private readonly bool _skipIfNotFound;
        private readonly int _unitId;
        private readonly bool _interactwithunit;
        private readonly int _gossip;

        public MoveToUnit(int unitId, Vector3 expectedPosition, string stepName = "MoveToUnit",
            bool findClosest = false, bool skipIfNotFound = false, bool interactWithUnit = false, int Gossip = 1) : base(stepName) {
            _expectedPosition = expectedPosition;
            _findClosest = findClosest;
            _skipIfNotFound = skipIfNotFound;
            _unitId = unitId;
            _interactwithunit = interactWithUnit;
            _gossip = Gossip;
        }

        public override bool Pulse()
        {
            WoWUnit foundUnit = _findClosest
                ? FindClosestUnit(unit => unit.Entry == _unitId)
                : ObjectManager.GetObjectWoWUnit().FirstOrDefault(unit => unit.Entry == _unitId);

            Vector3 myPosition = ObjectManager.Me.PositionWithoutType;
            
            if (foundUnit == null) {
                if(myPosition.DistanceTo(_expectedPosition) > 4) {
                    // Goto expected position
                    StartGoToThread(_expectedPosition, abortIf: () => IsCompleted);
                } else if (_skipIfNotFound) {
                    Logger.LogDebug($"[Step {Name}]: Skipping unit {_unitId} because he's not here.");
                    IsCompleted = true;
                    return true;
                }
            } else {
                Vector3 targetPosition = foundUnit.PositionWithoutType;
                float targetInteractDistance = foundUnit.InteractDistance;
                if (myPosition.DistanceTo(targetPosition) < targetInteractDistance) {
                    if (_interactwithunit)
                    {
                        Interact.InteractGameObject(foundUnit.GetBaseAddress);
                        Usefuls.SelectGossipOption(_gossip);
                    }
                    IsCompleted = true;
                    return true;
                }

                // Goto found unit
                if (!IsMovementThreadRunning ||
                    CurrentMovementTarget.DistanceTo(targetPosition) > targetInteractDistance)
                    StartGoToThread(targetPosition, abortIf: () => IsCompleted, checkCurrent: false);
            }

            return false;
        }
    }
}