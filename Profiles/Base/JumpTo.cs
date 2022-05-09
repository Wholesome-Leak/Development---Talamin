using robotManager.Helpful;
using System.Threading;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.Profiles.Base
{
    internal class JumpTo : Step
    {
        private readonly float _precision;
        private readonly float _randomizeEnd;
        private readonly float _randomization;
        private readonly Vector3 _startPosition;
        private readonly Vector3 _landingPosition;
        private readonly float _faceDirection;

        public JumpTo(Vector3 startPosition, float faceDirection, Vector3 landingPosition, string stepName = "JumpTo", float precision = 2f, float randomizeEnd = 0, float randomization = 0) : base(stepName)
        {
            _startPosition = startPosition;
            _faceDirection = faceDirection;
            _landingPosition = landingPosition;
            _precision = precision;
            _randomizeEnd = randomizeEnd;
            _randomization = randomization;
        }

        public override bool Pulse()
        {
            if (ObjectManager.Me.PositionWithoutType.DistanceTo(_landingPosition) < _precision)
            {
                IsCompleted = true;
                return true;
            }

            if(ObjectManager.Me.PositionWithoutType.DistanceTo(_startPosition) < _precision)
            {
                StopCurrentMovementThread();
                if(ObjectManager.Me.Rotation != _faceDirection)
                {
                    ObjectManager.Me.Rotation = _faceDirection;
                    Move.StrafeLeft();
                    Move.StrafeRight();
                }
                Move.Forward(Move.MoveAction.DownKey);
                Move.JumpOrAscend();
                Thread.Sleep(1500);
                Move.Forward(Move.MoveAction.UpKey);
            }

            if (!IsMovementThreadRunning && ObjectManager.Me.PositionWithoutType.DistanceTo(_startPosition) > _precision && ObjectManager.Me.PositionWithoutType.DistanceTo(_landingPosition) > _precision)
            {
                StartGoToThread(_startPosition, abortIf: () => IsCompleted, randomizeEnd: _randomizeEnd, randomization: _randomization);
            }

            return IsCompleted;
        }
    }
}