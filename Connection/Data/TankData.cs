using robotManager.Helpful;
using wManager.Wow.Enums;

namespace WholesomeDungeons.Connection.Data
{
    public class TankData
    {
        public Vector3 Position { get; set; }
        public int ContinentID { get; set; }

        public TankData(Vector3 position, int continentid)
        {
            Position = position;
            ContinentID = continentid;
        }
    }
}
