using Battle;
using EntityPlayer;
using Items;
using System;
using System.Runtime.CompilerServices;
using System.Text;
using Util;
using World;
[assembly: InternalsVisibleTo("text-adventure-game.Test")]
namespace Core
{
    class CommandHandler
    {
        private readonly Player player;

        public CommandHandler(Player player)
        {
            this.player = player;
        }

        public void Handle(string[] input)
        {
            if (input.Length == 0)
            {
                Console.WriteLine("Digite algum comando");
                Console.ReadLine();
                return;
            }
            string command = input[0].ToLower();
            //Separar parâmetro com espaços para verificação mais intuitiva excluindo o espaço na última parte do parâmetro               
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < input.Length; i++)
            {
                sb.Append(input[i]);
                if (input.Length - i != 1)
                    sb.Append(' ');
                else
                    break;
            }
            string? arg = input.Length > 1 ? sb.ToString().ToLower() : null;

            switch (command)
            {
                case "deslocar":
                    Deslocar(arg);
                    break;
                case "lutar":
                    Lutar();
                    break;
                case "examinar":
                    Examinar(arg);
                    break;
                case "equipar":
                    Equipar(arg);
                    break;
                case "status":
                    Console.WriteLine(player);
                    Console.ReadLine();
                    break;
                case "inventario":
                    player.inventory.ListItens(player);
                    break;
                default:
                    Console.WriteLine("Digite um comando válido");
                    Console.ReadLine();
                    break;
            }
        }

        private void Equipar(string? arg)
        {
            var nameWeapon = TextUtils.RemoverAcentos(arg);
            Weapon weaponToEquip = null;
            if (arg == null)
            {
                Console.WriteLine("Digite algo para equipar");
                Console.ReadLine();
                return;
            }
            if (player.BuscarArmaNoInventario(nameWeapon) != null)
            {
                weaponToEquip = player.BuscarArmaNoInventario(nameWeapon);
                player.SetWeapon(weaponToEquip);
                Console.WriteLine($"Você equipou {weaponToEquip.Name}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Você não tem esse item no inventario");
                Console.ReadLine();
            }
        }

        private void Examinar(string? arg)
        {
            if (arg == null)
            {
                Console.WriteLine("Digite o que quer examinar");
                Console.ReadLine();
                return;
            }

            string argSemAcento = TextUtils.RemoverAcentos(arg.ToLower());
            string nomeArmaSemAcento = TextUtils.RemoverAcentos(player.equippedWeapon.Name.ToLower());

            if (arg == "sala")
            {
                Console.WriteLine(player.CurrentRoom.Description);
                Console.ReadLine();
            }
            else if (argSemAcento == nomeArmaSemAcento)
            {
                Console.WriteLine(player.equippedWeapon.Description);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Escolha algo válido para examinar");
                Console.ReadLine();
            }
        }

        private void Lutar()
        {
            if (player.CurrentRoom.IsHostile)
            {
                BattleManager.SetInCombat(true);
                while (BattleManager.GetInCombat())
                    Encounter.RandomEncounter(player);                    
            }
            else
            {
                Console.WriteLine("Não há com o que lutar aqui.");
                Console.ReadLine();
            }
        }

        private void Deslocar(string? arg)
        {
            if (arg == null)
            {
                Console.WriteLine("Digite para onde quer ir");
                Console.ReadLine();
                return;
            }

            if (player.CurrentRoom.Exits.TryGetValue(arg, out Room nextRoom))
            {
                player.SetRoom(nextRoom);
                Console.WriteLine($"Você se moveu para {player.CurrentRoom.Name}.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Modo de usar: deslocar <saida>");
                Console.ReadLine();
            }
        }
    }
}