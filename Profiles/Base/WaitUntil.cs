using System;
using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using wManager.Wow.Enums;
using static WholesomeDungeons.Helper.Helpers;
using WholesomeDungeons.Helper;

namespace WholesomeDungeons.Profiles.Base
{
    internal class WaitUntil : Step
    {
        private readonly int _objectId;
        private readonly Vector3 _expectedPosition;
        private readonly Func<WoWGameObject, bool> _isCompleted;
        public WaitUntil(Vector3 expectedPosition, int ObjectId, Func<WoWGameObject, bool> isCompleted, string stepName = "InteractWith") : base(stepName)
        {
            _expectedPosition = expectedPosition;
            _isCompleted = isCompleted;
            _objectId = ObjectId;
        }

        public override bool Pulse()
        {
            WoWGameObject foundObject = ObjectManager.GetObjectWoWGameObject().FirstOrDefault(o => o.Entry == _objectId);
            Vector3 myPosition = ObjectManager.Me.PositionWithoutType;
            if (myPosition.DistanceTo(_expectedPosition) > 4)
            {
                // Go to expected position
                StartGoToThread(_expectedPosition, abortIf: () => IsCompleted);
            } // Else we just wait

            else
            {
                // Goto or interact with object
                if (_isCompleted(foundObject))
                {
                    IsCompleted = true;
                    return true;
                }
            }

            return IsCompleted;
        }
    }
}