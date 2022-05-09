using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.Uldaman2
{
    internal class Uldaman2 : Profile
    {
        private readonly Data _data;

        public Uldaman2(string profileName = "Uldaman2") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathtoEchomokDoor,"Open the Echomok Door"),
                new InteractWith(124370, new Vector3(-185.7989, 389.7121, -36.95535, "None"),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "Echomok Door", strictPosition: true),
                new MoveAlongPath(_data.PathToNormalRoute, "Moving to the normal Route"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take  Path to First  Boss"),
                new MoveToUnit(6908, new Vector3(-356.985, 132.094, -47.86775, "None"), "Approaching Olaf", skipIfNotFound:true),
                new MoveToUnit(6907, new Vector3(-358.335, 118.947, -44.48909, "None"), "Approaching Eric The Swift", skipIfNotFound:true),
                new MoveToUnit(6906, new Vector3(-353.007, 117.186, -44.45164, "None"), "Approaching Baelog", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss, "Take  Path to Second Boss"),
                new MoveToUnit(6910, new Vector3(-225.598, 161.224, -44.63006, "None"), "Approaching Revelosh", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToThirdBoss, "Take  Path to Third Boss"),
                new MoveToUnit(7206, new Vector3(-38.429, 221.253, -48.32748, "None"), "Approaching Ancient Stone Keeper", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToFourthBoss, "Take  Path to Fourth  Boss"),
                new MoveToUnit(7291, new Vector3(-10.3813, 414.708, -46.94037, "None"), "Approaching Galgann Firehammer", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToFifthBoss, "Take Path to Fifth Boss"),
                new MoveToUnit(4854, new Vector3(56.7096, 455.299, -41.04558, "None"), "Approaching Grimlok", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToTempleDoor, "Take Path to the Temple Door"),
                new InteractWith(124368, new Vector3(50.0446, 247.246, -26.5175, "None"),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "Temple  Door", strictPosition: true),
                new MoveAlongPath(_data.PathToAltarOfTheKeepers, "Take Path to the Altar of the Keepers"),
                new InteractWith(130511, new Vector3(104.845, 272.453, -26.5322, "None"),
                    Altar => (ObjectManager.GetObjectWoWUnit().Where(u=> u.Entry == 4857 && u.Reaction == Reaction.Hostile).Count() >=1 || (ObjectManager.GetObjectWoWUnit().Where(u=> u.Entry == 4857 && u.Reaction == Reaction.Friendly).Count() ==0)/* || !TraceLine.TraceLineGo(new Vector3(130.5737, 285.2426, -26.58806, "None"), new Vector3(141.6335, 290.1583, -26.58196, "None"),
                        CGWorldFrameHitFlags.HitTestLOS | CGWorldFrameHitFlags.HitTestMovableObjects)*/) , "Altar of the Keepers", strictPosition: true),
                new  MoveAlongPath(_data.PathToStonekeepers, "Moving Around the Altar to kill  Stonekeepers"),
                new MoveAlongPath(_data.PathToAltarOfArchaedas,"Take  Path to Altar of  Archaedas"),
                new InteractWith(133234, new Vector3(96.4808, 269.052, -52.1487, "None"),
                    Altar => (ObjectManager.GetObjectWoWUnit().Where(u=> u.Entry == 2748 && u.Reaction == Reaction.Hostile).Count() >=1 || ObjectManager.GetObjectWoWUnit().Where(u=> u.Entry == 2748 && u.Reaction == Reaction.Friendly).Count() ==0), "Altar of Archaedas", strictPosition: true),
                new MoveToUnit(2748, new Vector3(103.5122, 271.8458, -51.77919, "None"), "Approaching Archaedas", skipIfNotFound:true),
            };
    }
}