using EntityPlayer;
using World;

namespace States
{
    class GameState
    {
        public Player PlayerData { get; set; }
        public ItemShop ShopData { get; set; }
        public DateTime SaveTime { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan TotalPlayTime { get; set; }
    }
}
