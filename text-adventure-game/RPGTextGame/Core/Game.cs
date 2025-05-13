using EntityPlayer;
using Util;
using Scripts;
using Battle;
using World;
using Items;
using System.Text.Json;
using States;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core
{
    class Game
    {

        public static Player currentPlayer;
        public static Random GlobalRandom = new Random();
        public ItemShop ItemShop;
        public GameState State = new GameState();
        public void Start()
        {
            State = Load();

            List<Room> rooms = SetupWorld();
            currentPlayer = State.PlayerData;
            ItemShop = State.ShopData;
            currentPlayer.inventory.SetPlayer(currentPlayer);
            Room loadedRoom = rooms.Find(r => r.Name == currentPlayer.CurrentRoomName);

            if (loadedRoom != null)
            {
                currentPlayer.SetRoom(rooms.Find(r => r.Name == currentPlayer.CurrentRoomName));
            }
            else
            {
                currentPlayer.SetRoom(rooms.Find(r => r.Name == "Cemitério"));
            }

            CommandHandler handler = new CommandHandler(currentPlayer, ItemShop);

            while (true)
            {
                while (!BattleManager.GetInCombat())
                {
                    // TODO: Fazer uma hud decente
                    Console.Clear();
                    Console.Write("Sala atual: ");
                    Console.WriteLine(currentPlayer.CurrentRoom.Name);
                    PrintCurrentExits(currentPlayer.CurrentRoom);
                    if (currentPlayer.CurrentRoom == rooms.Find(r => r.Name == "Loja"))
                    {
                        Console.WriteLine();
                        ItemShop.PrintShop(currentPlayer);
                    }
                    Console.WriteLine();
                    Console.Write("> ");
                    string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    handler.Handle(input, this);
                    Console.Clear();
                }
            }
        }

        private void NewGame(int id)
        {
            Console.Clear();
            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer = new Player(Console.ReadLine(), 30, 10);
            ItemShop = new ItemShop();
            currentPlayer.SetId(id);
            ItemShop.SetId(id);
            ItemShop.SetList();
            State = new GameState()
            {
                PlayerData = currentPlayer,
                ShopData = ItemShop,
                StartTime = DateTime.Now,
                SaveTime = DateTime.Now,
                TotalPlayTime = TimeSpan.Zero
            };
            ScriptManager.ScriptedIntroScene(State.PlayerData);
            Encounter.FirstEncounter(State.PlayerData);
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

        public string SaveFileName()
        {
            string fileName = "saves/" + State.PlayerData.Id.ToString() + ".json";
            return fileName;
        }

        public void Save(GameState state)
        {
            state.SaveTime = DateTime.Now;
            state.TotalPlayTime = DateTime.Now - State.StartTime;
            state.PlayerData = currentPlayer;
            state.ShopData = ItemShop;
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            state.SaveTime = DateTime.Now;
            string jsonStringPlayer = JsonConvert.SerializeObject(state, settings);
            File.WriteAllText(SaveFileName(), jsonStringPlayer);
        }

        public GameState Load()
        {
            Console.Clear();
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            string[] paths = Directory.GetFiles("saves");
            List<GameState> saves = new List<GameState>();
            int idCount = 0;
            foreach (string p in paths)
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                string json = File.ReadAllText(p);

                GameState? state = JsonConvert.DeserializeObject<GameState>(json, settings);
                if (state != null)
                {
                    saves.Add(state);
                }
            }

            idCount = saves.Count;
            string[] data;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Escolha seu jogador: ");

                foreach (var state in saves)
                {
                    Console.WriteLine(state.PlayerData.Id + ": " + state.PlayerData.Name);
                }
                Console.WriteLine("Por favor, escreva o id ou o nome do jogador que deseja carregar(id:<id> ou <nome do jogador>)");
                data = Console.ReadLine().Split(':', StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    if (data[0] == "create")
                    {
                        NewGame(idCount);
                        return State;
                    }
                    else if (data[0] == "id")
                    {
                        if (int.TryParse(data[1], out int Id))
                        {
                            foreach (GameState state in saves)
                            {
                                if (state.PlayerData.Id == Id)
                                {
                                    return state;
                                }
                            }
                            Console.WriteLine("Não tem jogadores com esse Id");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Seu Id precisa ser um número! Pressione qualquer tecla para continuar");
                        }
                    }
                    else
                    {
                        foreach (var state in saves)
                        {
                            if (state.PlayerData.Name == data[0])
                            {
                                return state;
                            }
                        }
                        Console.WriteLine("Não tem jogador com esse nome!");
                        Console.ReadKey();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Seu Id precisa ser um número! Pressione qualquer tecla para continuar");
                    Console.ReadKey();
                }
            }
        }
    }
}
