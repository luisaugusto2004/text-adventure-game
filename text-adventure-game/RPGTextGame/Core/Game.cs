using EntityPlayer;
using Util;
using Scripts;
using Battle;
using World;
using States;


namespace Core
{
    class Game
    {

        public Player currentPlayer = new Player();
        public ItemShop ItemShop = new ItemShop();
        public GameState State = new GameState();
        public SaveLoadHandler Handler = new SaveLoadHandler();
        public List<Room> Rooms = new List<Room>();
        public void Start()
        {
            State = Handler.Load(this);
            Rooms = SetupWorld();
            currentPlayer = State.PlayerData;
            ItemShop = State.ShopData;
            currentPlayer.inventory.SetPlayer(currentPlayer);
            Room loadedRoom = Rooms.FirstOrDefault(r => r.Name == currentPlayer.CurrentRoomName);

            SetCurrentRoom(Rooms, loadedRoom);

            CommandHandler handler = new CommandHandler(this, currentPlayer, ItemShop);

            while (true)
            {
                while (!BattleManager.GetInCombat())
                {
                    // TODO: Fazer uma hud decente
                    Console.Clear();
                    Console.Write("Sala atual: ");
                    Console.WriteLine(currentPlayer.CurrentRoom.Name);
                    PrintCurrentExits(currentPlayer.CurrentRoom);
                    if (currentPlayer.CurrentRoom == Rooms.FirstOrDefault(r => r.Name == "Loja"))
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

        public void SetCurrentRoom(List<Room> rooms, Room loadedRoom)
        {
            if (loadedRoom != null)
            {
                currentPlayer.SetRoom(rooms.FirstOrDefault(r => r.Name == currentPlayer.CurrentRoomName));
            }
            else
            {
                currentPlayer.SetRoom(rooms.FirstOrDefault(r => r.Name == "Cemitério"));
            }
        }

        public void SetDefaultRoomOnDeath()
        {

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
    }
}
