using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using static WholesomeDungeons.Helper.Helpers;
using wManager;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.Helper;
using System.Drawing;
using System.Collections.Generic;

namespace WholesomeDungeons.States
{
    public class TargetingNEW : State
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
        private Timer timer = new Timer(300);
        private int calculatedStep = 0;

        public override bool NeedToRun
        {
            get
            {
                if (!Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                {
                    return false;
                }

                if (!ObjectManager.Me.IsAlive)
                {
                    return false;
                }

                if(ObjectManager.Me.InCombat)
                {
                    return false;
                }

                if (!timer.IsReady)
                {
                    return false;
                }

                if (CurrentMovementPath == null)
                {
                    return false;
                }

                WoWUnit foundUnit = FindClosestUnit(unit => (ObjectManager.Me.Position.DistanceTo(unit.Position) <= 25
                    && !Lists.BlackListedMobsGuidInt.Contains(unit.Guid)
                    && unit.IsAlive
                    && unit.Level > 1
                    && unit.Reaction <= Reaction.Neutral
                    && !TraceLine.TraceLineGo(ObjectManager.Me.Position, unit.Position)));



                if (foundUnit == null)
                {
                    return false;
                }

                timer = new Timer(300);
                //First we need to know how many Vectors we have in our CurrentMovementPath
                for(int i = 0; i < CurrentMovementPath.Count; i++)
                {
                    //check if the actual Points are in Range of 25xards
                    if(ObjectManager.Me.Position.DistanceTo(CurrentMovementPath[i]) < 25)
                    {
                        //check for LOS to the Points
                        if(!TraceLine.TraceLineGo(CurrentMovementPath[i]))
                        {
                            calculatedStep = calculatedStep + 1;
                        }
                    }
                }
                Logger.Log("Targeting: CalculatedStep, we have " + calculatedStep + " Points to check");
                Logger.Log("Targeting: Total CurrentMovementPathcount: " + CurrentMovementPath.Count);

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
            Logger.Log($"Targeting: Found Unit while moving ({foundTarget.Name})");
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
