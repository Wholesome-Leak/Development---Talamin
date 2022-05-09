using robotManager.FiniteStateMachine;
using static WholesomeDungeons.Helper.Helpers;
using wManager;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.Helper;
using robotManager.Helpful;


namespace WholesomeDungeons.States
{
    public class PrioTarget : State
    {
        public override string DisplayName
        {
            get { return "Found Prio Target"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private WoWUnit foundTarget;
        private Timer timer = new Timer(250);

        public override bool NeedToRun
        {
            get
            {
                if (!timer.IsReady)
                    return false;

                if (Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsHeal)
                    return false;

                WoWUnit foundUnit = FindClosestUnit(unit => unit.Reaction <= Reaction.Neutral
                    && Lists.PriorityTargetListInt.Contains(unit.Entry)
                    && ObjectManager.Me.Position.DistanceTo(unit.Position) <= 40
                    && unit.IsTargetingMeOrMyPetOrPartyMember 
                    && unit.IsAlive);

                timer = new Timer(500);

                if (foundUnit == null)
                    return false;

                foundTarget = foundUnit;
                return true;
            }
        }

        public override void Run()
        {
            Logger.Log($"Found Prio Target {foundTarget.Name}, switching over");
            StopAllMove(); 
            ObjectManager.Me.Target = foundTarget.Guid;
            Fight.StartFight(foundTarget.Guid);
        }
    }
}
