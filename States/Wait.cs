using robotManager.FiniteStateMachine;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using wManager.Wow.Bot.Tasks;
using robotManager.Helpful;
using WholesomeDungeons.Helper;
using System.Linq;

namespace WholesomeDungeons.States
{
    public class Wait : State
    {
        public override string DisplayName
        {
            get { return "Wait"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private Timer timer = new Timer(250);

        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {
                if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) <= 0)
                    return false;

                if (ObjectManager.Me.InCombat || !timer.IsReady)
                    return false;

                if (!Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                    return false;

                if (Helpers.GroupmembesOutOfRange())
                    return true;

                if (Helpers.GroupmembersHealAlive() && Helpers.GroupmemberisDead() || Helpers.GroupmemberisDead())
                    return true;

                timer = new Timer(500);
                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            Logger.Log($"Waiting = Groupmember is Out of Range {Helpers.GroupmembesOutOfRange()}");
            Helpers.StopCurrentMovementThread();
            MovementManager.StopMoveTo();
            MovementManager.StopMove();
            System.Threading.Thread.Sleep(2000);
        }

    }
}