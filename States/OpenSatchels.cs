using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholesomeDungeons.Bot;
using WholesomeDungeons.Helper;
using wManager;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.States
{
    public class OpenSatchels : State
    {
        private Timer timer = new Timer(250);
        public override string DisplayName
        {
            get { return "OpenSatchels"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        private WoWUnit leader = Bot.WholesomeDungeons.Tankunit;
        private int _priority;

        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {

                if (!timer.IsReady)
                    return false;


                timer = new Timer(4000);

                if(Bag.GetBagItem().Count(item => item.Name.Contains("Satchel of")) > 0 )
                return true;

                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            WoWItem item  = Bag.GetBagItem().FirstOrDefault(x => x.Name.Contains("Satchel of"));
            if(item != null)
            {
                Logger.Log($"Found Satchel {item}");
                ItemsManager.UseItem(item.Name);
            }
        }
    }
}
