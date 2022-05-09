using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Profiles.Base;
using WholesomeDungeons.States;
using wManager.Wow.Helpers;

namespace WholesomeDungeons.Profiles.Classic.WailingCaverns
{
    internal class WailingCaverns : Profile
    {
        private readonly Data _data;

        public WailingCaverns(string profileName = "Wailing Caverns") : base(profileName)
        {
            _data = new Data();
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToDruid, "Take path to Druid"),
                new MoveToUnit(3678, new Vector3(-134.965, 125.402, -78.17783, "None"), "Talking to Disciple of Naralex", skipIfNotFound: true, interactWithUnit:true),
                new MoveAlongPath(_data.PathToBoss1, "Take path to Boss1"),
                new MoveToUnit(3671,new Vector3(15.3449, 297.176, -87.78492, "None"),"Approaching Lady Anaconda", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToBoss1Back, "Move back to Entrance direction"),
                new MoveAlongPath(_data.PathToBoss2, "Take path to Boss2"),
                new MoveToUnit(3669, new Vector3(-151.139, 414.367, -72.71276, "None"), "Approaching Lord Cobrahn", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToBoss3, "Take path to Boss3"),
                new MoveToUnit(3670, new Vector3(36.8074, -241.064, -79.58217, "None"), "Approach Lord  Pythas", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToBoss4, "Take path to Boss4"),
                new MoveToUnit(3674, new Vector3(-285.579, -312.968, -69.1933, "None"), "Approaching Skun", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToBoss5, "Take path to Boss5"),
                new MoveToUnit(3673, new Vector3(-120.163, -24.624, -28.5832, "None"), "Approaching Lord Serpentis", skipIfNotFound:true),
                new MoveToUnit(5775, new Vector3(-81.8554, 32.2565, -31.07722, "None"), "Approaching Verdan the Everliving", skipIfNotFound:true),
                new MoveAlongPath(_data.PathToDruidBack, "Take path to Druid back"),
                new MoveToUnit(3678, new Vector3(-134.965, 125.402, -78.17783, "None"), "Talking to Disciple of Naralex", skipIfNotFound: true, interactWithUnit:true),
                new FollowUnit(3678, new Vector3(-134.965, 125.402, -78.17783, "None"),new Vector3(103.3748, 241.0997, -95.98096, "None"), "Follow Druid to Altar", skipIfNotFound:true),
                new DefendSpot(new Vector3(103.3748, 241.0997, -95.98096, "None"), 120000, "Defend Spot against enemies")
                //new MoveToUnit(3671,new Vector3(15.3449, 297.176, -87.78492, "None"),"Approaching Lady Anaconda", skipIfNotFound: false),
            };
    }
}