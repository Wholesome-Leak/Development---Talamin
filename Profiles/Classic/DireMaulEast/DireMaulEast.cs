using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;

namespace WholesomeDungeons.Profiles.Classic.DireMaulEast
{
    internal class DireMaulEast : Profile
    {
        private readonly Data _data;

        public DireMaulEast(string profileName = "Dire Maul East") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(14327, new Vector3(4.58, -456.84, 16.40371, "None"), "Approaching Lethtendris", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(13280, new Vector3(-16.64, -427.55, -59.95033, "None"), "Approaching Hydrospawn", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToThirdBossPart1, "Take path to Third Boss Part 1"),
                new MoveAlongPath(_data.PathToThirdBossPart2, "Take path to Third Boss Part 2"),
                new MoveToUnit(11490, new Vector3(-33.61, -448.26, -37.96179, "None"), "Approaching Zevrim Thornhoof", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToTalkingTree, "Take Path to find the talking Tree"),
                new MoveToUnit(11491, new Vector3(-56.59, -269.12, -58.04765, "None"),"Move To Old Iron Bark", false,true,true),
                new  MoveAlongPath(_data.PathToFourthBoss,  "Take Path to Fourth Boss"),
                new MoveToUnit(16097, new Vector3(255.13, -424.53, -119.9618, "None"), "Approaching Isalien", skipIfNotFound: true)            

            };
    }
}