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

        public static void Start()
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
            currentPlayer.SetRoom(cidade);
            while (true)
            {
                while (!Encounter.GetInCombat())
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

        private static void PrintCurrentExits(Room room)
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
