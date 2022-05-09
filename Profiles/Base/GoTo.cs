using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.Profiles.Base {
    internal class GoTo : Step {
        private readonly float _precision;
        private readonly float _randomizeEnd;
        private readonly float _randomization;
        private readonly Vector3 _targetPosition;

        public GoTo(Vector3 targetPosition, string stepName = "GoTo", float precision = 2f, float randomizeEnd = 0, float randomization = 0) : base(stepName) {
            _targetPosition = targetPosition;
            _precision = precision;
            _randomizeEnd = randomizeEnd;
            _randomization = randomization;
        }

        public override bool Pulse() {
            if (ObjectManager.Me.PositionWithoutType.DistanceTo(_targetPosition) < _precision) {
                IsCompleted = true;
                return true;
            }

            if (!IsMovementThreadRunning || CurrentMovementTarget.DistanceTo(_targetPosition) > _precision) {
                StartGoToThread(_targetPosition, abortIf: () => IsCompleted, randomizeEnd: _randomizeEnd, randomization: _randomization);
            }

            return IsCompleted;
        }
    }
}