using robotManager.FiniteStateMachine;
using System.Collections.Generic;
using wManager.Wow.Bot.Tasks;
using wManager.Wow.ObjectManager;
using Timer = robotManager.Helpful.Timer;
using WholesomeDungeons.Helper;
using static WholesomeDungeons.Helper.Helpers;
using WholesomeDungeons.Bot;
using wManager.Wow.Helpers;

namespace WholesomeDungeons.States
{
    public class Loot : State
    {
        public override string DisplayName
        {
            get { return "Looting Corpse"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public static int LootRange = 20;
        public static Timer ResetLootBlacklist = new Timer();
        public WoWUnit LootUnit;
        private Timer timer = new Timer(250);

        public override bool NeedToRun
        {
        get
            {
                if (!timer.IsReady || Bag.GetContainerNumFreeSlots <= 2)
                    return false;

                WoWUnit lootUnit = FindClosestUnit(unit => unit.IsLootable 
                    && !Blacklist.LootBlacklist.Contains(unit)
                    && unit.IsDead 
                    && unit.IsValid
                    && CalculatePathTotalDistance(ObjectManager.Me.Position, unit.Position) <= LootRange);

                timer = new Timer(500);

                if (lootUnit == null)
                    return false;

                if (!WholesomeDungeonsSettings.CurrentSetting.SetLootBoss && Lists.BossListInt.Contains(lootUnit.Entry)
                    || !WholesomeDungeonsSettings.CurrentSetting.SetLootAll && !Lists.BossListInt.Contains(lootUnit.Entry))
                {
                    Blacklist.LootBlacklist.Add(LootUnit);
                    return false;
                }

                if (ResetLootBlacklist.IsReady)
                    Blacklist.LootBlacklist.Clear();

                LootUnit = lootUnit;
                return true;
            }
        }

        public override void Run()
        {
            StopAllMove();
            LootingTask.Pulse(new List<WoWUnit>() { LootUnit });
            Lua.LuaDoString("CloseLoot()");
            Blacklist.LootBlacklist.Add(LootUnit);
            ResetLootBlacklist = new Timer(1000 * 10);
        }
    }
}