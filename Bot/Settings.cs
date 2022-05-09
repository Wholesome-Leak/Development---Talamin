using System;
using System.Collections.Generic;
using System.IO;
using robotManager.Helpful;
using WholesomeDungeons.Helper;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Bot {
    [Serializable]
    public class WholesomeDungeonsSettings : Settings {
        public bool SetAsTank { get; set; }
        public bool SetAsHeal { get; set; }
        public bool SetAsRDPS { get; set; }
        public bool SetAsMDPS { get; set; }
        public bool SetLootBoss { get; set; }
        public bool SetLootAll { get; set; }
        public List<string> GroupMembers { get; set; }
        public string TankName { get; set; }
        public int FollowRangeRDPS { get; set; }
        public int FollowRangeMDPS { get; set; }
        public int FollowRangeHeal { get; set; }
        public bool ServerClient { get; set; }
        public List<Vector3> SavedDeathrun { get; set; }
        public Vector3 SavedEntrance { get; set; }
        public int SavedMapId { get; set; }

        public bool LastBossKilled { get; set; }
        public bool BotStopped { get; set; }
        //auto Sell, buy ...
        public bool AutobuyFood { get; set; }
        public bool AutoBuyWater { get; set; }
        public bool AllowAutobuyPoison  { get; set; }
        public bool AutobuyAmmunition { get; set; }
        public int AutobuyAmmunitionAmount { get; set; }
        public bool AutoRepair { get; set; }
        public bool AllowAutoSell { get; set; }
        public bool AutoTrain { get; set; }
        public double LastUpdateDate  { get; set; }
        public int LastLevelTrained { get; set; }
        public List<int> TrainLevels { get; set; }
        public List<VendorItem> VendorItems { get; set; }
        public bool Smoothmove { get; set; }

        public struct VendorItem
        {
            public string Name;
            public int Stack;
            public int Price;

            public VendorItem(string name, int stack, int price)
            {
                Price = price;
                Name = name;
                Stack = stack;
            }
        }

        public static WholesomeDungeonsSettings CurrentSetting { get; set; }

        public WholesomeDungeonsSettings()
        {
            ServerClient = false;
            SetAsTank = false;
            SetAsHeal = false;
            SetAsRDPS = false;
            SetAsMDPS = false;
            SetLootAll = true;
            SetLootBoss = true;
            FollowRangeHeal = 30;
            FollowRangeMDPS = 15;
            FollowRangeRDPS = 25;
            SavedMapId = 0;
            SavedEntrance = new Vector3();
            SavedDeathrun = new List<Vector3>();
            GroupMembers = new List<string>();
            TankName = "";
            LastBossKilled = false;
            BotStopped = false;
            //Sell/Buy/Repair Settings
            AutobuyFood = true;
            AutoBuyWater = true;
            AllowAutobuyPoison = true;
            AutobuyAmmunition = true;
            AutobuyAmmunitionAmount = 2000;
            AutoRepair = true;
            AllowAutoSell = true;
            AutoTrain = true;
            LastUpdateDate = 0;
            LastLevelTrained = (int)ObjectManager.Me.Level;

            TrainLevels = new List<int> { };

            VendorItems = new List<VendorItem>();

            Smoothmove = false;
        }

        public bool Save() {
            try {
                return Save(
                    AdviserFilePathAndName("WholesomeDungeonsSettings", ObjectManager.Me.Name + "." + Usefuls.RealmName));
            } catch (Exception e) {
                Logger.LogError("WholesomeDungeonsSettings > Save(): " + e);
                return false;
            }
        }

        public static bool Load() {
            try {
                if (File.Exists(AdviserFilePathAndName("WholesomeDungeonsSettings",
                    ObjectManager.Me.Name + "." + Usefuls.RealmName))) {
                    CurrentSetting =
                        Load<WholesomeDungeonsSettings>(AdviserFilePathAndName("WholesomeDungeonsSettings",
                            ObjectManager.Me.Name + "." + Usefuls.RealmName));
                    return true;
                }

                CurrentSetting = new WholesomeDungeonsSettings();
            } catch (Exception e) {
                Logger.LogError("WholesomeDungeonsSettings > Load(): " + e);
            }

            return false;
        }
    }
}