using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.BlackrockSpire
{
    internal class BlackrockSpire : Profile
    {
        private readonly Data _data;

        public BlackrockSpire(string profileName = "Blackrock Spire") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(9237, new Vector3(-16.9764, -459.149, -18.6442, "None"), "Approaching War Master Voone", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondBossPart1, "Take path to Second Boss part 1"),
                new MoveAlongPath(_data.PathToSecondBossPart2, "Take path to Second Boss part 2"),
                new MoveToUnit(9568, new Vector3(-27.67, -486.62, 90.62707, "None"), "Approaching Overlord Wyrmthalak", skipIfNotFound: true)
            };
    }
}