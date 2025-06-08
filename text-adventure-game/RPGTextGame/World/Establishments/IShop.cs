using EntityPlayer;

namespace World
{
    interface IShop
    {
        public void PrintShop(Player player);
        public void ProcessPurchase(Player player, string? arg);
    }
}
