using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.Scholomance
{
    internal class Scholomance : Profile
    {
        private readonly Data _data;

        public Scholomance(string profileName = "Scholomance") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.GoToDoor1, "Take path to First Door"),
                new InteractWith(175611, new Vector3(201.211, 58.2171, 128.794, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175611).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate 1"),
                new MoveAlongPath(_data.GoToDoor2, "Take path to Second Door"),
                new InteractWith(175612, new Vector3(247.466, 37.5645, 115.725, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175612).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate 2"),
                new MoveAlongPath(_data.GoToDoor3, "Take path to Third Door"),
                new InteractWith(175613, new Vector3(261.929, 64.9468, 110.028, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175613).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate 3"),
                new MoveAlongPath(_data.GoToDoor4, "Take path to Fourth Door"),
                new InteractWith(175614, new Vector3(260.844, 137.045, 110.028, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175614).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate 4"),
                new  MoveAlongPath(_data.PathToFirstBoss, "Take Path to First Boss"),
                new MoveToUnit(11622, new Vector3(137.145, 171.676, 95.88313, "None"), "Approaching Rattlegore", skipIfNotFound: true),
                new MoveAlongPath(_data.GoToDoor5, "Take path to Fifth Door"),
                new InteractWith(175615, new Vector3(199.721, 125.504, 110.028, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175615).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate 5"),
                new MoveAlongPath(_data.GoToTheDoors, "Take path to the Doors"),
                new InteractWith(175167, new Vector3(179.6389, 75.28013, 104.716, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175167).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with the  Doors to Vieweing Room"),
                new MoveAlongPath(_data.PathToSecondBoss, "Take Path to Second Boss"),
                new InteractWith(175617, new Vector3(93.49361, 99.89328, 97.61217, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175617).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate"),
                new MoveAlongPath(_data.PathToSecondBossPart2, "Take Path to Second Boss"),
                new MoveToUnit(10508, new Vector3(-42.2048, 141.339, 83.93137, "None"), "Approaching Ras Frostwhisper", skipIfNotFound: true),
                new MoveAlongPath(_data.GoToDoor6, "Take path to Sixth Door"),
                new InteractWith(175618, new Vector3(135.8117, 61.80007, 102.7628, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(175618).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gate 4"),
                new MoveAlongPath(_data.PathToThirdBoss, "Take Path to Third Boss"),
                new MoveToUnit(10901, new Vector3(274.877, 1.33366, 85.22826, "None"), "Approaching Lorekeeper Polkelt", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFourthBoss, "Take Path to Fourth Boss"),
                new MoveToUnit(11261, new Vector3(182.246, -95.4371, 85.22827, "None"), "Approaching Doctor Theolen Krastinov", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFifthBoss, "Take Path to Fifth Boss"),
                new MoveToUnit(10505, new Vector3(86.6634, -1.96039, 85.22828, "None"), "Approaching Instructor Malicia", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSixthBoss, "Take Path to Sixth Boss"),
                new MoveToUnit(10502, new Vector3(265.956, 0.903429, 75.24996, "None"), "Approaching Lady Illucia Barov", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSeventhBoss, "Take Path to Seventh Boss"),
                new MoveToUnit(10507, new Vector3(103.305, -1.67752, 75.13348, "None"), "Approaching The Ravian", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToEightBoss, "Take Path to Eight Boss"),
                new MoveToUnit(10504, new Vector3(178.724, -91.0232, 70.7734, "None"), "Approaching Lord Alexei Barov", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToNineBoss, "Take Path to Ninth Boss"),
                new MoveToUnit(1853, new Vector3(180.7712, -5.428603, 75.48685, "None"), "Approaching Darkmaster Gandling", skipIfNotFound: true),

            };
    }
}