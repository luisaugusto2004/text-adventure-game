using EntityPlayer;
using Items;
using Core;
using World;
using Moq;
using States;
using Battle;

namespace text_adventure_game.Test
{
    public class CommandHandlerTest
    {

        private Player player;
        private Mock<ItemShop> _itemShopMock;
        private Mock<Game> _gameMock;
        private Mock<Saloon> _saloonMock;
        private Mock<BattleManager> _battleManager;
        private CommandHandler _commandHandler;
        private readonly TextWriter _originalConsoleOut;
        public CommandHandlerTest()
        {
            _itemShopMock = new Mock<ItemShop>();
            _gameMock = new Mock<Game>();
            _saloonMock = new Mock<Saloon>();
            _battleManager = new Mock<BattleManager>();


            var cemiterio = new Room("Cemitério", "Um cemit�rio caindo aos peda�os, muitas covas e criaturas host�s", true);
            var cidade = new Room("Cidade", "Uma cidade pacata, tem uma loja e um saloon", false);
            var saloon = new Room("Saloon", "Um saloon bem animado, tem um pianista e algumas dan�arinas", false);
            var loja = new Room("Loja", "Uma loja humilde por�m bem completa, tem tudo que um aventureiro precisa", false);

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

            var testRooms = new List<Room>
            {
                cemiterio,
                cidade,
                saloon,
                loja
            };
            _gameMock.Setup(g => g.Rooms).Returns(testRooms);

            player = new Player();
            player.SetRoom(cidade);

            _commandHandler = new CommandHandler(
                _gameMock.Object,
                player,
                _itemShopMock.Object,
                _saloonMock.Object
            );
        }

        public void Dispose()
        {
            Console.SetOut(_originalConsoleOut);
        }

        [Fact]
        public void Handle_Deslocar_ComComandoVazio_RetornaMensagemDeErro()
        {
            // Act
            string result = _commandHandler.Handle(new string[] { "deslocar", "" });

            // Assert
            Assert.Equal("Digite para onde quer ir", result);
        }

        [Fact]
        public void Handle_Deslocar_ParaLocalValido_MudaSalaDoJogadorERetornaMensagemDeSucesso()
        {
            //Act
            string result = _commandHandler.Handle(new string[] { "deslocar", "cemiterio" });

            //Assert
            Assert.Equal("Você se moveu para Cemitério.", result);
        }

        [Fact]
        public void Handle_Deslocar_ParaLocalInvalido_RetornaMensagemDeErro()
        {
            // Act
            string result = _commandHandler.Handle(new string[] { "deslocar", "floresta" });

            // Assert
            Assert.Equal("Modo de usar: deslocar <saida>", result);
        }
    }
}