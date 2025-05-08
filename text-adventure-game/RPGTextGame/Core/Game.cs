using EntityPlayer;
using Util;
using Scripts;
using System;
using Battle;
using World;
using Items;

namespace Core
{
    class Game
    {

        public static Player currentPlayer = new Player();
        public static Random GlobalRandom = new Random();

        public void Start()
        {
            Room cemiterio = new Room(
                "Cemitério",
                "Um cemitério caindo aos pedaços, muitas covas e criaturas hostís",
                true
                );
            Room cidade = new Room(
                "Cidade",
                "Uma cidade pacata, tem uma loja e um saloon"
                );
            Room saloon = new Room(
                "Saloon",
                "Um saloon bem animado, tem um pianista e algumas dançarinas"
                );
            Room loja = new Room(
                "Loja",
                "Uma loja humilde porém bem completa, tem tudo que um aventureiro precisa"
                );

            Dictionary<string, Room> cemeteryExits = new Dictionary<string, Room>
                {
                    { "cidade", cidade }
                };
            Dictionary<string, Room> cityExits = new Dictionary<string, Room>
                {
                    { "cemiterio", cemiterio },
                    { "saloon", saloon },
                    { "loja", loja }
                };
            Dictionary<string, Room> saloonExits = new Dictionary<string, Room>
                {
                    { "cidade", cidade }
                };
            Dictionary<string, Room> storeExits = new Dictionary<string, Room>
                {
                    { "cidade", cidade }
                };

            cemiterio.SetExits(cemeteryExits);
            cidade.SetExits(cityExits);
            saloon.SetExits(saloonExits);
            loja.SetExits(storeExits);            

            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer = new Player(Console.ReadLine(), 30, 10);

            Console.Clear();

            //ScriptManager.ScriptedIntroScene();
            //Encounter.FirstEncounter();
            Weapon weapon = new Weapon("Espada curta", 1, 6, "Uma espada enferrujada, porém bastante confiável");
            Weapon weapon2 = new Weapon("Espada do Guts", 3, 10, "Espada fodona");
            Weapon weapon3 = new Weapon("Caçadora de cabeças", 2, 12, "Pitola do chamber lol");
            currentPlayer.inventory.AddItem(weapon);
            currentPlayer.inventory.AddItem(weapon2);
            currentPlayer.inventory.AddItem(weapon3);

            currentPlayer.SetRoom(cidade);
            while (true)
            {
                while (!BattleManager.GetInCombat())
                {
                    Console.Clear();
                    Console.Write("Sala atual: ");
                    Console.WriteLine(currentPlayer.CurrentRoom.Name);
                    PrintCurrentExits(currentPlayer.CurrentRoom);
                    Console.WriteLine();
                    Console.Write("> ");
                    string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    CommandHandler handler = new CommandHandler(currentPlayer);
                    handler.Handle(input);
                    Console.Clear();
                }
            }
        }

        private void PrintCurrentExits(Room room)
        {
            Console.WriteLine("Saidas atuais: ");
            Console.WriteLine();
            foreach (var r in room.Exits)
            {
                Console.WriteLine(r.Key);
            }
        }
    }
}
