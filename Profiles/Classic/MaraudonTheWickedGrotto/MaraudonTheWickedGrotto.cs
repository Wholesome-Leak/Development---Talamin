using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.MaraudonTheWickedGrotto
{
    internal class MaraudonTheWickedGrotto : Profile
    {
        private readonly Data _data;

        public MaraudonTheWickedGrotto(string profileName = "Maraudon The Wicked Grotto") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                //new MoveToUnit
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(12236, new Vector3(748.874, -219.647, -47.77573, "None"), "Approach Lord Vyletongue", skipIfNotFound:true),
                //new JumpTo(new Vector3(-7.339958, -36.23986, -21.68921, "None"), 3.708274f, new Vector3(-11.37908, -39.07303, -21.5649, "None"), "Jump  Test")
            };
    }
}