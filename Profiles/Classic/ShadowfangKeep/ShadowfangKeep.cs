using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.ShadowfangKeep
{
    internal class ShadowfangKeep : Profile
    {
        private readonly Data _data;

        public ShadowfangKeep(string profileName = "Shadowfang Keep") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First  Boss"),
                new MoveToUnit(3914, new Vector3(-252.091, 2123.11, 81.1795, "None"), "Approach Rethilgore", skipIfNotFound: true),
                //new InteractWith(18900, new Vector3(-252.696, 2114.22, 82.8052), CheckDoorInsteadLever => ObjectManager.GetWoWGameObjectByEntry(18934).Where(o=> (o.Flags & GameObjectFlags.InUse) != 0).Count() > 0, "Interact with Door Lever"),
                new InteractWith(18900, new Vector3(-252.696, 2114.22, 82.8052), CheckDoorInsteadLever => ObjectManager.GetWoWGameObjectByEntry(18934).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Door Lever"),
                new MoveToUnit(3849, new Vector3(-243.712, 2113.72, 81.17957),"Move To Deathstalker Adamant Prisoner", false,true,true),
                //new  Interact and  TalkTo  Step
                new WaitUntil(new Vector3(-241.5556, 2156.794, 90.62408, "None"), 18895, WaitForOpenDoor => ObjectManager.GetWoWGameObjectByEntry(18895).Any(o=> (o.Flags & GameObjectFlags.InUse) != 0), "Wait for Open  Door"),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(3887, new Vector3(-275.342f, 2297.35f, 76.1535), "Approch Baron Silverlaine", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take Path to  Third  Boss"),
                new MoveToUnit(4278, new Vector3(-222.592, 2259.44, 102.7557, "None"), "Approach Commander Springvale", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFourthBoss, "Take  Path to fourth Boss"),
                new MoveToUnit(4279, new Vector3(-236.708, 2146.08, 100.0289, "None"), "Approach Odo the Blindwatcher", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFifthBoss, "Take  Path to Fifth Boss"),
                new MoveToUnit(4274, new Vector3(-135.609, 2168.66, 128.696, "None"), "Approach Fenrus the Devourer", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSixthBoss, "Take Path to Sixth Boss"),
                new MoveToUnit(3927, new Vector3(-120.725, 2162.03, 155.6785, "None"), "Approach Wolf Master Nandos", skipIfNotFound: true),
                new MoveToUnit(4275, new Vector3(-76.7541, 2152.41, 155.7087, "None"), "Approach Archmage Arugal", skipIfNotFound:true)
            };
    }
}