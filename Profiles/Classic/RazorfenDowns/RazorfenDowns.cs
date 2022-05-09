using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.RazorfenDowns
{
    internal class RazorfenDowns : Profile
    {
        private readonly Data _data;

        public RazorfenDowns(string profileName = "Razorfen Downs") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.GotoFirstGong, "Take path to First Gong"),
                new InteractWith(148917, new Vector3(2552.44,856.984,51.495, "None"), GongToCheck => ObjectManager.GetWoWGameObjectByEntry(148917).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with First Gong"),
                new MoveAlongPath(_data.ClearRoom, "Clearing the Room"),
                new MoveAlongPath(_data.GoToSecondGong, "Take path to Second Gong"),
                new InteractWith(148917, new Vector3(2552.44,856.984,51.495, "None"), GongToCheck => ObjectManager.GetWoWGameObjectByEntry(148917).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with Second Gong"),
                new MoveAlongPath(_data.ClearRoom, "Clearing the Room"),
                new MoveAlongPath(_data.GoToThirdGong, "Take path to Third Gong"),
                new InteractWith(148917, new Vector3(2552.44,856.984,51.495, "None"), GongToCheck => ObjectManager.GetWoWGameObjectByEntry(148917).Any(o => (o.Flags & GameObjectFlags.CanNotBeInteracted) != 0), "Interact with Third Gong"),
                new MoveAlongPath(_data.ClearRoom, "Clearing the Room"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(7355, new Vector3(2537.479, 874.02, 47.678, "None"), "Approach Tuten'kash", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(7357, new Vector3(2466.62, 671.443, 63.38605, "None"), "Approach Mordresh Fire Eye", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveAlongPath(_data.PathToFourthBoss, "Take path to Fourth Boss"),
                new MoveToUnit(7358, new Vector3(2403.37, 960.93, 55.05998, "None"), "Approach Amnennar the Coldbringer", skipIfNotFound:true),
            };
    }
}