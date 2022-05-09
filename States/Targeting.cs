using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using static WholesomeDungeons.Helper.Helpers;
using wManager;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.Helper;
using System.Drawing;

namespace WholesomeDungeons.States
{
    public class Targeting : State
    {
        public override string DisplayName
        {
            get { return "Targeting"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        private WoWUnit foundTarget;
        private int _priority;
        private Timer timer = new Timer (300);
        private int calculatedStep = 1;

        public override bool NeedToRun
        {
            get
            {
                if (!ObjectManager.Me.IsAlive)
                {
                    return false;
                }

                if (!timer.IsReady)
                {
                    return false;
                }

                if (!Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                {
                    return false;
                }

                WoWUnit foundUnit = FindClosestUnit(unit => (ObjectManager.Me.Position.DistanceTo(unit.Position) <= 25
                    && !Lists.BlackListedMobsGuidInt.Contains(unit.Guid)
                    && unit.IsAlive
                    && unit.Level > 1
                    && unit.Reaction <= Reaction.Neutral
                    && !TraceLine.TraceLineGo(ObjectManager.Me.Position, unit.Position)));

                timer = new Timer(300);

                if (CurrentMovementPath == null)
                {
                    return false;
                }

                if (foundUnit == null)
                {
                    return false;
                }


                //Count the actual Points of the given Path, and if it´s smaller then the variable calculatedStep, set it. This should prevent from using calculatedSteps from old paths.
                if (CurrentMovementPath.Count <= calculatedStep)
                {
                    calculatedStep = CurrentMovementPath.Count -1;
                }

                //match if the calculated step is smaller then currentmovementpath and it´s not only 1 step smaller
                if(calculatedStep < CurrentMovementPath.Count && calculatedStep != (CurrentMovementPath.Count - 1))
                {
                    //check for each point in the actual movementpath, if it´s not more then 25 yards away. If so, add it. So we try to build a not too huge path to check.
                    //We although check if we can see the actual point in our calculation
                    for (int i =0; i < CurrentMovementPath.Count; i++)
                    {
                        if (ObjectManager.Me.Position.DistanceTo(CurrentMovementPath[calculatedStep]) < 25 && !TraceLine.TraceLineGo(CurrentMovementPath[calculatedStep]))
                        {
                            calculatedStep = calculatedStep + 1;
                        }
                    }
                }
                //if we are running we check the Pointdistance to the point max 25yards away from us. There we build a straight line and check all the enemies against it.
                if (IsMovementThreadRunning && PointDistanceToLine(ObjectManager.Me.Position, CurrentMovementPath[calculatedStep], foundUnit.Position) <= 25)
                {
                    Logger.Log("Targetingstate: We found unit to Attack, attacking " + foundTarget.Name);
                    foundTarget = foundUnit;
                    return true;
                }
            
                return false;
            }
        }

        public override void Run()
        {
            Logger.Log($"Found Unit while moving ({foundTarget.Name})");
            StopAllMove();
            ObjectManager.Me.Target = foundTarget.Guid;
            Fight.StartFight(foundTarget.Guid);
            if (foundTarget.IsDead)
            {
                Statistics.Kills++;
            }

        }
    }
}
