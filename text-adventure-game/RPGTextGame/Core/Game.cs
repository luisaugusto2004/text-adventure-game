using EntityPlayer;
using Util;
using Scripts;
using System;
using Battle;
using World;

namespace Core
{
    class Game
    {

        public static Player currentPlayer = new Player();
        public static Random GlobalRandom = new Random();
        public static Room CurrentRoom;
        public static bool ProfeciaAtivada = false;
        public static int Turns = 1;
        public static bool InCombat = false;



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

            CurrentRoom = cidade;

            Console.ForegroundColor = ConsoleColor.Green;
            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer = new Player(Console.ReadLine(), 30, 10);
            Console.Clear();
            ScriptManager.ScriptedIntroScene();
            Encounter.FirstEncounter();
            Console.Clear();
            while (true)
            {
                while (!InCombat)
                {
                    Console.Write("Sala atual: ");
                    Console.WriteLine(CurrentRoom.Name);
                    PrintCurrentExits(CurrentRoom);
                    Console.WriteLine();
                    Console.Write("> ");
                    string action = Console.ReadLine();
                    string[] actionAndParameter = action.Split(' ');
                    if (actionAndParameter[0].ToLower() == "deslocar")
                    {
                        if (CurrentRoom.Exits.TryGetValue(actionAndParameter[1].ToLower(), out Room nextRoom))
                        {
                            CurrentRoom = nextRoom;
                            Console.WriteLine($"Você se moveu para {CurrentRoom.Name}.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Você não pode ir nessa direção.");
                            Console.ReadLine();
                        }
                    }
                    else if (actionAndParameter[0].ToLower() == "lutar" && CurrentRoom.IsHostile)
                    {
                        InCombat = true;
                        while (InCombat)
                            Encounter.RandomEncounter(GlobalRandom);
                    }
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
