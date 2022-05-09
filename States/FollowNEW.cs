using robotManager.FiniteStateMachine;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using wManager.Wow.Bot.Tasks;
using robotManager.Helpful;
using WholesomeDungeons.Helper;
using WholesomeDungeons.Bot;
using System.Linq;
using wManager.Wow.Enums;
using System.Collections.Generic;

namespace WholesomeDungeons.States
{
    public class FollowNEW : State
    {
        public override string DisplayName
        {
            get { return "Follow Leader"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;

        public float DistanceSearch = 150;
        public Vector3 oldleaderpos = new Vector3(0, 0, 0);
        public List<Vector3> oldleaderposlist = new List<Vector3>();
        private int FollowRange = 10;

        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {
                //checks if we are inside Dungeon
                if (Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) <= 0)
                    return false;

                //Checks if we are set as  Tank, so we don´t need the Follow State
                if (WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                {
                    Bot.WholesomeDungeons.Tankname = ObjectManager.Me.Name;
                    Bot.WholesomeDungeons.Tankunit = ObjectManager.Me;
                    return false;
                }

                //If our Tankunit is Zero or we donßt know the right Tankname, we have to detect it again
                if (Bot.WholesomeDungeons.Tankunit == null || WholesomeDungeonsSettings.CurrentSetting.TankName != Bot.WholesomeDungeons.Tankunit.Name)
                {
                    Logger.Log("Detect Tank in FollowState" + DisplayName);
                    Helpers.GroupCheck();
                    return false;
                }

                //check for leader is alive
                if (Bot.WholesomeDungeons.Tankunit.IsDead)
                {
                    return false;
                }

                //set the followrange for RDPS if we are one
                if (WholesomeDungeonsSettings.CurrentSetting.SetAsRDPS)
                    FollowRange = WholesomeDungeonsSettings.CurrentSetting.FollowRangeRDPS;

                //set the followrange for MDPS if we are one
                if (WholesomeDungeonsSettings.CurrentSetting.SetAsMDPS)
                    FollowRange = WholesomeDungeonsSettings.CurrentSetting.FollowRangeMDPS;

                //set the followrange for Heal if we are one
                if (WholesomeDungeonsSettings.CurrentSetting.SetAsHeal)
                    FollowRange = WholesomeDungeonsSettings.CurrentSetting.FollowRangeHeal;
                
                //////Making Preperations to possible Follow
                //checks if oldleaderpos isn´t set, mostly at the beginning of an Dungeon and the Position is not known
                if (oldleaderpos == new Vector3(0, 0, 0))
                {
                    //set oldleaderpos to actual leader Position
                    oldleaderpos = Bot.WholesomeDungeons.Tankunit.Position;
                }
                //Check if our Tank made 6 yards from his old position, then follow him
                if(Bot.WholesomeDungeons.Tankunit.Position.DistanceTo(oldleaderpos) > 6)
                {
                    oldleaderpos = Bot.WholesomeDungeons.Tankunit.Position;
                }

                //Checks when not to follow
                if (!TraceLine.TraceLineGo(Bot.WholesomeDungeons.Tankunit.Position))
                {
                    //If we are moving, return false
                    if (Helpers.IsMovementThreadRunning || MovementManager.InMoveTo)
                    {
                        return false;
                    }
                    //if we are not moving but we are in range of the old position from our tank
                    if (!Helpers.IsMovementThreadRunning && ObjectManager.Me.Position.DistanceTo(oldleaderpos) <= FollowRange)
                    { 
                        return false;                   
                    }
                }
                //Logger.Log("!TraceLine.TraceLineGo(Bot.WholesomeDungeons.Tankunit.Position): " + !TraceLine.TraceLineGo(Bot.WholesomeDungeons.Tankunit.Position));
                //Logger.Log("MovementThreatRunning: " + Helpers.IsMovementThreadRunning);
                //Logger.Log("MovementManager.InMovement: " +  MovementManager.InMovement);
                //Logger.Log("MovementManager.InMoveTo: " + MovementManager.InMoveTo);
                //Logger.Log("MovementManager.InMovement: " + MovementManager.InMovementLoop);
                if (ObjectManager.Me.Position.DistanceTo(oldleaderpos) > FollowRange 
                    || TraceLine.TraceLineGo(Bot.WholesomeDungeons.Tankunit.Position) 
                    || TraceLine.TraceLineGo(ObjectManager.Target.Position))
                {
                    return true;
                }

                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            //Now we try to calculate if the Tank is behind a cliff or something, by comparing the distance for sight and real path lenght.
            //If we differ 5%, we have to consider that the real path is larger to avoid an obstacle, so we use pathfinder and navigate relatively close to him
            //Else we use direkt moveto

            //calculates real distance by using pathfinder
            float pathcalc = Helpers.CalculatePathTotalDistance(ObjectManager.Me.Position, Bot.WholesomeDungeons.Tankunit.Position);
            //calculates distance by sightdistance
            float sight = ObjectManager.Me.Position.DistanceTo(Bot.WholesomeDungeons.Tankunit.Position);

            //Logger.Log($"Following State: Distance in sight: {sight} in pathcalc {pathcalc} Difference in pathcalc/sight {pathcalc / sight * 100}");

            //check for line of sight, if not use pathfinder until you can see the Target
            if (TraceLine.TraceLineGo(Bot.WholesomeDungeons.Tankunit.Position))
            {
                Logger.Log("Following State: We don´t have LOS to the Tank, so we start following");
                Helpers.StartMoveAlongThread(PathFinder.FindPath(ObjectManager.Me.Position, oldleaderpos, false), abortIf: IcanSee);
            }

            //check if the difference between calculated path and on sight is more then 5%, so we use pathfinder and navigate until we are near the half way of the follow state
            if ((pathcalc / sight) * 100 > 105)
            {
                Logger.Log("Following State: Leader is behind a Cliff, using Pathfinder to get along");
                Helpers.StartMoveAlongThread(PathFinder.FindPath(ObjectManager.Me.Position, oldleaderpos, false), abortIf: () => ObjectManager.Me.Position.DistanceTo(Bot.WholesomeDungeons.Tankunit.Position) <= (FollowRange / 2));
            }

            //check if the difference between calculated path and on sight is less then 5%, then we use the normal MoveAlong until we are in  Followrange
            if ((pathcalc / sight) * 100 <= 105)
            {
                Logger.Log("Following State: Leader is out of Range, following normal");
                //Helpers.StartGoToThread(oldleaderpos, abortIf: IamNear);
                Helpers.StartMoveAlongThread(PathFinder.FindPath(ObjectManager.Me.Position, oldleaderpos, false), abortIf: IamNear);
            }

            ////check if we still don´t see the target, then move to the tank
            if (ObjectManager.Target.IsAttackable && TraceLine.TraceLineGo(ObjectManager.Target.Position))
            {
                Logger.Log("Can´t see Target, move to Tank until i see it");
                Helpers.StartMoveAlongThread(PathFinder.FindPath(ObjectManager.Me.Position, Bot.WholesomeDungeons.Tankunit.Position, false), abortIf: () => !TraceLine.TraceLineGo(ObjectManager.Target.Position));
            }
        }

        public bool IamNear() //check if we are closer then our Followrange to the Tank, if yes return true
        {
            return ObjectManager.Me.Position.DistanceTo(oldleaderpos) <= FollowRange;
        }
        public bool IcanSee() //check if we have LOS to the Tank, if yes return true
        {
            return !TraceLine.TraceLineGo(Bot.WholesomeDungeons.Tankunit.Position);
        }
    }
}
