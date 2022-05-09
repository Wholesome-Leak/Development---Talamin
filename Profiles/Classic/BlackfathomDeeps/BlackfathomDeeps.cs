using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.BlackfathomDeeps
{
    internal class BlackfathomDeeps : Profile
    {
        private readonly Data _data;

        public BlackfathomDeeps(string profileName = "Blackfathom Deeps") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First  Boss"),
                new MoveToUnit(4887, new Vector3(-442.424, 211.822, -52.71998, "None"), "Approach Ghamoo-ra", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(4831, new Vector3(-299.917, 413.755, -57.1229, "None"), "Approach Lady Sarevess", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveToUnit(6243,new Vector3(-412.653, 40.919, -48.21954, "None"), "Approach Gelihast", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToFourthBoss, "Take path to Fourth Boss"),
                new MoveToUnit(4832, new Vector3(-818.832, -155.576, -25.87563, "None"), "Approach  Twilight Lord Kelris", skipIfNotFound:true),
                new InteractWith(21121,new Vector3(-823.88, -158.535, -24.5278),  FlameToCheck => ObjectManager.GetWoWGameObjectByEntry(21121).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with First Flame"),
                new InteractWith(21118,new Vector3(-813.47, -158.535, -24.5271),  FlameToCheck => ObjectManager.GetWoWGameObjectByEntry(21118).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with Second Flame"),
                new InteractWith(21119,new Vector3(-813.578, -170.461, -24.5276),  FlameToCheck => ObjectManager.GetWoWGameObjectByEntry(21119).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with Third Flame"),
                new InteractWith(21120,new Vector3(-823.955, -170.407, -24.5267),  FlameToCheck => ObjectManager.GetWoWGameObjectByEntry(21120).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with Fourth Flame"),
                //new InteractWith(21117, new Vector3(-818.361, -200.647, -25.7911), DoorToCheck=> ObjectManager.GetWoWGameObjectByEntry(21117).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Portal of Aku"),
                new MoveAlongPath(_data.PathToFifthBoss, "Take path to Fifth Boss"),
                new MoveToUnit(4829, new Vector3(-848.446, -453.865, -33.97552, "None"), "Approach Aku'nai", skipIfNotFound:true)
            };
    }
}