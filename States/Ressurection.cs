using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Bot;
using WholesomeDungeons.Helper;
using wManager;
using wManager.Wow.Bot.Tasks;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.States
{
    public class Resurrection : State
    {
        public override string DisplayName
        {
            get { return "Resurrection"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public static Vector3 EntranceVector;
        public static int MapId;
        public static bool ResetStepsBecauseRes;
        public static List<Vector3> Deathrun = new List<Vector3>();

        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {
                if (Helpers.GroupmembersHealAlive())
                    return false;

                if (Helpers.IsMovementThreadRunning)
                        return false;

                if (ObjectManager.Me.HaveBuff(6203) && Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0)
                {
                    Thread.Sleep(3500); // Let our killers reset.
                    Lua.RunMacroText("/click StaticPopup1Button2");
                    Thread.Sleep(1000);
                    return false;
                }

                if (ObjectManager.Me.IsDead && Conditions.InGameAndConnected)
                    return true;

                return false;
            }
        }

        public static bool HealAround()
        {
            return true;
        }

        // If NeedToRun() == true
        public override void Run()
        {
            #region Soulstone

            #endregion
            Thread.Sleep(50);
            Lua.LuaDoString("RepopMe()");
            ResetStepsBecauseRes = true;
            Thread.Sleep(4000);
            //Helpers.StopAllMove();

            if(Bot.WholesomeDungeonsSettings.CurrentSetting.SavedMapId != Usefuls.ContinentId)
            {

                if(Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun.Count() <= 0)
                {
                    Logger.Log("No Deathroute available, trying Pathfinder");
                    Helpers.StartMoveAlongThread(PathFinder.FindPath(EntranceVector, false), abortIf: IamInside);
                }
                if(Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun.Count()>0)
                {
                    Logger.Log("Deathroute available, trying Deathroute");
                    Helpers.StartMoveAlongThread(WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun, abortIf: IamInside);
                }

            }
            Statistics.Deaths++;
        }


    public bool IamInside()
        {
            return WholesomeDungeonsSettings.CurrentSetting.SavedMapId == Usefuls.ContinentId;
        }
    }
}
