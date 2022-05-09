using robotManager.FiniteStateMachine;
using System.Linq;
using System.Threading;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using robotManager.Helpful;
using Timer = robotManager.Helpful.Timer;

namespace WholesomeDungeons.States
{
    public class Queue : State
    {
        public override string DisplayName
        {
            get { return "Queue Dungeon"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public static string LFGStatus;
        public static bool Queued = false;
        private Timer timer = new Timer(250);



        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {
                if (!timer.IsReady)
                    return false;

                if (!Conditions.InGameAndConnectedAndAliveAndProductStartedNotInPause)
                    return false;

                if (!ObjectManager.Me.IsPartyLeader)
                    return false;

                if (Party.GetPartyNumberPlayers() < 5)
                    return false;

                if(Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0)
                    return false;

                if (Helpers.LUAGetLFGMode() == "nil" && !ObjectManager.Me.HaveBuff("Dungeon Deserter"))              
                    return true;
                    
                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            Logger.Log("Queueing for Dungeons");

            Logger.Log("Set Dungeon Finished to false");
            if (LeaveDungeon.LastBossKilled == true)
            {
                LeaveDungeon.LastBossKilled = false;
                Bot.WholesomeDungeonsSettings.CurrentSetting.LastBossKilled = false;
                DungeonLogic._DungeonLogicRunstate = true;
            }


            if (!Lua.LuaDoString<bool>("return LFDQueueFrame: IsVisible()"))
                Lua.RunMacroText("/lfd");

            if (Lua.LuaDoString<bool>("return LFDQueueFrame: IsVisible()") && !Lua.LuaDoString<bool>("return LFDQueueFrameRandom: IsVisible()"))
                Lua.LuaDoString("LFDQueueFrameTypeDropDownButton:Click(); DropDownList1Button2:Click()");

            if (Lua.LuaDoString<bool>("return LFDQueueFrame: IsVisible()") && Lua.LuaDoString<bool>("return LFDQueueFrameRandom: IsVisible()"))
                Lua.LuaDoString("LFDQueueFrameFindGroupButton:Click()");

            timer = new Timer(1000);

        }
    }
}
