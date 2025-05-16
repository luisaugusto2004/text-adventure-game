using Core;
using EntityPlayer;
using States;
using Util;
using World;

namespace text_adventure_game.Test
{
    public class SaveLoadHandlerTest
    {
        [Fact]
        public void SalvaOJogo_ComArquivoInexistente_CriaNovoArquivo()
        {
            //Arrange
            var handler = new SaveLoadHandler();
            var player = new Player("Teste", 30, 10) { Id = 999 };
            var shop = new ItemShop();
            shop.SetId(999);
            var state = new GameState
            {
                PlayerData = player,
                ShopData = shop,
                StartTime = DateTime.Now,
                TotalPlayTime = TimeSpan.Zero
            };
            var game = new Game();
            game.currentPlayer = player;
            game.ItemShop = shop;
            game.State = state;

            //Act
            handler.Save(game, state);

            //Assert
            string jsonPath = $"saves/player_999.json";
            string hashPath = jsonPath + ".hash";
            Assert.True(File.Exists(jsonPath));
            Assert.True(File.Exists(hashPath));

            string json = File.ReadAllText(jsonPath);
            string hash = File.ReadAllText(hashPath);
            string computed = TextUtils.GenerateHash(json);

            Assert.Equal(hash, computed);

            //Cleanup
            File.Delete(jsonPath);
            File.Delete(hashPath);
        }

        [Fact]
        public void CarregarOJogo_ComArquivoExistente_CarregaNormalmente()
        {
            //Arrange
            var handler = new SaveLoadHandler();
            var player = new Player("Teste", 30, 10) { Id = 998 };
            var shop = new ItemShop();
            shop.SetId(998);
            var state = new GameState
            {
                PlayerData = player,
                ShopData = shop,
                StartTime = DateTime.Now,
                TotalPlayTime = TimeSpan.Zero
            };
            var game = new Game();
            game.currentPlayer = player;
            game.ItemShop = shop;
            game.State = state;

            //Act
            handler.Save(game, state);
            GameState loaded = handler.Load(game, true);

            //Assert
            string jsonPath = $"saves/player_998.json";
            string hashPath = jsonPath + ".hash";

            Assert.Equal(state.PlayerData.Id, loaded.PlayerData.Id);
            Assert.Equal(state.PlayerData.Name, loaded.PlayerData.Name);
            Assert.Equal(state.PlayerData.CurrentRoomName, loaded.PlayerData.CurrentRoomName);
            Assert.Equal(state.ShopData.Id, loaded.ShopData.Id);
            Assert.Equal(state.ShopData.Itens.Count, loaded.ShopData.Itens.Count);
            Assert.Equal(state.PlayerData.Health, loaded.PlayerData.Health);

            //Cleanup
            File.Delete(jsonPath);
            File.Delete(hashPath);
        }

        [Fact]
        public void SaveFilmeName_RetornaCaminhoEsperado()
        {
            var handler = new SaveLoadHandler();
            var state = new GameState { PlayerData = new Player { Id = 123 } };
            string expected = "saves/player_123.json";

            string result = handler.SaveFileName(state);

            Assert.Equal(expected, result);
        }
    }
}
