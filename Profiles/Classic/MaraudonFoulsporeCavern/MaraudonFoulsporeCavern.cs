using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.MaraudonFoulsporeCavern
{
    internal class MaraudonFoulsporeCavern : Profile
    {
        private readonly Data _data;

        public MaraudonFoulsporeCavern(string profileName = "Maraudon Foulspore Cavern") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(13282, new Vector3(1130.4, -191.342, -80.08447, "None"), "Approach Noxxion", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(12258, new Vector3(978.887, -10.152, -62.58675, "None"), "Approach Mauradon", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveToUnit(12225, new Vector3(726.106, 77.9764, -86.67464, "None"), "Approach Celebras the Cursed", skipIfNotFound:true),
                //new MoveToUnit(11520, new Vector3(-239.8237, 150.3391, -18.79636), "Approach Taragaman the Hungerer")
                //new JumpTo(new Vector3(-7.339958, -36.23986, -21.68921, "None"), 3.708274f, new Vector3(-11.37908, -39.07303, -21.5649, "None"), "Jump  Test")
            };
    }
}