using EntityPlayer;
using Items;
using Core;
using World;

namespace text_adventure_game.Test
{
    public class CommandHandlerTest
    {
        [Fact]
        public void Deslocar_ParaSalaExistente_SalaAtualMuda()
        {
            //Arrange
            Player player = new Player("teste", 30 , 10);
            Room room1 = new Room("room1", "teste1");
            Room room2 = new Room("room2", "teste2");
            Dictionary<string, Room> room1Exits = new Dictionary<string, Room>
            {
                { "room2", room2 }
            };
            Dictionary<string, Room> room2Exits = new Dictionary<string, Room>
            {
                { "room1", room1 }
            };
            room1.SetExits(room1Exits);
            room2.SetExits(room2Exits);
            player.SetRoom(room1);
            Game game = new Game();
            //Act
            string[] input = { "deslocar", "room2"};
            CommandHandler handler = new CommandHandler(player);
            handler.Handle(input, game);

            //Assert
            Assert.Equal(room2, player.CurrentRoom);
        }

        [Fact]
        public void Deslocar_ParaSalaInexistente_SalaAtualNaoMuda()
        {
            //Arrange
            Player player = new Player("teste", 30, 10);
            Room room1 = new Room("room1", "teste1");
            Room room2 = new Room("room2", "teste2");
            Dictionary<string, Room> room1Exits = new Dictionary<string, Room>
            {
                { "room2", room2 }
            };
            Dictionary<string, Room> room2Exits = new Dictionary<string, Room>
            {
                { "room1", room1 }
            };
            room1.SetExits(room1Exits);
            room2.SetExits(room2Exits);
            player.SetRoom(room1);
            Game game = new Game();
            //Act
            string[] input = { "deslocar", "room3" };
            CommandHandler handler = new CommandHandler(player);
            handler.Handle(input, game);
            
            //Assert
            Assert.Equal(room1, player.CurrentRoom);
        }
    }
}