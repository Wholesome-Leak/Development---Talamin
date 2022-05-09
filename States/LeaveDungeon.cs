using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.States
{
    public class LeaveDungeon : State
    {
        public override string DisplayName
        {
            get { return "Leave Dungeon"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;


        public bool SatchelInBag;
        public string LFGStatus;
        public static bool LastBossKilled = false;



        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {

                if (!ObjectManager.Me.IsInGroup && Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0)
                {
                    Logger.Log("Group Left, looks like i am still in Dungeon, so i have to leave");
                    return true;
                }


                if(Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0 && ObjectManager.Me.IsInGroup && Party.GetPartyNumberPlayers() < 5)
                {
                    Logger.Log("One Member left the Group, expecting the  Dungeon to be finished, so we leave too");
                    return true;
                }

                if (LastBossKilled)
                    return true;

                return false;
                //return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            Logger.Log("LastBossKilled = " + LastBossKilled);
            if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0)
            {
             
                Logger.LogDebug("Leave Dungeon in 20 seconds...");
                Thread.Sleep(5000);
                Logger.LogDebug("Leave Dungeon in 15 seconds...");
                Thread.Sleep(5000);
                Logger.LogDebug("Leave Dungeon in 10 seconds...");
                Thread.Sleep(5000);
                Logger.LogDebug("Leave Dungeon in 5 seconds...");
                Thread.Sleep(5000);
                Helpers.LUAInstancePortOutOfDungeon();
                Thread.Sleep(5000);
                Logger.Log($"Resetting _DungeonLogicRunstate to {DungeonLogic._DungeonLogicRunstate} ");
                DungeonLogic._DungeonLogicRunstate = true;
                if (LastBossKilled == true)
                {
                    LastBossKilled = false;
                    Bot.WholesomeDungeonsSettings.CurrentSetting.LastBossKilled = false;
                }
            }
            if(ObjectManager.Me.IsInGroup && Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) <= 0)
            {
                Helpers.LUALeaveParty();
                if (LastBossKilled == true)
                {
                    LastBossKilled = false;
                    Bot.WholesomeDungeonsSettings.CurrentSetting.LastBossKilled = false;
                }
                Thread.Sleep(3000);
            }
            if (!ObjectManager.Me.IsInGroup)
            {
                if (LastBossKilled == true)
                {
                    LastBossKilled = false;
                    Bot.WholesomeDungeonsSettings.CurrentSetting.LastBossKilled = false;
                }
            }

        }
    }
}
