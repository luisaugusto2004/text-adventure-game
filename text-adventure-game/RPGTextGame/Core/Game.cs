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

            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer = new Player(Console.ReadLine(), 30, 10);
            Console.Clear();
            //ScriptManager.ScriptedIntroScene();
            //Encounter.FirstEncounter();
            while (true)
            {
                while (!InCombat)
                {
                    Console.Clear();
                    Console.WriteLine(currentPlayer);
                    Console.Write("Sala atual: ");
                    Console.WriteLine(CurrentRoom.Name);
                    PrintCurrentExits(CurrentRoom);
                    Console.WriteLine();
                    Console.Write("> ");
                    string[] actionAndParameter = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (actionAndParameter.Length == 0) {
                        Console.WriteLine("Digite algum comando");
                        Console.ReadLine();
                        continue;
                    }
                    
                    string command = actionAndParameter[0].ToLower();
                    string? arg = actionAndParameter.Length > 1 ? actionAndParameter[1].ToLower() : null;

                    if (command == "deslocar")
                    {

                        if(arg == null)
                        {
                            Console.WriteLine("Digite para onde quer ir");
                            Console.ReadLine();
                            continue;
                        }

                        if (CurrentRoom.Exits.TryGetValue(arg, out Room nextRoom))
                        {
                            CurrentRoom = nextRoom;
                            Console.WriteLine($"Você se moveu para {CurrentRoom.Name}.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Modo de usar: deslocar <saida>");
                            Console.ReadLine();
                        }
                    }
                    else if (command == "lutar" && CurrentRoom.IsHostile)
                    {
                        InCombat = true;
                        while (InCombat)
                            Encounter.RandomEncounter(GlobalRandom);
                    }
                    else if (command == "examinar")
                    {
                        if(arg == null)
                        {
                            Console.WriteLine("Digite o que quer examinar");
                            Console.ReadLine();
                            continue;
                        }

                        if (arg == "sala")
                        {
                            Console.WriteLine(CurrentRoom.Description);
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Escolha algo válido para examinar");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Digite um comando válido.");
                        Console.ReadLine();
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
