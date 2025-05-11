using EntityPlayer;
using Util;
using Scripts;
using Battle;
using World;
using Items;
using System.Text.Json;
using States;

namespace Core
{
    class Game
    {

        public static Player currentPlayer = new Player();
        public static Random GlobalRandom = new Random();
        public static ItemShop ItemShop = new ItemShop(currentPlayer);
        public GameState State = new GameState();
        public void Start()
        {
            List<Room> rooms = SetupWorld();

            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer = new Player(Console.ReadLine(), 30, 10);

            currentPlayer.SetRoom(rooms.Find(r => r.Name == "Loja"));

            ItemShop = new ItemShop(currentPlayer);

            CommandHandler handler = new CommandHandler(currentPlayer, ItemShop);
            State = new GameState()
            {
                PlayerData = currentPlayer,
                ShopData = ItemShop,
                StartTime = DateTime.Now,
                SaveTime = DateTime.Now,
                TotalPlayTime = TimeSpan.Zero
            };
            Console.Clear();

            //ScriptManager.ScriptedIntroScene();
            //Encounter.FirstEncounter();
            while (true)
            {
                while (!BattleManager.GetInCombat())
                {
                    // TODO: Fazer uma hud decente
                    Console.Clear();
                    Console.Write("Sala atual: ");
                    Console.WriteLine(currentPlayer.CurrentRoom.Name);
                    PrintCurrentExits(currentPlayer.CurrentRoom);
                    if(currentPlayer.CurrentRoom == rooms.Find(r => r.Name == "Loja"))
                    {
                        Console.WriteLine();
                        ItemShop.PrintShop();
                    }
                    Console.WriteLine();
                    Console.Write("> ");
                    string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);                                       
                    handler.Handle(input);
                    Console.Clear();
                }
            }
        }

        private List<Room> SetupWorld()
        {
            List<Room> rooms = new List<Room>();
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

            rooms.Add(cemiterio);
            rooms.Add(cidade);
            rooms.Add(saloon);
            rooms.Add(loja);

            return rooms;
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

        public static void Save(GameState state)
        {
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }            
            string fileName = "saves/" + state.PlayerData.Id.ToString() + ".json";
            var options = new JsonSerializerOptions { WriteIndented = true };   
            string jsonStringPlayer = JsonSerializer.Serialize(state, options);
            File.WriteAllText(fileName, jsonStringPlayer);
        }

        public void SaveGame()
        {
            Save(State);
        }
    }
}
