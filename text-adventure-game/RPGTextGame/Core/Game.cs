using EntityPlayer;
using Util;
using Scripts;
using Battle;
using World;
using States;
using System.Text;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Core
{
    class Game
    {

        public Player currentPlayer = new Player();
        public ItemShop ItemShop = new ItemShop();
        public virtual GameState State { get; set; } = new GameState();
        public SaveLoadHandler Handler = new SaveLoadHandler();
        public virtual List<Room> Rooms { get; set; } = new List<Room>();
        public Saloon Saloon = new Saloon();
        public void Start()
        {
            State = Handler.Load(this);
            Rooms = SetupWorld();
            if (State.PlayerData == null)
            {
                Console.WriteLine("Iniciando novo jogo...");
                // Crie um novo jogador e defina a sala inicial padrão
                currentPlayer = new Player("Novo Aventureiro", 30, 10);
                currentPlayer.SetRoom(Rooms.FirstOrDefault(r => r.Name == "Cemitério")); // Defina a sala inicial real
                ItemShop = new ItemShop();
                State = new GameState { PlayerData = currentPlayer, ShopData = ItemShop, StartTime = DateTime.Now };
                ScriptManager.ScriptedIntroScene(State.PlayerData); // Se for um novo jogo, rode a intro
                Encounter.FirstEncounter(State.PlayerData, this); // Primeiro encontro
            }
            else
            {
                // Se o save foi válido, use os dados carregados
                currentPlayer = State.PlayerData;
                ItemShop = State.ShopData;
                currentPlayer.inventory.SetPlayer(currentPlayer);

                // Agora, encontre a *instância correta* da sala na sua lista 'Rooms'
                Room loadedRoomInstance = Rooms.FirstOrDefault(r => r.Name == currentPlayer.CurrentRoomName);

                // Se a sala salva não foi encontrada no mundo recém-criado (pode acontecer se você mudar o nome de uma sala)
                if (loadedRoomInstance == null)
                {
                    Console.WriteLine($"A sala '{currentPlayer.CurrentRoomName}' salva não foi encontrada. Indo para o Cemitério.");
                    currentPlayer.SetRoom(Rooms.FirstOrDefault(r => r.Name == "Cemitério")); // Defina uma sala padrão segura
                }
                else
                {
                    currentPlayer.SetRoom(loadedRoomInstance);
                }
            }

            CommandHandler handler = new CommandHandler(this, currentPlayer, ItemShop, Saloon);

            while (true)
            {
                while (!BattleManager.GetInCombat())
                {
                    var room = currentPlayer.CurrentRoom;
                    // TODO: Fazer uma hud decente                   
                    Console.Clear();
                    Console.WriteLine(ShowHud(currentPlayer, currentPlayer.CurrentRoom));

                    if (room.Name == "Loja")
                    {
                        Console.WriteLine();
                        ItemShop.PrintShop(currentPlayer);
                    }
                    if(room.Name == "Saloon")
                    {
                        Console.WriteLine();
                        Saloon.PrintShop(currentPlayer);
                    }
                    Console.WriteLine();
                    Console.Write("> ");
                    string[] input = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    string resultado = handler.Handle(input);
                    Console.WriteLine(resultado);
                    Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                }
            }
        }

        public void SaveGame()
        {
            Handler.Save(this, State);
        }

        public GameState NewGame(int id)
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
            Encounter.FirstEncounter(State.PlayerData, this);
            return State;
        }

        public List<Room> SetupWorld()
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

        public string PrintCurrentExits(Room room)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var r in room.Exits)
            {
                sb.Append(r.Key + " │ ");
            }
            sb.Remove(sb.ToString().Length - 2, 2);
            return sb.ToString();
        }

        private string ShowHud(Player player, Room currentRoom)
        {
            const int boxWidth = 34;
            string topBorder = "┌" + new string('─', boxWidth) + "┐";
            string bottomBorder = "└" + new string('─', boxWidth) + "┘";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(topBorder);
            sb.AppendLine("│" + TextUtils.CenterLeftText($"\U0001f9cd Jogador: {player.Name}", boxWidth) + "│");
            sb.AppendLine("│" + TextUtils.CenterLeftText($"❤️ HP: {player.Health} / {player.MaxHealth}", boxWidth) + "│");
            sb.AppendLine("│" + TextUtils.CenterLeftText($"💰 Gold: {player.Coins}", boxWidth) + "│");
            sb.AppendLine("│" + TextUtils.CenterLeftText($"🗺️ Sala: {player.CurrentRoomName}", boxWidth) + " │");
            sb.AppendLine("│" + new string(' ', boxWidth) + "│");
            sb.AppendLine("│" + TextUtils.CenterLeftText($"Saidas: {PrintCurrentExits(currentRoom)}" , boxWidth) + "│");
            sb.AppendLine(bottomBorder);
            sb.AppendLine("Cheguei aqui");
            return sb.ToString();
        }
    }
}
