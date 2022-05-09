using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholesomeDungeons.Bot;
using WholesomeDungeons.Helper;
using wManager;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Helper.Helpers;

namespace WholesomeDungeons.States
{
    public class ReviveParty : State
    {
        public override string DisplayName
        {
            get { return "ReviveParty"; }
        }

        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        private int _priority;
        public WoWUnit _foundUnit;

        public Dictionary<WoWClass, string> RezzClasses = new Dictionary<WoWClass, string> { { WoWClass.Druid, "Revive" }, { WoWClass.Paladin, "Redemption" }, { WoWClass.Priest, "Resurrection" }, { WoWClass.Shaman, "Ancestral Spirit" } };

        // If this method return true, wrobot launch method Run(), if return false wrobot go to next state in FSM
        public override bool NeedToRun
        {
            get
            {
                if (ObjectManager.Me.IsDead)
                    return false;

                if (ObjectManager.Me.InCombat)
                    return false;

                if (ObjectManager.Me.InCombatFlagOnly)
                    return false;

                if (RezzClasses.ContainsKey(ObjectManager.Me.WowClass) && Party.GetPartyHomeAndInstance().Any(p => p.IsDead))
                    return true;

                return false;
            }
        }

        // If NeedToRun() == true
        public override void Run()
        {

            if(!Party.GetPartyHomeAndInstance().Any(s => RezzClasses.ContainsValue(s.CastingSpell.Name)))
            {
                WoWUnit foundUnit = Party.GetPartyHomeAndInstance().Where(p => p.IsDead).FirstOrDefault();

                _foundUnit = foundUnit;

                if (foundUnit.GetDistance >= 28)
                    StartGoToThread(foundUnit.Position, abortIf: IamNear);

                if (IamNear())
                    Interact.InteractGameObject(foundUnit.GetBaseAddress);

                var spell = RezzClasses[ObjectManager.Me.WowClass];


                SleepRandomTime();
                if (SpellManager.KnowSpell(spell) && SpellManager.SpellUsableLUA(spell))
                {
                    SpellManager.CastSpellByNameLUA(spell);
                    Usefuls.WaitIsCasting();
                }
            }         
        }
        
        public bool IamNear()
        {
            if (_foundUnit.GetDistance < 28 && !TraceLine.TraceLineGo(ObjectManager.Me.Position, _foundUnit.Position, CGWorldFrameHitFlags.HitTestSpellLoS | CGWorldFrameHitFlags.HitTestMovableObjects))
                return true;
            return false;
        }

    }
}
