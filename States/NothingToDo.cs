using robotManager.FiniteStateMachine;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using wManager.Wow.Bot.Tasks;
using robotManager.Helpful;
using WholesomeDungeons.Helper;
using System.Linq;

namespace WholesomeDungeons.States
{
    public class NothingToDo : State
    {
        public override string DisplayName
        {
            get { return "NothingToDo"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private Timer timer = new Timer(10000);

        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {

                if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) <= 0)
                    return true;

                return false;

            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            if(timer.IsReady)
            {
                if (Bot.WholesomeDungeons.TownRunActive == true)
                {
                    Bot.WholesomeDungeons.TownRunActive = false;
                    Logger.Log("We set the Bot.WholesomeDungeons.TownRunActive = false");
                }
                timer = new Timer(10000);
            }

            Logger.Log($"Nothing ToDo, we are sleeping now ");
                System.Threading.Thread.Sleep(1000);
        }

    }
}
