using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Enums;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.Classic.TheTempleOfAtalHakkar
{
    internal class TheTempleOfAtalHakkar : Profile
    {
        private readonly Data _data;

        public TheTempleOfAtalHakkar(string profileName = "The Temple Of Atal Hakkar") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToMiniBoss1, "Take path to MiniBoss1"),
                new MoveAlongPath(_data.PathToMiniBoss2, "Take path to MiniBoss2"),
                new MoveAlongPath(_data.PathToMiniBoss3, "Take path to MiniBoss3"),
                new MoveAlongPath(_data.PathToMiniBoss4, "Take path to MiniBoss4"),
                new MoveAlongPath(_data.PathToMiniBoss5, "Take path to MiniBoss5"),
                new MoveAlongPath(_data.PathToMiniBoss6, "Take path to MiniBoss6"),
                new MoveAlongPath(_data.GoingDownstairs,"Taking the path Downstairs"),
                new MoveAlongPath(_data.PathToSouthStatue,"Taking the path SouthStatue"),
                new InteractWith(148830,new Vector3(-515.553 , 95.2582 , -148.74, "None"),  StatueToCheck => ObjectManager.GetWoWGameObjectByEntry(148830).Any(o => (o.Flags & GameObjectFlags.ConditionalInteraction) != 0), "Interact with South Statue"),
                new MoveAlongPath(_data.PathToNorthStatue,"Taking the path NorthStatue"),
                new InteractWith(148831,new Vector3(-419.849 , 94.4837 , -148.74, "None"),  StatueToCheck => ObjectManager.GetWoWGameObjectByEntry(148831).Any(o => (o.Flags & GameObjectFlags.ConditionalInteraction) != 0), "Interact with North Statue"),
                new MoveAlongPath(_data.PathToSouthWestStatue,"Taking the path SouthWestStatue"),
                new InteractWith(148832,new Vector3(-491.4 , 135.97 , -148.74, "None"),  StatueToCheck => ObjectManager.GetWoWGameObjectByEntry(148832).Any(o => (o.Flags & GameObjectFlags.ConditionalInteraction) != 0), "Interact with South West Statue"),
                new MoveAlongPath(_data.PathToSouthEastStatue,"Taking the path SouthEastStatue"),
                new InteractWith(148833,new Vector3(-491.491 , 53.4818 , -148.74, "None"),  StatueToCheck => ObjectManager.GetWoWGameObjectByEntry(148833).Any(o => (o.Flags & GameObjectFlags.ConditionalInteraction) != 0), "Interact with South East Statue"),
                new MoveAlongPath(_data.PathToNorthWestStatue,"Taking the path NorthWestStatue"),
                new InteractWith(148834,new Vector3(-443.855 , 136.101 , -148.74, "None"),  StatueToCheck => ObjectManager.GetWoWGameObjectByEntry(148834).Any(o => (o.Flags & GameObjectFlags.ConditionalInteraction) != 0), "Interact with North West Statue"),
                new MoveAlongPath(_data.PathToNorthEastStatue,"Taking the path NorthEastStatue"),
                new InteractWith(148835,new Vector3(-443.417 , 53.8312 , -148.74, "None"),  StatueToCheck => ObjectManager.GetWoWGameObjectByEntry(148835).Any(o => (o.Flags & GameObjectFlags.ConditionalInteraction) != 0), "Interact with North West Statue"),
                new MoveAlongPath(_data.PathToFirstBoss,"Take Path to First Boss"),
                new MoveToUnit(8580, new Vector3(-466.5134, 95.19822, -189.7296, "None"), "Approach Atal'alarion", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToSecondBoss,"Take Path to Second Boss"),
                new MoveToUnit(5710, new Vector3(-425.894, -86.0747, -88.224, "None"), "Approach Jammal'an the Prophet", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToThirdBoss,"Take Path to Third Boss"),
                new MoveToUnit(5709, new Vector3(-658.379, -35.7623, -90.8352, "None"), "Approach Shade of Eranikus", skipIfNotFound:true)

            };
    }
}