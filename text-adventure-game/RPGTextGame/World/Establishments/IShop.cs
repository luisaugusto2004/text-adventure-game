using EntityPlayer;

namespace World
{
    interface IShop
    {
        public void PrintShop(Player player);
        public string ProcessPurchase(Player player, string? arg);
    }
}
