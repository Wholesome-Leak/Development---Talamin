using robotManager.FiniteStateMachine;
using static WholesomeDungeons.Helper.Helpers;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.Helper;
using robotManager.Helpful;
using System.Linq;
using WholesomeDungeons.Bot;
using wManager.Wow.Bot.Tasks;
using System.ComponentModel;

namespace WholesomeDungeons.States
{
    public class DefendAll : State
    {
        public override string DisplayName
        {
            get { return "DefendAll"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        private WoWUnit foundTarget;
        private Timer timer = new Timer(250);


        public override bool NeedToRun
        {
            get
            {
                if (!ObjectManager.Me.IsAlive)
                    return false;

                if (!timer.IsReady)
                    return false;

                if (!MeInsideDungeon())
                {
                    return false;
                }

                if(ObjectManager.Target.IsDead)
                {
                    Interact.ClearTarget();
                }

                if (Bot.WholesomeDungeons.Tankunit == null || Bot.WholesomeDungeons.Tankunit.Name == "")
                {
                    Logger.Log("DefendAll: Groupcheck");
                    GroupCheck();
                    return false;
                }

                //Tank Section
                //when we are Tank and alive
                if (Bot.WholesomeDungeons.Tankunit.IsAlive 
                    && WholesomeDungeonsSettings.CurrentSetting.SetAsTank
                    && ObjectManager.Target.Reaction <= Reaction.Neutral)
                {

                    //set foundUnitAttackgroup so we have to call the function only one time per pulse
                    WoWUnit foundunitattackgroup = foundUnitattackGroup();
                    //if we fond a mob attacking the group
                    if (foundunitattackgroup != null)
                    {
                        //set mob to target
                        foundTarget = foundunitattackgroup;
                        Logger.Log("DefendAll: Found TargetUnit which attacks our groupmembers: " + foundTarget.Name);
                        //set timer to 500 to prevent spam
                        timer = new Timer(500);
                        return true;
                    }

                    //set foundunitattackpet  so we have to call the function only one time per pulse
                    WoWUnit foundunitattackpet = foundUnitattakPet();
                    //if we found a mob attacking the group pet
                    if (foundunitattackpet != null)
                    {
                        foundTarget = foundunitattackpet;
                        Logger.Log("DefendAll: Found TargetUnit which attacks our PartyPet: " + foundTarget.Name);
                        timer = new Timer(500);
                        return true;
                    }

                    //set foundunitattackingtank  so we have to call the function only one time per pulse
                    WoWUnit foundunitattackingtank = foundUnitattackingtank();
                    //if we found a mob attacking us, the tank!
                    if (foundunitattackingtank != null && ObjectManager.Me.Target != foundunitattackingtank.Guid)
                    {                      
                        foundTarget = foundUnitattackingtank();
                        Logger.Log("DefendAll: Found TargetUnit which attacks Me: " + foundTarget.Name);
                        timer = new Timer(500);
                        return true;                        
                    }
                }

                //Follower Section for Assist
                if (Bot.WholesomeDungeons.Tankunit.IsAlive && !WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                {
                    //if we are no tank and our target is not the Tanktagret, set it
                    if(Bot.WholesomeDungeons.Tankunit.Target != ObjectManager.Me.Target && ObjectManager.Target.Reaction <= Reaction.Neutral)
                    {
                        foundTarget = Bot.WholesomeDungeons.Tankunit.TargetObject;
                        Logger.Log("DefendAll: Found TankAssist Unit: " + foundTarget.Name);
                        timer = new Timer(500);
                        return true;
                    }
                }
                return false;
            }
        }

        public override void Run()
        {
            //stop old movement
            StopAllMove();
            //if we are no healand our target is not the foundTarget GUID
            if (ObjectManager.Me.Target != foundTarget.Guid && foundTarget.IsAlive && (Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsMDPS || Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsRDPS || Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsTank))
            {
                //stop the old Fight
                Fight.StopFight();
                //stop old movement
                StopAllMove();
                //set target to found Target
                ObjectManager.Me.Target = foundTarget.Guid;
                //start a fight with the new target
                Fight.StartFight(foundTarget.Guid, false);
            }
            //if we are heal, handle the same targeting, maybe obsolet
            if (ObjectManager.Me.Target != foundTarget.Guid && foundTarget.IsAlive && Bot.WholesomeDungeonsSettings.CurrentSetting.SetAsHeal)
            {
                Fight.StopFight();
                StopAllMove();
                ObjectManager.Me.Target = foundTarget.Guid;
                Fight.StartFight(foundTarget.Guid, false);
            }
        }


        public static void Targetswitcher(WoWUnit target, CancelEventArgs cancable)
        {
            //OnFight Tank Event Loop
            if (WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
            {
                if (!ObjectManager.Target.IsTargetingMe)
                    return;

                //set foundunitattackgroup  so we have to call the function only one time per pulse
                WoWUnit foundunitattackgroup = foundUnitattackGroup();              
                //If we have a unit which attacks the group and is not our target
                if (foundunitattackgroup != null && ObjectManager.Target != foundunitattackgroup) 
                {
                    //call a target handler
                    HandleAttackOnFight(foundunitattackgroup);
                }

                //set foundunitattackinggrouppet  so we have to call the function only one time per pulse
                WoWUnit foundunitattackinggrouppet = foundUnitattakPet();
                //if we have a unit attacking our party pet
                if (foundunitattackgroup == null && foundunitattackinggrouppet != null && ObjectManager.Target != foundunitattackinggrouppet)
                {
                    //call a target handler
                    HandleAttackOnFight(foundunitattackinggrouppet);
                }
            }

            //OnFight Follower Assist Loop
            if (WholesomeDungeonsSettings.CurrentSetting.SetAsMDPS || WholesomeDungeonsSettings.CurrentSetting.SetAsRDPS)
            {
                //set foundunitonpriolist so we have to call the function only one time per pulse
                WoWUnit foundunitonpriolist = foundUnitonpriolist();
                //check if there is a prio target and try to kill it
                if(foundunitonpriolist !=null && ObjectManager.Target != foundunitonpriolist)
                {
                    //call Target Handler
                    HandleAttackOnFight(foundunitonpriolist);
                }

                //set foundfleeingunit  so we have to call the function only one time per pulse
                WoWUnit foundfleeingunit = foundUnitfleeing();                
                //if a unit is fleeing and not our target and no prio Target is around
                if (foundunitonpriolist == null 
                    && foundfleeingunit != null 
                    && ObjectManager.Target != foundfleeingunit)
                {
                    //call a target handler
                    HandleAttackOnFight(foundfleeingunit);
                }

                //set unitattackintank  so we have to call the function only one time per pulse
                WoWUnit unitattackingtank = foundUnitattackingtank();
                //if a unit is attacking our tank and is not our target
                //no fleeing unit?
                if (foundfleeingunit == null 
                    //we have a unit which attacks our tank?
                    && unitattackingtank != null 
                    //our  Target is not the new Unit which attacks the tank?
                    && ObjectManager.Target != unitattackingtank)
                {
                    //call a target handler
                    HandleAttackOnFight(unitattackingtank);
                }
            }
        }
        public static void HandleAttackOnFight(WoWUnit unit) 
        {
            //Interact.ClearTarget();
            //stop old fight
            Fight.StopFight();
            //stop old movement
            //StopAllMove();
            Logger.Log("DefendAll OnFight: Found Target " + nameof(unit) + " , switching to: " + unit.Name);
            //Interact.InteractGameObject(unit.GetBaseAddress);
            //set my target
            ObjectManager.Me.Target = unit.Guid;
            //attacking Target
            Fight.StartFight(unit.Guid, false);
            //Fight.CurrentTarget = unit;
        }
    }
}
