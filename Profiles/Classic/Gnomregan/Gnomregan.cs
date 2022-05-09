using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;

namespace WholesomeDungeons.Profiles.Classic.Gnomregan
{
    internal class Gnomregan : Profile
    {
        private readonly Data _data;

        public Gnomregan(string profileName = "Gnomregan") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToStairs, "Take path to Stairs"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take Path to  First Boss"),
                new MoveAlongPath(_data.PathToFirstandLastBoss, "Still Taking Path to First and Last Boss"),
                new InteractWith(142207, new Vector3(-622.9493, 691.1923, -327.0535, "None"),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "The  Final Chamber", strictPosition: true),
                new MoveToUnit(7800,new Vector3(-531.324, 670.159, -325.2682, "None"), "Approaching Nekgineer Thermaplugg", skipIfNotFound: true)
            };
    }
}