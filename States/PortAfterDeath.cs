using robotManager.FiniteStateMachine;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using wManager.Wow.Bot.Tasks;
using robotManager.Helpful;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Bot;
using System.Linq;
using wManager.Wow.Enums;
using System.Collections.Generic;

namespace WholesomeDungeons.States
{
    public class PortAfterDeath : State
    {
        public override string DisplayName
        {
            get { return "PortAfterDeath"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;


        public override bool NeedToRun
        {
            get
            {
                if (Party.GetPartyHomeAndInstance().Any(m => m.HaveBuff(8326)) && !ObjectManager.Me.InCombat)
                {
                    if (ObjectManager.Me.Position.DistanceTo(Resurrection.EntranceVector) >= 100 && Helpers.MeInsideDungeon())  //when inside Dungeon
                    {
                        return true;
                    }
                    if (!Helpers.MeInsideDungeon()) //when outside Dungeon
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            if (Helpers.MeInsideDungeon() && !Usefuls.IsLoadingOrConnecting)
            {
                Helpers.LUAInstancePortOutOfDungeon();
                Logger.Log("");
            }

            if (!Helpers.MeInsideDungeon() && !Usefuls.IsLoadingOrConnecting)
            {
                Helpers.LUAInstancePortToDungeon();
                Logger.Log("");
            }
        }
    }
}
