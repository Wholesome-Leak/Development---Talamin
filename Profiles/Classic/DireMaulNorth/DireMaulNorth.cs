using robotManager.Helpful;
using System.Linq;
using System.Threading;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.DireMaulNorth
{
    internal class DireMaulNorth : Profile
    {
        private readonly Data _data;

        public DireMaulNorth(string profileName = "Dire Maul North") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(14326, new Vector3(399.83, -11.59, -24.9361, "None"), "Approaching Guard Mol'dar", skipIfNotFound: true),
                new MoveAlongPath(_data.GoForTheKey, "Go For the  Key"),
                new InteractWith(179516,new Vector3(385.1945, 257.5649, 11.43957, "None"), CheckforChest=> ObjectManager.GetWoWGameObjectByEntry(179516).Count() == 0, "Interact with Chest"),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(14321, new Vector3(385.2072, 286.7735, 11.20647, "None"), "Approaching Guard Fengus", skipIfNotFound: true),
                new MoveAlongPath(_data.GoToTheDoors, "Take path to the Doors"),
                new InteractWith(177219, new Vector3(385.327 , 374.232 , -1.34314, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(177219).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Door"),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveToUnit(14323, new Vector3(476.1408, 590.2229, -25.40519, "None"), "Approaching Guard Slip'kik", skipIfNotFound: true),
                new MoveAlongPath(_data.GoingToTheSecondDoor, "Going to the Second Door"),
                new InteractWith(177217, new Vector3(491.204 , 515.133 , 29.4675, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(177217).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Door"),
                new MoveAlongPath(_data.PathToFourthBoss, "Take path to Fourth Boss"),
                new MoveToUnit(14325, new Vector3(613.05, 481.95, 29.46418, "None"), "Approaching Captain Kromcrush", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFifthBoss, "Take path to Fifth Boss"),
                new MoveToUnit(11501, new Vector3(814.5, 479.44, 37.31824, "None"), "Approaching King Gordok", skipIfNotFound: true),
                new Execute(() => Thread.Sleep(5000), stepName: "Wait until Mizzle the Crafty appears"),
                new MoveToUnit(14353, new Vector3(816.53, 482.302, 37.31826, "None"),"Talk to Mizzle the Crafty", false,true,true),
                new Execute(() => Thread.Sleep(1000), stepName: "Wait until King Ceremony"),
                new MoveToUnit(14353, new Vector3(816.53, 482.302, 37.31826, "None"),"Talk to Mizzle the Crafty", false,true,true),
                new Execute(() => Thread.Sleep(1000), stepName: "Wait until King Ceremony"),
                new MoveToUnit(14353, new Vector3(816.53, 482.302, 37.31826, "None"),"Talk to Mizzle the Crafty", false,true,true),
                new Execute(() => Thread.Sleep(1000), stepName: "Wait until King Ceremony"),
                new MoveToUnit(14353, new Vector3(816.53, 482.302, 37.31826, "None"),"Talk to Mizzle the Crafty", false,true,true),
                new Execute(() => Thread.Sleep(1000), stepName: "Wait until King Ceremony"),
                new MoveToUnit(14353, new Vector3(816.53, 482.302, 37.31826, "None"),"Talk to Mizzle the Crafty", false,true,true),
                new InteractWith(179564,new Vector3(808.37 , 482.128 , 37.3182, "None"), CheckforChest=> ObjectManager.GetWoWGameObjectByEntry(179564).Count() == 0, "Interact with Chest")

            };
    }
}