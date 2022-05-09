using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic._Example
{
    internal class _Example : Profile
    {
        private readonly Data _data;

        public _Example(string profileName = "_Example") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(9018, new Vector3(310.558, -146.251, -70.38618, "None"), "Approaching High Interrogator Gerstahn", skipIfNotFound: true),
                new InteractWith(177221, new Vector3(50.5863 , 501.941 , -23.1499, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(177221).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Door"),
            };
    }
}