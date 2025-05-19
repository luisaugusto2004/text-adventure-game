using Battle;
using EntityPlayer;
using Util;

namespace Core
{
    class RespawnManager
    {
        public static void HandlePlayerDeath(Player player, Game game)
        {
            TextPrinter.Print("Você morreu.", 50);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("Você acorda na cidade.", 40);
            TextPrinter.Print("Parte do seu dinheiro foi roubado", 40);
            Console.ReadLine();
            player.Revive();
            player.TakeDamage(player.MaxHealth / 2);
            player.LoseGold(player.Coins / 2);
            player.SetRoom(game.Rooms.FirstOrDefault(r => r.Name == "Cidade"));
        }
    }
}
