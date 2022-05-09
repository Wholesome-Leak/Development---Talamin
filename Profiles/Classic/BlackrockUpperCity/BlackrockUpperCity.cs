using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.BlackrockUpperCity
{
    internal class BlackrockUpperCity : Profile
    {
        private readonly Data _data;

        public BlackrockUpperCity(string profileName = "Blackrock Upper City") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First Boss"),
                new MoveToUnit(9156, new Vector3(1009.75, -239.017, -61.3871, "None"),"Approaching Ambassador Flamelash", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take path to Second Boss"),
                new MoveToUnit(9039, new Vector3(1278.632, -280.774, -78.2187, "None"),"Move To Doom'rel", false,true,true),
                new MoveToUnit(9039, new Vector3(1278.632, -280.774, -78.2187, "None"),"Move To Doom'rel", false,true,true),
                new MoveAlongPath(_data.KillingintheName, "Killing in the Name"),
                new InteractWith(169243, new Vector3(1262.933, -251.4256, -78.21898, "None"), CheckChestoftheseven => ObjectManager.GetWoWGameObjectByEntry(169243).Count() == 0, "Interact with Chest"),
                new InteractWith(170558, new Vector3(1309.134, -342.1599, -92.04291, "None"), CheckDoor => ObjectManager.GetWoWGameObjectByEntry(170558).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Door"),
                new MoveAlongPath(_data.PathToTorchRoom, "Take path to Torch Room"),
                new InteractWith(174744, new Vector3(1329.406, -511.8877, -88.87058, "None"), Checkbrazier => ObjectManager.GetWoWGameObjectByEntry(174744).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Brazier"),
                new MoveAlongPath(_data.GoingThroughTorchRoom, "Going through the Torch Room"),
                new InteractWith(174745, new Vector3(1428.208, -508.9855, -88.86982, "None"), Checkbrazier => ObjectManager.GetWoWGameObjectByEntry(174745).Any(o => (o.Flags & GameObjectFlags.InUse) != 0), "Interact with Brazier"),
                new MoveAlongPath(_data.PathToThirdBoss, "Take path to Third Boss"),
                new MoveToUnit(9938, new Vector3(1380.72, -659.314, -92.05434, "None"), "Approaching Magmus", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFourthBoss, "Take path to Fourth Boss"),
                new MoveToUnit(8929, new Vector3(1396.307, -816.0905, -91.98132, "None"), "Approaching Princess Moira Bronzebeard", skipIfNotFound: true)
            };
    }
}