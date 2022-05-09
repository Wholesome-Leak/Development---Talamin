using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.DireMaulWest
{
    internal class DireMaulWest : Profile
    {
        private readonly Data _data;

        public DireMaulWest(string profileName = "Dire Maul West") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(11489, new Vector3(-3.87, 483.3, -23.29772, "None"), "Approaching Tendris Warpwood", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(11488, new Vector3(-11.46, 542.11, 28.60364, "None"), "Approaching Illyanna Ravenoak", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveAlongPath(_data.PathToFourthBossPart1, "Take path to Fourth Boss Part1"),
                new InteractWith(177221, new Vector3(50.5863 , 501.941 , -23.1499, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(177221).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Door"),
                new MoveAlongPath(_data.PathToFourthBossPart2, "Take path to Fourth Boss Part2"),

                new MoveAlongPath(_data.FollowAroundPylons, "Follow  around the  Pylons"),
                new MoveAlongPath(_data.PathToFifthBoss, "Take path to Fifth Boss"),
                new MoveToUnit(11496, new Vector3(-40.45, 811.14, -29.5357, "None"), "Approaching Immol'thar", skipIfNotFound: true),
            };
    }
}