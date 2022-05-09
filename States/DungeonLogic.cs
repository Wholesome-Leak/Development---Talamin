using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.States {
    public class DungeonLogic : State {
        public override string DisplayName => LogicRunner.CurrentState;
        public bool Runstate { get; private set; }
        public static bool _DungeonLogicRunstate = true;

        public override bool NeedToRun
        {
            get 
            {
                if (!LogicRunner.IsFinished //check for finishing the Logicstate before
                && Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0 //check if we are inside Dungeon
                && !LogicRunner.OverrideNeedToRun //check if there is no needed Override
                && Party.GetPartyHomeAndInstance().All(p => p.IsAlive)) //check if all Partymembers in the ObjectManager are alive
                    return true;


                return false;
            }
        }
        public static void OnMapChanged()
        { //executed on event OnMapChanged
            {
                if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 1)
                {
                    //if the Dungeon  Map ID is available more then one time (like the Scarlet Dungeons, determine the closest entry
                     var actualDungeon = Lists.AllDungeons.Where(d => d.MapId == Usefuls.ContinentId).OrderBy(o => o.Start.DistanceTo(ObjectManager.Me.Position)).FirstOrDefault();
                    Logger.Log($"[DungeonLogic] Starting {actualDungeon.profile.Name}.");
                    //set Entrancevector for Deathrun from the actua Dungeon
                    Resurrection.EntranceVector = actualDungeon.EntranceLoc;
                    //set Resurrection Map Id to the actual Dungeon  Map id
                    Resurrection.MapId = actualDungeon.MapId;
                    //save the actual  Dungeon so we can start and stop the bot
                    Bot.WholesomeDungeonsSettings.CurrentSetting.SavedMapId = actualDungeon.MapId;
                    Bot.WholesomeDungeonsSettings.CurrentSetting.Save();
                    Logger.Log($"[DungeonLogic] Set {actualDungeon.EntranceLoc} to ResurrectionPoint for MapId {actualDungeon.MapId}.");
                    //load the actual  Dungeon Profile
                    Helpers.GroupCheck();
                    actualDungeon.profile.Load();
                }
                if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) == 1)
                {
                    //if we have only one dungeon  Entry, choose the correct Dungeon out of the List
                    var actualDungeon = Lists.AllDungeons.Where(d => d.MapId == Usefuls.ContinentId).FirstOrDefault();
                    Logger.Log($"[DungeonLogic] Starting {actualDungeon.profile.Name}.");
                    //set Entrancevector for Deathrun from the actua Dungeon
                    Resurrection.EntranceVector = actualDungeon.EntranceLoc;
                    //set Resurrection Map Id to the actual Dungeon  Map id
                    Resurrection.MapId = actualDungeon.MapId;
                    //save the actual  Dungeon so we can start and stop the bot
                    Bot.WholesomeDungeonsSettings.CurrentSetting.SavedMapId = actualDungeon.MapId;
                    Bot.WholesomeDungeonsSettings.CurrentSetting.Save();
                    Logger.Log($"[DungeonLogic] Set {actualDungeon.EntranceLoc} to ResurrectionPoint for MapId {actualDungeon.MapId}.");
                    //load the actual  Dungeon Profile
                    Helpers.GroupCheck();
                    actualDungeon.profile.Load();
                }

                if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) <= 0)
                {
                    Logger.Log("Don't know this zone. Doing nothing.");
                }

            }
        }

        public override void Run()
        {
            LogicRunner.Pulse();
        }
    }
}