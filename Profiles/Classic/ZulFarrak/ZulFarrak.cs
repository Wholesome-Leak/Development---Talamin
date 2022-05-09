using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.ZulFarrak
{
    internal class ZulFarrak : Profile
    {
        private readonly Data _data;

        public ZulFarrak(string profileName = "ZulFarrak") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new InteractWith(141832, new Vector3 (1650.91, 1171.88, 10.901, "None"), CheckThis => ObjectManager.GetWoWGameObjectByEntry(141832).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Gong", interactDistance: 6f),
                new MoveToUnit(7273, new Vector3(1700.54, 1220.82, 8.876832, "None"), "Approach Gahz'rilla", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Third Boss"),
                new MoveToUnit(7274, new Vector3(1886.8, 1289.89, 45.93704, "None"), "Sandfury  Executioner", skipIfNotFound:true),
                new GoTo(new Vector3(1892.881, 1295.923, 46.27091, "None"),  "Move near the Cage"),
                new InteractWith(141070, new Vector3(1893.234, 1295.162, 46.22925, "None"), CheckThis => ObjectManager.GetWoWGameObjectByEntry(141070).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Free Prisoners", interactDistance: 7f),
                new FollowUnit(7604,new Vector3(1881.473, 1273.374, 41.96457, "None"),new Vector3(1878.877, 1206.644, 8.876914, "None"), "Follow Sergeant Bly", skipIfNotFound:true),
                new MoveAlongPath(_data.PathDownTheStairs, "Move Down the Stairs"),
                new MoveToUnit(7604,new Vector3(1883.82, 1200.83, 8.877327, "None"),"Talk to Sergeant Bly", false, true, true),
                new MoveToUnit(7604, new Vector3(1886.8, 1289.89, 45.93704, "None"), "Approach  Sergeant Bly", skipIfNotFound:true),
                new MoveToUnit(7607, new Vector3(1877.52, 1199.63, 8.876909, "None"),"Talk to Veegli Blastfuse", false, true, true),
                new WaitUntil(new Vector3(1856.693, 1145.503, 15.31448, "None"), 146084, WaitForOpenDoor => ObjectManager.GetWoWGameObjectByEntry(146084).Any(o=> (o.Flags & GameObjectFlags.InUse) != 0), "Wait for Open  Door"),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Fourth Boss"),
            };
    }
}