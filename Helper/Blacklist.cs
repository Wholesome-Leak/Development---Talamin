using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wManager.Wow.ObjectManager;

namespace WholesomeDungeons.Helper
{
    public static class Blacklist
    {
        public static readonly HashSet<WoWUnit> LootBlacklist = new HashSet<WoWUnit>
        { };

        public static readonly Dictionary<int, float> KnownAoE = new Dictionary<int, float> {
            // { Entry, Radius }
            {69024, 4f}, // Toxic Waste
            {70274, 4f}, // Toxic Waste
            {70436, 4f}, // Toxic Waste
            {36610, 5.5f} // Exploding Orb - Casted by spell Explosive Barrage (69015)
        };
    }
}
