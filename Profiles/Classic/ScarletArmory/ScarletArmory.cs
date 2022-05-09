using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;

namespace WholesomeDungeons.Profiles.Classic.ScarletArmory
{
    internal class ScarletArmory : Profile
    {
        private readonly Data _data;

        public ScarletArmory(string profileName = "Scarlet Armory") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToTheDoor, "Take path to the  Door"),
                new InteractWith(101854, new Vector3(1931.569, -431.5286, 18.00808, "None"), door=> (door.Flags & GameObjectFlags.InUse) != 0, "Interact with First Door", strictPosition: true),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to the first Boss"),
                new MoveToUnit(3975, new Vector3(1965.09, -431.607, 6.177938, "None"), "Approaching Herod", skipIfNotFound:true),
            };
    }
}