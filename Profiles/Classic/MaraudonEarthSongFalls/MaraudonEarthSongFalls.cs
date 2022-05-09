using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.MaraudonEarthSongFalls
{
    internal class MaraudonEarthSongFalls : Profile
    {
        private readonly Data _data;

        public MaraudonEarthSongFalls(string profileName = "Maraudo Earth Song Falls") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(12203, new Vector3(356.681, -185.455, -59.89888, "None"), "Approach Landslide", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(12201, new Vector3(27.96711, 61.66707, -123.384, "None"), "Approach Princess Theradras", skipIfNotFound:true),
            };
    }
}