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

        public Player currentPlayer = new Player();
        public static Random GlobalRandom = new Random();
        public ItemShop ItemShop = new ItemShop();
        public GameState State = new GameState();
        public void Start()
        {
            State = Load();

            List<Room> rooms = SetupWorld();
            currentPlayer = State.PlayerData;
            ItemShop = State.ShopData;
            currentPlayer.inventory.SetPlayer(currentPlayer);
            Room loadedRoom = rooms.FirstOrDefault(r => r.Name == currentPlayer.CurrentRoomName);

            if (loadedRoom != null)
            {
                currentPlayer.SetRoom(rooms.FirstOrDefault(r => r.Name == currentPlayer.CurrentRoomName));
            }
            else
            {
                currentPlayer.SetRoom(rooms.FirstOrDefault(r => r.Name == "Cemitério"));
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
                    if (currentPlayer.CurrentRoom == rooms.FirstOrDefault(r => r.Name == "Loja"))
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
            string fileName = $"saves/player_{State.PlayerData.Id.ToString()}.json";
            return fileName;
        }

        public void Save(GameState state)
        {
            state.SaveTime = DateTime.Now;
            state.TotalPlayTime += DateTime.Now - State.StartTime;
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

            string jsonStringPlayer = JsonConvert.SerializeObject(state, settings);
            string hash = TextUtils.GenerateHash(jsonStringPlayer);

            File.WriteAllText(SaveFileName(), jsonStringPlayer);
            File.WriteAllText(SaveFileName() + ".hash", hash);
        }

        private bool IsSaveFileValid(string jsonPath)
        {
            string hashPath = jsonPath + ".hash";
            if (!File.Exists(hashPath)) return false;

            string json = File.ReadAllText(jsonPath);
            string storedHash = File.ReadAllText(hashPath);
            string computedHash = TextUtils.GenerateHash(json);

            return storedHash == computedHash;
        }

        public GameState Load()
        {
            Console.Clear();
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }

            string[] paths = Directory.GetFiles("saves", "*.json");

            List<string> corruptedFiles = new List<string>();
            List<GameState> saves = new List<GameState>();

            int idCount = 0;
            int corruptedSaves = 0;
            FileInfo fileInfo;

            foreach (string p in paths)
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                try
                {
                    string jsonStringPlayer = File.ReadAllText(p);

                    if (!IsSaveFileValid(p))
                    {
                        corruptedFiles.Add(p);
                        corruptedSaves++;
                        continue;
                    }

                    GameState? state = JsonConvert.DeserializeObject<GameState>(jsonStringPlayer, settings);

                    if (state != null)
                    {
                        saves.Add(state);
                    }
                }
                catch(JsonSerializationException e)
                {
                    Console.WriteLine($"Erro ao deserializar o arquivo: {p}");
                    Console.WriteLine(e.Message);
                    corruptedFiles.Add(p);
                    corruptedSaves++;
                    continue;
                }
                catch (IOException e)
                {
                    Console.WriteLine($"Erro ao ler o arquivo: {p}");
                    Console.WriteLine(e.Message);
                    corruptedFiles.Add(p);
                    corruptedSaves++;
                    continue;
                }
            }

            idCount = saves.Count + corruptedSaves;
            string[] data;
            while (true)
            {
                if(corruptedFiles.Count > 0)
                {
                    Console.WriteLine("Os seguintes arquivos estão corrompidos: ");
                    foreach (var corrupted in corruptedFiles)
                    {
                        fileInfo = new FileInfo(corrupted);
                        Console.WriteLine($" - {fileInfo.Name}");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Jogadores encontrados: ");

                foreach (var state in saves)
                {
                    string? totalPlayTime = state.TotalPlayTime.Days > 0 ? state.TotalPlayTime.ToString(@"dd\.hh\:mm\:ss") : state.TotalPlayTime.ToString(@"hh\:mm\:ss");
                    Console.WriteLine($"{state.PlayerData.Id}: {state.PlayerData.Name}, Tempo de jogo: {totalPlayTime}");
                    Console.WriteLine($"Último save em: {state.SaveTime}");
                    Console.WriteLine();
                }
                Console.WriteLine("Digite 'id:<id>' ou '<nome do jogador>' para carregar, ou 'criar' para um novo jogador.");
                Console.Write("> ");

                try
                {
                    data = Console.ReadLine().Split(':', StringSplitOptions.RemoveEmptyEntries);

                    if (data[0] == "criar")
                    {
                        NewGame(idCount);
                        return State;
                    }

                    GameState save = data[0] == "id"
                        ? FindSaveById(int.Parse(data[1]), saves)
                        : FindSaveByName(data[0], saves);

                    if (save != null)
                    {
                        save.StartTime = DateTime.Now;
                        return save;
                    }

                    Console.WriteLine("Jogador não encontrado! Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Digite um id existente! Pressione qualquer tecla para continuar");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (FormatException)
                {
                    Console.WriteLine("Formato de ID inválido. Digite 'id:<número>'! Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                    Console.Clear();
                }

            }
        }

        private GameState FindSaveById(int id, List<GameState> saves)
        {
            return saves.FirstOrDefault(save => save.PlayerData.Id == id);
        }

        private GameState FindSaveByName(string name, List<GameState> saves)
        {
            return saves.FirstOrDefault(save => save.PlayerData.Name == name);
        }
    }
}
