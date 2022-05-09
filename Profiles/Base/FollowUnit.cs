using System.Linq;
using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.Profiles.Base
{
    internal class FollowUnit : Step
    {
        private readonly Vector3 _expectedStartPosition;
        private readonly Vector3 _expectedEndPosition;
        private readonly bool _findClosest;
        private readonly bool _skipIfNotFound;
        private readonly int _unitId;

        public FollowUnit(int unitId, 
            Vector3 expectedStartPosition, 
            Vector3 expectedEndPosition, 
            string stepName = "FollowUnit",
            bool findClosest = false, 
            bool strictPosition = false, 
            bool skipIfNotFound = false) : base(stepName)
        {
            _expectedStartPosition = expectedStartPosition;
            _expectedEndPosition = expectedEndPosition;
            _findClosest = findClosest;
            _skipIfNotFound = skipIfNotFound;
            _unitId = unitId;
        }

        public override bool Pulse()
        {
            WoWUnit foundUnit = _findClosest
                ? FindClosestUnit(unit => unit.Entry == _unitId)
                : ObjectManager.GetObjectWoWUnit().FirstOrDefault(unit => unit.Entry == _unitId);

            Vector3 myPosition = ObjectManager.Me.PositionWithoutType;

            if (foundUnit == null)
            {
                if (myPosition.DistanceTo(_expectedEndPosition) >= 15)
                {
                    // Goto expected position
                    StartMoveAlongThread(PathFinder.FindPath(_expectedStartPosition), abortIf: () => IsCompleted);
                }
                else if (_skipIfNotFound || myPosition.DistanceTo(_expectedEndPosition) < 15)
                {
                    Logger.LogDebug($"[Step {Name}]: Skipping unit {_unitId} because he's not here.");
                    IsCompleted = true;
                    return true;
                }
            }
            else
            {
                if(myPosition.DistanceTo(_expectedEndPosition) < 15)
                {
                    Logger.LogDebug($"[Step {Name}]: Skipping Step with {_unitId} because we reached our Enddestination.");
                    IsCompleted = true;
                    return true;
                }
                Vector3 targetPosition = foundUnit.PositionWithoutType;
                float followDistance = 15;

                // Goto found unit
                if (!IsMovementThreadRunning ||
                    CurrentMovementTarget.DistanceTo(targetPosition) > followDistance)
                    StartGoToThread(targetPosition, abortIf: () => ObjectManager.Me.Position.DistanceTo(targetPosition) < followDistance, checkCurrent: false);
            }

            return false;
        }
    }
}