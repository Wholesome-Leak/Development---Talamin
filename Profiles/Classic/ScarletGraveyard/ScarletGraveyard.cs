using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.ScarletGraveyard
{
    internal class ScarletGraveyard : Profile
    {
        private readonly Data _data;

        public ScarletGraveyard(string profileName = "Scarlet Graveyard") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(3983, new Vector3(-239.8237, 150.3391, -18.79636), "Approach Interrogator Vishas", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(4543, new Vector3(1820.34, 1416.71, -7.91731, "None"), "Approach Bloodmage Thalnos")
            };
    }
}