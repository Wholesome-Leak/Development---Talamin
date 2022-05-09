using robotManager.Helpful;
using WholesomeDungeons.DungeonLogic;
using WholesomeDungeons.Profiles.Base;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.States;
using WholesomeDungeons.Helper;

namespace WholesomeDungeons.Profiles.Classic.TheDeadmines {
    internal class TheDeadmines : Profile {
        private readonly Data _data;

        public TheDeadmines(string profileName = "The DeadMines") : base(profileName) {
            _data = new Data();
        }



        protected override Step[] GetSteps() =>
            new Step[] {
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.SavedDeathrun = _data.Deathrun, stepName: "We set our  Deathrun"),
                new Execute(() => Bot.WholesomeDungeonsSettings.CurrentSetting.Save(), stepName: "Saved Deathrun  Settings"),
                new Execute(()=> Helpers._RunStateDungeonLogic(), stepName:"CheckRole"),
                new MoveAlongPath(_data.PathToFirstBoss, "Take path to First  Boss"),
                new MoveToUnit(644, new Vector3(-192.6211f, -444.8483f, 54.0567), "Approach Rhahk'Zor", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToSecondDoor, "Take path to second  Door"),
                new InteractWith(17153, new Vector3(-252.9927, -481.9624, 49.44714),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "Interact with the First Door before second Boss", strictPosition: true),
                new MoveAlongPath(_data.PathTosecondBoss,"Take Path to second Boss"),
                new MoveToUnit(642, new Vector3(-303.1003, -515.5302, 52.43622, "None"), "Approach Sneeds Shredder", skipIfNotFound: true),
                new InteractWith(17153, new Vector3(-290.294, -536.96, 49.4353),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "Interact with the Door after second Boss", strictPosition : true),
                new MoveAlongPath(_data.PathToThirdDoor,"Take Path to third Door"),
                new InteractWith(17154, new Vector3(-242.965 , -578.561 , 51.1366),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "Interact with the Third Door"),
                new MoveAlongPath(_data.MoveDownTheGoblinFondry,"Approaching third Boss"),
                new MoveToUnit(1763, new Vector3(-178.7875, -575.2722, 19.31014, "None"), "Approach Gilnid", skipIfNotFound: true),
                new InteractWith(17153, new Vector3(-168.514, -579.861, 19.3159),
                    door => (door.Flags & GameObjectFlags.InUse) != 0, "Open the foundry exit door", true),
                new MoveAlongPath(_data.MoveDowntoGunpowder, "Moving to Gunpowder for fourth door"),
                new PickupObject(5397, 17155, new Vector3(-106.409 , -617.284 , 13.8495), "Pickup Gunpowder"),
                new MoveAlongPath(_data.MovetoCannon, "Moving down to the Cannon"),
                new InteractWith(16398, new Vector3(-107.562 , -659.674 , 7.21211),
                    Tracelinecheck => !TraceLine.TraceLineGo(new Vector3(-102.5544, -654.9741, 9.0), new Vector3(-98.72768, -678.571, 9.0),
                        CGWorldFrameHitFlags.HitTestLOS | CGWorldFrameHitFlags.HitTestMovableObjects),
                    "Interact with Cannon and check for open door",
                    strictPosition: true, interactDistance: 7),
                
                new MoveAlongPath(_data.PathToThirdBoss,"Moving to the third Boss"),
                new MoveToUnit(646, new Vector3(-22.8471, -797.283, 20.29121, "None"), "Approach Mr Smite", skipIfNotFound: true),
                new MoveAlongPath(_data.PathToFourthBoss, "Moving up the Ship to Boss 4 and 5"),
                new MoveToUnit(647, new Vector3(-57.64916, -809.9777, 41.786), "Approach Captain Greenskin", skipIfNotFound: true),
                new MoveToUnit(639, new Vector3(-59.67401, -831.8054, 41.63031, "None"), "Edwin VanCleef", skipIfNotFound: true),
            };
    }
}