using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Profiles.Base;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Profiles.WotLK.ForgeOfSouls {
    internal class ForgeOfSouls : Profile {
        public ForgeOfSouls(string profileName = "Forge of Souls BETA") : base(profileName) { }

        protected override void UpdateSteps() {
            if (Steps[2].IsCompleted && !ObjectManager.Me.IsAlive)
                Steps[2].IsCompleted = false; // Go to first gathering point again
        }

        protected override Step[] GetSteps() =>
            new Step[] {
                new MoveToUnit(37597, new Vector3(4899.98, 2208.16, 638.7335), "Move to Lady Jaina Proudmoore"),
                new Execute(() => Lua.LuaDoString("SendChatMessage(\"What's up, Proudmoore?\");"), stepName: "Say <What's up, Proudmoore?>"),
                new GoTo(new Vector3(4906.965, 2207.652, 638.734), stepName: "Move to first gathering point")
            };
    }
}