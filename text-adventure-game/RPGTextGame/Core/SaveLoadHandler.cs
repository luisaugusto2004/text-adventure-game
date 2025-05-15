using Newtonsoft.Json;
using States;
using Util;

namespace Core
{
    class SaveLoadHandler
    {
        public string SaveFileName(GameState state)
        {
            string fileName = $"saves/player_{state.PlayerData.Id.ToString()}.json";
            return fileName;
        }

        public void Save(Game game, GameState state)
        {
            state.SaveTime = DateTime.Now;
            state.TotalPlayTime += DateTime.Now - game.State.StartTime;
            state.PlayerData = game.currentPlayer;
            state.ShopData = game.ItemShop;

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

            File.WriteAllText(SaveFileName(state), jsonStringPlayer);
            File.WriteAllText(SaveFileName(state) + ".hash", hash);
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

        public GameState Load(Game game)
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
                catch (JsonSerializationException e)
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
                if (corruptedFiles.Count > 0)
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
                        return game.NewGame(idCount);
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
                catch (OverflowException)
                {
                    Console.WriteLine("Este número é muito grande para ter um jogador! Pressione qualquer tecla para continuar");
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
