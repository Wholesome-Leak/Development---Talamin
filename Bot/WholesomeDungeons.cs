using System;
using System.Linq;
using System.Threading;
using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using WholesomeDungeons.Helper;
using wManager;
using wManager.Wow.Bot.States;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using System.Collections.Generic;
using WholesomeDungeons.Connection;
using robotManager.Events;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace WholesomeDungeons.Bot {
    public class WholesomeDungeons {
        public static bool InDungeon;
        public static string DungeonName;
        public static Vector3 Location;
        public static string Tankname = "";
        public static WoWUnit Tankunit = null;
        private static Engine _fsm = new Engine();
        public static string DisplayName;
        private static readonly Engine Fsm = new Engine();
        public static bool TownRunActive = false;

        public static bool InitialSetup() {
            try {
                _fsm = new Engine();
                TownRunActive = false;
                EventsLuaWithArgs.OnEventsLuaStringWithArgs += Eventhandler.HandleLuaEvents;
                EventsLua.AttachEventLua("PLAYER_ENTERING_WORLD", m=> States.DungeonLogic.OnMapChanged());
                PathFinder.OffMeshConnections.AddRange(BundledOffmesh.offMeshes);
                FiniteStateMachineEvents.OnRunState += StateChecker;
                FiniteStateMachineEvents.OnBeforeCheckIfNeedToRunState += RunStateBeforeChecker;
                wManager.Events.FightEvents.OnFightLoop += DefendAll.Targetswitcher;
                //EventsLuaWithArgs.OnEventsLuaStringWithArgs += Eventhandler.HandeLos;

                // Update spell list
                SpellManager.UpdateSpellBook();

                // Load CC:
                CustomClass.LoadCustomClass();

                // Disable Teleport Security Settings
                wManagerSetting.CurrentSetting.CloseIfPlayerTeleported = false;

                // FSM
                _fsm.States.Clear();
                _fsm.AddState(new Relogger {Priority = 500});
                _fsm.AddState(new Pause {Priority = 400});
                //***OWN STATES
                //--------------------------------------------------------
                _fsm.AddState(new LeaveDungeon { Priority = 26 });
                _fsm.AddState(new Trainers { Priority = 25 });
                _fsm.AddState(new ToTown { Priority = 24 });
                _fsm.AddState(new Invite { Priority = 23 });
                _fsm.AddState(new Queue { Priority = 22 });
                _fsm.AddState(new Resurrection { Priority = 21 });
                _fsm.AddState(new DefendAll { Priority = 20 });
                _fsm.AddState(new Regeneration { Priority = 19 });
                _fsm.AddState(new Wait { Priority = 18 });
                _fsm.AddState(new Rest { Priority = 17 });
                _fsm.AddState(new ReviveParty { Priority = 16 });
                _fsm.AddState(new TargetingNEW { Priority = 15 });
                _fsm.AddState(new Loot { Priority = 14 });
                _fsm.AddState(new States.DungeonLogic { Priority = 13 });
                _fsm.AddState(new FollowNEW { Priority = 12 });
                _fsm.AddState(new OpenSatchels { Priority = 11 });
                _fsm.AddState(new NothingToDo { Priority = 10 });

                //***END OF OWN STATES
                //_fsm.AddState(new BattlegrounderCombination { Priority = 15 });
                //_fsm.AddState(new SelectProfileState {Priority = 14});
                //_fsm.AddState(new Resurrect {Priority = 13});
                //_fsm.AddState(new MyMacro { Priority = 12 });
                //_fsm.AddState(new wManager.Wow.Bot.States.IsAttacked { Priority = 13 });
                //_fsm.AddState(new BattlePetState { Priority = 11 });
                //_fsm.AddState(new Looting { Priority = 9 });

                //_fsm.AddState(new Farming {Priority = 8});
                //_fsm.AddState(new MillingState {Priority = 7});
                //_fsm.AddState(new ProspectingState { Priority = 6 });
                //_fsm.AddState(new FlightMasterTakeTaxiState { Priority = 6 });
                //_fsm.AddState(new ToTown {Priority = 4});
                //_fsm.AddState(new FlightMasterDiscoverState { Priority = 3 });
                //_fsm.AddState(new Talents {Priority = 3});
                //_fsm.AddState(new Trainers {Priority = 3});
                //_fsm.AddState(_grinding);
                //_fsm.AddState(_movementLoop);


                _fsm.States.Sort();
                _fsm.StartEngine(15, "WholesomeDungeons");

                StopBotIf.LaunchNewThread();

                if (WholesomeDungeonsSettings.CurrentSetting.SetAsTank && WholesomeDungeonsSettings.CurrentSetting.ServerClient)
                    Server.Main();

                if (!WholesomeDungeonsSettings.CurrentSetting.SetAsTank && WholesomeDungeonsSettings.CurrentSetting.ServerClient)
                    Client.Main();

                // NpcScan:
                if (wManagerSetting.CurrentSetting.NpcScanRepair)
                    NPCScanState.LaunchNewThread();

                States.DungeonLogic._DungeonLogicRunstate = true;


                return true;
            } catch (Exception e) {
                try {
                    Dispose();
                } catch {
                    // ignored
                }

                Logger.LogError("WholesomeDungeons > Bot > Bot  > Pulse(): " + e);
                return false;
            }
        }

        public static void Dispose() {
            try {
                WholesomeDungeonsSettings.CurrentSetting.BotStopped = true;
                WholesomeDungeonsSettings.CurrentSetting.Save();
                FiniteStateMachineEvents.OnRunState -= StateChecker;
                FiniteStateMachineEvents.OnBeforeCheckIfNeedToRunState -= RunStateBeforeChecker;
                wManager.Events.FightEvents.OnFightLoop -= DefendAll.Targetswitcher;
                EventsLuaWithArgs.OnEventsLuaStringWithArgs -= Eventhandler.HandleLuaEvents;
                CustomClass.DisposeCustomClass();
                _fsm.StopEngine();
                Fight.StopFight();
                MovementManager.StopMove();
            } catch (Exception e) {
                Logger.LogError("WholesomeDungeons > Bot > Bot  > Dispose(): " + e);
            }
        }

        private static void StateChecker(Engine engine,  State state, CancelEventArgs canc)
        {
            //don´t accept Queue to Dungeon while doing Townrun
            if(state != null && (state.DisplayName == "WV Buying Ammunition" 
                || state.DisplayName == "WV Buying Drink"
                || state.DisplayName == "WV Buying Food"
                || state.DisplayName == "WV Repair and Sell"
                || state.DisplayName == "WV Training"
                || state.DisplayName == "WV Buying Poison"
                || state.DisplayName == "WV Buying Bags"))
                {
                    TownRunActive = true;
                }
            //break Regeneration when group is attacked
            if (state != null && state is Regeneration && Helpers.FindClosestUnit(u => u.IsTargetingPartyMember && u.Reaction <= Reaction.Neutral && u.IsAlive) != null)
                canc.Cancel = true;
        }

        private static void RunStateBeforeChecker (Engine engine, State state, CancelEventArgs canc)
        {
            try
            {
                if (state is States.DungeonLogic && States.DungeonLogic._DungeonLogicRunstate == false)
                    canc.Cancel = true;

            }
            catch 
            { }
        }
    }
}