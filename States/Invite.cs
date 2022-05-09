using robotManager.FiniteStateMachine;
using System.Collections.Generic;
using System.Threading;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using WholesomeDungeons.Bot;
using WholesomeDungeons.Helper;
using System.Linq;

namespace WholesomeDungeons.States
{
    public class Invite : State
    {
        public override string DisplayName
        {
            get { return "Invite to Group"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;


        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {
                if (!WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                    return false;

                if (!ObjectManager.Me.IsAlive)
                    return false;

                if(Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) > 0)
                    return false;

                if (Party.GetPartyNumberPlayers() < 5)
                {
                    return true;
                }
                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {
            if (Party.GetPartyNumberPlayers() < 5)
            {
                foreach (var playerName in WholesomeDungeonsSettings.CurrentSetting.GroupMembers)
                {
                    if (!InParty(playerName))
                    {
                        Lua.LuaDoString(Usefuls.WowVersion > 5875
                            ? $@"InviteUnit('{playerName}');"
                            : $@"InviteByName('{playerName}');");
                    }
                    Thread.Sleep(1000);
                }              
            }

        }
        public bool InParty(string name)
        {
            return Lua.LuaDoString<bool>($@"
            for i=1,4 do
                if (string.lower(UnitName('party'..i)) == '{name.ToLower()}') then
                    return true;
                end
            end
            return false;
        ");
        }
    }
}
