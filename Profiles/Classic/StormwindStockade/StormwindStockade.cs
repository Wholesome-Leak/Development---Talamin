using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.StormwindStockade
{
    internal class StormwindStockade : Profile
    {
        private readonly Data _data;

        public StormwindStockade(string profileName = "Stormwind Stockade") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First  Boss"),
                new MoveToUnit(1696, new Vector3(159.582, 1.25311, -25.6062, "None"), "Approach Targorr the Dread", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second  Boss"),
                new MoveToUnit(1717,new Vector3(105.523, -105.795, -35.18956, "None"), "Approach Hamhock", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveToUnit(1716, new Vector3(89.5518f, -136.922f, -33.9396), "Approach Bazil Thredd", skipIfNotFound: true)
            };
    }
}