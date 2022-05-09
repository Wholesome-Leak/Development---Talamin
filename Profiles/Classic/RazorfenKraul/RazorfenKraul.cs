using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.RazorfenKraul
{
    internal class RazorfenKraul : Profile
    {
        private readonly Data _data;

        public RazorfenKraul(string profileName = "Razorfen Kraul") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First  Boss"),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(4424, new Vector3(2082.31, 1463.54, 73.0956, "None"), "Approach Aggen Thorncurse", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveToUnit(4428, new Vector3(2146.43, 1411.24, 73.8709, "None"), "Approach Death Speaker Jargba", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToFourthBoss, "Take path to Fourth Boss"),
                new MoveToUnit(4420, new Vector3(2203.13, 1640.05, 85.81951, "None"), "Approach Overlord Rantusk", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFifthBoss, "Take path to Fifth Boss"),
                new MoveToUnit(4422, new Vector3(1996.361, 1978.62, 63.60109, "None"), "Approach Agathelos the Raging", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSixthBoss, "Take path to Sixth Boss"),
                new MoveToUnit(4425, new Vector3(2200.76, 1978.19, 56.64501, "None"), "Approach Blind Hunter", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSeventhBoss, "Take path to Seventh Boss"),
                new MoveToUnit(4421, new Vector3(2190.4, 1864.29, 79.05584, "None"), "Approach Charlga Razorflank", skipIfNotFound:true),
            };
    }
}