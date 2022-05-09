using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using WholesomeDungeons.Bot;
using System.Threading;
using WholesomeDungeons.States;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Helper
{
    internal static class Eventhandler
    {
        public static bool EventLOS;

        public static void HandeLos(string id, List<string> args)//event for LOS in  Dungeons
        {
            //returns true if args11 as given text
            if (args[11] == "Target not in line of sight")
            {
                EventLOS = true;
            }
            //returns false if args11 is not the text
            if (args[11] != "Target not in line of sight")
            {
                EventLOS = false;
            }
        }
        public static void HandleLuaEvents(string id, List<string> args) //handles different LUA Events and reactions
        {
            switch (id)
            {
                case "INSTANCE_BOOT_START":
                    {
                        Logger.Log("Accepting Kick Vote");
                        //Accepts Kickvote through LUA
                        Lua.LuaDoString($"SetLFGBootVote(true)");
                        break;
                    }
                case "PLAYER_EQUIPMENT_CHANGED":
                    {
                        //checks for ranged weapon type
                        Helpers.GetRangedWeaponType();
                        break;
                    }
                case "PLAYER_ENTERING_WORLD":
                    {
                        Logger.Log("Detect Tank on Player_Entering_World  Event");
                        //executes  Group  Check when entering the Dungeon
                        Helpers.GroupCheck();
                        //stops all movement
                        Helpers.StopAllMove();
                        //starts Dungeonlogic for Mapchanges
                        States.DungeonLogic.OnMapChanged();
                        break;
                    }
                case "PLAYER_LEVEL_UP":
                    {
                        Logger.Log("Level UP! Reload Fight Class.");
                        // Update spell list
                        SpellManager.UpdateSpellBook();

                        // Load CC:
                        CustomClass.ResetCustomClass();
                        break;
                    }
                case "PARTY_INVITE_REQUEST":
                    {
                        //checks for Partyinvite Requests from our  Tank
                        if (!string.IsNullOrWhiteSpace(WholesomeDungeonsSettings.CurrentSetting.TankName) && args[0].ToLower() == WholesomeDungeonsSettings.CurrentSetting.TankName)
                            //accepts the invite if it is correct
                            Logger.Log($"Accepting Invite from: {args[0].ToLower()}");
                            Lua.LuaDoString("StaticPopup1Button1:Click()");

                        break;
                    }
                case "LFG_ROLE_CHECK_SHOW":
                    {
                        //Checks if actual Townrun is active and breaks if true
                        if (Bot.WholesomeDungeons.TownRunActive == true)
                            break;
                        Logger.Log("Rolecheck Window Opened, Accepting");
                        //accepts if no townrun is active
                        Lua.LuaDoString("LFDRoleCheckPopupAcceptButton:Click()");
                        break;
                    }
                case "EQUIP_BIND_CONFIRM":
                case "AUTOEQUIP_BIND_CONFIRM":
                case "LOOT_BIND_CONFIRM":
                case "USE_BIND_CONFIRM":
                case "CONFIRM_BINDER":
                    {
                        //confirms Bind on Pickup
                        Usefuls.SelectGossipOption(1);
                        Lua.LuaDoString("StaticPopup1Button1:Click()");
                        break;
                    }
                case "LFG_COMPLETION_REWARD":
                    {
                        //since only the tank knows that the dungeon is finished through the profile, the followers react to the LFC/Completion  Reward
                        if (WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                            break;
                        Logger.Log("Set Dungeon to completed through LUA");
                        //sets the variable LastBossKilled to true
                        if (LeaveDungeon.LastBossKilled != true)
                        {
                            LeaveDungeon.LastBossKilled = true;                         
                            Logger.Log("LFG_COMPLETION_REWARD fired, set LastBosskilled =" + LeaveDungeon.LastBossKilled);
                        }

                        break;
                    }
                case "LFG_PROPOSAL_SHOW":
                    {
                        //accepts proposals
                        Thread.Sleep(2000);
                        Lua.LuaDoString("AcceptProposal()");
                        break;
                    }
                case "RESURRECT_REQUEST":
                    {
                        //if there is a ressurection request, accept it
                        if (Lua.LuaDoString<bool>("return StaticPopup1 and StaticPopup1:IsVisible();"))
                        {
                            Lua.LuaDoString("StaticPopup1Button1:Click()");
                            Logger.Log("Accepted Resurrection");
                        }
                            break;
                    }
            }
        }
    }
}
