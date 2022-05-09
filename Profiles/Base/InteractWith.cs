using System;
using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;
using WholesomeDungeons.Helper;
using System.Threading;

namespace WholesomeDungeons.Profiles.Base
{
    internal class InteractWith : Step
    {
        private readonly int _objectId;
        private readonly Vector3 _expectedPosition;
        private readonly Func<WoWGameObject, bool> _interactionComplete;
        private readonly bool _findClosest;
        private readonly bool _strictPosition;
        private readonly float _interactDistance;

        public InteractWith(int objectId, 
            Vector3 expectedPosition, 
            Func<WoWGameObject, bool> isCompleted, 
            string stepName = "InteractWith", 
            bool findClosest = false, 
            bool strictPosition = false,
            float interactDistance = 4f) : base(stepName)
        {
            _objectId = objectId;
            _expectedPosition = expectedPosition;
            _interactionComplete = isCompleted;
            _findClosest = findClosest;
            _strictPosition = strictPosition;
            _interactDistance = interactDistance;
        }

        public override bool Pulse()
        {
            // Closest object from me or from its supposed position?
            Vector3 referencePosition = _strictPosition ? _expectedPosition : ObjectManager.Me.Position;

            WoWGameObject foundObject = _findClosest || _strictPosition
                ? FindClosestObject(gameObject => gameObject.Entry == _objectId, referencePosition)
                : ObjectManager.GetObjectWoWGameObject().FirstOrDefault(o => o.Entry == _objectId);


            // Move close to expected object position
            if (!IsMovementThreadRunning && ObjectManager.Me.PositionWithoutType.DistanceTo(_expectedPosition) > 20 + _interactDistance)
            {
                Logger.Log($"Moving to object {_objectId} at {_expectedPosition}");
                StartGoToThread(_expectedPosition, abortIf: () => ObjectManager.Me.PositionWithoutType.DistanceTo(foundObject.Position) <= 20 + _interactDistance);
                return IsCompleted = false;
            }

            // Is it present?
            if (foundObject == null)
            {
                Logger.Log($"Expected interactive object {_objectId} but it's absent");
                Thread.Sleep(2000);
                return IsCompleted = false;
            }

            // Move to real object position
            if (!IsMovementThreadRunning && ObjectManager.Me.PositionWithoutType.DistanceTo(foundObject.Position) > _interactDistance)
            {
                Logger.Log($"Interactive object found. Approaching {_objectId} at {foundObject.Position}");
                StartGoToThread(foundObject.Position, checkCurrent: false, abortIf: () => ObjectManager.Me.PositionWithoutType.DistanceTo(foundObject.Position) <= _interactDistance);
                return IsCompleted = false;
            }

            // Interact with object
            if (!IsMovementThreadRunning)
            {
                Interact.InteractGameObject(foundObject.GetBaseAddress);
                Usefuls.WaitIsCasting();
            }

            if (_interactionComplete(foundObject))
            {
                Logger.Log($"Interaction with object {foundObject.Entry} is complete");
                return IsCompleted = true;
            }

            return IsCompleted;
        }
    }
}