using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.Profiles.Base
{
    internal class DefendSpot : Step
    {
        private readonly float _precision;
        private readonly float _randomizeEnd;
        private readonly float _randomization;
        private readonly Vector3 _targetPosition;
        private readonly int _timer;
        private Timer stepTimer = new Timer();

        public DefendSpot(Vector3 targetPosition, int timer, string stepName = "Defend Spot", float precision = 2f, float randomizeEnd = 0, float randomization = 0) : base(stepName)
        {
            _targetPosition = targetPosition;
            _timer = timer;
            _precision = precision;
            _randomizeEnd = randomizeEnd;
            _randomization = randomization;
            stepTimer = new Timer(timer);
        }


        public override bool Pulse()
        {
            if (ObjectManager.Me.PositionWithoutType.DistanceTo(_targetPosition) < _precision && stepTimer.IsReady)
            {
                IsCompleted = true;
                return true;
            }

            if (!IsMovementThreadRunning || CurrentMovementTarget.DistanceTo(_targetPosition) > _precision)
            {
                StartGoToThread(_targetPosition, abortIf: () => IsCompleted, randomizeEnd: _randomizeEnd, randomization: _randomization);
            }

            return IsCompleted;
        }
    }
}