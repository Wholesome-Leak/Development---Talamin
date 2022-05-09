using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;

namespace WholesomeDungeons.Profiles.Classic.ScarletCathedral
{
    internal class ScarletCathedral : Profile
    {
        private readonly Data _data;

        public ScarletCathedral(string profileName = "Scarlet Cathedral") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToTheDoor, "Take path to the  Door"),
                new InteractWith(104591, new Vector3 (1069.95, 1399.14, 30.7956), door=> (door.Flags & GameObjectFlags.InUse) != 0, "Interact with First Door", strictPosition: true),
                new MoveAlongPath(_data.PathToFirstBoss, "Take Path to the First  Boss"),
                new MoveToUnit(3976, new Vector3(1153.87, 1398.39, 32.52792, "None"), "Approaching Scarlet Commander Mograine", skipIfNotFound:true),
                new MoveToUnit(3977, new Vector3(1202.13, 1399.07, 29.00989, "None"), "Approaching High Inquisitor Whitemane", skipIfNotFound:true),
            };
    }
}